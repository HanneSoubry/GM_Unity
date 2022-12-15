using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddlePuzzleWall : BasicPuzzle
{
    protected float _turnSpeed = 90;  // degrees/second
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
            if (direction == 1)
            {
                --LocationNr;
                if (LocationNr == 0)
                    LocationNr = nrPieces;
            }
            else if (direction == -1)
            {
                ++LocationNr;
                if (LocationNr > nrPieces)
                    LocationNr = 1;
            }
        }

        angle *= direction;
        //transform.Rotate(transform.forward, angle);
        transform.Rotate(Vector3.forward, angle);

        return done;
    }
}
