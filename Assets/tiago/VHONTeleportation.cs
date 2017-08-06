using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

[RequireComponent(typeof(NVRHand))]
public class VHONTeleportation : MonoBehaviour {

	public Valve.VR.InteractionSystem.TeleportArc teleportArc;

    public Color LineColor;
    public float LineWidth = 0.02f;
    private LineRenderer Line;
    private NVRHand Hand;
    private NVRPlayer Player;
    private void Awake()
    {
        Line = this.GetComponent<LineRenderer>();
        Hand = this.GetComponent<NVRHand>();
        if (Line == null)
        {
            Line = this.gameObject.AddComponent<LineRenderer>();
        }
        if (Line.sharedMaterial == null)
        {
            Line.material = new Material(Shader.Find("Unlit/Color"));
            Line.material.SetColor("_Color", LineColor);
            NVRHelpers.LineRendererSetColor(Line, LineColor, LineColor);
        }
        Line.useWorldSpace = true;
    }
    private void Start()
    {
        Player = Hand.Player;
    }

	public void Update()
    {
		Vector3 pointerStart = transform.position; //pointerStartTransform.position;
		Vector3 pointerEnd;
		Vector3 pointerDir = transform.position; //pointerStartTransform.forward;
		bool hitSomething = false;
		bool showPlayAreaPreview = false;
		Vector3 playerFeetOffset = transform.position; //player.trackingOriginTransform.position - player.feetPositionGuess;
		Vector3 arcVelocity = pointerDir * 3; //arcDistance;
		//TeleportMarkerBase hitTeleportMarker = null;
		//Check pointer angle
		float dotUp = Vector3.Dot( pointerDir, Vector3.up );
		float dotForward = Vector3.Dot( pointerDir, transform.forward); //player.hmdTransform.forward );
		bool pointerAtBadAngle = false;
		if ( ( dotForward > 0 && dotUp > 0.75f ) || ( dotForward < 0.0f && dotUp > 0.5f ) )
		{
			pointerAtBadAngle = true;
		}

		//Trace to see if the pointer hit anything
		RaycastHit hitInfo;
		teleportArc.SetArcData( pointerStart, arcVelocity, true, pointerAtBadAngle );
		if ( teleportArc.DrawArc( out hitInfo ) )
		{
			hitSomething = true;
		}
    }

    private void LateUpdate()
    {
        Line.enabled = (Hand != null && Hand.Inputs[NVRButtons.Trigger].SingleAxis > 0.01f);
        if (Line.enabled == true)
        {
            Line.material.SetColor("_Color", LineColor);
            NVRHelpers.LineRendererSetColor(Line, LineColor, LineColor);
            NVRHelpers.LineRendererSetWidth(Line, LineWidth, LineWidth);
            RaycastHit hitInfo;

			teleportArc.DrawArc(out hitInfo);
            bool hit = Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, 1000);
            Vector3 endPoint;
            if (hit == true)
            {
				
                endPoint = hitInfo.point;
                if (Hand.Inputs[NVRButtons.Trigger].PressDown == true)
                {
                    NVRInteractable LHandInteractable = Player.LeftHand.CurrentlyInteracting;
                    NVRInteractable RHandInteractable = Player.RightHand.CurrentlyInteracting;
                    Vector3 offset = Player.Head.transform.position - Player.transform.position;
                    offset.y = 0;
                    Player.transform.position = hitInfo.point - offset;
                    if (LHandInteractable != null)
                    {
                        LHandInteractable.transform.position = Player.LeftHand.transform.position;
                    }
                    if (RHandInteractable != null)
                    {
                        RHandInteractable.transform.position =Player.RightHand.transform.position;
                    }
                }
            }
            else
            {
                endPoint = this.transform.position + (this.transform.forward * 1000f);
            }
            Line.SetPositions(new Vector3[] { this.transform.position, endPoint });
        }
    }
}
