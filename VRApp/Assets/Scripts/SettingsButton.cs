using UnityEngine;

/**
 * This class is used to handle the settings button,
 * It is used in non-VR to replace the Cardboard button giving access to the settings menu
 */
public class SettingsButton : MonoBehaviour
{
	public Canvas canvas;
	
	void Start ()
	{
		// Show or hide settings button according to VR settings
		ScenesManager.ShowOrHideCanvas(canvas, false);
		Zoom.Update2DZoom();
	}

	public void ClickOnSettingsButton()
	{
		ScenesManager.LoadSettingsMenu();
	}

}
