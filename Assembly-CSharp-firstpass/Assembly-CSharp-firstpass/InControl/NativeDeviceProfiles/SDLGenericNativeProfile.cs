using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001E1 RID: 481
	[Preserve]
	[NativeInputDeviceProfile]
	public class SDLGenericNativeProfile : SDLControllerNativeProfile
	{
		// Token: 0x060008DB RID: 2267 RVA: 0x000461C0 File Offset: 0x000443C0
		public override void Define()
		{
			base.Define();
			base.DeviceStyle = InputDeviceStyle.Xbox360;
			base.LastResortMatchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController
				}
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				SDLControllerNativeProfile.Action1Mapping("A"),
				SDLControllerNativeProfile.Action2Mapping("B"),
				SDLControllerNativeProfile.Action3Mapping("X"),
				SDLControllerNativeProfile.Action4Mapping("Y"),
				SDLControllerNativeProfile.LeftCommandMapping("Back", InputControlType.Back),
				SDLControllerNativeProfile.RightCommandMapping("Start", InputControlType.Start),
				SDLControllerNativeProfile.SystemMapping("System", InputControlType.System),
				SDLControllerNativeProfile.LeftStickButtonMapping(),
				SDLControllerNativeProfile.RightStickButtonMapping(),
				SDLControllerNativeProfile.LeftBumperMapping("Left Bumper"),
				SDLControllerNativeProfile.RightBumperMapping("Right Bumper"),
				SDLControllerNativeProfile.DPadUpMapping(),
				SDLControllerNativeProfile.DPadDownMapping(),
				SDLControllerNativeProfile.DPadLeftMapping(),
				SDLControllerNativeProfile.DPadRightMapping(),
				SDLControllerNativeProfile.Misc1Mapping("Share", InputControlType.Share),
				SDLControllerNativeProfile.Paddle1Mapping(),
				SDLControllerNativeProfile.Paddle2Mapping(),
				SDLControllerNativeProfile.Paddle3Mapping(),
				SDLControllerNativeProfile.Paddle4Mapping(),
				SDLControllerNativeProfile.TouchPadButtonMapping()
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
				SDLControllerNativeProfile.RightTriggerMapping("Right Trigger"),
				SDLControllerNativeProfile.AccelerometerXMapping(),
				SDLControllerNativeProfile.AccelerometerYMapping(),
				SDLControllerNativeProfile.AccelerometerZMapping(),
				SDLControllerNativeProfile.GyroscopeXMapping(),
				SDLControllerNativeProfile.GyroscopeYMapping(),
				SDLControllerNativeProfile.GyroscopeZMapping()
			};
		}
	}
}
