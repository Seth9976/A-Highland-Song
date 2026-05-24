using System;

namespace InControl.UnityDeviceProfiles
{
	// Token: 0x02000131 RID: 305
	[Preserve]
	[UnityInputDeviceProfile]
	public class XTR55_G2_WindowsUnityProfile : InputDeviceProfile
	{
		// Token: 0x06000756 RID: 1878 RVA: 0x00038F94 File Offset: 0x00037194
		public override void Define()
		{
			base.Define();
			base.DeviceName = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.DeviceNotes = "SAILI Simulator XTR5.5 G2 FMS Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "Windows" };
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					NameLiteral = "SAILI Simulator --- XTR5.5+G2+FMS Controller"
				}
			};
		}
	}
}
