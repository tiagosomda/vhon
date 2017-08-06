using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapBehavior : MonoBehaviour
{
    /// Client API

    public enum State
    {
        Closed,
        Open,
    }

    public State desiredState;
    public GameObject moveObject;
    public float transitionSecs = 15;
    public Vector3 closedPosition;
    public Vector3 openPosition;

    private Coroutine mover;

    public void Open()
    {
        desiredState = State.Open;

        if (mover != null)
        {
            StopCoroutine(mover);
        }

        mover = StartCoroutine(DoMove());

    }

    public void Close()
    {
        desiredState = State.Closed;

        if (mover != null)
        {
            StopCoroutine(mover);
        }

        mover = StartCoroutine(DoMove());
    }


    /// Callbacks
	
    private Vector3 desiredPosition;

    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (desiredState == State.Closed)
                Open();
            else
                Close();
        }
    }
    */

    public bool defaultTriggerReverseMode;

    private void DefaultTrigger(bool activate)
    {
        if (defaultTriggerReverseMode)
        {
            if (activate)
                Close();
            else
                Open();
        }
        else  // Normal mode
        {
            if (activate)
                Open();
            else
                Close();
        }
    }

    private IEnumerator DoMove()
    {
        var travelPercentage = 0f;
        Vector3 startPos = moveObject.transform.localPosition;
        Vector3 movePos;

        var speed = 1 / transitionSecs;
        desiredPosition = DetermineDesiredPosition();
        travelPercentage = 0f;

        while (travelPercentage < 1f)
        {
            yield return new WaitForEndOfFrame();

            travelPercentage += Time.deltaTime * speed;

            movePos = Vector3.Lerp(startPos, desiredPosition, travelPercentage);
            moveObject.transform.localPosition = movePos;
        }
    }


    /// Helpers

    private Vector3 DetermineDesiredPosition()
    {
        return (desiredState == State.Closed) ? closedPosition : openPosition;
    }
}
