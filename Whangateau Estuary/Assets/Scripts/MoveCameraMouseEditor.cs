using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraMouseEditor : MonoBehaviour {

	public float speedh = 2.0f;
	public float speedv = 2.0f;

	private float yaw = 0.0f;
	private float pitch = 0.0f;

	// Update is called once per frame
	void Update () {
		yaw += speedh * Input.GetAxis ("Mouse X");
		pitch -= speedv * Input.GetAxis ("Mouse Y");

		transform.eulerAngles = new Vector3 (pitch, yaw, 0.0f);
	}
}
