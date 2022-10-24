using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Text _movesText = null;
    [SerializeField] Text _levelText = null;
    [SerializeField] Text _gameEndText = null;

    private PuzzleManager _puzzle = null;

    void Update()
    {
        // sync data
        if(_movesText && _puzzle)
        {
            _movesText.text = _puzzle.Moves.ToString();
        }
    }

    public void NextLevel(int nrLevel, PuzzleManager newLevel)
    {
        _puzzle = newLevel;
        if(_levelText)
        {
            _levelText.text = (nrLevel + 1).ToString();
        }
    }

    public void GameComplete()
    {
        if (_gameEndText)
        {
            _gameEndText.text = "Game completed!";
        }
    }
    public void GameOver()
    {
        if (_gameEndText)
        {
            _gameEndText.text = "Out of moves \nGame over!";
        }
    }
}
