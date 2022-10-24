using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlappingPuzzlePiece : BasicPuzzlePiece
{
    [SerializeField] BasicPuzzlePiece _piece = null;
    [SerializeField] int _firstCircle = 1;
    [SerializeField] int _secondCircle = 2;

    private Transform _initialTransform;



    private void Awake()
    {
        if( _piece != null )
            _initialTransform = _piece.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
