using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001EA RID: 490
	[Preserve]
	[NativeInputDeviceProfile]
	public class SDLXboxOneNativeProfile : SDLControllerNativeProfile
	{
		// Token: 0x060008ED RID: 2285 RVA: 0x00047298 File Offset: 0x00045498
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Xbox One Controller";
			base.DeviceStyle = InputDeviceStyle.XboxOne;
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1118,
					ProductID = 746
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1118,
					ProductID = 736
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1118,
					ProductID = 765
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1118,
					ProductID = 767
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1118,
					ProductID = 766
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
				SDLControllerNativeProfile.SystemMapping("Guide", InputControlType.Guide),
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

		// Token: 0x02000229 RID: 553
		private enum ProductId : ushort
		{
			// Token: 0x0400052D RID: 1325
			XBOX_ONE_S = 746,
			// Token: 0x0400052E RID: 1326
			XBOX_ONE_S_REV1_BLUETOOTH = 736,
			// Token: 0x0400052F RID: 1327
			XBOX_ONE_S_REV2_BLUETOOTH = 765,
			// Token: 0x04000530 RID: 1328
			XBOX_ONE_RAW_INPUT_CONTROLLER = 767,
			// Token: 0x04000531 RID: 1329
			XBOX_ONE_XINPUT_CONTROLLER = 766
		}
	}
}
