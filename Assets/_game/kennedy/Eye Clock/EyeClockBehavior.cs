using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeClockBehavior : MonoBehaviour
{
    /// Client API

    public enum State
    {
        Stopped,
        Started,
    }

    public State desiredState;
    public float numOfSecs = 120f;
    public string initText;

    private Text clockText;

    public void StartCountdown()
    {
        desiredState = State.Started;
    }

    public void StopCountdown()
    {
        desiredState = State.Stopped;
    }

    public bool defaultTriggerReverseMode;

    private void DefaultTrigger(bool activate)
    {
        if (defaultTriggerReverseMode)
        {
            if (activate)
                StopCountdown();
            else
                StartCountdown();
        }
        else  // Normal mode
        {
            if (activate)
                StartCountdown();
            else
                StopCountdown();
        }
    }


    /// Callbacks

    private float countdown;

    void Start()
    {
        clockText = GetComponentInChildren<Text>();
        clockText.text = initText;
        countdown = numOfSecs;
    }

    void LateUpdate()
    {
        if (desiredState == State.Started)
        {
            if (countdown < 0f)
            {
                desiredState = State.Stopped;
                return;
            }

            int mins = Mathf.FloorToInt(countdown / 60f);
            int secs = Mathf.FloorToInt(countdown - mins * 60);

            clockText.text = string.Format("{0:00}:{1:00}", mins, secs);

            countdown -= Time.deltaTime;
        }
    }
}
