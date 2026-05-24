using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000145 RID: 325
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriBlueSoloControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600077E RID: 1918 RVA: 0x00039AB4 File Offset: 0x00037CB4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Blue Solo Controller ";
			base.DeviceNotes = "Hori Blue Solo Controller\ton Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 64001
				}
			};
		}
	}
}
