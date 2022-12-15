using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Text _movesText = null;
    [SerializeField] private Text _levelText = null;
    [SerializeField] private Text _goldText = null;
    [SerializeField] private Text _gameEndText = null;
    [SerializeField] private Image _textBackground = null;

    private PuzzleManager _puzzle = null;
    private PlayerCharacter _player = null;

    private void Awake()
    {
        // cache player
        _player = FindObjectOfType<PlayerCharacter>();
    }
    public void Update()
    {
        // sync data
        if(_movesText && _puzzle)
            _movesText.text = _puzzle.Moves.ToString();

        if (_goldText && _player)
            _goldText.text = _player.Gold.ToString();
    }

    public void NextLevel(int nrLevel, PuzzleManager newLevel)
    {
        _puzzle = newLevel;     // cache current puzzle -> nr moves in update
        if(_levelText)
            _levelText.text = (nrLevel + 1).ToString();     //level index 0 - ... -> display level 1 -> ...+ 1
    }

    public void GameComplete()
    {
        if (_gameEndText)
            _gameEndText.text = "Game completed!\nGold: " + _goldText.text + "\n Replay: press L";

        if(_textBackground)
            _textBackground.enabled = true;
    }
    public void GameOver()
    {
        if (_gameEndText)
            _gameEndText.text = "Out of moves\nRestart level = - 1 gold: press K\n Retry game: press L";

        if (_textBackground)
            _textBackground.enabled = true;
    }
    public void ResetHUD(int nrLevel, PuzzleManager newLevel)
    {
        NextLevel(nrLevel, newLevel);
        Update();

        if (_gameEndText)
            _gameEndText.text = string.Empty;
        if (_textBackground)
            _textBackground.enabled = false;

    }
}
