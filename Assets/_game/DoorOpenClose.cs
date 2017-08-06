using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenClose : MonoBehaviour {

	public float doorSpeed;
	public float left_Open;
	public GameObject DoorLeft;

	public float right_Open;
	public GameObject DoorRight;


	// temp variable
	private Vector3 pos;
	private float targetLeft;
	private float targetRight;


	float t = 0.0f;
	Coroutine routine;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			OpenDoor();
		}

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			CloseDoor();
		}
	}

	public void DefaultTrigger(bool state)
	{

	}

	public void OpenDoor()
	{
		// // LEFT DOOR
		// pos = DoorLeft.transform.localPosition;
		// pos.x = left_Open;
		// DoorLeft.transform.localPosition = pos;

		// // RIGHT DOOR
		// pos = DoorRight.transform.localPosition;
		// pos.x = right_Open;
		// DoorRight.transform.localPosition = pos;

		if(routine != null)
		{
			StopCoroutine(routine);	
		}
		StartCoroutine(OpenCloseDoor(true));
		
		Debug.Log("OpenDoor");
	}

	public void CloseDoor()
	{
		// // LEFT DOOR
		// pos = DoorLeft.transform.localPosition;
		// pos.x = 0;
		// DoorLeft.transform.localPosition = pos;

		// // RIGHT DOOR
		// pos = DoorRight.transform.localPosition;
		// pos.x = 0;
		// DoorRight.transform.localPosition = pos;

		if(routine != null)
		{
			StopCoroutine(routine);	
		}
		StartCoroutine(OpenCloseDoor(false));

		Debug.Log("CloseDoor");
	}


	private IEnumerator OpenCloseDoor(bool open)
    {
		Debug.Log("Opening/Closing Doors...");
		t = 0;
		targetLeft = 0;
		targetRight = 0;

		if(open)
		{
			targetLeft = left_Open;
			targetRight = right_Open;
		}

		var left_Start = DoorLeft.transform.localPosition.x;
		var right_Start = DoorRight.transform.localPosition.x;

		float timeCounter =0f;
		while(t < 1f)
		{
			yield return new WaitForEndOfFrame();
			t += doorSpeed * Time.deltaTime;
			Mathf.Lerp(left_Start, targetLeft, t);
		}
    } 
}
