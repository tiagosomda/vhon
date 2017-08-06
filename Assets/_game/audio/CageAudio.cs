using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageAudio : MonoBehaviour {

    private static CageAudio _instance;
    public static CageAudio Instance { get { return _instance; } }

    public AudioClip OpenCage;
    public AudioClip CloseCage;

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
        if (state)
        {
            Open();
        } else
        {
            Close();
        }
    }

    public void Open()
    {
        AudioSource.Stop();
        AudioSource.PlayOneShot(OpenCage);
    }
    public void Close()
    {
        AudioSource.Stop();
        AudioSource.PlayOneShot(CloseCage);
    }
}
