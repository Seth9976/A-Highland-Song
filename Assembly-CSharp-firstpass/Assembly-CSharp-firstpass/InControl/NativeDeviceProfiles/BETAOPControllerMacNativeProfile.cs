using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000137 RID: 311
	[Preserve]
	[NativeInputDeviceProfile]
	public class BETAOPControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000762 RID: 1890 RVA: 0x000392EC File Offset: 0x000374EC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "BETAOP Controller";
			base.DeviceNotes = "BETAOP Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 4544,
					ProductID = 21766
				}
			};
		}
	}
}
