using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.VR;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class CardboardInput : MonoBehaviour {

	void OnMouseTouch()
	{
		Debug.Log("CardboardInpout.OnMouseTouch");
		Input.GetTouch (1);
		SceneManager.LoadScene (1);
	}
	// Update is called once per frame
	/*void Update () {
		if (Input.GetMouseButtonDown () {
			InputTracking.Recenter ();
		}
	}*/
}
