using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.Video;

namespace Viewers
{
	/// <summary>
	/// This class is used to display a 360 video 
	/// </summary>
	public class VideoController : OpenBrowser
	{
		private VideoPlayer _videoPlayer;
		private bool _showMenu;
		private float _menuDistance = 10;
		private static bool _autoReplay = true;

		public static bool AutoReplay
		{
			get { return _autoReplay; }
			set { _autoReplay = value; }
		}

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
			InitFileBrowser(FileType.Video);
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

			_videoPlayer.isLooping = _autoReplay;
		}

		/// <summary>
		/// Pauses the video and displays the pause menu 
		/// </summary>
		private void ShowPauseMenu()
		{
			_videoPlayer.Pause();
			Reticle.gameObject.SetActive(true);
			// Sets the pause menu and file browser in front of the camera
			CanvasManager.FaceCamera(PauseMenuCanvas, _menuDistance);
			CanvasManager.FaceCamera(BrowserCanvas, _menuDistance);
			PauseMenuCanvas.gameObject.SetActive(true);
		}

		/// <summary>
		/// Hides pause menu and plays video 
		/// </summary>
		private void HidePauseMenu()
		{
			Reticle.gameObject.SetActive(false);
			PauseMenuCanvas.gameObject.SetActive(false);
			_videoPlayer.Play();
		}

		/// <summary>
		/// Click on play handler
		/// This function is triggered by clicking on "Play" button in the pause menu 
		/// </summary>
		public void ClickOnPlay()
		{
			_showMenu = false;
			HidePauseMenu();
		}

		/// <summary>
		/// Click on quit handler
		/// This function is triggered by clicking on "Quit" button in the pause menu
		/// </summary>
		public void ClickOnQuit()
		{
			ScenesManager.LoadMenu();
		}

		/// <summary>
		/// Click on change video handler 
		/// This function is triggered by clicking on "Change video" button in the pause menu
		///  It allows to select a new video to be played in the local device storage
		/// </summary>
		public void ClickOnChangeVideo()
		{
			StartCoroutine(OpenBrowserCoroutine("Load Video"));
		}

		protected override void ActionResult(string path)
		{
			_videoPlayer.url = path;
			_videoPlayer.Pause();
		}
	}
}
