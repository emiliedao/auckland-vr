using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
	private VideoPlayer _videoPlayer;
	private bool _showMenu;
	private float _menuDistance = 10;
	
	public GvrReticlePointer Reticle;
	public Canvas PauseMenuCanvas;
	public Canvas BrowserCanvas;

	void Awake()
	{
		_videoPlayer = GetComponent<VideoPlayer>();
	}

	void Start()
	{
		Reticle.gameObject.SetActive(false);
		PauseMenuCanvas.gameObject.SetActive(false);
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Videos", ".mp4"));
		FileBrowser.HideDialog();
	}
	
	void Update () {
		// Shows pause menu on Cardboard click or Space key down
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) 
		{
			if (!_showMenu)
			{
				_showMenu = true;
				ShowPauseMenu();
			}
		}
	}

	/**
	 * Pauses the video and displays the pause menu
	 */
	private void ShowPauseMenu()
	{
		_videoPlayer.Pause();
		Reticle.gameObject.SetActive(true);
		// Sets the pause menu and file browser in front of the camera
		ScenesManager.CanvasFaceCamera(PauseMenuCanvas, _menuDistance);
		ScenesManager.CanvasFaceCamera(BrowserCanvas, _menuDistance);
		PauseMenuCanvas.gameObject.SetActive(true);
	}

	/**
	 * Hides pause menu and plays video
	 */
	private void HidePauseMenu()
	{
		Reticle.gameObject.SetActive(false);
		PauseMenuCanvas.gameObject.SetActive(false);
		_videoPlayer.Play();
	}

	/**
	 * Click on play handler
	 * This function is triggered by clicking on "Play" button in the pause menu
	 */
	public void ClickOnPlay()
	{
		_showMenu = false;
		HidePauseMenu();
	}

	/**
	 * Click on quit handler
	 * This function is triggered by clicking on "Quit" button in the pause menu
	 */
	public void ClickOnQuit()
	{
		ScenesManager.LoadMenu();
	}

	/**
	 * Click on change video handler
	 * This function is triggered by clicking on "Change video" button in the pause menu
	 * It allows to select a new video to be played in the local device storage
	 */
	public void ClickOnChangeVideo()
	{
		StartCoroutine(OpenBrowserCoroutine());
	}

	/**
	 * Opens a file browser to select a video in the device storage
	 */
	IEnumerator OpenBrowserCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(false, null, "Load Video", "OK");
		Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
		if (!FileBrowser.Success) yield break;

		_videoPlayer.url = FileBrowser.Result;
		_videoPlayer.Pause();
	}

}
