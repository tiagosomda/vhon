using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZPM_Detector : MonoBehaviour
{
    public GameObject ZPM;
    public float AttachTime = .25f;

    public UnityEvent Activatees;

    // Run-time property for storing handle
    private Coroutine AttachCoroutine;

	void Start()
    {
        
	}
	
	void Update()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == ZPM.name)
        {
            AttachCoroutine = StartCoroutine(ActivateThings());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == ZPM.name)
        {
            StopCoroutine(AttachCoroutine);
        }
    }

    IEnumerator ActivateThings()
    {
        yield return new WaitForSeconds(AttachTime);
        Activatees.Invoke();
    }
}
