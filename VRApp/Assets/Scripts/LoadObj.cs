using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadObj : MonoBehaviour
{
	void Start () {
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Obj", ".obj"));
		FileBrowser.HideDialog();
	}
	
	public void Open()
	{
		StartCoroutine(OpenBrowserCoroutine());
	}
	
	IEnumerator OpenBrowserCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(false, null, "Load OBJ", "OK");
		Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
		if (!FileBrowser.Success) yield break;
	
		GameObject obj = OBJLoader.LoadOBJFile(FileBrowser.Result);
		Debug.Log(obj.name);
		InitObject(obj);
		AddTriggerEvent(obj);
	}

	private void InitObject(GameObject obj)
	{
		obj.transform.position = new Vector3(50, 0, 100);
		obj.transform.localScale = new Vector3(30f, 30f, 30f);
		obj.transform.Rotate(45f, 0, 0);
	}

	private void AddTriggerEvent(GameObject obj)
	{
		Debug.Log("Add trigger event");
		obj.AddComponent<BoxCollider>();
		obj.AddComponent<EventTrigger>();
//		obj.AddComponent<ObjEventTrigger>();
		
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener((eventData) =>
		{
			Debug.Log("Click on " + obj.name);
		});
		obj.GetComponent<EventTrigger>().triggers.Add(entry);
		Debug.Log("done");
	}
	
}
