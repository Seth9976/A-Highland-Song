using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001AC RID: 428
	[Preserve]
	[NativeInputDeviceProfile]
	public class RazerWolverineUltimateControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600084C RID: 2124 RVA: 0x0003DD2C File Offset: 0x0003BF2C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Razer Wolverine Ultimate Controller";
			base.DeviceNotes = "Razer Wolverine Ultimate Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5426,
					ProductID = 2580
				}
			};
		}
	}
}
