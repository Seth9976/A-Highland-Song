using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001A5 RID: 421
	[Preserve]
	[NativeInputDeviceProfile]
	public class QanbaFightStickPlusMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600083E RID: 2110 RVA: 0x0003D8CC File Offset: 0x0003BACC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Qanba Fight Stick Plus";
			base.DeviceNotes = "Qanba Fight Stick Plus on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 48879
				}
			};
		}
	}
}
