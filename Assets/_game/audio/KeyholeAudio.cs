using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyholeAudio : MonoBehaviour {

    private static KeyholeAudio _instance;
    public static KeyholeAudio Instance { get { return _instance; } }

    public AudioClip InsertKey;

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
        Insert();
    }

    public void Insert()
    {
        AudioSource.Stop();
        AudioSource.PlayOneShot(InsertKey);
    }

}
