using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000199 RID: 409
	[Preserve]
	[NativeInputDeviceProfile]
	public class PDPVersusControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000826 RID: 2086 RVA: 0x0003CE80 File Offset: 0x0003B080
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PDP Versus Controller";
			base.DeviceNotes = "PDP Versus Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 63748
				}
			};
		}
	}
}
