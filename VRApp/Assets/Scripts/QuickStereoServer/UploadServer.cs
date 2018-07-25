#if UNITY_EDITOR
#endif
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace QuickStereoServer
{
	/// <summary>
	/// This class is used to upload stereo images taken from a device camera to the QuickStereo server
	/// </summary>
	public class UploadServer : ServerConnect
	{
		private const string UploadUrl = "http://www.ivs.auckland.ac.nz/quick_stereo/upload_file.php";

		private Texture2D _leftTexture;
		private Texture2D _rightTexture;

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
			_leftTexture = (Texture2D)left;
			_rightTexture = (Texture2D)right;
		}
	

		/// <summary>
		/// Uploads left image, right image and info textfile to the server
		/// </summary>
		public void Upload()
		{
			UploadImage(_leftTexture, LeftFile, UploadType.Left);
			UploadImage(_rightTexture, RightFile, UploadType.Right);
			UploadTextFile();
		}

		/// <summary>
		/// Converts a texture into a image and uploads it on the server 
		/// </summary>
		/// <param name="texture">Texture to be converted and send to the server</param>
		/// <param name="filename">Name of the image file sent</param>
		/// <param name="uploadType">Type of file sent</param>
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

		/// <summary>
		/// Uploads a text file on the quick stereo server
		/// According to the server requirements, the file contains a timestamp followed by a * and by the device unique identifier
		/// </summary>
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

		/// <summary>
		/// Uploads a file on the quick stereo server
		/// </summary>
		/// <param name="bytes">File bytes sent to the server</param>
		/// <param name="filename">Name of the file sent</param>
		/// <param name="mimeType">Mime type of file sent</param>
		/// <param name="uploadType">Type of file sent</param>
		/// <returns></returns>
		private IEnumerator Upload(byte[] bytes, string filename, string mimeType, UploadType uploadType)
		{
			WWWForm form = new WWWForm();
			form.AddBinaryData("file", bytes, filename, mimeType);
			_info = filename;
		
			Debug.Log("Upload " + filename);
			using (UnityWebRequest www = UnityWebRequest.Post(UploadUrl, form))
			{
				yield return www.SendWebRequest();
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
					Debug.Log(uploadType + " = " + _success[(int)uploadType]);
					_info = " sent " + filename;
					_success[(int)uploadType] = true;
				}
			}
		}

		/// <summary>
		/// Generates a timestamp followed by a random number
		/// This function is used to send a correct textfile to the server as part of the upload requirements
		/// </summary>
		/// <returns>The generated timestamp</returns>
		private string GetTimestamp()
		{
			int rand = Random.Range(1000, 2000);
			return DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + rand.ToString();
		}

		public override bool Success()
		{
			return _success[0] && _success[1] && _success[2];
		}

		public override void ResetSuccess()
		{
			for (int i = 0; i < 3; i++)
			{
				_success[i] = false;
			}
		}
	
	}
}
