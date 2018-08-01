using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetViewCamera : MonoBehaviour {

/*
	public float speedh = 2.0f;
	public float speedv = 2.0f;

	private float yaw = 0.0f;
	private float pitch = 0.0f;
*/

	// Update is called once per frame
/*	void Update () {
		yaw += speedh * Input.GetAxis ("Mouse X");
		pitch -= speedv * Input.GetAxis ("Mouse Y");

		transform.eulerAngles = new Vector3 (pitch, yaw, 0.0f);
	}*/

	public float speed = 3.5f;
	private float X;
	private float Y;
 
//	void Update() {
//		if(Input.GetMouseButton(0)) {
//			transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, - Input.GetAxis("Mouse X") * speed, 0));
//			X = transform.rotation.eulerAngles.x;
//			Y = transform.rotation.eulerAngles.y;
//			transform.rotation = Quaternion.Euler(X, Y, 0);
//		}
//	}
}
