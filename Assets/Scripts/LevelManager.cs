using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private HUD _HUD = null;
    private PuzzleManager[] _puzzles;
    private int _currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        _currentLevel = 0;
        _puzzles = FindObjectsOfType<PuzzleManager>();
        SortLevels();
        _HUD = FindObjectOfType<HUD>();

        if(_HUD && _puzzles[_currentLevel])
        {
            _HUD.NextLevel(_currentLevel, _puzzles[_currentLevel]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _puzzles[_currentLevel].UpdatePuzzle();

        if (_puzzles[_currentLevel].PuzzleComplete() == true)
        {
            if(_currentLevel == _puzzles.Length - 1)
            {
                _HUD.GameComplete();
            }
            else
            {
                ++_currentLevel;
                _HUD.NextLevel(_currentLevel, _puzzles[_currentLevel]);
            }
        }
        else if(_puzzles[_currentLevel].Moves == 0)
        {
            _HUD.GameOver();
        }
    }

    private void SortLevels()
    {
        for(int i = 0; i < _puzzles.Length; i++)
        {
            while(_puzzles[i].NrLevel != i + 1)
            {
                SwapPuzzles(i, _puzzles[i].NrLevel - 1);
            }
        }
    }

    private void SwapPuzzles(int first, int second)
    {
        PuzzleManager puzzle = _puzzles[first];
        _puzzles[first] = _puzzles[second];
        _puzzles[second] = puzzle;
    }
}
