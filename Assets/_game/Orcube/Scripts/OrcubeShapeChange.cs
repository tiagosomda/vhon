using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcubeShapeChange : MonoBehaviour {
	SkinnedMeshRenderer skinnedMeshRenderer;
	float blendOne = 0f;
	float blendTwo = 100f;
	float blendSpeed = 2f;
	float blendGoal;
	bool isSet;

	void Start(){
		skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer> ();
		blendGoal = blendOne;

	}

	//Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(0),blendGoal,blendSpeed)
	void Update(){
		skinnedMeshRenderer.SetBlendShapeWeight(0,Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(0), blendGoal,blendSpeed * Time.deltaTime));
	}

	void OnTriggerEnter() {
		Debug.Log ("Entering Trigger");
		blendGoal = blendTwo;
	}

	void OnTriggerExit() {
		Debug.Log ("exiting Trigger");
		blendGoal = blendOne;
	}







}
