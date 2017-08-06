using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(NVRHand))]
public class VHONTeleportation : MonoBehaviour {

	public Valve.VR.InteractionSystem.TeleportArc teleportArc;

    public LayerMask traceLayerMask;
    public float arcDistance;
    public Color LineColor;
    public float LineWidth = 0.02f;
    private LineRenderer Line;
    private NVRHand Hand;
    private NVRPlayer Player;

	public Transform destinationReticleTransform;
	public Transform invalidReticleTransform;

    public Color pointerLockedColor;
    public Color pointerValidColor;

    private Vector3 pointedAtPosition;

    public Transform offsetReticleTransform;

    private TeleportMarkerBase pointedAtTeleportMarker;

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

        teleportArc.traceLayerMask = traceLayerMask;
    }
    private void Start()
    {
        Player = Hand.Player;
    }

    private void LateUpdate()
    {
        //Line.enabled = (Hand != null && Hand.Inputs[NVRButtons.Trigger].SingleAxis > 0.01f);

        Line.enabled = (Hand != null && Hand.Inputs[NVRButtons.Touchpad].IsPressed);
        if (Line.enabled == true)
        {
            //NvrTeleport();
            VHONTeleport();
        }
        else {
            teleportArc.Hide();
        }
    }

    private void NvrTeleport()
    {
            Line.material.SetColor("_Color", LineColor);
            NVRHelpers.LineRendererSetColor(Line, LineColor, LineColor);
            NVRHelpers.LineRendererSetWidth(Line, LineWidth, LineWidth);
            RaycastHit hitInfo;

			//teleportArc.DrawArc(out hitInfo);
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

    private void VHONTeleport()
    {
        Debug.Log("VHONTING");
        Vector3 pointerStart = Hand.gameObject.transform.position; //pointerStartTransform.position;
		Vector3 pointerEnd;
		Vector3 pointerDir = Hand.gameObject.transform.forward; //pointerStartTransform.forward;
		bool hitSomething = false;
		bool showPlayAreaPreview = false;
		Vector3 playerFeetOffset = Player.gameObject.transform.position; //player.trackingOriginTransform.position - player.feetPositionGuess;
		Vector3 arcVelocity = pointerDir * arcDistance; //arcDistance;
		
        TeleportMarkerBase hitTeleportMarker = null;

		//Check pointer angle
		float dotUp = Vector3.Dot( pointerDir, Vector3.up );
		float dotForward = Vector3.Dot( pointerDir, Player.gameObject.transform.forward); //player.hmdTransform.forward );
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

        if ( pointerAtBadAngle )
		{
			hitTeleportMarker = null;
		}

		//HighlightSelected( hitTeleportMarker );

		if ( hitTeleportMarker != null ) //Hit a teleport marker
		{
			if ( hitTeleportMarker.locked )
			{
			    teleportArc.SetColor( pointerLockedColor );
                #if (UNITY_5_4)
				Line.SetColors( pointerLockedColor, pointerLockedColor ); //pointerLineRenderer.SetColors( pointerLockedColor, pointerLockedColor );
                #else
				Line.startColor = pointerLockedColor;//pointerLineRenderer.startColor = pointerLockedColor;
				Line.endColor = pointerLockedColor;//pointerLineRenderer.endColor = pointerLockedColor;
                #endif
				destinationReticleTransform.gameObject.SetActive( false );
			}
			else
			{
			    teleportArc.SetColor( pointerValidColor );
                #if (UNITY_5_4)
				Line.SetColors( pointerValidColor, pointerValidColor );//pointerLineRenderer.SetColors( pointerValidColor, pointerValidColor );
                #else
				Line.startColor = pointerValidColor;//pointerLineRenderer.startColor = pointerValidColor;
				Line.endColor = pointerValidColor; //pointerLineRenderer.endColor = pointerValidColor;
                #endif
				destinationReticleTransform.gameObject.SetActive( hitTeleportMarker.showReticle );
			}

			offsetReticleTransform.gameObject.SetActive( true );

			invalidReticleTransform.gameObject.SetActive( false );

			pointedAtTeleportMarker = hitTeleportMarker;
			pointedAtPosition = hitInfo.point;
        }
        /*
        else //Hit neither
		{
				destinationReticleTransform.gameObject.SetActive( false );
				offsetReticleTransform.gameObject.SetActive( false );

				teleportArc.SetColor( pointerInvalidColor );
#if (UNITY_5_4)
				pointerLineRenderer.SetColors( pointerInvalidColor, pointerInvalidColor );
#else
				pointerLineRenderer.startColor = pointerInvalidColor;
				pointerLineRenderer.endColor = pointerInvalidColor;
#endif
				invalidReticleTransform.gameObject.SetActive( !pointerAtBadAngle );

				//Orient the invalid reticle to the normal of the trace hit point
				Vector3 normalToUse = hitInfo.normal;
				float angle = Vector3.Angle( hitInfo.normal, Vector3.up );
				if ( angle < 15.0f )
				{
					normalToUse = Vector3.up;
				}
				invalidReticleTargetRotation = Quaternion.FromToRotation( Vector3.up, normalToUse );
				invalidReticleTransform.rotation = Quaternion.Slerp( invalidReticleTransform.rotation, invalidReticleTargetRotation, 0.1f );

				//Scale the invalid reticle based on the distance from the player
				float distanceFromPlayer = Vector3.Distance( hitInfo.point, player.hmdTransform.position );
				float invalidReticleCurrentScale = Util.RemapNumberClamped( distanceFromPlayer, invalidReticleMinScaleDistance, invalidReticleMaxScaleDistance, invalidReticleMinScale, invalidReticleMaxScale );
				invalidReticleScale.x = invalidReticleCurrentScale;
				invalidReticleScale.y = invalidReticleCurrentScale;
				invalidReticleScale.z = invalidReticleCurrentScale;
				invalidReticleTransform.transform.localScale = invalidReticleScale;

				pointedAtTeleportMarker = null;

				if ( hitSomething )
				{
					pointerEnd = hitInfo.point;
				}
				else
				{
					pointerEnd = teleportArc.GetArcPositionAtTime( teleportArc.arcDuration );
				}

				//Debug floor
				if ( debugFloor )
				{
					floorDebugSphere.gameObject.SetActive( false );
					floorDebugLine.gameObject.SetActive( false );
				}
		}
        */
    }         
}
