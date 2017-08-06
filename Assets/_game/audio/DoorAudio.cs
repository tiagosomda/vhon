using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour {

    private static DoorAudio _instance;
    public static DoorAudio Instance { get { return _instance; } }

    public AudioClip DoorOpen;
    public AudioClip DoorClose;

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
        AudioSourceDoor = GetComponent<AudioSource>();
    }

    public void Open()
    {
        AudioSourceDoor.Stop();
        AudioSourceDoor.PlayOneShot(DoorOpen);
        Debug.Log("Playing door audio");
    }

    public void Close()
    {
        AudioSourceDoor.Stop();
        AudioSourceDoor.PlayOneShot(DoorClose);

    }
}
