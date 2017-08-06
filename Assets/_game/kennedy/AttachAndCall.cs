using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NewtonVR;

public class AttachAndCall : NVRAttachJoint
{
    // Time after attach but before events
    public float attachEventTimeout = .25f;
    public UnityEvent attachEvents;
    private Coroutine _attachCoroutine;

    // Time after detach but before events
    public float detachEventTimeout = .1f;
    public UnityEvent detachEvents;
    private Coroutine _detachCoroutine;



    protected override void Attach(NVRAttachPoint point)
    {
        base.Attach(point);

        _attachCoroutine = StartCoroutine(AttachEvents());

        if (_detachCoroutine != null)
            StopCoroutine(_detachCoroutine);
    }

    protected override void Detach()
    {
        base.Detach();

        _detachCoroutine = StartCoroutine(DetachEvents());
        
        if (_attachCoroutine != null)
            StopCoroutine(_attachCoroutine);
    }

    private IEnumerator AttachEvents()
    {
        yield return new WaitForSeconds(attachEventTimeout);
        attachEvents.Invoke();
    }

    private IEnumerator DetachEvents()
    {
        yield return new WaitForSeconds(detachEventTimeout);
        detachEvents.Invoke();
    }
}
