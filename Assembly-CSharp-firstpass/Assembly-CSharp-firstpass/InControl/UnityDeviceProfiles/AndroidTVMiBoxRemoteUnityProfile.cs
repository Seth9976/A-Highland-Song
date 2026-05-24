using System;

namespace InControl.UnityDeviceProfiles
{
	// Token: 0x0200008C RID: 140
	[Preserve]
	[UnityInputDeviceProfile]
	public class AndroidTVMiBoxRemoteUnityProfile : InputDeviceProfile
	{
		// Token: 0x0600060C RID: 1548 RVA: 0x0001774C File Offset: 0x0001594C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Xiaomi Remote";
			base.DeviceNotes = "Xiaomi Remote on Android TV";
			base.DeviceClass = InputDeviceClass.Remote;
			base.IncludePlatforms = new string[] { "Android" };
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					NameLiteral = "Xiaomi Remote"
				}
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Name = "A",
					Target = InputControlType.Action1,
					Source = InputDeviceProfile.Button(0)
				},
				new InputControlMapping
				{
					Name = "Back",
					Target = InputControlType.Back,
					Source = InputDeviceProfile.EscapeKey
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
