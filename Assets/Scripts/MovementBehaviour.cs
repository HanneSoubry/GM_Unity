using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 1.0f;

    private Rigidbody _rigidBody;

    private Vector3 _desiredMovementDirection = Vector3.zero;
    public Vector3 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value; }
    }

    private float _turnAngle = 0;
    public float TurnAngle
    {
        get { return _turnAngle; }
        set { _turnAngle = value; }
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 movement = _desiredMovementDirection.normalized;
        movement *= _movementSpeed;

        _rigidBody.velocity = movement;
    }

    private void HandleRotation()
    {
        transform.Rotate(Vector3.up, _turnAngle);
    }
}
