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
    public Color pointerInvalidColor;

    private Vector3 pointedAtPosition;
    private Vector3 prevPointedAtPosition;
    private TeleportMarkerBase pointedAtTeleportMarker;

    private float invalidReticleMinScale = 0.2f;
	private float invalidReticleMaxScale = 1.0f;
	private float invalidReticleMinScaleDistance = 0.4f;
	private float invalidReticleMaxScaleDistance = 2.0f;
	private Vector3 invalidReticleScale = Vector3.one;
    private Quaternion invalidReticleTargetRotation = Quaternion.identity;

    private bool visible = false;


    [Header( "Debug" )]
	public bool debugFloor = false;
	public bool showOffsetReticle = false;
	public Transform offsetReticleTransform;
	public MeshRenderer floorDebugSphere;
	public LineRenderer floorDebugLine;

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
            NvrTeleport();
            //VHONTeleport();
        }
        else {
            //teleportArc.Hide();
            //HidePointer();
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
                if (Hand.Inputs[NVRButtons.Touchpad].PressDown == true)
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
            hitTeleportMarker = hitInfo.collider.GetComponentInParent<TeleportMarkerBase>();
		}

        if ( pointerAtBadAngle )
		{
			hitTeleportMarker = null;
		}

		HighlightSelected( hitTeleportMarker );

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
                destinationReticleTransform.rotation = Quaternion.LookRotation(Vector3.forward); //Quaternion. .Slerp( invalidReticleTransform.rotation, invalidReticleTargetRotation, 0.1f );
			}

			offsetReticleTransform.gameObject.SetActive( true );

			invalidReticleTransform.gameObject.SetActive( false );

			pointedAtTeleportMarker = hitTeleportMarker;
			pointedAtPosition = hitInfo.point;

            /*
            if ( showPlayAreaMarker )
			{
				//Show the play area marker if this is a teleport area
				TeleportArea teleportArea = pointedAtTeleportMarker as TeleportArea;
				if ( teleportArea != null && !teleportArea.locked && playAreaPreviewTransform != null )
				{
					Vector3 offsetToUse = playerFeetOffset;
					//Adjust the actual offset to prevent the play area marker from moving too much
					if ( !movedFeetFarEnough )
					{
						float distanceFromStartingOffset = Vector3.Distance( playerFeetOffset, startingFeetOffset );
						if ( distanceFromStartingOffset < 0.1f )
						{
							offsetToUse = startingFeetOffset;
						}
						else if ( distanceFromStartingOffset < 0.4f )
						{
							offsetToUse = Vector3.Lerp( startingFeetOffset, playerFeetOffset, ( distanceFromStartingOffset - 0.1f ) / 0.3f );
						}
						else
						{
							movedFeetFarEnough = true;
						}
					}

					playAreaPreviewTransform.position = pointedAtPosition + offsetToUse;
						showPlayAreaPreview = true;
				}
			}
            */

            pointerEnd = hitInfo.point;
        }
        else //Hit neither
		{
			destinationReticleTransform.gameObject.SetActive( false );
			offsetReticleTransform.gameObject.SetActive( false );

			teleportArc.SetColor( pointerInvalidColor );
            #if (UNITY_5_4)
			Line.SetColors( pointerInvalidColor, pointerInvalidColor );//pointerLineRenderer.SetColors( pointerInvalidColor, pointerInvalidColor );
            #else
			Line.startColor = pointerInvalidColor; //pointerLineRenderer.startColor = pointerInvalidColor;
			Line.endColor = pointerInvalidColor; //pointerLineRenderer.endColor = pointerInvalidColor;
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
			float distanceFromPlayer = Vector3.Distance( hitInfo.point, Player.transform.position );// Vector3.Distance( hitInfo.point, player.hmdTransform.position );
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

        //if ( playAreaPreviewTransform != null )
		//{
		//	playAreaPreviewTransform.gameObject.SetActive( showPlayAreaPreview );
		//}

		if ( !showOffsetReticle )
		{
			offsetReticleTransform.gameObject.SetActive( false );
		}
		destinationReticleTransform.position = pointedAtPosition;
		invalidReticleTransform.position = pointerEnd;
		//onActivateObjectTransform.position = pointerEnd;
		//onDeactivateObjectTransform.position = pointerEnd;
		offsetReticleTransform.position = pointerEnd - playerFeetOffset;
		//reticleAudioSource.transform.position = pointedAtPosition;
		Line.SetPosition( 0, pointerStart ); //pointerLineRenderer.SetPosition( 0, pointerStart );
		Line.SetPosition( 1, pointerEnd ); //pointerLineRenderer.SetPosition( 1, pointerEnd );
    }         

    private void HighlightSelected(TeleportMarkerBase hitTeleportMarker )
	{
		if ( pointedAtTeleportMarker != hitTeleportMarker ) //Pointing at a new teleport marker
		{
			if ( pointedAtTeleportMarker != null )
			{
				pointedAtTeleportMarker.Highlight( false );
			}
			if ( hitTeleportMarker != null )
			{
				hitTeleportMarker.Highlight( true );
				prevPointedAtPosition = pointedAtPosition;
				//PlayPointerHaptic( !hitTeleportMarker.locked );
				//PlayAudioClip( reticleAudioSource, goodHighlightSound );
				//loopingAudioSource.volume = loopingAudioMaxVolume;
			}
			else if ( pointedAtTeleportMarker != null )
			{
				//PlayAudioClip( reticleAudioSource, badHighlightSound );
				//loopingAudioSource.volume = 0.0f;
			}
		}
		else if ( hitTeleportMarker != null ) //Pointing at the same teleport marker
		{
			if ( Vector3.Distance( prevPointedAtPosition, pointedAtPosition ) > 1.0f )
			{
				prevPointedAtPosition = pointedAtPosition;
				//PlayPointerHaptic( !hitTeleportMarker.locked );
			}
		}
	}


    private void HidePointer()
	{
		if ( visible )
		{
			//pointerHideStartTime = Time.time;
		}

		visible = false;
		if ( Hand )
		{
            /*
			if ( ShouldOverrideHoverLock() )
			{
				//Restore the original hovering interactable on the hand
				if ( originalHoverLockState == true )
				{
					pointerHand.HoverLock( originalHoveringInteractable );
				}
				else
				{
					pointerHand.HoverUnlock( null );
				}
			}
            */

			//Stop looping sound
			//loopingAudioSource.Stop();
			//PlayAudioClip( pointerAudioSource, pointerStopSound );
		}
		
        //teleportPointerObject.SetActive( false );

		teleportArc.Hide();
		/*
        foreach ( TeleportMarkerBase teleportMarker in teleportMarkers )
		{
			if ( teleportMarker != null && teleportMarker.markerActive && teleportMarker.gameObject != null )
			{
				teleportMarker.gameObject.SetActive( false );
			}
		}
        */
		destinationReticleTransform.gameObject.SetActive( false );
		invalidReticleTransform.gameObject.SetActive( false );
		offsetReticleTransform.gameObject.SetActive( false );
		//if ( playAreaPreviewTransform != null )
		//{
		//	playAreaPreviewTransform.gameObject.SetActive( false );
		//}
		//if ( onActivateObjectTransform.gameObject.activeSelf )
		//{
		//	onActivateObjectTransform.gameObject.SetActive( false );
		//}
		//onDeactivateObjectTransform.gameObject.SetActive( true );
		//pointerHand = null;
	}
}
