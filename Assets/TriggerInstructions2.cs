using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInstructions2 : MonoBehaviour {

	public SceneManagerBehavior sceneManagerB;

	public bool hasPlayed;

	void OnTriggerEnter(Collider other) {

		Debug.Log("asdhnaisuiwhendfi");
		Debug.Log("Collided with: " + other.gameObject.name);
		if(hasPlayed)
		{
			return;
		}

		if (other.gameObject.name.Equals("trackhat"))
		{
			Debug.Log("Collided with player");
			hasPlayed = true;
			StartCoroutine(sceneManagerB.SuccessAction());
        }
	}
}
