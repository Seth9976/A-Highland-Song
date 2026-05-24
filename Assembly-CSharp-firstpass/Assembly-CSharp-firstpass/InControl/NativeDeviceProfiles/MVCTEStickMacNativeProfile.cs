using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000190 RID: 400
	[Preserve]
	[NativeInputDeviceProfile]
	public class MVCTEStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000814 RID: 2068 RVA: 0x0003C668 File Offset: 0x0003A868
		public override void Define()
		{
			base.Define();
			base.DeviceName = "MVC TE Stick";
			base.DeviceNotes = "MVC TE Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61497
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 46904
				}
			};
		}
	}
}
