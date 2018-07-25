using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
	public GameObject Obj;
	private Transform _controller;

	private const int Offset = 10;
	private Vector3 _initPosition;
	private float _initScale = 0.05f;

	public Slider RotateXSlider;
	public Slider RotateYSlider;
	public Slider RotateZSlider;

	public Text TopBarText;

	public void Start()
	{
		_controller = Obj.transform.GetChild(0);
		Relocate();
		_controller.transform.localScale = new Vector3(_initScale, _initScale, _initScale);
		TopBarText.text = Obj.name;
	}

	public void LateUpdate()
	{
		// Prevents the controller panel to be modified when the object is translated/rotated
		_controller.position = _initPosition;
		
		// Always faces the camera
		_controller.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
		
	}

	public void SetObject(GameObject gameObject)
	{
		Obj = gameObject;
	}

	/**
	 * Translates the object on the given direction
	 */
	public void Translate(string direction)
	{
		Vector3 pos = Obj.transform.position;

		switch (direction)
		{
			case "up":
				pos.y += Offset;
				break;
				
			case "down":
				pos.y -= Offset;
				break;
				
			case "left":
				pos.x -= Offset;
				break;
				
			case "right":
				pos.x += Offset;
				break;
		}

		Obj.transform.position = new Vector3(pos.x, pos.y, pos.z);
	}

	/**
	 * Rotates the object around the given axis
	 */
	public void Rotate(string axis)
	{
		Obj.transform.Rotate(RotateXSlider.value, RotateYSlider.value, RotateYSlider.value);
		Vector3 rotation = new Vector3();
		
		switch (axis)
		{
			case "x":
				rotation.x = RotateXSlider.value;
				break;
				
			case "y":
				rotation.y = RotateYSlider.value;
				break;
				
			case "z":
				rotation.z = RotateZSlider.value;
				break;
		}

		Obj.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
	}

	public void Expand()
	{
		Rescale(+1);
	}

	public void Collapse()
	{
		Rescale(-1);
	}

	private void Rescale(float value)
	{
		// Detaches the controller to avoid affecting its scale
		_controller.parent = null;
		
		Vector3 scale = Obj.transform.localScale;
		Obj.transform.localScale = new Vector3(scale.x + value, scale.y + value, scale.z + value);
		
		// Reattaches it
		_controller.parent = Obj.transform;
	}
	
	/**
	 * Returns true if the controller panel is active
	 */
	public bool IsActive()
	{
		return _controller.gameObject.activeSelf;
	}

	/**
	 * Shows the controller panel
	 */
	public void Show()
	{
		_controller.gameObject.SetActive(true);
		Relocate();
	}

	/**
	 * Hides the controller panels
	 */
	public void Hide()
	{
		_controller.gameObject.SetActive(false);
	}

	/**
	 * Relocates the controller panel near the object
	 */
	private void Relocate()
	{
		_controller.localPosition = new Vector3(-3, 1, 0);
		_initPosition = _controller.position;
	}


}
