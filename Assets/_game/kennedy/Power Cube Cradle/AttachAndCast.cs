using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class AttachAndCast : NVRAttachJoint
{
    
    public List<GameObject> acceptableObjects;

    // Time after attach but before events
    public float attachTriggerTimeout = .25f;
    public List<GameObject> attachTriggers;
    private List<Coroutine> _attachCoroutines;

    // Time after detach but before events
    public float detachTriggerTimeout = .175f;
    public List<GameObject> detachTriggers;
    private List<Coroutine> _detachCoroutines;

	void Start()
    {
        _attachCoroutines = new List<Coroutine>();
        _detachCoroutines = new List<Coroutine>();
	}

    // This override enforces copatability with acceptble objs to interact with
    protected override void OnTriggerStay(Collider col)
    {
        if (IsAttached == false && acceptableObjects.Contains(col.gameObject))
        {
            base.OnTriggerStay(col);
            Debug.Log("Is NOt Attached");
        }

        Debug.Log("Is attached");
    }

    protected override void Attach(NVRAttachPoint point)
    {
        base.Attach(point);
        
        Debug.Log("Attach method");
        StartCoroutine(AttachTriggers());
    }

    protected override void Detach()
    {
        base.Detach();

        StartCoroutine(DetachTriggers());
    }

    private IEnumerator AttachTriggers()
    {
        yield return new WaitForSeconds(attachTriggerTimeout);

        foreach (GameObject other in attachTriggers)
        {
            Debug.Log("Foreach");
            if (_detachCoroutines.Count > 0)
            {
                foreach (Coroutine c in _detachCoroutines)
                    StopCoroutine(c);
            }

            Debug.Log("Send DefaultTrigger");
            other.SendMessage("DefaultTrigger", true,
                SendMessageOptions.DontRequireReceiver);
        }
    } 

    private IEnumerator DetachTriggers()
    {
        yield return new WaitForSeconds(detachTriggerTimeout);


        foreach (GameObject other in detachTriggers)
        {
            if (_attachCoroutines.Count > 0)
            {
                foreach (Coroutine c in _attachCoroutines)
                    StopCoroutine(c);
            }

            other.SendMessage("DefaultTrigger", false,
                SendMessageOptions.DontRequireReceiver);
        }
    } 
}
