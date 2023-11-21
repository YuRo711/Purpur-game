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
    [SerializeField] protected int x;
    [SerializeField] protected int y;
    [SerializeField] protected int health = 1;
    [SerializeField] protected int size = 100;

    #endregion
    
    #region Properties

    public GameGrid levelGrid;
    public EnemyManager enemyManager;
    public Direction LookDirection { get; set; }

    #endregion

    #region Interactions

    protected Dictionary<Type, Action<GameEntity>> CollisionInteractions;
    
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
        var newX = x;
        var newY = y;
        var moveVector = LookDirection.TurnTo(moveDir).Vector;
        newX += (int)moveVector.x * speed;
        newY += (int)moveVector.y * speed;
        MoveTo(newX, newY);
        CallSync();
    }

    // For precise movement: warp, etc.
    public virtual void MoveTo(int destX, int destY, bool callSync = true)
    {
        var borderCheck = levelGrid.CheckForBorder(destX, destY);
        if (borderCheck)
            return;
        
        x = destX;
        y = destY;
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
        Debug.Log(name + " took " + damage + " damage");
        health -= damage;
        if (health <= 0)
            Die();
        CallSync();
    }
    
    public virtual void Die()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    
    public virtual void SetStartParameters(int id)
    {
        entityId = id;
        photonView.RPC("SyncStart", RpcTarget.AllBuffered, entityId);
    }

    // That looks bad. Rewrite it once you have free time, Y
    [PunRPC]
    public virtual void SyncStart(int id)
    {
        Debug.LogError(id + " " + entityId);
        if (entityId != id)
            return;
        levelGrid = FindObjectOfType<GameGrid>();
        enemyManager = FindObjectOfType<EnemyManager>();
        if (this is PlayerShip playerShip)
            FindObjectOfType<ControlPanelGenerator>()
                .ConnectToPlayer(playerShip);
        MoveTo(x, y);
        transform.localScale = new Vector3(1, 1, 1);
        LookDirection = new Direction(0, 1);
    }

    [PunRPC]
    public void SyncParameters(int id, int newHealth, int newX, int newY, int turnX, int turnY)
    {
        if (entityId != id)
            return;
        health = newHealth;
        MoveTo(newX, newY, false);
        LookDirection = new Direction(turnX, turnY);
        TurnTo(TurnDirections.Forward, false);
    }

    #endregion

    #region Protected Methods

    protected void CallSync()
    {
        var vector = LookDirection is null ? new Vector2(0, 1) : LookDirection.Vector;
        photonView.RPC("SyncParameters", RpcTarget.AllBuffered, 
            entityId, health, x, y, (int)vector.x, (int)vector.y);
    }

    protected void AdaptTransform(GridCell cell)
    {
        var rt = (RectTransform)transform;
        rt.SetParent(cell.transform);
        rt.sizeDelta = new Vector2(size, size);
        rt.localPosition = Vector3.zero;
    }
    
    protected Action<GameEntity> GetInteraction(GridCell cell, 
        Dictionary<Type, Action<GameEntity>> dictionary)
    {
        if (cell.GameEntity is null)
            return null;
        var gameEntity = cell.GameEntity;
        var geType = gameEntity.GetType();
        dictionary.TryGetValue(geType, out var action);
        return action;
    }

    protected void InteractWithCellEntity(GridCell cell,
        Dictionary<Type, Action<GameEntity>> dictionary)
    {
        var action = GetInteraction(cell, dictionary);
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