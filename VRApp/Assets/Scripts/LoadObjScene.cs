using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;

public class LoadObjScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Obj", ".obj"));
		FileBrowser.HideDialog();
	}

	public void OpenScene()
	{
		StartCoroutine(OpenSceneCoroutine());
	}
	
	IEnumerator OpenSceneCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(false, null, "Load OBJ", "OK");
		Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
		if (!FileBrowser.Success) yield break;
	
		GameObject objScene = OBJLoader.LoadOBJFile(FileBrowser.Result);
		InitScene(objScene);
	}

	private void InitScene(GameObject scene)
	{
		scene.transform.localScale = new Vector3(100f, 100f, 100f);
	} 
}
