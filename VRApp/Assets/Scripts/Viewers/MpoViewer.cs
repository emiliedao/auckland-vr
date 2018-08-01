using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace Viewers
{
	/// <summary>
	/// This class is used to handle the Mpo/Stereo viewer
	/// It allows to initialize the picture displayed in the viewer or change the picture 
	/// </summary>
	public class MpoViewer : MonoBehaviour
	{
		public Canvas MpoViewerLeft;
		public Canvas MpoViewerRight;

		public RawImage LeftImage;
		public RawImage RightImage;

		private static Texture _leftTexture;
		private static Texture _rightTexture;
	
		void Start()
		{
			if (!XRSettings.enabled)
			{
				EnableXR.ResetCameras();
				StartCoroutine(EnableXR.SwitchToVR());	
			}
		
			// If textures were modified somewhere else (basically through the menu), updates them in the viewer
			if (_leftTexture != null && _rightTexture != null)
			{
				LeftImage.texture = _leftTexture;
				RightImage.texture = _rightTexture;
			}
		}

		/// <summary>
		/// Changes the picture displayed in the viewer by setting the left and right textures 
		/// </summary>
		/// <param name="left">Left texture</param>
		/// <param name="right">Right texture</param>
		public static void SetTextures(Texture left, Texture right)
		{
			_leftTexture = left;
			_rightTexture = right;
		}

		public static Texture GetTexture()
		{
			return _leftTexture;
		}

	}
}
