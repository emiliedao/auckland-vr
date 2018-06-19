using UnityEngine;
using System.Collections;

public class RotateModelWithTouch : MonoBehaviour {

	float rotSpeed = 100;
	private float _sensitivity = 0.4f;
	private Vector3 _mouseReference;
	private Vector3 _mouseOffset;
	private Vector3 _rotation;

	void OnMouseDrag()
	{
		Debug.Log("onMouseDrag");
		float rotX = Input.GetAxis ("Mouse X") * rotSpeed * Mathf.Deg2Rad;
		float rotY = Input.GetAxis ("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
		
		Debug.Log("rotation x,y = " + rotX + " " + rotY);

		//transform.RotateAround (Vector3.up, Vector3.up, -rotX);
		//transform.RotateAround (Vector3.right, Vector3.right, rotY);
		transform.RotateAround (Vector3.zero, Vector3.up, -rotX);
		transform.RotateAround (Vector3.zero, Vector3.right, rotY);
		
	/*	// offset
		_mouseOffset = (Input.mousePosition - _mouseReference);
		// apply rotation
		
		_rotation.y = -(_mouseOffset.x) * _sensitivity;
		_rotation.x = -(_mouseOffset.y) * _sensitivity;
		// rotate
		
		transform.eulerAngles += _rotation;
		// store mouse
		_mouseReference = Input.mousePosition;*/

	}
}