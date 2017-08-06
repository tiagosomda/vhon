using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class ForceFieldCageBehavior : MonoBehaviour
{
    /// Client API

    public enum State
    {
        Closed,
        Open,
    }

    public NVRInteractableItem key;
    public BoxCollider keyCollider;
    public State desiredState = State.Closed;
    public GameObject moveObject;
    public float transitionSecs = 3;

    private Coroutine mover;

    void Start()
    {
        key.enabled = false;
        keyCollider = key.gameObject.GetComponent<BoxCollider>();
        keyCollider.enabled = false;
    }

    public void Open()
    {
        key.enabled = true;
        keyCollider.enabled = true;
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

    private void DefaultTrigger(bool activate)
    {
        if (activate)
            Open();
        else
            Close();
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

    private Vector3 closedPosition = new Vector3(0, 2f, 0);
    private Vector3 openPosition = new Vector3(0, .5f, 0);

    private Vector3 DetermineDesiredPosition()
    {
        return (desiredState == State.Closed) ? closedPosition : openPosition;
    }
}
