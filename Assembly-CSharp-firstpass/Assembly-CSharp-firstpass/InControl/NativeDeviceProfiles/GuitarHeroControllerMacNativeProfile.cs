using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000140 RID: 320
	[Preserve]
	[NativeInputDeviceProfile]
	public class GuitarHeroControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000774 RID: 1908 RVA: 0x00039848 File Offset: 0x00037A48
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Guitar Hero Controller";
			base.DeviceNotes = "Guitar Hero Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5168,
					ProductID = 18248
				}
			};
		}
	}
}
