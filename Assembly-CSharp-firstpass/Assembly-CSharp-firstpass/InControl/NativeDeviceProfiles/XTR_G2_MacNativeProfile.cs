using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001D8 RID: 472
	[Preserve]
	[NativeInputDeviceProfile]
	public class XTR_G2_MacNativeProfile : InputDeviceProfile
	{
		// Token: 0x060008A4 RID: 2212 RVA: 0x000440B4 File Offset: 0x000422B4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "KMODEL Simulator XTR G2 FMS Controller";
			base.DeviceNotes = "KMODEL Simulator XTR G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "OS X" };
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 2971,
					ProductID = 16402,
					NameLiteral = "KMODEL Simulator - XTR+G2+FMS Controller"
				}
			};
		}
	}
}
