using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Util {

	/**
	 * Saves a file into the specified directory
	 */
	public static void SaveFile(string dir, string filename, byte[] data)
	{
		string path = Application.dataPath + "/" + dir + "/" + filename;
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
			Debug.Log("Unable to save " + path);
		}
	}
}
