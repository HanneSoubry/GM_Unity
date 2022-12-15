using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoor : MonoBehaviour
{
    [SerializeField] private PuzzleManager _puzzle = null;
    [SerializeField] private float _openSpeed = 2;

    [SerializeField] private GameObject _door = null;
    [SerializeField] private GameObject _target = null;
    AudioSource _audioSource;
    private bool _playAudio = false;
    private float _playTime = 0;
    private float _stopTime = 2.0f;

    bool _locked = true;
    bool _opening = false;

    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (_puzzle != null)
        {
            if(_locked && _puzzle.PuzzleComplete)
            {
                // start opening door
                _locked = false;
                _opening = true;
            }
        }

        if (_opening && _target != null && _door != null)
        {
            _playAudio = true;
            _door.transform.position = Vector3.Lerp(_door.transform.position, _target.transform.position, _openSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        if (_playAudio)
        {
            // handle audio
            if(_audioSource.isPlaying == false)
                _audioSource.Play();
            else
            {
                // stop audio after some time
                _playTime += Time.deltaTime;
                if(_playTime > _stopTime)
                {
                    _audioSource.Stop();
                    _playAudio = false;
                }
            }
        }

    }
}
