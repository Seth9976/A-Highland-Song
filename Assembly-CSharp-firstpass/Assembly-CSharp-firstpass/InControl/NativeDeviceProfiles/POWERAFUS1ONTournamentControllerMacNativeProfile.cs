using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200019E RID: 414
	[Preserve]
	[NativeInputDeviceProfile]
	public class POWERAFUS1ONTournamentControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000830 RID: 2096 RVA: 0x0003D4E8 File Offset: 0x0003B6E8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "POWER A FUS1ON Tournament Controller";
			base.DeviceNotes = "POWER A FUS1ON Tournament Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21399
				}
			};
		}
	}
}
