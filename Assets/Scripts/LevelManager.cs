using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static bool _created = false;
    private static bool _fullReset = false;
    const string RESET_BUTTON = "Reset";

    [SerializeField] private int _startLevel = 0;
    [SerializeField] private int _endLevel = 0;
    [SerializeField] private GameObject _playerPositions = null;
    private PlayerCharacter _player = null;

    private HUD _HUD = null;
    private int _savedGold = 0;
    private PuzzleManager[] _puzzles;
    private static int _currentLevel;

    void Start()
    {
        // initialize cache
        _puzzles = FindObjectsOfType<PuzzleManager>();
        SortLevels();       // -> array index == levelNr

        _HUD = FindObjectOfType<HUD>();

        if (!_created || _fullReset)
        {
            // reset levelNr
            _currentLevel = _startLevel;

            _created = true;
            _fullReset = false;
        }
        else
        {
            // destroy gold bars from previous levels
            Gold[] goldBars = FindObjectsOfType<Gold>();
            foreach (Gold g in goldBars)
            {
                if (g.Level <= _currentLevel)
                {
                    g.Destroyed = true;
                }
            }
        }

        // set HUD
        if (_HUD && _puzzles[_currentLevel])
        {
            _HUD.ResetHUD(_currentLevel, _puzzles[_currentLevel]);
        }

        _player = FindObjectOfType<PlayerCharacter>();

        // return saved value
        if (_player != null)
            _player.Gold = _savedGold;

        // set player & camera position, depending on levelNr
        if (_playerPositions != null && _player)
        {
            PlayerPosition[] positions = _playerPositions.GetComponentsInChildren<PlayerPosition>();
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i].LevelNr == _currentLevel)
                {
                    _player.transform.position = positions[i].transform.position;
                    _player.transform.rotation = positions[i].transform.rotation;

                    CameraBehaviour camera = FindObjectOfType<CameraBehaviour>();
                    camera.transform.position = positions[i].transform.position;
                    camera.transform.rotation = positions[i].transform.rotation;
                }
            }
        }
    }

    void Update()
    {
        // input for reset
        if (Input.GetAxis(RESET_BUTTON) > 0)
        {
            _savedGold = _player.GoldForReset(1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (Input.GetAxis(RESET_BUTTON) < 0)
        {
            _fullReset = true;
            _savedGold = _player.Gold = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // update current puzzle
        _puzzles[_currentLevel].UpdatePuzzle();

        // check completion
        if (_puzzles[_currentLevel].PuzzleComplete == true)
        {
            if(_currentLevel == _endLevel)
            {
                // final level completed,
                // wait for player to collect final reward
                if(_player.FinalTreasureCollected == true)
                    _HUD.GameComplete();
            }
            else
            {
                // next level
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
            while(_puzzles[i].NrLevel != i)
            {
                SwapPuzzles(i, _puzzles[i].NrLevel);
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
