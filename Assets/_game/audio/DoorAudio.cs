using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour {

    private static DoorAudio _instance;
    public static DoorAudio Instance { get { return _instance; } }

    public AudioClip DoorOpen;
    public AudioClip DoorClose;
    public AudioClip InsertKey;

    private AudioSource AudioSourceDoor;

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
        AudioSourceDoor = GameObject.FindObjectOfType<AudioSource>();
    }

    public void Open()
    {
        AudioSourceDoor.Stop();
        AudioSourceDoor.PlayOneShot(DoorOpen);
    }

    public void Close()
    {
        AudioSourceDoor.Stop();
        AudioSourceDoor.PlayOneShot(DoorClose);

    }

    public void Insert()
    {
        AudioSourceDoor.Stop();
        AudioSourceDoor.PlayOneShot(InsertKey);
    }

}
