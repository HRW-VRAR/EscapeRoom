﻿using UnityEngine;
using System.Collections;
using UnityEngine.SpatialTracking;

public class scr_camera : MonoBehaviour
{

	public float rotate_amount;
	public float rotate_speed;
	public Camera cam;
	public GameObject XROrigin;
	public CharacterScript character;

	private float initial_y;


	// Use this for initialization
	void Start()
	{
		initial_y = transform.eulerAngles.y;
	}

	// Update is called once per frame
	void Update()
	{
		transform.rotation = Quaternion.Euler(transform.eulerAngles.x, initial_y + Mathf.Sin(Time.realtimeSinceStartup * rotate_speed) * rotate_amount, transform.eulerAngles.z);
	}

	/**
	 * Activates the CCTV camera. Moves the XR Origin to the camera's transform hierarchy and position.
	 **/
	public void activateCamera()
	{
		Camera mainCam = Camera.main;

		if (mainCam == cam)
        {
			return;
        }

		CCTVCameraActivationData activationData = new CCTVCameraActivationData();

		// Move XR Origin into CCTV camera structure, keep local coordinates
		XROrigin.transform.SetParent(cam.transform, false);

		// Reset XR Origin rotation
		Vector3 xrOriginRot = XROrigin.transform.localEulerAngles;
		activationData.xrOriginYRotation = xrOriginRot.y;
		xrOriginRot.y = 0;
		XROrigin.transform.localEulerAngles = xrOriginRot;

		var camOffsetTransform = XROrigin.transform.GetChild(0);

		// Disable position (height) tracking and reset Y position
		var trd = camOffsetTransform.GetChild(0).GetComponent<TrackedPoseDriver>();
		trd.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
		Vector3 camOffsetLP = camOffsetTransform.localPosition;
		activationData.cameraOffsetYPosition = camOffsetLP.y;
		camOffsetLP.y = 0;
		camOffsetTransform.localPosition = camOffsetLP;
		// Reset Y position of child transforms and find relevant components
		for (int i = 0; i < camOffsetTransform.childCount; i++)
        {
			var child = camOffsetTransform.GetChild(i);
			Vector3 childLP = child.localPosition;
			childLP.y = 0;
			child.localPosition = childLP;

			// Tell controllers we're in a CCTV camera
			var xrController = child.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRControllerCustom>();
			if (xrController != null)
            {
				xrController.m_bInCam = true;
			}

			// Disable walking
			var xrRayInteractor = child.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRRayInteractor>();
			if (xrRayInteractor != null)
			{
				xrRayInteractor.interactionLayers &= ~(1 << UnityEngine.XR.Interaction.Toolkit.InteractionLayerMask.NameToLayer("Walkable area"));
			}
		}

		// Remove layer and tag from previously active CCTV camera
		GameObject[] activeCCTVCams = GameObject.FindGameObjectsWithTag("ActiveCCTVCam");
		for (int i = 0; i < activeCCTVCams.Length; i++)
		{
			var cam = activeCCTVCams[i];
			cam.layer = 0;
			cam.tag = "Untagged";
			RecursiveSetLayerIfTagged(cam.transform, 0, "VisibleCameraPart");
		}

		// Change layer and tag of CCTV camera container
		var cctvCam = transform.parent.gameObject;
		var activeCamLayer = LayerMask.NameToLayer("ActiveCCTVCam");
		cctvCam.layer = activeCamLayer;
		cctvCam.tag = "ActiveCCTVCam";

		RecursiveSetLayerIfTagged(cctvCam.transform, activeCamLayer, "VisibleCameraPart");

		// Notify player character
		character.onCCTVActivated(activationData);
	}

	private void RecursiveSetLayerIfTagged(Transform parent, int layer, string tag)
	{
		for (int i = 0; i < parent.childCount; i++)
		{
			var child = parent.GetChild(i);
			if (child.tag == tag)
			{
				child.gameObject.layer = layer;
			}
			RecursiveSetLayerIfTagged(child, layer, tag);
		}
	}
}