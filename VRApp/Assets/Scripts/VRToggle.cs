using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class VRToggle : MonoBehaviour, IPointerClickHandler
{
	public Toggle toggle;
	private static bool enableVR = true;
	
	void Update()
	{
		toggle.isOn = enableVR;
	}

	/**
	 * Handles clicks on VR toggle
	 * This function is used instead of OnValueChanged because the toggle state is reset whenever scenes change
	 */
	public void OnPointerClick(PointerEventData eventData)
	{
		enableVR = !enableVR;
		if (enableVR)
		{
			StartCoroutine((EnableXR.SwitchToVR()));
		}

		else
		{
			StartCoroutine(EnableXR.SwitchTo2D());
		}
	}

}
