using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    protected MovementBehaviour _movementBehaviour;
    protected Collider _collider;
    protected virtual void Awake()
    {
        // cache components
        _movementBehaviour = GetComponent<MovementBehaviour>();
        _lastMousePos = Input.mousePosition;
        _collider = GetComponent<Collider>();
    }

    private int _collectedGold = 0;
    private bool _finalTreasureCollected = false;
    public int Gold
    {
        get { return _collectedGold; }
        set { _collectedGold = value; }
    }
    public bool FinalTreasureCollected
    {
        get { return _finalTreasureCollected; }
    }


    public int GoldForReset(int penalty)
    {
        // pass nr to levelmanager, save for scene reset
        return _collectedGold - penalty;
    }

    const string MOVEMENT_HORIZONTAL = "MovementHorizontal";
    const string MOVEMENT_VERTICAL = "MovementVertical";

    // Update is called once per frame
    private void Update()
    {
        HandleMovementInput();
    }

    const string TAG_GOLD = "Gold";
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == TAG_GOLD)
        {
            Gold gold = collision.gameObject.GetComponent<Gold>();
            if (gold.Destroyed == false)    // gold not about to be destroyed
            {
                _collectedGold += gold.Worth;
                gold.Collected = true;
                if(gold.Final == true)
                {
                    _finalTreasureCollected = true;
                }
            }
        }
    }

    private Vector3 _lastMousePos = Vector3.zero;
    [SerializeField] private float _turnSpeed = 1 ; 

    void HandleMovementInput()
    {
        if (_movementBehaviour == null)
            return;

        // movement
        float horizontalMovement = Input.GetAxis(MOVEMENT_HORIZONTAL);
        float verticalMovement = Input.GetAxis(MOVEMENT_VERTICAL);

        Vector3 movement = horizontalMovement * transform.right + verticalMovement * transform.forward;

        _movementBehaviour.DesiredMovementDirection = movement;

        // rotation
        if(Input.GetMouseButton(0))     // left mouse button down
        {
            Vector3 mouseDelta = Input.mousePosition - _lastMousePos;
            _movementBehaviour.TurnAngle = _turnSpeed * mouseDelta.x;
        }
        else
        {
            _movementBehaviour.TurnAngle = 0;
        }

        _lastMousePos = Input.mousePosition;
    }
}
