using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;

namespace Viewers
{
	/// <summary>
	/// This class is used in the menu to select an Mpo image in the device storage in order to display it in the mpo/stereo viewer
	/// </summary>
	public class MpoSelect : OpenBrowser
	{
		public RawImage ImagePreview;
		
		void Start () {
			InitFileBrowser(FileType.Mpo);

			if (MpoViewer.GetTexture() == null)
			{
				// Initializes the viewer with default textures
				Texture left = (Texture) Resources.Load("360Viewer/left");
				Texture right = (Texture) Resources.Load("360Viewer/right");
				MpoViewer.SetTextures(left, right);
			}

			ImagePreview.texture = MpoViewer.GetTexture();
		}

		public void OpenBrowser()
		{
			StartCoroutine(OpenBrowserCoroutine("Load MPO"));
		}
		
		protected override void ActionResult(string path)
		{
			List<Texture2D> textures = Mpo.GetMpoImages(path);

			if (textures != null)
			{
				MpoViewer.SetTextures(textures[0], textures[1]);
				ImagePreview.texture = textures[0];
			}
		}
	}
}
