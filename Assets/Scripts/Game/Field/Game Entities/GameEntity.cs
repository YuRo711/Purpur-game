using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public abstract class GameEntity : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Serializable Fields

    [SerializeField] protected int entityId = -1;
    [SerializeField] protected int health = 1;
    [SerializeField] protected int size = 100;
    [SerializeField] protected Vector2 moveVector;
    [SerializeField] protected GameGrid levelGrid;
    [SerializeField] protected EnemyManager enemyManager;
    [SerializeField] public bool isBackground;

    [SerializeField] public string spawnClip;
    [SerializeField] protected string collisionClip;
    [SerializeField] protected SoundManager soundManager;
    [SerializeField] protected bool isSoundOn;

    #endregion
    
    #region Properties

    public LevelManager LevelManager { get; set; }
    public Direction LookDirection { get; set; }
    public EventHandler<Type> OnObjectDie { get; set; }
    [SerializeField] public int X { get; protected set; }
    [SerializeField] public int Y { get; protected set; }

    #endregion

    #region Interactions

    protected Dictionary<Type, Action<GameEntity>> CollisionInteractions = new ();
    
    protected void DamageEntity(GameEntity gameEntity, int damage, int damageToSelf = 0)
    {
        if (gameEntity is not PlayerShip)
            gameEntity.TakeDamage(damage);
        if (damageToSelf != 0)
            TakeDamage(damageToSelf);
        if (gameEntity is PlayerShip)
        {
            gameEntity.TakeDamage(damage);
            if (gameEntity.health > 0)
                gameEntity.MoveTo(gameEntity.X, gameEntity.Y);
        }
    }

    #endregion
    
    #region Public Methods
    
    // For movement in some direction
    public virtual void MoveInDirection(TurnDirections moveDir, int speed = 1)
    {
        var newX = X;
        var newY = Y;
        moveVector = LookDirection.TurnTo(moveDir).Vector;
        newX += (int)moveVector.x * speed;
        newY += (int)moveVector.y * speed;
        MoveTo(newX, newY);
        CallSync();
    }

    // For precise movement: warp, etc.
    public virtual void MoveTo(int destX, int destY, bool callSync = true, bool ignoreObjectCollision = false)
    {
        if (levelGrid.CheckForBorder(destX, destY))
            return;
        DeleteFromCell(entityId);
        X = destX;
        Y = destY;
        var newCell = levelGrid.Cells[Y, X];
        AdaptTransform(newCell);
        CollideWithCellEntity(newCell, ignoreObjectCollision);
        if (isBackground)
            newCell.BgEntity = this;
        else
            newCell.GameEntity = this;
        if (callSync)
            CallSync(ignoreObjectCollision);
    }

    public virtual void TurnTo(TurnDirections turnDirections, bool callSync = true)
    {
        LookDirection = LookDirection.TurnTo(turnDirections);
        var angle = LookDirection.GetDirectionAngle();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (callSync)
            CallSync();
    }

    public virtual void TakeDamage(int damage)
    {
        //Debug.LogError(name + " took damage");
        health -= damage;
        CallSync();
    }
    
    public virtual void SetStartParameters(int id, int x, int y)
    {
        entityId = id;
        photonView.RPC("SyncStart", RpcTarget.AllBuffered, entityId, x, y);
    }

    [PunRPC]
    public virtual void SyncStart(int id, int x, int y)
    {
        if (entityId == -1)
            StartCoroutine(WaitForId(id, x, y));

        if (entityId != id)
            return;
        
        LevelManager ??= FindObjectOfType<LevelManager>();
        LevelManager.AddEntity(this);
        levelGrid = LevelManager.levelGrid;
        enemyManager = LevelManager.enemyManager;
        soundManager = LevelManager.soundManager;
        if (this is PlayerShip playerShip)
            LevelManager.controlPanelGenerator.ConnectToPlayer(playerShip);
        
        LookDirection = new Direction(0, 1);
        MoveTo(x, y);
        transform.localScale = new Vector3(1, 1, 1);
    }

    [PunRPC]
    public void SyncParameters(int id, int newHealth, int newX, int newY, int turnX, int turnY,
        bool ignoreObjectCollision = false)
    {
        if (entityId != id)
            return;
        health = newHealth;
        if (health <= 0)
        {
            Die();
            return;
        }

        MoveTo(newX, newY, false, true);
        LookDirection = new Direction(turnX, turnY);
        TurnTo(TurnDirections.Forward, false);
        isSoundOn = true;
    }

    #endregion

    #region Protected Methods
    
    protected virtual void Die()
    {
        photonView.RPC("DeleteFromCell", RpcTarget.AllBuffered, entityId, true);
        OnObjectDie?.Invoke(this, GetType());
    }

    protected IEnumerator WaitForId(int id, int x, int y)
    {
        while (entityId == -1)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SyncStart(id, x, y);
    }

    [PunRPC]
    protected void DeleteFromCell(int id, bool destroy = false)
    {
        if (entityId != id)
            return;
        if (isBackground)
            levelGrid.Cells[Y, X].BgEntity = null;
        else
            levelGrid.Cells[Y, X].GameEntity = null;
        if (destroy)
            Destroy(gameObject);
    }

    protected void CallSync(bool ignoreObjectCollision = false)
    {
        var vector = LookDirection is null ? new Vector2(0, 1) : LookDirection.Vector;
        photonView.RPC("SyncParameters", RpcTarget.AllBuffered, 
            entityId, health, X, Y, (int)vector.x, (int)vector.y, ignoreObjectCollision);
    }

    protected void AdaptTransform(GridCell cell)
    {
        var rt = (RectTransform)transform;
        rt.SetParent(cell.transform);
        rt.sizeDelta = new Vector2(size, size);
        rt.localPosition = Vector3.zero;
    }

    protected void CollideWithCellEntity(GridCell cell, bool ignoreObjectCollision = false)
    {
        var gameEntity = cell.GameEntity;
        if (gameEntity is null) return;
        if (!gameEntity.isBackground && ignoreObjectCollision) return;
        var geType = gameEntity.GetType();
        CollisionInteractions.TryGetValue(geType, out var action);
        if (action is null)
            return;
        action.Invoke(cell.GameEntity);
        PlayAudioClip(collisionClip);
    }

    protected void PlayAudioClip(string clipLink)
    {
        if (clipLink is null || !isSoundOn)
            return;
        soundManager.PlayAudioClip(clipLink);
    }

    #endregion

    #region Private Methods
    private void Update()
    {
        if (levelGrid != null && levelGrid.IsGhostHuntEnabled)
            CheckForGhost();
    }

    public void CheckForGhost()
    {
        if (levelGrid.Cells[Y, X].GameEntity == this)
            return;

        //Debug.LogError($"Ghost caught! Object at [{X},{Y}] did not belong to any cell.");
        levelGrid.Cells[Y, X].GameEntity = this;
    }
    #endregion

    #region IPunObservable Callbacks

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(entityId);
        }
        else
        {
            entityId = (int)stream.ReceiveNext();
        }
    }

    #endregion
}