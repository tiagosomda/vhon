using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondRoomAudioTrigger : MonoBehaviour {

	// Use this for initialization
	public SceneManagerBehavior sceneManager;
	public bool hasPlayed;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider)
	{
		if(hasPlayed)
		{
			return;
		}
		if( collider.gameObject.name.Equals("trackhat"))
		{
			hasPlayed = true;
			StartCoroutine(sceneManager.SuccessAction());
		}
	}
}
