using System;

namespace InControl.UnityDeviceProfiles
{
	// Token: 0x02000132 RID: 306
	[Preserve]
	[UnityInputDeviceProfile]
	public class XTR_G2_WindowsUnityProfile : InputDeviceProfile
	{
		// Token: 0x06000758 RID: 1880 RVA: 0x00039008 File Offset: 0x00037208
		public override void Define()
		{
			base.Define();
			base.DeviceName = "KMODEL Simulator XTR G2 FMS Controller";
			base.DeviceNotes = "KMODEL Simulator XTR G2 FMS Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "Windows" };
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					NameLiteral = "KMODEL Simulator - XTR+G2+FMS Controller"
				}
			};
		}
	}
}
