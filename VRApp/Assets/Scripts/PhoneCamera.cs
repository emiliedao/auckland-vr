using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{

	private WebCamTexture _backCam;
	private WebCamTexture _frontCam;
	private WebCamTexture _currentCam;
	private Texture _defaultBackground;
	private Texture _firstPicTexture;
	private Texture _secPicTexture;
	
	private bool _frontCamAvailable;
	private bool _backCamAvailable;
	private bool _picTaken;
	private bool _firstPicDone;

	private string _takeFirstPicture = "Take the first picture";
	private string _takeSecondPicture = "Take the second picture";
	private string _validate = "Validate?";
	private string _runVr = "Click to run VR";

	private UploadServer _uploadServer;
	private DownloadServer _downloadServer;

	public RawImage Background;
	public RawImage FirstPic;
	public AspectRatioFitter Fitter;
	public Button TakePicButton;
	
	// Buttons to validate or cancel a pic
	public Button ValidateButton;
	public Button CancelButton;
	
	// Buttons after both pics were taken
	public List<Button> actionsButtons;
	public Button StereoRenderingButton;
	public Button QuickStereoButton;
	public Button CancelAllButton;
	
	public Text TopBarText;

	private bool _uploaded;
	private bool _downloaded;
	
	void Start ()
	{
		UpdateButtons();
		actionsButtons = new List<Button>() { StereoRenderingButton, QuickStereoButton, CancelAllButton };
		ShowActionButtons(false);
		_defaultBackground = Background.texture;
		WebCamDevice[] devices = WebCamTexture.devices;

		if (devices.Length == 0)
		{
			Debug.Log("No camera detected");
			_frontCamAvailable = false;
			_backCamAvailable = false;
		}

		else
		{
			foreach (var device in devices)
			{
				if (!device.isFrontFacing)
				{
					_backCam = new WebCamTexture(device.name, Screen.width, Screen.height);
					_backCamAvailable = true;
				}

				else
				{
					_frontCam = new WebCamTexture(device.name, Screen.width, Screen.height);
					_frontCamAvailable = true;
				}
			}

			if (_backCamAvailable || _frontCamAvailable)
			{
				// Priority to back cam
				_currentCam = _backCamAvailable ? _backCam : _frontCam;
				_currentCam.Play();
				Background.texture = _currentCam;
			}
			

			else
			{
				Debug.Log("Unable to find camera");
				TopBarText.text = "Unable to find camera. Please check the camera authorization for the app.";
			}
			
			StartCoroutine(EnableXR.SwitchTo2D());
		}
	}
	
	void Update ()
	{
		// Quits scene
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ScenesManager.LoadMenu();
			StartCoroutine(EnableXR.SwitchToVR());
		}
		
		// Adapts screen to the device size & orientation
		if (_currentCam != null && !_picTaken)
		{
			UpdateScale();
			UpdateOrientation();
		}

		// Pictures were sent uploaded on the server
		if (_uploaded)
		{
			TopBarText.text = _uploadServer.GetInfo();
			if (UploadServer.Success())
			{
				DownloadResults();
				_downloaded = true;
			}
		}

		if (_downloaded)
		{
			TopBarText.text = _downloadServer.GetInfo();
			if (DownloadServer.Success())
			{
				TopBarText.text = "Downloaded ok";
			}
		}
	}

	/**
	 * Adapts the screen to the device scale
	 */
	public void UpdateScale()
	{
		float ratio = (float) _currentCam.width / (float)_currentCam.height;
		float scaleY = _currentCam.videoVerticallyMirrored ? -1f : 1f;
		Background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
	}

	/**
	 * Adapts the screen to the device orientation
	 */
	public void UpdateOrientation()
	{
		int orientation = -_currentCam.videoRotationAngle;
		Background.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
	}

	/**
	 * Handles click on take picture button
	 */
	public void TakePictureButton()
	{
		_picTaken = true;
		UpdateButtons();
		TopBarText.text = _validate;
		StartCoroutine(TakePicture());
	}

	/**
	 * Takes a picture by saving the current frame
	 */
	private IEnumerator TakePicture()
	{
		yield return new WaitForEndOfFrame(); 
		Texture2D photo = new Texture2D(_currentCam.width, _currentCam.height);
		photo.SetPixels(_currentCam.GetPixels());
		photo.Apply();
		
		Background.texture = photo;
	}
	
	/**
	 * Cancels the picture taken
	 */
	public void Cancel()
	{
		_picTaken = false;
		UpdateButtons();

		TopBarText.text = _firstPicDone ? _takeSecondPicture : _takeFirstPicture;
		Background.texture = _currentCam;
	}
	
	public void CancelAll()
	{
		_firstPicDone = false;
		ShowActionButtons(false);
		Cancel();
	}

	public void OpenInStereoViewer()
	{
		MpoViewer.SetTextures(_firstPicTexture, _secPicTexture);
		StartCoroutine(EnableXR.SwitchToVR());
		ScenesManager.Load("StereoViewer");
	}

	public void SendToQuickStereoServer()
	{
		GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/UploadServer"));
		_uploadServer = obj.GetComponent<UploadServer>();
		_uploadServer.SetImages(_firstPicTexture, _secPicTexture);
		_uploadServer.Upload();
		_uploaded = true;
	}


	private void DownloadResults()
	{
		GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/DownloadServer"));
		_downloadServer = obj.GetComponent<DownloadServer>();
		_downloadServer.Download("model");
	}

	/**
	 * Validates the picture taken
	 */
	public void Validate()
	{
		// First (left) picture
		if (!_firstPicDone)
		{
			_firstPicDone = true;
			FirstPic.gameObject.SetActive(true);

			_picTaken = false;
			UpdateButtons();
			FirstPic.texture = Background.texture;
			_firstPicTexture = Background.texture;
			Color color = FirstPic.color;
			FirstPic.color = new Color(color.r, color.g, color.b, 0.5f);
			Background.texture = _currentCam;
			TopBarText.text = _takeSecondPicture;
		}

		// Second (right) picture
		else
		{
			_secPicTexture = Background.texture;
			HidePicButtons();
			ShowActionButtons(true);
			TopBarText.text = _runVr;
		}
	}

	private void UpdateButtons()
	{
		TakePicButton.gameObject.SetActive(!_picTaken);
		ValidateButton.gameObject.SetActive(_picTaken);
		CancelButton.gameObject.SetActive(_picTaken);
	}

	private void ShowActionButtons(bool show)
	{
		foreach (var button in actionsButtons)
		{
			button.gameObject.SetActive(show);
		}
	}

	private void HidePicButtons()
	{
		TakePicButton.gameObject.SetActive(false);
		ValidateButton.gameObject.SetActive(false);
		CancelButton.gameObject.SetActive(false);
	}

}
