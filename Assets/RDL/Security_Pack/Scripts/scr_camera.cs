using UnityEngine;
using System.Collections;

public class scr_camera : MonoBehaviour
{

	public float rotate_amount;
	public float rotate_speed;
	public Camera cam;
	public GameObject XROrigin;

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

		cam.tag = "MainCamera";
		cam.targetTexture = null;
		mainCam.enabled = false;
	}
}