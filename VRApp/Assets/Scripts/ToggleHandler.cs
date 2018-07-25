using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ToggleHandler : MonoBehaviour, IPointerClickHandler
{
	public Toggle Toggle;
	
	public void OnPointerClick(PointerEventData eventData)
	{
		OnToggleClick();
	}

	protected abstract void OnToggleClick();
}
