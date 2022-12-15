using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private bool _puzzleComplete = false;
    public bool PuzzleComplete
    {
        get { return _puzzleComplete; }
    }

    [SerializeField] private int _nrLevel = 1;
    public int NrLevel
    {
        get { return _nrLevel; }
    }

    private PuzzleCircle[] _circles;
    private OverlappingPuzzlePiece[] _overlappingPieces;

    [SerializeField] private int _moves = 4;
    public int Moves
    {
        get { return _moves; }
    }

    [SerializeField] private float _turnSpeed = 45;

    private void Awake()
    {
        _circles = GetComponentsInChildren<PuzzleCircle>();
        _overlappingPieces = GetComponentsInChildren<OverlappingPuzzlePiece>();
    }
    private void Start()
    {
        // check if puzzles started correct
        for (int i = 0; i < _circles.Length; i++)
        {
            if (_circles[i])
            {
                if (_overlappingPieces.Length > 0)
                {
                    foreach (OverlappingPuzzlePiece overlapPiece in _overlappingPieces)
                    {
                        if (overlapPiece != null && (overlapPiece.FirstCircle == i || overlapPiece.SecondCircle == i))
                        {
                            BasicPuzzlePiece piece = overlapPiece.GetPiece(i);
                            if (piece != null)
                                _circles[i].AddPiece(piece);
                        }
                    }
                }

                _circles[i].StartCheck();

                if (_overlappingPieces.Length > 0)
                {
                    for (int p = 0; p < _overlappingPieces.Length; ++p)
                    {
                        if (_overlappingPieces[p] != null && (_overlappingPieces[p].FirstCircle == i || _overlappingPieces[p].SecondCircle == i))
                        {
                            _overlappingPieces[p].SetPiece(i, _circles[i].RemovePiece());
                        }
                    }
                }
            }
        }

        // set turning speed
        if (_circles.Length > 0)
        {
            for (int i = 0; i < _circles.Length; i++)
            {
                _circles[i].SetSpeed(_turnSpeed);
            }
        }

        if (_overlappingPieces.Length > 0)
        {
            for (int i = 0; i < _overlappingPieces.Length; i++)
            {
                _overlappingPieces[i].SetPieceSpeed(_turnSpeed);
            }
        }

    }

    public void UpdatePuzzle()
    {
        // safety checks
        if (_circles.Length == 0)
            return;
        foreach (PuzzleCircle circle in _circles)
        {
            if (circle == null)
                return;
        }

        // update
        if (_puzzleComplete)
            return;

        bool rotating = false;

        for (int i = 0; i < _circles.Length; i++)
        {
            if(_circles[i] && _circles[i].Rotating)
            {
                if (!_circles[i].RotateCircle())     // not done rotating
                    rotating = true;
                else                                // done rotating
                {
                    --_moves;
                    if (CheckPuzzleComplete())
                    {
                        _puzzleComplete = true;
                    }
                    if (_overlappingPieces.Length > 0)
                    {
                        for (int p = 0; p < _overlappingPieces.Length; ++p)
                        {
                            if (_overlappingPieces[p] != null && (_overlappingPieces[p].FirstCircle == i || _overlappingPieces[p].SecondCircle == i))
                            {
                                _overlappingPieces[p].SetPiece(i, _circles[i].RemovePiece());
                            }
                        }
                    }
                }

                break;
            }
        }

        if(!rotating && _moves > 0)
        {
            for (int i = 0; i < _circles.Length; i++)
            {
                if (_circles[i])
                {
                    if (_circles[i].GetInputOnCircle())
                    {
                        if (_overlappingPieces.Length > 0)
                        {
                            foreach (OverlappingPuzzlePiece overlapPiece in _overlappingPieces)
                            {
                                if(overlapPiece != null && (overlapPiece.FirstCircle == i || overlapPiece.SecondCircle == i))
                                {
                                    BasicPuzzlePiece piece = overlapPiece.GetPiece(i);
                                    if (piece != null)
                                        _circles[i].AddPiece(piece);
                                }
                            }
                        }
                        break;
                    }
                    else
                    {
                        _circles[i].CheckParticles();
                    }

                }
            }
        }

    }

    public bool CheckPuzzleComplete()
    {
        if (_circles.Length == 0)
            return false;

        // function

        bool complete = true;
        for (int i = 0; i < _circles.Length; i++)
        {
            if(_circles[i])
            {
                if(!_circles[i].Complete)
                {
                    complete = false;
                }

            }
        }

        return complete;
    }
}
