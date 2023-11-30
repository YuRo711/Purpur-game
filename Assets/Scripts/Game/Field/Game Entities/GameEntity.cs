using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public abstract class GameEntity : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Serializable Fields

    [SerializeField] protected int entityId;
    [SerializeField] protected int health = 1;
    [SerializeField] protected int size = 100;
    [SerializeField] protected Vector2 moveVector;
    [SerializeField] protected GameGrid levelGrid;
    [SerializeField] protected EnemyManager enemyManager;
    [SerializeField] public bool isBackground;

    #endregion
    
    #region Properties

    public LevelManager LevelManager { get; set; }
    public Direction LookDirection { get; set; }
    [SerializeField] public int X { get; protected set; }
    [SerializeField] public int Y { get; protected set; }

    #endregion

    #region Interactions

    protected Dictionary<Type, Action<GameEntity>> CollisionInteractions = new ();
    
    protected void DamageEntity(GameEntity gameEntity, int damage, int damageToSelf = 0)
    {
        gameEntity.TakeDamage(damage);
        if (damageToSelf != 0)
            TakeDamage(damageToSelf);
    }

    #endregion
    
    #region Public Methods
    
    // For movement in some direction
    public void MoveInDirection(TurnDirections moveDir, int speed = 1)
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
    public virtual void MoveTo(int destX, int destY, bool callSync = true)
    {
        if (levelGrid.CheckForBorder(destX, destY))
            return;
        DeleteFromCell(entityId);
        X = destX;
        Y = destY;
        var newCell = levelGrid.Cells[Y, X];
        AdaptTransform(newCell);
        CollideWithCellEntity(newCell);
        if (isBackground)
            newCell.BgEntity = this;
        else
            newCell.GameEntity = this;
        if (callSync)
            CallSync();
    }

    public virtual void TurnTo(TurnDirections turnDirections, bool callSync = true)
    {
        LookDirection = LookDirection.TurnTo(turnDirections);
        if (callSync)
            CallSync();
    }

    public void TakeDamage(int damage)
    {
        Debug.LogError(name + " took damage");
        health -= damage;
        CallSync();
    }
    
    public virtual void Die()
    {
        PhotonNetwork.Destroy(gameObject);
        photonView.RPC("DeleteFromCell", RpcTarget.AllBuffered, entityId);
    }
    
    public virtual void SetStartParameters(int id)
    {
        entityId = id;
        photonView.RPC("SyncStart", RpcTarget.AllBuffered, entityId);
    }

    [PunRPC]
    public virtual void SyncStart(int id)
    {
        if (entityId != id)
            return;
        levelGrid = LevelManager.levelGrid;
        enemyManager = LevelManager.enemyManager;
        if (this is PlayerShip playerShip)
            LevelManager.controlPanelGenerator.ConnectToPlayer(playerShip);
        LookDirection = new Direction(0, 1);
    }

    [PunRPC]
    public void SyncParameters(int id, int newHealth, int newX, int newY, int turnX, int turnY)
    {
        if (entityId != id)
            return;
        health = newHealth;
        if (health <= 0)
            Die();
        MoveTo(newX, newY, false);
        LookDirection = new Direction(turnX, turnY);
        TurnTo(TurnDirections.Forward, false);
    }

    #endregion

    #region Protected Methods

    [PunRPC]
    protected void DeleteFromCell(int id)
    {
        if (entityId != id)
            return;
        if (isBackground)
            levelGrid.Cells[Y, X].BgEntity = null;
        else
            levelGrid.Cells[Y, X].GameEntity = null;
    }

    protected void CallSync()
    {
        var vector = LookDirection is null ? new Vector2(0, 1) : LookDirection.Vector;
        photonView.RPC("SyncParameters", RpcTarget.AllBuffered, 
            entityId, health, X, Y, (int)vector.x, (int)vector.y);
    }

    protected void AdaptTransform(GridCell cell)
    {
        var rt = (RectTransform)transform;
        rt.SetParent(cell.transform);
        rt.sizeDelta = new Vector2(size, size);
        rt.localPosition = Vector3.zero;
    }

    protected void CollideWithCellEntity(GridCell cell)
    {
        var gameEntity = cell.GameEntity;
        if (gameEntity is null) return;
        var geType = gameEntity.GetType();
        CollisionInteractions.TryGetValue(geType, out var action);
        if (action is null)
            return;
        action.Invoke(cell.GameEntity);
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