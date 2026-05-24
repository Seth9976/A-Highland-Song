using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000133 RID: 307
	[Preserve]
	[NativeInputDeviceProfile]
	public class AirFloControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600075A RID: 1882 RVA: 0x0003907C File Offset: 0x0003727C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Air Flo Controller";
			base.DeviceNotes = "Air Flo Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21251
				}
			};
		}
	}
}
