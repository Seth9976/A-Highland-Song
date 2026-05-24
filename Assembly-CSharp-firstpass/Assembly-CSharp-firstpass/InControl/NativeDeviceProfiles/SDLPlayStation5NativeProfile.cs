using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001E8 RID: 488
	[Preserve]
	[NativeInputDeviceProfile]
	public class SDLPlayStation5NativeProfile : SDLControllerNativeProfile
	{
		// Token: 0x060008E9 RID: 2281 RVA: 0x00046EC8 File Offset: 0x000450C8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PlayStation 5 Controller";
			base.DeviceStyle = InputDeviceStyle.PlayStation5;
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.SDLController,
					VendorID = 1356,
					ProductID = 3302
				}
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				SDLControllerNativeProfile.Action1Mapping("Cross"),
				SDLControllerNativeProfile.Action2Mapping("Circle"),
				SDLControllerNativeProfile.Action3Mapping("Square"),
				SDLControllerNativeProfile.Action4Mapping("Triangle"),
				SDLControllerNativeProfile.LeftCommandMapping("Create", InputControlType.Create),
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
				SDLControllerNativeProfile.Misc1Mapping("Mute", InputControlType.Mute),
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

		// Token: 0x02000228 RID: 552
		private enum ProductId : ushort
		{
			// Token: 0x0400052B RID: 1323
			SONY_DS5 = 3302
		}
	}
}
