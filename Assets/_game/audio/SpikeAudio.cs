using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAudio : MonoBehaviour {

    public bool skipFirstTime;
    public AudioClip RoofDrop;
    private AudioSource AudioSource;


    private void Awake()
    {
    }

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void DefaultTrigger(bool state)
    {
        if(skipFirstTime)
        {
            skipFirstTime = false;
            return;
        }

        AudioSource.Stop();
        
        if(!state)
            AudioSource.PlayOneShot(RoofDrop);
    }

    public void Drop()
    {

    }
}
