using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;

namespace Viewers
{
	/// <summary>
	/// This class is used in the menu to select an Mpo image in the device storage in order to display it in the mpo/stereo viewer
	/// </summary>
	public class MpoSelect : OpenBrowser {

		void Start () {
			InitFileBrowser(FileType.Mpo);
		}

		public void OpenBrowser()
		{
			StartCoroutine(OpenBrowserCoroutine("Load MPO"));
		}
		
		protected override void ActionResult()
		{
			List<Texture2D> textures = Mpo.GetMpoImages(FileBrowser.Result);

			if (textures != null)
			{
				MpoViewer.SetTextures(textures[0], textures[1]);
				ScenesManager.Load("StereoViewer");
			}
		}
	}
}
