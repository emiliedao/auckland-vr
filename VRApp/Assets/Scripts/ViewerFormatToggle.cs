using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * This class is used to handle the click event on the viewer format toggles
 * 
 * The click event is treated instead of native OnValueChanged which causes several issues (not only triggered by the user but also from the scripts)
 * 
 * The toggles attached to this script are used to choose the stereoscopic images format (left/right or up/down)
 */
public class ViewerFormatToggle : MonoBehaviour, IPointerClickHandler {

	public Toggle Toggle;
	
	public void OnPointerClick(PointerEventData eventData)
	{
		bool upDown = (Toggle.name == "ToggleUpDown");
		Viewer360.OnToggleChange(upDown);
	}
}
