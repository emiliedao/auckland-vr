using System.Collections;
using System.Collections.Generic;
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

	public Texture ReplaceImage(string path)
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
}
