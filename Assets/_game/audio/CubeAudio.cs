using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAudio : MonoBehaviour {

    private static CubeAudio _instance;
    public static CubeAudio Instance { get { return _instance; } }

    public AudioClip LoopLow;
    public AudioClip LoopHi;
    public AudioClip LoopRise;
    public AudioClip LoopFall;
    public AudioClip Suck;

    public AudioSource audiosource;
    public AudioSource oneShotAudio;
    public AudioSource suckSource;

    private float defaultVolume;

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

    // Use this for initialization
    void Start () {
        defaultVolume = audiosource.volume;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void DefaultTrigger(bool state)
    {
        GoSuck();
    }

    public void GoSuck()
    {
        suckSource.PlayOneShot(Suck);
    }
    public void GoHi()
    {
        audiosource.volume = defaultVolume / 2.0f;
        StartCoroutine(PlayLoopRise());
    }

    public void GoLow()
    {
        audiosource.volume = defaultVolume / 2.0f;
        StartCoroutine(PlayLoopFall());
    }

    IEnumerator PlayLoopRise()
    {
        oneShotAudio.PlayOneShot(LoopRise);
        yield return new WaitForSeconds(LoopRise.length);
        audiosource.Stop();
        audiosource.clip = LoopHi;
        audiosource.volume = defaultVolume;
        audiosource.Play();

    }
    IEnumerator PlayLoopFall()
    {
        oneShotAudio.PlayOneShot(LoopFall);
        yield return new WaitForSeconds(LoopFall.length);
        audiosource.Stop();
        audiosource.clip = LoopLow;
        audiosource.volume = defaultVolume;
        audiosource.Play();

    }



}
