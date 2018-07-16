using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SettingsMenu : MonoBehaviour
{
	private bool _open;
	public Canvas SettingsMenuCanvas;
	public GvrReticlePointer Reticle;

	void Start()
	{
		SettingsMenuCanvas.gameObject.SetActive(false);
		Reticle.gameObject.SetActive(false);
	}

	void Update()
	{
		// Click on Cardboard button
		if ((Input.GetMouseButtonDown(0) && XRSettings.enabled) || Input.GetKeyDown(KeyCode.Space))
		{
			if (!_open)
			{
				_open = true;
				LoadSettingsMenu();	
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (_open)
			{
				CloseSettingsMenu();
			}
		}
	}

	private void LoadSettingsMenu()
	{
		Debug.Log("open");
		Reticle.gameObject.SetActive(true);
		ScenesManager.CanvasFaceCamera(SettingsMenuCanvas, 3);
		SettingsMenuCanvas.gameObject.SetActive(true);
	}

	public void CloseSettingsMenu()
	{
		Debug.Log("close");
		_open = false;
		SettingsMenuCanvas.gameObject.SetActive(false);
		Reticle.gameObject.SetActive(false);
	}

	public void LoadMainMenu()
	{
		ScenesManager.LoadMenu();
	}
}
