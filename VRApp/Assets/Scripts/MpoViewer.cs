
using UnityEngine;
using UnityEngine.UI;

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
		if (_leftTexture != null && _rightTexture != null)
		{
			LeftImage.texture = _leftTexture;
			RightImage.texture = _rightTexture;
		}

		else
		{
			LeftImage.texture = (Texture) Resources.Load("360Viewer/left");
			RightImage.texture = (Texture) Resources.Load("360Viewer/right");
		}
	}

	public static void SetTextures(Texture left, Texture right)
	{
		_leftTexture = left;
		_rightTexture = right;
	}

}
