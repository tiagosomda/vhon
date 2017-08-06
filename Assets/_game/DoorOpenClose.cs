using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenClose : MonoBehaviour {

	public float animationInSecs;

	private float left_Open;
	private Transform DoorLeft;

	private float right_Open;
	private Transform DoorRight;


	// temp variable
	private Vector3 pos;
	private float left_Start;
	private float right_Start;
	private float targetLeft;
	private float targetRight;

	private float doorSpeed;
	private float elapsedTime = 0.0f;
	private float tempVal;
	private Coroutine routine;

	// Use this for initialization
	void Start () {
		left_Open = -1;
		right_Open = 1;

		foreach (Transform child in transform){
       		if (child.name == "Door.001"){ // LEFT DOOR
			   DoorLeft = child;
       		}
			else if (child.name == "Door")
			{
				DoorRight = child;
			} 
   		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			DefaultTrigger(true);
		}

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			DefaultTrigger(false);
		}
	}

	public void DefaultTrigger(bool state)
	{
		if(routine != null)
		{
			StopCoroutine(routine);	
		}
		StartCoroutine(OpenCloseDoor(state));
	}

	private IEnumerator OpenCloseDoor(bool open)
    {
		targetLeft = 0;
		targetRight = 0;

		if(open)
		{
			targetLeft = left_Open;
			targetRight = right_Open;
		}

		left_Start = DoorLeft.localPosition.x;
		right_Start = DoorRight.localPosition.x;

		elapsedTime = 0;

		doorSpeed = 1 / animationInSecs;
		while(elapsedTime < 1f)
		{
			yield return new WaitForEndOfFrame();
			elapsedTime += doorSpeed * Time.deltaTime;

			tempVal = Mathf.Lerp(left_Start, targetLeft, elapsedTime);

			pos = DoorLeft.localPosition;
			pos.x = tempVal;
			DoorLeft.localPosition = pos;

			tempVal = Mathf.Lerp(right_Start, targetRight, elapsedTime);
			pos = DoorRight.localPosition;
			pos.x = tempVal;
			DoorRight.localPosition = pos;
		}
    } 
}
