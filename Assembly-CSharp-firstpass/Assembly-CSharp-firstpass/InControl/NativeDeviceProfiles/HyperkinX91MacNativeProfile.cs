using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200015E RID: 350
	[Preserve]
	[NativeInputDeviceProfile]
	public class HyperkinX91MacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007B0 RID: 1968 RVA: 0x0003A9BC File Offset: 0x00038BBC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hyperkin X91";
			base.DeviceNotes = "Hyperkin X91 on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 11812,
					ProductID = 5768
				}
			};
		}
	}
}
