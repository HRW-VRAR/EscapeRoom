using UnityEngine;
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
	private bool rendered = false;


	// Use this for initialization
	void Start()
	{
		initial_y = transform.eulerAngles.y;
	}

	// Update is called once per frame
	void Update()
	{
		//transform.rotation = Quaternion.Euler(transform.eulerAngles.x, (Mathf.Sin(Time.realtimeSinceStartup * rotate_speed) * rotate_amount) + transform.eulerAngles.y, transform.eulerAngles.z);
		transform.rotation = Quaternion.Euler(transform.eulerAngles.x, initial_y + Mathf.Sin(Time.realtimeSinceStartup * rotate_speed) * rotate_amount, transform.eulerAngles.z);

		if (false && Time.realtimeSinceStartup >= 15 && !rendered)
        {
			rendered = true;
			//cam.Render();
			Camera mainCam = Camera.main;
			cam.tag = "MainCamera";
			cam.targetTexture = null;
			mainCam.enabled = false;
        }
	}

	public void activateCamera()
	{
		Camera mainCam = Camera.main;

		if (mainCam == cam)
        {
			return;
        }

		XROrigin.transform.SetParent(cam.transform, false);

		var camOffsetTransform = XROrigin.transform.GetChild(0);
		var trd = camOffsetTransform.GetChild(0).GetComponent<TrackedPoseDriver>();
		trd.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
		for (int i = 0; i < camOffsetTransform.childCount; i++)
        {
			var child = camOffsetTransform.GetChild(i);
			Vector3 childLP = child.localPosition;
			childLP.y = 0;
			child.localPosition = childLP;

			var xrController = child.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRControllerCustom>();
			if (xrController != null)
            {
				xrController.m_bInCam = true;
            }
        }

		// change layer and tag of cctv camera container
		var cctvCam = transform.parent.gameObject;
		cctvCam.layer = LayerMask.NameToLayer("ActiveCCTVCam");
		cctvCam.tag = "ActiveCCTVCam";

		// notify player character
		character.onCCTVActivated();

		//cam.tag = "MainCamera";
		//cam.targetTexture = null;
		//mainCam.enabled = false;
	}
}