using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NewtonVR;

public class AttachAndEmit : NVRAttachJoint
{
    // Time after attach but before activate
    public float attachTimeout = .25f;
    // ---
    public UnityEvent activatees;


    // Stores coroutine for possible stopping
    private Coroutine _activate;


    protected override void Attach(NVRAttachPoint point)
    {
        base.Attach(point);
        _activate = StartCoroutine(ActivateThings());
    }

    protected override void Detach()
    {
        base.Detach();
        
        if (_activate != null)
        {
            StopCoroutine(_activate);
        }
    }

    private IEnumerator ActivateThings()
    {
        yield return new WaitForSeconds(attachTimeout);
        activatees.Invoke();
    }
}
