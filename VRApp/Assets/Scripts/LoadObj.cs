using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * This class is used to manage the Import OBJ scene
 * In this scene, the user can import small .obj files with material (.mtl) and texture (.tga, .jpg, .png) and interact with them through a control panel
 */
public class LoadObj : MonoBehaviour
{
	private static Vector3 _defaultPosition;
	
	void Start () {
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Obj", ".obj"));
		FileBrowser.HideDialog();
	}
	
	public void Open()
	{
		StartCoroutine(OpenBrowserCoroutine());
	}
	
	/**
	 * Opens a file browser to select the .obj file to load
	 */
	IEnumerator OpenBrowserCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(false, null, "Load OBJ", "OK");
		Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
		if (!FileBrowser.Success) yield break;
	
		GameObject obj = OBJLoader.LoadOBJFile(FileBrowser.Result);
		InitObject(obj);
		Debug.Log("loaded");
		
//		CreateSpheres(obj);
	}
	
//	public bool V3Equal(Vector3 a, Vector3 b){
//		return Vector3.SqrMagnitude(a - b) < 1f;
//	}
	
//	public void CreateSpheres(GameObject obj)
//	{
//		var vertices = obj.GetComponent<MeshFilter>().sharedMesh.vertices;
//		Debug.Log(vertices.Length);
//		GameObject spheresList = new GameObject();
//		List<GameObject> spheres = new List<GameObject>();
//		int count = 0;
//		foreach (Vector3 vert in vertices)
//		{
//			if (spheres.Any(sph => V3Equal(sph.transform.position, vert))) continue;
//			count++;
//			Debug.Log(count);
//			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//			sphere.transform.position = vert;
//			sphere.transform.parent = spheresList.transform;
//			spheres.Add(sphere);
//		}
//	}

	/**
	 * Initializes the object loaded, by setting its position, scale, and adding a control panel triggered on click on the object
	 */
	private void InitObject(GameObject obj)
	{	
		// Randomly sets the new default position
		ChangeDefaultPosition();
		
		// Sets the object's location and scale
		obj.transform.position = new Vector3(_defaultPosition.x, _defaultPosition.y, _defaultPosition.z);
		obj.transform.localScale = new Vector3(20f, 20f, 20f);
		
		// Adds a panel controller to the object
		AddController(obj);
		
		// Enables trigger event on the object
		AddTriggerEvent(obj);
	}

	/**
	 * Adds a trigger event handler to the object
	 */ 
	private void AddTriggerEvent(GameObject obj)
	{
		obj.AddComponent<BoxCollider>();
		obj.AddComponent<EventTrigger>();
		
		// Adds a new entry for PointerClick event
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener((eventData) =>
		{
			Debug.Log("Click on " + obj.name);
			OnObjClick(obj);
		});
		obj.GetComponent<EventTrigger>().triggers.Add(entry);
		
	}

	/**
	 * Shows the control panel when clicking on the object
	 * If the control panel is already open, hides it on click 
	 */
	private void OnObjClick(GameObject obj)
	{
		var objectController = obj.transform.GetChild(0).GetComponent<ObjectController>();
		if (objectController.IsActive())
		{
			objectController.Hide();
		}

		else
		{
			objectController.Show();
		}
	}

	/**
	 * Chooses randomly a new default position for the next object loaded
	 */
	private void ChangeDefaultPosition()
	{
		Vector3 pos = new Vector3();
		System.Random rand = new System.Random();
		pos.x = rand.Next(-100, 100);
		pos.y = rand.Next(-100, 0);
		pos.z = rand.Next(100, 150);
		_defaultPosition = pos;
	}

	/**
	 * Adds the control panel to the object
	 */
	private void AddController(GameObject obj)
	{
		GameObject controllerPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/ObjController"));
		
		// Sets controller as a child of the objects
		controllerPrefab.transform.SetParent(obj.transform);
		
		ObjectController objectController = controllerPrefab.GetComponent<ObjectController>();
		objectController.SetObject(obj);
	}

	/**
	 * Quits the import obj scene and returns to the main menu
	 */
	public void Quit()
	{
		ScenesManager.LoadMenu();
	}
	
}
