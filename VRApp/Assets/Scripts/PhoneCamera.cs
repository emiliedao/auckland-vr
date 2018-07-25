using System;
using System.Collections;
using System.Collections.Generic;
using QuickStereoServer;
using UnityEngine;
using UnityEngine.UI;
using Viewers;

/// <summary>
/// This class is used to take pictures with the device where the app is installed, and send them to te quick stereo server for processing
///  The sending part is done through the class QuickStereoServer/UploadServer
/// </summary>
public class PhoneCamera : MonoBehaviour
{

	private WebCamTexture _backCam;
	private WebCamTexture _frontCam;
	private WebCamTexture _currentCam;
	private Texture _firstPicTexture;
	private Texture _secPicTexture;
	
	private bool _frontCamAvailable;
	private bool _backCamAvailable;
	
	// Indicates if a picture has just been taken
	private bool _picTaken;
	// Indicates if the first picture was taken
	private bool _firstPicShot;

	// UI instructions
	private const string TakeFirstPic = "Take the first picture";
	private const string TakeSecondPic = "Take the second picture";
	private const string ValidatePic = "Validate?";
	private const string NextAction = "What's next?";
	private const string UnableFindCam = "Unable to find camera. Please check the camera authorization for the app.";
	private const string CheckAuthorization = "Please check your device authorization for taking pictures.";

	private UploadServer _uploadServer;

	public RawImage Background;
	public RawImage FirstPic;
	public AspectRatioFitter Fitter;
	public Button TakePicButton;
	
	// Buttons to validate or cancel a pic
	public Button ValidateButton;
	public Button CancelButton;
	
	// Buttons after both pics were taken
	public List<Button> ActionsButtons;
	public Button StereoRenderingButton;
	public Button QuickStereoButton;
	public Button CancelAllButton;
	
	public Text TopBarText;

	private bool _uploaded;
	private bool _downloaded;
	
	IEnumerator Start ()
	{
		// Does not seem to work on Android. Authorization must be done manually	
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
		if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
		{
			TopBarText.text = CheckAuthorization;
		}

		UpdateButtons();
		ActionsButtons = new List<Button>() { StereoRenderingButton, QuickStereoButton, CancelAllButton };
		ShowActionButtons(false);
		WebCamDevice[] devices = WebCamTexture.devices;

		// No camera detected
		if (devices.Length == 0)
		{
			Debug.Log("No camera detected");
			_frontCamAvailable = false;
			_backCamAvailable = false;
		}

		else
		{
			if (FindCamera(devices))
			{
				// Priority to back cam
				_currentCam = _backCamAvailable ? _backCam : _frontCam;
				_currentCam.Play();
				Background.texture = _currentCam;
			}
		

			else
			{
				Debug.Log("Unable to find camera");
				TopBarText.text = UnableFindCam;
			}
		
			StartCoroutine(EnableXR.SwitchTo2D());
		}		
	}

	/// <summary>
	/// Finds the back/front cameras and stores them as attributes 
	/// </summary>
	/// <param name="devices">Webcam devices detected</param>
	/// <returns>True if at least one camera was found (back or front)</returns>
	bool FindCamera(WebCamDevice[] devices)
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

		return (_backCamAvailable || _frontCamAvailable);
	}
	
	void Update ()
	{
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
			Debug.Log(_uploadServer.Success());
			if (_uploadServer.Success())
			{
				_uploaded = false;
				TopBarText.text = "Sent!";
				_uploadServer.ResetSuccess();
				ScenesManager.Load("ImportObj");
			}
		}
	}

	/// <summary>
	/// Adapts the screen to the device scale 
	/// </summary>
	private void UpdateScale()
	{
		Fitter.aspectRatio = (float) _currentCam.width / _currentCam.height;
		float scaleY = _currentCam.videoVerticallyMirrored ? -1f : 1f;
		Background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
	}

	/// <summary>
	/// Adapts the screen to the device orientation 
	/// </summary>
	private void UpdateOrientation()
	{
		int orientation = -_currentCam.videoRotationAngle;
		Background.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
	}

	/// <summary>
	/// Handles click on take picture button 
	/// </summary>
	public void TakePictureButton()
	{
		_picTaken = true;
		UpdateButtons();
		TopBarText.text = ValidatePic;
		StartCoroutine(TakePicture());
	}

	/// <summary>
	/// Takes a picture by saving the current frame 
	/// </summary>
	/// <returns></returns>
	private IEnumerator TakePicture()
	{
		yield return new WaitForEndOfFrame(); 
		Texture2D photo = new Texture2D(_currentCam.width, _currentCam.height);
		photo.SetPixels(_currentCam.GetPixels());
		photo.Apply();
		
		Background.texture = photo;
	}
	
	/// <summary>
	/// Cancels the picture taken 
	/// </summary>
	public void Cancel()
	{
		_picTaken = false;
		UpdateButtons();

		TopBarText.text = _firstPicShot ? TakeSecondPic : TakeFirstPic;
		Background.texture = _currentCam;
	}
	
	/// <summary>
	/// Cancels the two pictures taken
	/// This function is called when the user clicks on the "Cancel" button instead of choosing to see the pictures in stereo viewer or send them to the server 
	/// </summary>
	public void CancelAll()
	{
		_firstPicShot = false;
		ShowActionButtons(false);
		FirstPic.gameObject.SetActive(false);
		Cancel();
	}

	/// <summary>
	/// Initializes the stereo viewer with the two pictures taken and redirects to the stereo viewer scene
	/// This function is called when the user clicks on the "Stereo rendering" button after having taken the pictures  
	/// </summary>
	public void OpenInStereoViewer()
	{
		MpoViewer.SetTextures(_firstPicTexture, _secPicTexture);
		ScenesManager.Load("StereoViewer");
	}

	/// <summary>
	/// Sends the two pictures taken to the quick stereo server for processing
	/// This function is called when the user clicks on the "Quick stereo server" button after having taken the two pictures  
	/// </summary>
	public void SendToQuickStereoServer()
	{
		GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/UploadServer"));
		_uploadServer = obj.GetComponent<UploadServer>();
		_uploadServer.SetImages(_firstPicTexture, _secPicTexture);
		_uploadServer.ResetSuccess();
		_uploadServer.Upload();
		_uploaded = true;
	}

	/// <summary>
	/// Validates the picture taken 
	/// </summary>
	public void Validate()
	{
		// First (left) picture
		if (!_firstPicShot)
		{
			_firstPicShot = true;
			FirstPic.gameObject.SetActive(true);

			_picTaken = false;
			UpdateButtons();
			FirstPic.texture = Background.texture;
			_firstPicTexture = Background.texture;
			Color color = FirstPic.color;
			FirstPic.color = new Color(color.r, color.g, color.b, 0.5f);
			Background.texture = _currentCam;
			TopBarText.text = TakeSecondPic;
		}

		// Second (right) picture
		else
		{
			_secPicTexture = Background.texture;
			HidePicButtons();
			ShowActionButtons(true);
			TopBarText.text = NextAction;
		}
	}

	/// <summary>
	/// Displays or hide the buttons according to the taking pictures process stage 
	/// </summary>
	private void UpdateButtons()
	{
		TakePicButton.gameObject.SetActive(!_picTaken);
		ValidateButton.gameObject.SetActive(_picTaken);
		CancelButton.gameObject.SetActive(_picTaken);
	}

	/// <summary>
	/// Shows the different possible action buttons after the user has taken the two pictures
	/// The user can choose between "Stereo Rendering", "Quick stereo server" or "Cancel"  
	/// </summary>
	/// <param name="show"></param>
	private void ShowActionButtons(bool show)
	{
		foreach (var button in ActionsButtons)
		{
			button.gameObject.SetActive(show);
		}
	}

	/// <summary>
	/// Hides the buttons used to take a picture and validate or cancel the shooting 
	/// </summary>
	private void HidePicButtons()
	{
		TakePicButton.gameObject.SetActive(false);
		ValidateButton.gameObject.SetActive(false);
		CancelButton.gameObject.SetActive(false);
	}

}
