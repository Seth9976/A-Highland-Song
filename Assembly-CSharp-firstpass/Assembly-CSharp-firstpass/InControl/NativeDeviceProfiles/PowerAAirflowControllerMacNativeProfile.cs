using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200019D RID: 413
	[Preserve]
	[NativeInputDeviceProfile]
	public class PowerAAirflowControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600082E RID: 2094 RVA: 0x0003D46C File Offset: 0x0003B66C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PowerA Airflow Controller";
			base.DeviceNotes = "PowerA Airflow Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5604,
					ProductID = 16138
				}
			};
		}
	}
}
