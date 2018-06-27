using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidInput : MonoBehaviour {

	void Update() {
		// Click on android back button
		if (Input.GetKeyDown(KeyCode.Escape)) {
			// Closes settings menu
			if (ScenesManager.SettingsOpen)
			{
				ScenesManager.CloseSettingsMenu();
			}
			
			// Quits application
			else if (ScenesManager.CurrentScene == ScenesManager.Menu)
			{
				ScenesManager.Quit();
			}

			else
			{
				ScenesManager.LoadMenu();	
			}
		}
	}
	
}
