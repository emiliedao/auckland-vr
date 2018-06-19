using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ZoomSlider : MonoBehaviour
{
	public Slider Slider;
	private bool start = true;

	public void Start()
	{
		Slider.value = Zoom.Factor;
		start = false;
	}

 	public void OnValueChanged()
	{ 
		if (!start)
		{
			Zoom.Factor = (Slider.value);
		}
	}
	

}
