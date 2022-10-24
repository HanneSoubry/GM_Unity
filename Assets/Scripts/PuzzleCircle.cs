using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCircle : MonoBehaviour
{
    [SerializeField] private GameObject _player = null;
    [SerializeField] private float _maxButtonDistance = 2;
    [SerializeField] private float _turnSpeed = 45;
    [SerializeField] private int _nrPieces = 4;

    private bool _rotating = false;
    public bool Rotating
    {
        get { return _rotating; }
    }
    private bool _complete = false;
    public bool Complete
    {
        get { return _complete; }
    }

    private enum Direction
    {
        left = 1,
        right = -1
    }
    private Direction _direction = Direction.right;

    private BasicPuzzlePiece[] _piecesArray;

    // for overlapping puzzle
    public void AddPiece(BasicPuzzlePiece piece)
    {
        _piecesArray[_nrPieces - 1] = piece;
    }
    public void RemovePiece()
    { 
        _piecesArray[_nrPieces - 1] = null; 
    }

    private BasicPuzzleWall[] _wallsArray;
    private SphereCollider _button; 

    private void Awake()
    {
        _piecesArray = GetComponentsInChildren<BasicPuzzlePiece>();
        _wallsArray = GetComponentsInChildren<BasicPuzzleWall>();

        for (int i = 0; i < _piecesArray.Length; i++)
            _piecesArray[i].TurnSpeed = _turnSpeed;

        _button = GetComponentInChildren<SphereCollider>();
    }

    public bool GetInputOnCircle()
    {
        float distance = 0;
        if (_player != null)
        {
            distance = (_button.transform.position - _player.transform.position).sqrMagnitude;
        }
        if (distance < _maxButtonDistance * _maxButtonDistance)
        {
            if (Input.GetAxis("Action") > 0)
            {
                _rotating = true;
                _direction = Direction.right;
                return true;
            }
            else if (Input.GetAxis("Action") < 0)
            {
                _rotating = true;
                _direction = Direction.left;
                return true;
            }
        }

        return false;
    }

    public bool RotateCircle()  // true when done
    { 
        for(int i = 0; i < _piecesArray.Length; ++i)
        {
            if(_piecesArray[i].Rotate((int)_direction, _nrPieces))  // returns true if done rotating
            {
                 _rotating = false;

                if(i == (_piecesArray.Length - 1))
                {
                    if (CheckPuzzleComplete())
                    {
                        _complete = true;
                    }
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckPuzzleComplete()
    {
        const float margin = 0.01f;

        for(int w = 0; w < _wallsArray.Length; ++w)
        {
            for(int p = 0; p < _piecesArray.Length; ++p)
            {
                Vector3 pieceAngles = _piecesArray[p].transform.localEulerAngles;
                Vector3 wallAngles = _wallsArray[w].transform.localEulerAngles;

                if (Mathf.Abs(pieceAngles.x - wallAngles.x) < margin &&
                    Mathf.Abs(pieceAngles.y - wallAngles.y) < margin &&
                    Mathf.Abs(pieceAngles.z - wallAngles.z) < margin)
                {
                    if (!_piecesArray[p].Compare(_wallsArray[w]))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}

