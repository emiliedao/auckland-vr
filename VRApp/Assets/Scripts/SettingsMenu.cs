using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class SettingsMenu : MonoBehaviour
{
	private bool _open;
	public Canvas SettingsMenuCanvas;
	private CanvasManager _settingsMenuCanvasManager;
	public GvrReticlePointer Reticle;
	private static float _defaultScale = 0.5f;

	public static float DefaultScale
	{
		get { return _defaultScale; }
	}

	void Start()
	{
		SettingsMenuCanvas.gameObject.SetActive(false);
		Reticle.gameObject.SetActive(false);
		
		Zoom.SettingsMenuScale = _defaultScale;
		_settingsMenuCanvasManager = SettingsMenuCanvas.GetComponent<CanvasManager>();
	}

	void Update()
	{
		// Click on Cardboard button
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
		{
			if (!SettingsMenuCanvas.gameObject.activeSelf)
			{
				LoadSettingsMenu();
			}

			else
			{
				if (!_settingsMenuCanvasManager.PointerInside)
				{
					CloseSettingsMenu();
				}
			}
		}
		
		float scale = Zoom.SettingsMenuScale;
		SettingsMenuCanvas.transform.parent.localScale = new Vector3(scale, scale, scale);
	}

	private void LoadSettingsMenu()
	{
		Reticle.gameObject.SetActive(true);
		CanvasManager.FaceCamera(SettingsMenuCanvas, 3);
		SettingsMenuCanvas.gameObject.SetActive(true);
	}

	private void CloseSettingsMenu()
	{
		SettingsMenuCanvas.gameObject.SetActive(false);
		Reticle.gameObject.SetActive(false);
	}

	public void LoadMainMenu()
	{
		ScenesManager.LoadMenu();
	}
}
