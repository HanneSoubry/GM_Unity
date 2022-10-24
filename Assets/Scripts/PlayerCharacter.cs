using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    protected MovementBehaviour _movementBehaviour;

    protected virtual void Awake()
    {
        _movementBehaviour = GetComponent<MovementBehaviour>();
        _lastMousePos = Input.mousePosition;
    }

    const string MOVEMENT_HORIZONTAL = "MovementHorizontal";
    const string MOVEMENT_VERTICAL = "MovementVertical";

    // Update is called once per frame
    private void Update()
    {
        HandleMovementInput();
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
