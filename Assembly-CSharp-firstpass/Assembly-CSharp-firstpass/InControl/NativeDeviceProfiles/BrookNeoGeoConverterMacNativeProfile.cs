using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000139 RID: 313
	[Preserve]
	[NativeInputDeviceProfile]
	public class BrookNeoGeoConverterMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000766 RID: 1894 RVA: 0x000393E4 File Offset: 0x000375E4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Brook NeoGeo Converter";
			base.DeviceNotes = "Brook NeoGeo Converter on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3090,
					ProductID = 2036
				}
			};
		}
	}
}
