using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000136 RID: 310
	[Preserve]
	[NativeInputDeviceProfile]
	public class BatarangControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000760 RID: 1888 RVA: 0x00039270 File Offset: 0x00037470
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Batarang Controller";
			base.DeviceNotes = "Batarang Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5604,
					ProductID = 16144
				}
			};
		}
	}
}
