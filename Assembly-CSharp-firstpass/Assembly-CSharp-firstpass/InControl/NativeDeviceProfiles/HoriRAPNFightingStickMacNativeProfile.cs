using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000153 RID: 339
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriRAPNFightingStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600079A RID: 1946 RVA: 0x0003A3EC File Offset: 0x000385EC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori RAP N Fighting Stick";
			base.DeviceNotes = "Hori RAP N Fighting Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 174
				}
			};
		}
	}
}
