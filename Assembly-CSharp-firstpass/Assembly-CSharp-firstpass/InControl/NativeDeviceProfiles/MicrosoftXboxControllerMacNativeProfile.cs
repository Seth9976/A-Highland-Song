using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200018B RID: 395
	[Preserve]
	[NativeInputDeviceProfile]
	public class MicrosoftXboxControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600080A RID: 2058 RVA: 0x0003C240 File Offset: 0x0003A440
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Microsoft Xbox Controller";
			base.DeviceNotes = "Microsoft Xbox Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = ushort.MaxValue,
					ProductID = ushort.MaxValue
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1118,
					ProductID = 649
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1118,
					ProductID = 648
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1118,
					ProductID = 645
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1118,
					ProductID = 514
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1118,
					ProductID = 647
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1118,
					ProductID = 648
				}
			};
		}
	}
}
