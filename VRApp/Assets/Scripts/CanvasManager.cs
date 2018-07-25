using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	private bool _pointerInside;

	public bool PointerInside
	{
		get { return _pointerInside; }
		set { _pointerInside = value; }
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_pointerInside = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_pointerInside = false;
	}
	
	/// <summary>
	/// Sets the canvas in front of the camera
	/// </summary>
	/// <param name="canvas"></param>
	/// <param name="distance"></param>
	public static void FaceCamera(Canvas canvas, float distance)
	{
		canvas.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
		canvas.transform.rotation = Camera.main.transform.rotation;
	}
	
	/// <summary>
	/// Displays a canvas
	/// </summary>
	/// <param name="canvas"></param>
	public static void ShowCanvas(Canvas canvas)
	{
		var canvasGroup = canvas.GetComponent<CanvasGroup>();
		canvasGroup.alpha = 1f;
		canvasGroup.blocksRaycasts = true;
	}

	
	/// <summary>
	/// Hides a canvas
	/// </summary>
	/// <param name="canvas"></param>
	public static void HideCanvas(Canvas canvas)
	{
		var canvasGroup = canvas.GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = false;
	}
}
