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
	private static float _defaultScale = 0.3f;

	public static float DefaultScale
	{
		get { return _defaultScale; }
	}

	void Start()
	{
		SettingsMenuCanvas.gameObject.SetActive(false);
		Reticle.gameObject.SetActive(false);
		
		SettingsMenuCanvas.transform.parent.localScale = new Vector3(_defaultScale, _defaultScale, _defaultScale);

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
				Open();
			}

			else
			{
				if (!_settingsMenuCanvasManager.PointerInside)
				{
					Close();
				}
			}
		}
		
//		float scale = Zoom.SettingsMenuScale;
//		SettingsMenuCanvas.transform.parent.localScale = new Vector3(scale, scale, scale);
	}

	private void Open()
	{
		Reticle.gameObject.SetActive(true);
		CanvasManager.FaceCamera(SettingsMenuCanvas, 2);
		SettingsMenuCanvas.gameObject.SetActive(true);
	}

	public void Close()
	{
		SettingsMenuCanvas.gameObject.SetActive(false);
		Reticle.gameObject.SetActive(false);
	}

	public void LoadMainMenu()
	{
		ScenesManager.LoadMenu();
	}
}
