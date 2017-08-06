using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcubeShapeChange : MonoBehaviour {

    public CubeAudio ca;

	public string PowerMorpherTag = "PowerMorpher";

	SkinnedMeshRenderer skinnedMeshRenderer;
	float blendOne = 0f;
	float blendTwo = 100f;
	float blendSpeed = 2f;
	float blendGoal;
	bool isSet;

	SphereCollider sphereCollider;
	BoxCollider boxCollider;

	void Start(){
		ca = CubeAudio.Instance;
		skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer> ();
		blendGoal = blendOne;
	}

	//Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(0),blendGoal,blendSpeed)
	void Update(){
		skinnedMeshRenderer.SetBlendShapeWeight(0,Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(0), blendGoal, blendSpeed * Time.deltaTime));
	}

	void OnTriggerEnter(Collider other) {

		if (other.CompareTag(PowerMorpherTag))
		{
			blendGoal = blendTwo;
            ca.GoHi();
        }
	}

	void OnTriggerExit(Collider other)  {
		if (other.CompareTag(PowerMorpherTag))
		{
			blendGoal = blendOne;
            ca.GoLow();
		}
	}
}
