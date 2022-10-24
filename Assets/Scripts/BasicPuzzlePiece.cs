using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPuzzlePiece : BasicPuzzle
{
    protected float _turnSpeed = 45;  // degrees/second
    public float TurnSpeed
    {
        get { return _turnSpeed; }
        set { _turnSpeed = value; }
    }

    private float _angleDone = 0;

    // functions
    public bool Rotate(int direction, int nrPieces)
    {
        bool done = false;
        float pieceSize = 360 / nrPieces;   // angleSize of one piece
        float angle = _turnSpeed * Time.deltaTime;
        _angleDone += angle;

        if (_angleDone > pieceSize)
        {
            angle -= (_angleDone - pieceSize);  // eventueel teveel aftrekken
            _angleDone = 0;
            done = true;
        }

        angle *= direction;
        transform.Rotate(transform.forward, angle);

        return done;
    }
}
