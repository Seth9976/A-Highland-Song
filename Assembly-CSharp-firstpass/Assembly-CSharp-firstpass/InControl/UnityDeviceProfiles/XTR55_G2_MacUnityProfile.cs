using System;

namespace InControl.UnityDeviceProfiles
{
	// Token: 0x020000ED RID: 237
	[Preserve]
	[UnityInputDeviceProfile]
	public class XTR55_G2_MacUnityProfile : InputDeviceProfile
	{
		// Token: 0x060006CE RID: 1742 RVA: 0x00028EF0 File Offset: 0x000270F0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.DeviceNotes = "SAILI Simulator XTR5.5 G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "OS X" };
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					NameLiteral = "              SAILI Simulator --- XTR5.5+G2+FMS Controller"
				}
			};
		}
	}
}
