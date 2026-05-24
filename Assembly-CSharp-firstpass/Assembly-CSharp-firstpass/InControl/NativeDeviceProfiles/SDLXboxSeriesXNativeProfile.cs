using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001EB RID: 491
	[Preserve]
	[NativeInputDeviceProfile]
	public class SDLXboxSeriesXNativeProfile : SDLControllerNativeProfile
	{
		// Token: 0x060008EF RID: 2287 RVA: 0x00047538 File Offset: 0x00045738
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Xbox Series X Controller";
			base.DeviceStyle = InputDeviceStyle.XboxSeriesX;
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1118,
					ProductID = 2834
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1118,
					ProductID = 2835
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1118,
					ProductID = 8193
				}
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				SDLControllerNativeProfile.Action1Mapping("A"),
				SDLControllerNativeProfile.Action2Mapping("B"),
				SDLControllerNativeProfile.Action3Mapping("X"),
				SDLControllerNativeProfile.Action4Mapping("Y"),
				SDLControllerNativeProfile.LeftCommandMapping("View", InputControlType.View),
				SDLControllerNativeProfile.RightCommandMapping("Menu", InputControlType.Menu),
				SDLControllerNativeProfile.SystemMapping("Xbox", InputControlType.System),
				SDLControllerNativeProfile.Misc1Mapping("Share", InputControlType.Share),
				SDLControllerNativeProfile.LeftStickButtonMapping(),
				SDLControllerNativeProfile.RightStickButtonMapping(),
				SDLControllerNativeProfile.LeftBumperMapping("Left Bumper"),
				SDLControllerNativeProfile.RightBumperMapping("Right Bumper"),
				SDLControllerNativeProfile.DPadUpMapping(),
				SDLControllerNativeProfile.DPadDownMapping(),
				SDLControllerNativeProfile.DPadLeftMapping(),
				SDLControllerNativeProfile.DPadRightMapping()
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				SDLControllerNativeProfile.LeftStickLeftMapping(),
				SDLControllerNativeProfile.LeftStickRightMapping(),
				SDLControllerNativeProfile.LeftStickUpMapping(),
				SDLControllerNativeProfile.LeftStickDownMapping(),
				SDLControllerNativeProfile.RightStickLeftMapping(),
				SDLControllerNativeProfile.RightStickRightMapping(),
				SDLControllerNativeProfile.RightStickUpMapping(),
				SDLControllerNativeProfile.RightStickDownMapping(),
				SDLControllerNativeProfile.LeftTriggerMapping("Left Trigger"),
				SDLControllerNativeProfile.RightTriggerMapping("Right Trigger")
			};
		}

		// Token: 0x0200022A RID: 554
		private enum ProductId : ushort
		{
			// Token: 0x04000533 RID: 1331
			XBOX_SERIES_X = 2834,
			// Token: 0x04000534 RID: 1332
			XBOX_SERIES_X_BLUETOOTH,
			// Token: 0x04000535 RID: 1333
			XBOX_SERIES_X_POWERA = 8193
		}
	}
}
