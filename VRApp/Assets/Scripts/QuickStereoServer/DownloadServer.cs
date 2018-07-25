using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace QuickStereoServer
{
	/// <summary>
	/// This class is used to download an object (.obj + .mtl + .jpg files) from the quick stereo server
	/// </summary>
	public class DownloadServer : ServerConnect
	{
		private const string DownloadUrl = "http://www.ivs.auckland.ac.nz/quick_stereo/upload_stereo/";
		private static int _success = 0;
	
		/// <summary>
		/// Downloads 3 files from the server: object (.obj), material (.mtl) and texture (.jpg) 
		/// </summary>
		/// <param name="filename">The file name without extension</param>
		public void Download(string filename)
		{
			string[] files = { filename + ".jpg", filename + ".obj", filename + ".mtl" };
			foreach (var file in files)
			{
				StartCoroutine(DownloadFile(file));
			}
		}
	
		/// <summary>
		/// Downloads a file from the server and stores it 
		/// </summary>
		/// <param name="filename">Name of the file to download</param>
		/// <returns></returns>
		IEnumerator DownloadFile(string filename) {
			UnityWebRequest www = UnityWebRequest.Get(DownloadUrl + filename);
			yield return www.SendWebRequest();
 
			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log("Download error");
				Debug.Log(www.error);
				_success = 0;
				Info = "Download error";
			}
		
			else {
				Debug.Log("Downloaded " + filename);
				// Retrieves results as binary data
				byte[] results = www.downloadHandler.data;
				Util.SaveFile("", filename, results);
				_success++;
				Info = "Downloaded " + filename;
				Debug.Log("success : " + _success);
			}
		}

		/// <summary>
		/// Indicates if the three files (obj, mtl, jpg) were correclty downloaded from the server 
		/// </summary>
		/// <returns></returns>
		public override bool Success()
		{
			return _success == 3;
		}

		/// <summary>
		/// Resets the success status once the downloaded results have been used 
		/// </summary>
		public override void ResetSuccess()
		{
			_success = 0;
		}
	
	}
}
