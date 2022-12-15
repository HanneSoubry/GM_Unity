using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] private int _worth = 1;
    [SerializeField] private float _rotationSpeed = 1;
    [SerializeField] private int _level = 0;
    [SerializeField] private bool _finalTreasure = false;

    private bool _collected = false;
    private bool _destroyed = false;

    AudioSource _audioSource;

    // get & set private variables
    public int Level
    { get { return _level; } }  
    public bool Collected
    { set { _collected = value; } }
    public bool Destroyed
    { get { return _destroyed; } set { _destroyed = value; } }
    public int Worth
    { get { return _worth; } }
    public bool Final
    { get { return _finalTreasure; } }

    
    public void Awake()
    {
        // cashe audio
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_destroyed)
        {   
            // levelManager will set this true if reset in higher level,
            // destroy this in first next update
            Destroy(this.gameObject);
        }

        // rotate object
        float angle = _rotationSpeed * Time.deltaTime;
        transform.Rotate(transform.up, angle);

        if(_collected)
        {
            if(_audioSource.isPlaying == false)
            {
                // make invisible & no collision
                MeshRenderer[] meshes = this.gameObject.GetComponentsInChildren<MeshRenderer>();
                foreach(MeshRenderer mesh in meshes)
                {
                    mesh.enabled = false;
                }
                this.gameObject.GetComponent<Collider>().enabled = false;

                // play sound and destroy with delay (when sound is done playing)
                _audioSource.Play();
                Destroy(this.gameObject, 1.5f);
            }
        }
    }
}
