using System;

namespace InControl.UnityDeviceProfiles
{
	// Token: 0x020000B3 RID: 179
	[Preserve]
	[UnityInputDeviceProfile]
	public class NexusPlayerRemoteAndroidUnityProfile : InputDeviceProfile
	{
		// Token: 0x0600065A RID: 1626 RVA: 0x0001E12C File Offset: 0x0001C32C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Nexus Player Remote";
			base.DeviceNotes = "Nexus Player Remote";
			base.DeviceClass = InputDeviceClass.Remote;
			base.IncludePlatforms = new string[] { "Android" };
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					NameLiteral = "Google Nexus Remote"
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
