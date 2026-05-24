using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001AE RID: 430
	[Preserve]
	[NativeInputDeviceProfile]
	public class RedOctaneGuitarMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000850 RID: 2128 RVA: 0x0003DE64 File Offset: 0x0003C064
		public override void Define()
		{
			base.Define();
			base.DeviceName = "RedOctane Guitar";
			base.DeviceNotes = "RedOctane Guitar on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5168,
					ProductID = 1803
				}
			};
		}
	}
}
