using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001D7 RID: 471
	[Preserve]
	[NativeInputDeviceProfile]
	public class XTR55_G2_MacNativeProfile : InputDeviceProfile
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x00044010 File Offset: 0x00042210
		public override void Define()
		{
			base.Define();
			base.DeviceName = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.DeviceNotes = "SAILI Simulator XTR5.5 G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "OS X" };
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 2971,
					ProductID = 16402,
					NameLiteral = "SAILI Simulator --- XTR5.5+G2+FMS Controller"
				}
			};
		}
	}
}
