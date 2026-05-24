using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000180 RID: 384
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzSF4FightStickRound2TEMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007F4 RID: 2036 RVA: 0x0003BB70 File Offset: 0x00039D70
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz SF4 Fight Stick Round 2 TE";
			base.DeviceNotes = "Mad Catz SF4 Fight Stick Round 2 TE on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61496
				}
			};
		}
	}
}
