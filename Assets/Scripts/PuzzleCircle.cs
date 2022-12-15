using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCircle : MonoBehaviour
{
    [SerializeField] private GameObject _player = null;
    [SerializeField] private float _maxButtonDistance = 2;
    [SerializeField] private int _nrPieces = 4;
    [SerializeField] private GameObject _firstPiecePosition = null;

    [SerializeField] private Material _neutralMaterial;
    [SerializeField] private ParticleSystem _particlesOrange = null;
    [SerializeField] private ParticleSystem _particlesGreen = null;

    private enum Particle { orange, green, none }
    private Particle _currentParticle;

    const string ACTION_BUTTON = "Action";

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
        left = -1,
        right = 1
    }
    private Direction _direction = Direction.right;

    private BasicPuzzlePiece[] _piecesArray;

    // for overlapping puzzle
    int _overlapLocation = 0;
    public void AddPiece(BasicPuzzlePiece piece)
    {
        _piecesArray[_nrPieces - 1] = piece;
        _overlapLocation = piece.LocationNr;
    }
    public BasicPuzzlePiece RemovePiece()
    { 
        // return piece to overlap object
        BasicPuzzlePiece pieceToReturn = null;
        for(int i = 0; i < _nrPieces; i++)
        {
            if(_piecesArray[i].LocationNr == _overlapLocation)
            {
                pieceToReturn = _piecesArray[i];
                _piecesArray[i] = _piecesArray[_nrPieces - 1];
                break;
            }
        }
        _piecesArray[_nrPieces - 1] = null;
        return pieceToReturn;
    }
    // end overlapping puzzle code

    private BasicPuzzleWall[] _wallsArray;
    private MiddlePuzzleWall[] _middleWallsArray;
    private SphereCollider _button;

    private void Awake()
    {
        // cache pieces
        _piecesArray = new BasicPuzzlePiece[_nrPieces];
        BasicPuzzlePiece[] getPieces = GetComponentsInChildren<BasicPuzzlePiece>();

        for (int i = 0; i < getPieces.Length; i++)
        {
            _piecesArray[i] = getPieces[i];
        }

        // add empty spaces in array for overlapping pieces
        int difference = _piecesArray.Length - getPieces.Length;
        if (difference > 0)
        {
            for (int i = _piecesArray.Length - difference; i < _piecesArray.Length; i++)
            {
                _piecesArray[i] = null;
            }
        }

        // cache components
        _wallsArray = GetComponentsInChildren<BasicPuzzleWall>();
        _middleWallsArray = GetComponentsInChildren<MiddlePuzzleWall>();

        _button = GetComponentInChildren<SphereCollider>();

        // initialize component locationNrs
        InitializeLocationNumbers(_piecesArray);
        _firstPiecePosition.transform.Rotate(Vector3.forward, 360.0f / _nrPieces);
        InitializeLocationNumbers(_wallsArray);
        _firstPiecePosition.transform.Rotate(Vector3.forward, 360.0f / _nrPieces);
        InitializeLocationNumbers(_middleWallsArray);

        // button particle effect (makes button more visible)
        StartParticles(Particle.orange);
    }

    public void StartCheck()
    {
        if (CheckPuzzleComplete())
            _complete = true;
    }

    private void InitializeLocationNumbers(BasicPuzzle[] pieces)
    {
        // find first piece
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
            {
                Vector3 difference = pieces[i].transform.localEulerAngles - _firstPiecePosition.transform.localEulerAngles;
                difference.x = Mathf.Abs(difference.x);
                difference.y = Mathf.Abs(difference.y);
                difference.z = Mathf.Abs(difference.z);

                if (difference.x <= 0.01f && difference.y <= 0.01f && difference.z <= 0.01f)
                {
                    pieces[i].LocationNr = 1;
                    break;
                }
            }
        }

        // next pieces
        int nextPieceNr = 2;
        while (nextPieceNr <= pieces.Length)
        {
            _firstPiecePosition.transform.Rotate(Vector3.forward, 360.0f / _nrPieces);

            for (int i = 0; i < pieces.Length; i++)
            {
                if (pieces[i] != null)
                {
                    Vector3 difference = pieces[i].transform.localEulerAngles - _firstPiecePosition.transform.localEulerAngles;
                    difference.x = Mathf.Abs(difference.x);
                    difference.y = Mathf.Abs(difference.y);
                    difference.z = Mathf.Abs(difference.z);

                    if (difference.x <= 0.01f && difference.y <= 0.01f && difference.z <= 0.01f)
                    {
                        pieces[i].LocationNr = nextPieceNr;
                        break;
                    }
                }
            }
            nextPieceNr++;
        }

        // skip empty spaces in array (if overlapping)
        while (nextPieceNr <= _nrPieces)
        {
            _firstPiecePosition.transform.Rotate(Vector3.forward, 360.0f / _nrPieces);
            ++nextPieceNr;
        }
    }

    public bool GetInputOnCircle()
    {
        float distance = 0;
        if (_player != null)
        {
            distance = (_button.transform.position - _player.transform.position).sqrMagnitude;      // squared distance
        }

        // if close enough
        if (distance < _maxButtonDistance * _maxButtonDistance)
        {
            if (Input.GetAxis(ACTION_BUTTON) > 0)
            {
                // turn right
                _rotating = true;
                _direction = Direction.right;
                StopParticles();
                return true;
            }
            else if (Input.GetAxis(ACTION_BUTTON) < 0)
            {
                // turn left
                _rotating = true;
                _direction = Direction.left;
                StopParticles();
                return true;
            }
        }

        return false;
    }

    public bool RotateCircle()  // true when done
    {
        // rotate middle
        for (int i = 0; i < _middleWallsArray.Length; ++i)
        {
            _middleWallsArray[i].Rotate((int)_direction, _nrPieces);
        }

        // rotate pieces
        for (int i = 0; i < _piecesArray.Length; ++i)
        {
            if(_piecesArray[i].Rotate((int)_direction, _nrPieces)) 
            {     
                // returns true if done rotating
                _rotating = false;

                if(i == (_piecesArray.Length - 1))
                {
                    if (CheckPuzzleComplete())
                    {
                        _complete = true;
                    }
                    else _complete = false;
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckPuzzleComplete()
    {
        if (_wallsArray.Length == 0)
            return false;
        if (_piecesArray.Length == 0)
            return false;

        for (int w = 0; w < _wallsArray.Length; ++w)
        {
            if (_wallsArray[w].GetMeshRenderer().sharedMaterial == _neutralMaterial)
                continue;

            for (int p = 0; p < _piecesArray.Length; ++p)
            {
                if (_piecesArray[p].GetMeshRenderer().sharedMaterial == _neutralMaterial)
                    continue;

                if (_piecesArray[p].LocationNr == _wallsArray[w].LocationNr)
                {
                    if (!_piecesArray[p].Compare(_wallsArray[w]))
                    {
                        return false;
                    }

                    break;
                }
            }
        }

        if(_middleWallsArray.Length > 0)
        {
            for (int w = 0; w < _middleWallsArray.Length; ++w)
            {
                if (_middleWallsArray[w].GetMeshRenderer().sharedMaterial == _neutralMaterial)
                    continue;

                for (int p = 0; p < _piecesArray.Length; ++p)
                {
                    if (_piecesArray[p].LocationNr == _middleWallsArray[w].LocationNr)
                    {
                        if (!_piecesArray[p].Compare(_middleWallsArray[w]))
                        {
                            return false;
                        }

                        break;
                    }
                }
            }
        }
        

        return true;
    }

    public void SetSpeed(float turnSpeed)
    {
        if (_piecesArray.Length > 0)
        {
            for (int i = 0; i < _piecesArray.Length; i++)
            {
                if (_piecesArray[i] != null)
                    _piecesArray[i].TurnSpeed = turnSpeed;
            }
        }

        if (_middleWallsArray.Length > 0)
        {
            for (int i = 0; i < _middleWallsArray.Length; i++)
            {
                if (_middleWallsArray[i] != null)
                    _middleWallsArray[i].TurnSpeed = turnSpeed;
            }
        }
    }

    private void StartParticles(Particle particleType)
    {
        if (particleType == Particle.orange)
        {
            _particlesGreen.Stop();
            _particlesOrange.Play();
            _currentParticle = Particle.orange;
        }
        else if(particleType == Particle.green)
        {
            _particlesOrange.Stop();
            _particlesGreen.Play();
            _currentParticle = Particle.green;
        }
    }

    private void StopParticles()
    {
        _particlesOrange.Stop();
        _particlesGreen.Stop();
        _currentParticle = Particle.none;
    }

    public void CheckParticles()
    {
        float distance = 0;
        if (_player != null)
        {
            distance = (_button.transform.position - _player.transform.position).sqrMagnitude;  // squared distance
        }


        if (_currentParticle != Particle.green && distance < _maxButtonDistance * _maxButtonDistance)
        {
            StartParticles(Particle.green);
        }
        else if(_currentParticle != Particle.orange && distance > _maxButtonDistance * _maxButtonDistance)
        {
            StartParticles(Particle.orange);
        }
    }
}

