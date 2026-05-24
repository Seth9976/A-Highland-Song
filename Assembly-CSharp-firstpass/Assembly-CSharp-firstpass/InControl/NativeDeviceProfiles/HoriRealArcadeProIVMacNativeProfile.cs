using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000158 RID: 344
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriRealArcadeProIVMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007A4 RID: 1956 RVA: 0x0003A658 File Offset: 0x00038858
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Real Arcade Pro IV";
			base.DeviceNotes = "Hori Real Arcade Pro IV on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 140
				}
			};
		}
	}
}
