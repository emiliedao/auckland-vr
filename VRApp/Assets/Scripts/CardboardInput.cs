using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class CardboardInput : MonoBehaviour {

	void Update()
	{
		// Click on Cardboard button
		if (Input.GetMouseButtonDown(0) && XRSettings.enabled)
		{
			Debug.Log("Cardboard click");
			Debug.Log("SettingsOpen : " + ScenesManager.SettingsOpen);
			if (!ScenesManager.SettingsOpen)
			{
				ScenesManager.LoadSettingsMenu();
			}	
		}
		
	}
}
