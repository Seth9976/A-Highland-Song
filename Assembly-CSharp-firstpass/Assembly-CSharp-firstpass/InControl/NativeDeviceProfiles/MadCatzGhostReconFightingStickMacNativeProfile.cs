using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000175 RID: 373
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzGhostReconFightingStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007DE RID: 2014 RVA: 0x0003B61C File Offset: 0x0003981C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Ghost Recon Fighting Stick";
			base.DeviceNotes = "Mad Catz Ghost Recon Fighting Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61473
				}
			};
		}
	}
}
