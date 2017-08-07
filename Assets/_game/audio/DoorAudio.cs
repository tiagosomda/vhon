using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour {

    public AudioClip DoorOpen;
    public AudioClip DoorClose;

    private AudioSource AudioSourceDoor;

    private void Awake()
    {
    }

    private void Start()
    {
        AudioSourceDoor = GetComponent<AudioSource>();
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
}
