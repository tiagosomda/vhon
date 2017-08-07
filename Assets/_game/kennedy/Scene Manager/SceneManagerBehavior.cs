using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class SceneManagerBehavior : MonoBehaviour
{
    public AudioSource audioSource;
    private float timer;


    public AudioClip introClip;
    public float introWait;
    private bool cpIntroClip;
    private float introClipTime;


    public Camera fadeInCamera;
    public float fadeInDuration;
    public float fadeInWait;
    private bool cpFadeIn;
    private float fadeInTime;


    public AudioClip instructionClip;
    public float instructionWait;
    private bool cpIntruction;
    private float instructionTime;

    public NVRPlayer player;

    private  VHONTeleportation LeftTeleport;
    private  VHONTeleportation RightTeleport;


    public float unlockControlsWait;
    private bool cpUnlockControls;
    private float unlockControlsTime;


//    public AudioClip waitingRoomClip; // reset timer when kicking off this clip
//    public float openDoorTime;


    void Start()
    {
        introClipTime = Mathf.Max(introWait, 0.0001f);

        LeftTeleport = player.LeftHand.GetComponent<VHONTeleportation>();
        RightTeleport = player.RightHand.GetComponent<VHONTeleportation>();

        LeftTeleport.enabled = false;
        RightTeleport.enabled = false;

        Debug.Log("Disabling hands");
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!cpIntroClip && introClipTime != 0 && timer > introClipTime)
        {
            cpIntroClip = true;

            audioSource.PlayOneShot(introClip);
            //Debug.Log("played introClip");

            fadeInTime = timer + introClip.length + fadeInWait;
            return;
        }

        else if (!cpFadeIn && fadeInTime != 0 && timer > fadeInTime)
        {
            cpFadeIn = true;

            StartCoroutine(FadeIn());
            //Debug.Log("started FadeIn");

            instructionTime = timer + fadeInDuration + instructionWait;
            return;
        }

        else if (!cpIntruction && instructionTime != 0 && timer > instructionTime)
        {
            cpIntruction = true;

            audioSource.PlayOneShot(instructionClip);
            //Debug.Log("played instructionClip");

            unlockControlsTime = timer + instructionClip.length + unlockControlsWait;
        }

        
        else if (!cpUnlockControls && unlockControlsTime != 0 && timer > unlockControlsTime)
        {
            cpUnlockControls = true;

        LeftTeleport.enabled = true;
        RightTeleport.enabled = true;
            //Debug.Log("Unlocked controls");
        }
    }


    private IEnumerator FadeIn()
    {
        // TODO: implement me
        //   * use "fadeInCamera" and "fadeInDuration"
        yield return new WaitForEndOfFrame();
    }
	
}
