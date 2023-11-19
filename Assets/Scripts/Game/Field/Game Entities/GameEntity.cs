using System;
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
        var borderCheck = CheckForBorder(destX, destY);
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
    public void SyncStart(int id)
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
    
    protected bool CheckForBorder(int newX, int newY)
    {
        return newX >= levelGrid.width || newX < 0 ||
               newY >= levelGrid.height || newY < 0;
    }

    protected void CallSync()
    {
        var vector = LookDirection is null ? new Vector2(0, 1) : LookDirection.Vector;
        photonView.RPC("SyncParameters", RpcTarget.AllBuffered, 
            entityId, health, x, y, (int)vector.x, (int)vector.y);
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