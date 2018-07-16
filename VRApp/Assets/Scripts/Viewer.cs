using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;

public class Viewer
{
	private string _sceneName;
	
	public string SceneName
	{
		get { return _sceneName; }
	}

	private List<Material> _materials;
	private string _defaultTexture;
	private int _maxSize;

	public Viewer(string sceneName, List<Material> materials, string defaultTexture, int maxSize)
	{
		_sceneName = sceneName;
		_materials = materials;
		_defaultTexture = defaultTexture;
		_maxSize = maxSize;
	}
	

	public Texture InitTexture()
	{
		Texture2D texture = (Texture2D) Resources.Load(_defaultTexture);
		foreach (var m in _materials)
		{
			m.mainTexture = texture;
		}
		return texture;
	}

	/**
	 * Replaces texture 
	 */
	public Texture ReplaceTexture(string path)
	{
		// Create Texture from selected image
		var texture = NativeGallery.LoadImageAtPath(path, _maxSize);
		if (texture == null)
		{
			Debug.Log("Couldn't load texture from " + FileBrowser.Result);
		}

		else
		{
			foreach (var m in _materials)
			{
				m.mainTexture = texture;
			}
		}
		return texture;
	}

	public List<Texture2D> ReplaceMpoTextures(string path)
	{
		// Extracts 2 images from the MPO file
		var textures = Mpo.GetMpoImages(path);
		
		// Updates left and right sphere textures
		_materials[0].mainTexture = textures[0];
		_materials[1].mainTexture = textures[1];
		
		return textures;
	}
	
	
}
