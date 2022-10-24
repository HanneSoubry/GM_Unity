using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private int _nrLevel = 1;
    public int NrLevel
    {
        get { return _nrLevel; }
    }
    private PuzzleCircle[] _circles;

    [SerializeField] private int _moves = 4;
    public int Moves
    {
        get { return _moves; }
    }

    private void Awake()
    {
        _circles = GetComponentsInChildren<PuzzleCircle>();
    }

    public void UpdatePuzzle()
    {
        if (PuzzleComplete())
            return;

        bool rotating = false;

        for (int i = 0; i < _circles.Length; i++)
        {
            if(_circles[i] && _circles[i].Rotating)
            {
                if(!_circles[i].RotateCircle())
                    rotating = true;

                break;
            }
        }

        if(!rotating)
        {
            for (int i = 0; i < _circles.Length; i++)
            {
                if (_circles[i])
                {
                    if (_circles[i].GetInputOnCircle())
                    {
                        // TODO: PuzzleManager: add piece to overlapping circle
                        break;
                    }

                }
            }
        }

    }

    public bool PuzzleComplete()
    {
        for(int i = 0; i < _circles.Length; i++)
        {
            if(_circles[i])
            {
                if(!_circles[i].Complete)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
