using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlappingPuzzlePiece : MonoBehaviour
{
    [SerializeField] BasicPuzzlePiece _piece = null;

    [SerializeField] int _firstCircle = 0;
    [SerializeField] int _firstCircleLocNr = 1;
    [SerializeField] GameObject _firstCirclePos = null;
    public int FirstCircle
    { get { return _firstCircle; } }

    [SerializeField] int _secondCircle = 1;
    [SerializeField] int _secondCircleLocNr = 1;
    [SerializeField] GameObject _secondCirclePos = null;
    public int SecondCircle
    { get { return _secondCircle; } }

    private Vector3 _distanceToSecond;

    private void Awake()
    {
        // calculate distance between 2 overlapping circles (centers)
        if(_firstCirclePos != null && _secondCirclePos != null)
        {
            _distanceToSecond = _secondCirclePos.transform.position - _firstCirclePos.transform.position;
        }

        if(_piece != null)
            _piece.LocationNr = _firstCircleLocNr;
    }

    public BasicPuzzlePiece GetPiece(int circleNr)
    {
        if (_piece == null)
            return null;

        if (circleNr == _firstCircle)
        {
            // give piece to first circle
            _piece.LocationNr = _firstCircleLocNr;
            return _piece;
        }
        else if(circleNr == _secondCircle)
        {
            // correct piece location and rotation
            _piece.transform.Rotate(Vector3.up, 180.0f);
            _piece.transform.Rotate(Vector3.right, 180.0f);
            _piece.transform.Translate(_distanceToSecond, Space.World);

            // give piece to second circle
            _piece.LocationNr = _secondCircleLocNr;
            return _piece;
        }

        return null;
    }

    public void SetPiece(int circleNr, BasicPuzzlePiece newPiece)
    {
        // take piece back from circle
        if (circleNr == _firstCircle)
            _piece = newPiece;
        else if (circleNr == _secondCircle)
        {
            // correct piece location and rotation
            newPiece.transform.Translate(-1 * _distanceToSecond, Space.World);                      
            newPiece.transform.Rotate(Vector3.right, -180.0f);
            newPiece.transform.Rotate(Vector3.up, -180.0f);

            _piece = newPiece;
        }
    }

    public void SetPieceSpeed(float turnSpeed)
    {
        _piece.TurnSpeed = turnSpeed;
    }

}
