using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if UNITY_EDITOR
	using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

/**
 * This class is used to upload stereo images taken from a device camera to the QuickStereo server
 */
public class UploadServer : MonoBehaviour
{
	private const string UploadUrl = "http://www.ivs.auckland.ac.nz/quick_stereo/upload_file.php";

	private Texture2D LeftTexture;
	private Texture2D RightTexture;

	private const string LeftFile = "left.jpg";
	private const string RightFile = "right.jpg";
	private const string UploadFile = "upload.txt";

	private string _info;
	private UploadType _status;

	private static bool[] _success = new bool[3] {false, false, false};

	private enum UploadType
	{
		Left, Right, Text
	}
	
	public void SetImages(Texture left, Texture right)
	{
		LeftTexture = (Texture2D)left;
		RightTexture = (Texture2D)right;
	}
	
	/**
	 * Uploads left image, right image and info textfile to the server
	 */
	public void Upload()
	{
		UploadImage(LeftTexture, LeftFile, UploadType.Left);
		UploadImage(RightTexture, RightFile, UploadType.Right);
		UploadTextFile();
	}

	/**
	 * Converts a texture into a image and uploads it on the server
	 */
	private void UploadImage(Texture2D texture, string filename, UploadType uploadType)
	{
		byte[] image = texture.EncodeToJPG();
		Debug.Log(filename + " encoded");
		
		if (image != null)
		{
			Debug.Log("not null");
			StartCoroutine(Upload(image, filename, "image/jpeg", uploadType));	
		}
	}

	/**
	 * Uploads a text file on the server
	 * According to the server requirements, the file contains a timestamp followed by a * and by the device unique identifier
	 */
	private void UploadTextFile()
	{
		string content = GetTimestamp() + "*" + SystemInfo.deviceUniqueIdentifier;

		string path = Application.persistentDataPath + "/" + UploadFile;
		Debug.Log(path);
		
		// Writes timestamp into upload file
		File.WriteAllText(path, content);
		byte[] textBytes = File.ReadAllBytes(path);

		StartCoroutine(Upload(textBytes, UploadFile, "text/plain", UploadType.Text));
	}

	/**
	 * Uploads a file on the server
	 * @bytes File bytes sent to the server
	 * @filename Name of the file sent
	 * @type 
	 */
	private IEnumerator Upload(byte[] bytes, string filename, string type, UploadType uploadType)
	{
		WWWForm form = new WWWForm();
		form.AddBinaryData("file", bytes, filename, type);
		_info = filename;
		
		Debug.Log("Upload " + filename);
		using (UnityWebRequest www = UnityWebRequest.Post(UploadUrl, form))
		{
			yield return www.SendWebRequest();
			Debug.Log("ok");
			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log("Upload error");
				Debug.Log(www.error);
				_info = " www error";
				ResetSuccess();
			}
			else
			{
				Debug.Log("Successfully sent " + filename);
				Debug.Log(www.downloadHandler.text);
				_info = " sent " + filename;
				_success[(int)uploadType] = true;
			}
		}

		if (Success())
		{
			ResetSuccess();	
		}
	}

	private string GetTimestamp()
	{
		int rand = Random.Range(1000, 2000);
		return DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + rand.ToString();
	}

	public string GetInfo()
	{
		return _info;
	}

	public static bool Success()
	{
		return _success[0] && _success[1] && _success[3];
	}

	private void ResetSuccess()
	{
		for (int i = 0; i < 3; i++)
		{
			_success[i] = false;
		}
	}
	
}
