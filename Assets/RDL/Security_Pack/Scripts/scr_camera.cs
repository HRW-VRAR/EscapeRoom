using UnityEngine;
using System.Collections;

public class scr_camera : MonoBehaviour
{

	public float rotate_amount;
	public float rotate_speed;

	private float initial_y;


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
	}
}