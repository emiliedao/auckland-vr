using UnityEngine;
using System.Collections;

public class RotateModelWithTouch : MonoBehaviour {

	float rotSpeed = 100;

	void OnMouseDrag()
	{
		Debug.Log("onMouseDrag");
		float rotX = Input.GetAxis ("Mouse X") * rotSpeed * Mathf.Deg2Rad;
		float rotY = Input.GetAxis ("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

		//transform.RotateAround (Vector3.up, Vector3.up, -rotX);
		//transform.RotateAround (Vector3.right, Vector3.right, rotY);
		transform.RotateAround (Vector3.zero, Vector3.up, -rotX);
		transform.RotateAround (Vector3.zero, Vector3.right, rotY);
	}
}