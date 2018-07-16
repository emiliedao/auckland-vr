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
public class StereoViewerToggle : MonoBehaviour, IPointerClickHandler {

	public Toggle Toggle;
	
	public void OnPointerClick(PointerEventData eventData)
	{
		Viewer360.StereoToggle toggle;
		if (Toggle.name == "ToggleTopBottom")
		{
			toggle = Viewer360.StereoToggle.TopBottom;
		}
		
		else if (Toggle.name == "ToggleLeftRight")
		{
			toggle = Viewer360.StereoToggle.LeftRight;
		}

		else
		{
			toggle = Viewer360.StereoToggle.Mpo;
		}
		Viewer360.OnToggleChange(toggle);
	}
}
