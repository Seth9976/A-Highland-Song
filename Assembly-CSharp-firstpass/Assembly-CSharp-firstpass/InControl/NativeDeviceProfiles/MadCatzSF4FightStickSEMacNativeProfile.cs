using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000181 RID: 385
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzSF4FightStickSEMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007F6 RID: 2038 RVA: 0x0003BBEC File Offset: 0x00039DEC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz SF4 Fight Stick SE";
			base.DeviceNotes = "Mad Catz SF4 Fight Stick SE on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 18200
				}
			};
		}
	}
}
