using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    [SerializeField] private int _levelNr = 0;
    public int LevelNr
    { 
        get { return _levelNr; } 
    }

}
