using System;

namespace InControl.UnityDeviceProfiles
{
	// Token: 0x020000B8 RID: 184
	[Preserve]
	[UnityInputDeviceProfile]
	public class NvidiaShieldRemoteAndroidUnityProfile : InputDeviceProfile
	{
		// Token: 0x06000664 RID: 1636 RVA: 0x0001ED4C File Offset: 0x0001CF4C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "NVIDIA Shield Remote";
			base.DeviceNotes = "NVIDIA Shield Remote on Android";
			base.DeviceClass = InputDeviceClass.Remote;
			base.DeviceStyle = InputDeviceStyle.NVIDIAShield;
			base.IncludePlatforms = new string[] { "Android" };
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					NameLiteral = "SHIELD Remote"
				},
				new InputDeviceMatcher
				{
					NamePattern = "SHIELD Remote"
				}
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Name = "A",
					Target = InputControlType.Action1,
					Source = InputDeviceProfile.Button(0)
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				InputDeviceProfile.DPadLeftMapping(4),
				InputDeviceProfile.DPadRightMapping(4),
				InputDeviceProfile.DPadUpMapping(5),
				InputDeviceProfile.DPadDownMapping(5)
			};
		}
	}
}
