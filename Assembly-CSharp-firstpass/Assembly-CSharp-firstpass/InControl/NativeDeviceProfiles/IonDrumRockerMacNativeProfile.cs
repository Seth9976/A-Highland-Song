using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000160 RID: 352
	[Preserve]
	[NativeInputDeviceProfile]
	public class IonDrumRockerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007B4 RID: 1972 RVA: 0x0003AAB4 File Offset: 0x00038CB4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Ion Drum Rocker";
			base.DeviceNotes = "Ion Drum Rocker on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 304
				}
			};
		}
	}
}
