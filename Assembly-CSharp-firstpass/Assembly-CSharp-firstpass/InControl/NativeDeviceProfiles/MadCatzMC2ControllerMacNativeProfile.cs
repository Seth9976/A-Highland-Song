using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000177 RID: 375
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzMC2ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007E2 RID: 2018 RVA: 0x0003B714 File Offset: 0x00039914
		public override void Define()
		{
			base.Define();
			base.DeviceName = "MadCatz MC2 Controller";
			base.DeviceNotes = "MadCatz MC2 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 18208
				}
			};
		}
	}
}
