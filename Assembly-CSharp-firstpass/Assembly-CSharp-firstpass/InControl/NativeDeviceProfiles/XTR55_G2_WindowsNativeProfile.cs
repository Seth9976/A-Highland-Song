using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000211 RID: 529
	[Preserve]
	[NativeInputDeviceProfile]
	public class XTR55_G2_WindowsNativeProfile : InputDeviceProfile
	{
		// Token: 0x0600093B RID: 2363 RVA: 0x00052F48 File Offset: 0x00051148
		public override void Define()
		{
			base.Define();
			base.DeviceName = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.DeviceNotes = "SAILI Simulator XTR5.5 G2 FMS Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "Windows" };
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.DirectInput,
					VendorID = 2971,
					ProductID = 16402,
					NameLiteral = "SAILI Simulator --- XTR5.5+G2+FMS Controller"
				}
			};
		}
	}
}
