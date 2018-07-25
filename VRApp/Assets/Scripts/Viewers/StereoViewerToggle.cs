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
namespace Viewers
{
	/**
	 * This class is used to update the stereo viewer toggles
	 * The main event method OnPointerClick is called when the user selects the toggles "Left/right" or "Top/Bottom" in order to display the stereo image properly
	 * 
	 * OnPointerClick is used instead of the native UI.Toggle.onValueChanged because the latter is called even when modifying the value in the script, and it can leads to many problems, while the former is called only when the user interacts with the toggles
	 */
	public class StereoViewerToggle : ToggleHandler {

		protected override void OnToggleClick()
		{
			StereoViewer360.StereoToggle toggle;
			if (Toggle.name == "ToggleLeftRight")
			{
				toggle = StereoViewer360.StereoToggle.LeftRight;
			}
		
			else
			{
				toggle = StereoViewer360.StereoToggle.TopBottom;
			}

			StereoViewer360.OnToggleChange(toggle);
		}
	}
}
