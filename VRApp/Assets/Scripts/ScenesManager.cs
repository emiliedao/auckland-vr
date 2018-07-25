using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

/**
 * This class is used as a unique entry point for loading scenes
 * This allows to correclty set up different parameters (zoom...) when changing scenes 
 */
public static class ScenesManager
{
	public const string Menu = "MainMenu";

	private static string _currentScene = Menu;
	public static string CurrentScene
	{
		get { return _currentScene; }
	}

	public static string CurrentMenuCanvas { get; set; }

	public static bool SettingsOpen;

	/**
	 * Loads a scene
	 */
	public static void Load(string sceneName)
	{
		_currentScene = sceneName;
		SceneManager.LoadScene(sceneName);
	}

	/**
	 * Loads menu scene
	 */
	public static void LoadMenu()
	{
		Zoom.ResetZoom();
		SettingsOpen = false;
		Load(Menu);
		
	}

	/**
	 * Loads settings menu scene
	 * Zoom is temporarily reset and gets back to its value if scene is not quitted
	 */
	public static void LoadSettingsMenu()
	{
		SettingsOpen = true;
		XRDevice.fovZoomFactor = 1;
		SceneManager.LoadScene("SettingsMenuScene");
	}

	/**
	 * Closes settings menu and returns to last open scene
	 */
	public static void CloseSettingsMenu()
	{
		SettingsOpen = false;
		XRDevice.fovZoomFactor = Zoom.Factor;
		SceneManager.LoadScene(_currentScene);
	}

	
	/**
	 * Shows or hides a canvas group
	 * @showInVr is true if the canvas group must be shown while VR is enabled
	 */
	public static void ShowOrHideCanvas(Canvas canvas, bool showInVr)
	{
		if ((showInVr && XRSettings.enabled) || (!showInVr && !XRSettings.enabled))
		{
			CanvasManager.ShowCanvas(canvas);
		}

		else
		{
			CanvasManager.HideCanvas(canvas);
		}
	}

	/**
	 * Quits application
	 */
	public static void Quit()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
            Debug.Log("Quit");
            Application.Quit();
		#endif
	}
	
}
