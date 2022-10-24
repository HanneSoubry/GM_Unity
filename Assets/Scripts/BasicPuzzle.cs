using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPuzzle : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public bool Compare(BasicPuzzle other)
    {
        if (_meshRenderer.sharedMaterial.GetColor("_Color") == other._meshRenderer.sharedMaterial.GetColor("_Color"))
        {
            return true;
        }
        else return false;
    }
}
