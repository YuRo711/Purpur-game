using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public abstract class GameEntity : MonoBehaviour, IPunObservable
{
    #region Serializable Fields

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
    }

    // For precise movement: warp, etc.
    public virtual void MoveTo(int destX, int destY)
    {
        var borderCheck = CheckForBorder(destX, destY);
        if (borderCheck)
            return;
        
        x = destX;
        y = destY;
    }

    public virtual void TurnTo(TurnDirections turnDirections)
    {
        LookDirection = LookDirection.TurnTo(turnDirections);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(name + " took " + damage + " damage");
        health -= damage;
        if (health <= 0)
            Die();
    }
    
    public virtual void Die()
    {
        Destroy(gameObject);
    }
    
    public virtual void SetStartParameters()
    {
        Debug.LogError("moving to start");
        MoveTo(x, y);
        transform.localScale = new Vector3(1, 1, 1);
    }

    #endregion

    #region Protected Methods
    
    protected bool CheckForBorder(int newX, int newY)
    {
        return newX >= levelGrid.width || newX < 0 ||
               newY >= levelGrid.height || newY < 0;
    }

    #endregion

    #region IPunObservable Callbacks

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(x);
            stream.SendNext(y);
            stream.SendNext(LookDirection is null ?
                new Vector2(0, 1) :
                LookDirection.Vector);
        }
        else
        {
            health = (int)stream.ReceiveNext();
            x = (int)stream.ReceiveNext();
            y = (int)stream.ReceiveNext();
            LookDirection = new Direction((Vector2)stream.ReceiveNext());
        }
    }

    #endregion
    
    #region MonoBehaviour Callbacks

    private void Awake()
    {
        LookDirection = new Direction(0, 1);
        Debug.LogError(LookDirection.Vector);
    }

    #endregion
}