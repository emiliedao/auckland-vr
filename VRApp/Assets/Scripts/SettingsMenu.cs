using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour {

	public void LoadSettingsMenu()
	{
		ScenesManager.LoadSettingsMenu();
	}

	public void CloseSettingsMenu()
	{
		ScenesManager.CloseSettingsMenu();
	}

	public void LoadMainMenu()
	{
		ScenesManager.LoadMenu();
	}
}
