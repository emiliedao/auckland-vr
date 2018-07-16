using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadServer : MonoBehaviour
{

	private const string DownloadUrl = "http://www.ivs.auckland.ac.nz/quick_stereo/upload_stereo/";
	private static int _success;
	private string _info;

	void Start()
	{
		Debug.Log("yo");
		_success = 0;
	}

	/**
	 * Downloads 3 files from the server: object (.obj), material (.mtl) and texture (.jpg)
	 * @filename The file name without extension
	 */
	public void Download(string filename)
	{
		string[] files = { filename + ".jpg", filename + ".obj", filename + ".mtl" };
		foreach (var file in files)
		{
			Debug.Log("Downloading " + file);
			StartCoroutine(DownloadFile(file));
		}
	}
	
	/**
	 * Downloads a file from the server and stores it in the Resources/QuickStereo folder
	 */
	IEnumerator DownloadFile(string filename) {
		UnityWebRequest www = UnityWebRequest.Get(DownloadUrl + filename);
		yield return www.SendWebRequest();
 
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log("Download error");
			Debug.Log(www.error);
			_success = 0;
			_info = "Download error";
		}
		
		else {
			Debug.Log("Downloaded" + filename);
			// Or retrieve results as binary data
			byte[] results = www.downloadHandler.data;
			Util.SaveFile("Resources/QuickStereo", filename, results);
			_success++;
			_info = "Downloaded " + filename;
		}
	}


	public string GetInfo()
	{
		return _info;
	}

	public static bool Success()
	{
		return _success == 3;
	}
	
}
