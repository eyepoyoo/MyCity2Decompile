using System;
using UnityEngine;

namespace UnitySampleAssets.CrossPlatformInput.PlatformSpecific
{
	public class MobileInput : VirtualInput
	{
		private void AddButton(string name)
		{
			CrossPlatformInputManager.RegisterVirtualButton(new CrossPlatformInputManager.VirtualButton(name));
		}

		private void AddAxes(string name)
		{
			new CrossPlatformInputManager.VirtualAxis(name);
		}

		public override float GetAxis(string name, bool raw)
		{
			return (!virtualAxes.ContainsKey(name)) ? 0f : virtualAxes[name].GetValue;
		}

		public override void SetButtonDown(string name)
		{
			if (!virtualButtons.ContainsKey(name))
			{
				AddButton(name);
			}
			virtualButtons[name].Pressed();
		}

		public override void SetButtonUp(string name)
		{
			if (!virtualButtons.ContainsKey(name))
			{
				AddButton(name);
			}
			try
			{
				virtualButtons[name].Released();
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception: " + ex.Message + " for item: " + name);
			}
		}

		public override void SetAxisPositive(string name)
		{
			if (!virtualAxes.ContainsKey(name))
			{
				AddAxes(name);
			}
			virtualAxes[name].Update(1f);
		}

		public override void SetAxisNegative(string name)
		{
			if (!virtualAxes.ContainsKey(name))
			{
				AddAxes(name);
			}
			virtualAxes[name].Update(-1f);
		}

		public override void SetAxisZero(string name)
		{
			if (!virtualAxes.ContainsKey(name))
			{
				AddAxes(name);
			}
			virtualAxes[name].Update(0f);
		}

		public override void SetAxis(string name, float value)
		{
			if (!virtualAxes.ContainsKey(name))
			{
				AddAxes(name);
			}
			virtualAxes[name].Update(value);
		}

		public override bool GetButtonDown(string name)
		{
			if (virtualButtons.ContainsKey(name))
			{
				return virtualButtons[name].GetButtonDown;
			}
			AddButton(name);
			return virtualButtons[name].GetButtonDown;
		}

		public override bool GetButtonUp(string name)
		{
			if (virtualButtons.ContainsKey(name))
			{
				return virtualButtons[name].GetButtonUp;
			}
			AddButton(name);
			return virtualButtons[name].GetButtonUp;
		}

		public override bool GetButton(string name)
		{
			if (virtualButtons.ContainsKey(name))
			{
				return virtualButtons[name].GetButton;
			}
			AddButton(name);
			return virtualButtons[name].GetButton;
		}

		public override Vector3 MousePosition()
		{
			return base.virtualMousePosition;
		}
	}
}
