using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroHUD : MonoBehaviour
{
    [SerializeField] private Text _middleText = null;
    private enum State
    { templeFound, goldQuest, moves }

    private State state = State.templeFound;

    private void Awake()
    {
        if(_middleText != null)
        {
            // intro
            _middleText.text = "You finally found the ancient temple!"; 
        }
    }

    public void OnButtonPress()
    {
        switch (state)
        {
            case State.templeFound:
            {
                if (_middleText != null)
                {
                    _middleText.text = "How much gold can you collect?";
                }
                state = State.goldQuest;
                break;
            }
            case State.goldQuest:
            {
                if (_middleText != null)
                {
                    _middleText.text = "Watch your moves...\nRestarting a level costs a part of your treasure.";
                }
                state = State.moves;
                break;
            }
            case State.moves:
                SceneManager.LoadScene("Game");     // start game
                break;
        }
    }
}
