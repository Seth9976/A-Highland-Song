using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001BB RID: 443
	[Preserve]
	[NativeInputDeviceProfile]
	public class ThrustmasterTXGIPMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600086A RID: 2154 RVA: 0x0003E668 File Offset: 0x0003C868
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Thrustmaster TX GIP";
			base.DeviceNotes = "Thrustmaster TX GIP on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1103,
					ProductID = 46692
				}
			};
		}
	}
}
