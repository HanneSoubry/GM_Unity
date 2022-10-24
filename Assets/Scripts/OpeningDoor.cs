using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoor : MonoBehaviour
{
    [SerializeField] private PuzzleManager _puzzle = null;
    [SerializeField] private float _openSpeed = 2;

    [SerializeField] private GameObject _door = null;
    [SerializeField] private GameObject _target = null;

    bool _locked = true;
    bool _opening = false;

    private void FixedUpdate()
    {
        if (_puzzle != null)
        {
            if(_locked && _puzzle.PuzzleComplete())
            {
                _locked = false;
                _opening = true;
            }
        }

        if (_opening && _target != null && _door != null)
        {
            _door.transform.position = Vector3.Lerp(_door.transform.position, _target.transform.position, _openSpeed * Time.deltaTime);
        }
    }
}
