﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VHONTeleportArea : TeleportMarkerBase 
{
	//Public properties
	public Bounds meshBounds { get; private set; }
	//Private data
	private MeshRenderer areaMesh;
	private int tintColorId = 0;
	private Color visibleTintColor = Color.clear;
	private Color highlightedTintColor = Color.clear;
	private Color lockedTintColor = Color.clear;
	private bool highlighted = false;
	//-------------------------------------------------
	public void Awake()
	{
		//areaMesh = GetComponent<MeshRenderer>();
		//tintColorId = Shader.PropertyToID( "_TintColor" );
		//CalculateBounds();
	}
	//-------------------------------------------------
	public void Start()
	{
		//visibleTintColor = VHONTeleportRight.instance.areaVisibleMaterial.GetColor( tintColorId );
		//highlightedTintColor = VHONTeleportRight.instance.areaHighlightedMaterial.GetColor( tintColorId );
		//lockedTintColor = VHONTeleportRight.instance.areaLockedMaterial.GetColor( tintColorId );
	}
	//-------------------------------------------------
	public override bool ShouldActivate( Vector3 playerPosition )
	{
		return true;
	}
	//-------------------------------------------------
	public override bool ShouldMovePlayer()
	{
		return true;
	}
	//-------------------------------------------------
	public override void Highlight( bool highlight )
	{
/* 		if ( !locked )
		{
			highlighted = highlight;
			if ( highlight )
			{
				areaMesh.material = VHONTeleportRight.instance.areaHighlightedMaterial;
			}
			else
			{
				areaMesh.material = VHONTeleportRight.instance.areaVisibleMaterial;
			}
		} */
	}
	//-------------------------------------------------
	public override void SetAlpha( float tintAlpha, float alphaPercent )
	{
/*  		Color tintedColor = GetTintColor();
		tintedColor.a *= alphaPercent;
		areaMesh.material.SetColor( tintColorId, tintedColor ); */
	}
	//-------------------------------------------------
	public override void UpdateVisuals()
	{
/* 		if ( locked )
		{
			areaMesh.material = VHONTeleportRight.instance.areaLockedMaterial;
		}
		else
		{
			areaMesh.material = VHONTeleportRight.instance.areaVisibleMaterial;
		} */
	}
	//-------------------------------------------------
	public void UpdateVisualsInEditor()
	{
/* 		areaMesh = GetComponent<MeshRenderer>();
		if ( locked )
		{
			areaMesh.sharedMaterial = VHONTeleportRight.instance.areaLockedMaterial;
		}
		else
		{
			areaMesh.sharedMaterial = VHONTeleportRight.instance.areaVisibleMaterial;
		} */
	}
	//-------------------------------------------------
	private bool CalculateBounds()
	{
/* 		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if ( meshFilter == null )
		{
			return false;
		}
		Mesh mesh = meshFilter.sharedMesh;
		if ( mesh == null )
		{
			return false;
		}
		meshBounds = mesh.bounds;
		return true; */

		return true;
	}
	//-------------------------------------------------
	private Color GetTintColor()
	{
/* 		if ( locked )
		{
			return lockedTintColor;
		}
		else
		{
			if ( highlighted )
			{
				return highlightedTintColor;
			}
			else
			{
				return visibleTintColor;
			}
		} */

		return Color.white;
	}
}

#if UNITY_EDITOR
//-------------------------------------------------------------------------
[CustomEditor( typeof( VHONTeleportArea ) )]
public class VHONTeleportAreaEditor : Editor
{
		//-------------------------------------------------
	void OnEnable()
	{
		if ( Selection.activeTransform != null )
		{
			VHONTeleportArea teleportArea = Selection.activeTransform.GetComponent<VHONTeleportArea>();
			if ( teleportArea != null )
			{
				teleportArea.UpdateVisualsInEditor();
			}
		}
	}


	//-------------------------------------------------
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if ( Selection.activeTransform != null )
		{
			VHONTeleportArea teleportArea = Selection.activeTransform.GetComponent<VHONTeleportArea>();
			if ( GUI.changed && teleportArea != null )
			{
				teleportArea.UpdateVisualsInEditor();
			}
		}
	}
}
#endif
