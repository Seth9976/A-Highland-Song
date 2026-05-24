using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001A9 RID: 425
	[Preserve]
	[NativeInputDeviceProfile]
	public class RazerSabertoothEliteControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000846 RID: 2118 RVA: 0x0003DB7C File Offset: 0x0003BD7C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Razer Sabertooth Elite Controller";
			base.DeviceNotes = "Razer Sabertooth Elite Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5769,
					ProductID = 65024
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 23812
				}
			};
		}
	}
}
