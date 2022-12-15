using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPuzzle : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    public  MeshRenderer GetMeshRenderer()
    {
        return _meshRenderer;
    }

    private int _locationNr = 0;
    public int LocationNr
    { 
        get { return _locationNr; }
        set { _locationNr = value; }
    }

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public bool Compare(BasicPuzzle other)
    {
        if (_meshRenderer.sharedMaterial == other._meshRenderer.sharedMaterial)
        {
            return true;
        }
        else return false;
    }
}
