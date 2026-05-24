using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001E7 RID: 487
	[Preserve]
	[NativeInputDeviceProfile]
	public class SDLPlayStation4NativeProfile : SDLControllerNativeProfile
	{
		// Token: 0x060008E7 RID: 2279 RVA: 0x00046C68 File Offset: 0x00044E68
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PlayStation 4 Controller";
			base.DeviceStyle = InputDeviceStyle.PlayStation4;
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1356,
					ProductID = 1476
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1356,
					ProductID = 2976
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1356,
					ProductID = 2508
				}
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				SDLControllerNativeProfile.Action1Mapping("Cross"),
				SDLControllerNativeProfile.Action2Mapping("Circle"),
				SDLControllerNativeProfile.Action3Mapping("Square"),
				SDLControllerNativeProfile.Action4Mapping("Triangle"),
				SDLControllerNativeProfile.LeftCommandMapping("Share", InputControlType.Share),
				SDLControllerNativeProfile.RightCommandMapping("Options", InputControlType.Options),
				SDLControllerNativeProfile.SystemMapping("System", InputControlType.System),
				SDLControllerNativeProfile.LeftStickButtonMapping(),
				SDLControllerNativeProfile.RightStickButtonMapping(),
				SDLControllerNativeProfile.LeftBumperMapping("L1"),
				SDLControllerNativeProfile.RightBumperMapping("R1"),
				SDLControllerNativeProfile.DPadUpMapping(),
				SDLControllerNativeProfile.DPadDownMapping(),
				SDLControllerNativeProfile.DPadLeftMapping(),
				SDLControllerNativeProfile.DPadRightMapping(),
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
				SDLControllerNativeProfile.LeftTriggerMapping("L2"),
				SDLControllerNativeProfile.RightTriggerMapping("R2"),
				SDLControllerNativeProfile.AccelerometerXMapping(),
				SDLControllerNativeProfile.AccelerometerYMapping(),
				SDLControllerNativeProfile.AccelerometerZMapping(),
				SDLControllerNativeProfile.GyroscopeXMapping(),
				SDLControllerNativeProfile.GyroscopeYMapping(),
				SDLControllerNativeProfile.GyroscopeZMapping()
			};
		}

		// Token: 0x02000227 RID: 551
		private enum ProductId : ushort
		{
			// Token: 0x04000527 RID: 1319
			SONY_DS4 = 1476,
			// Token: 0x04000528 RID: 1320
			SONY_DS4_DONGLE = 2976,
			// Token: 0x04000529 RID: 1321
			SONY_DS4_SLIM = 2508
		}
	}
}
