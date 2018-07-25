using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Util {

	/**
	 * 
	 */
	/// <summary>
	/// Saves a file into the specified directory 
	/// </summary>
	/// <param name="dir">Relative directory name to save the file into (Do not specify the whole application path since</param>
	/// <param name="filename"></param>
	/// <param name="data"></param>
	public static void SaveFile(string dir, string filename, byte[] data)
	{
		string path = "";
		if (string.IsNullOrEmpty(dir))
		{
			path = Application.persistentDataPath + "/" + filename;
		}
		
		else
		{
			path = Application.persistentDataPath + "/" + dir + "/" + filename;	
		}
		
		try
		{
			var file = File.Open(path, FileMode.Create);
			var binary = new BinaryWriter(file);
			binary.Write(data);
			file.Close();
			Debug.Log("Saved file at " + path);
		}

		catch (Exception e)
		{
			Debug.Log(e.Message);
			Debug.Log("Unable to save " + path);
		}
	}
}
