using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAudio : MonoBehaviour {

    private static SpikeAudio _instance;
    public static SpikeAudio Instance { get { return _instance; } }

    public AudioClip RoofDrop;


    private AudioSource AudioSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void DefaultTrigger(bool state)
    {
        Drop();
    }

    public void Drop()
    {
        AudioSource.Stop();
        AudioSource.PlayOneShot(RoofDrop);
    }
}
