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

    private AudioSource audiosource;
    private AudioSource oneShotAudio = new AudioSource();

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
        audiosource = GameObject.FindObjectOfType<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void GoHi()
    {
        audiosource.Stop();
        audiosource.clip = LoopHi;
        StartCoroutine(PlayLoopRise());
    }

    public void GoLow()
    {
        audiosource.Stop();
        audiosource.clip = LoopLow;
        StartCoroutine(PlayLoopFall());

    }

    IEnumerator PlayLoopRise()
    {
        oneShotAudio.PlayOneShot(LoopRise);
        yield return new WaitForSeconds(LoopRise.length);
        audiosource.Play();

    }
    IEnumerator PlayLoopFall()
    {
        oneShotAudio.PlayOneShot(LoopFall);
        yield return new WaitForSeconds(LoopFall.length);
        audiosource.Play();

    }



}
