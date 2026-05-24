using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200013A RID: 314
	[Preserve]
	[NativeInputDeviceProfile]
	public class BrookPS2ConverterMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000768 RID: 1896 RVA: 0x00039460 File Offset: 0x00037660
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Brook PS2 Converter";
			base.DeviceNotes = "Brook PS2 Converter on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3090,
					ProductID = 2289
				}
			};
		}
	}
}
