using System;

namespace InControl.UnityDeviceProfiles
{
	// Token: 0x020000EE RID: 238
	[Preserve]
	[UnityInputDeviceProfile]
	public class XTR_G2_MacUnityProfile : InputDeviceProfile
	{
		// Token: 0x060006D0 RID: 1744 RVA: 0x00028F64 File Offset: 0x00027164
		public override void Define()
		{
			base.Define();
			base.DeviceName = "KMODEL Simulator XTR G2 FMS Controller";
			base.DeviceNotes = "KMODEL Simulator XTR G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "OS X" };
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					NameLiteral = "FeiYing Model KMODEL Simulator - XTR+G2+FMS Controller"
				}
			};
		}
	}
}
