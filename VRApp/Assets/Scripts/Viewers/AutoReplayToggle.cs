using UnityEngine;

namespace Viewers
{
	public class AutoReplayToggle : ToggleHandler {

		void Update()
		{
			Toggle.isOn = VideoController.AutoReplay;
		}

		protected override void OnToggleClick()
		{
			VideoController.AutoReplay = !VideoController.AutoReplay;
		}
	}
}
