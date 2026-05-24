using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003B5 RID: 949
	internal static class CameraEventUtils
	{
		// Token: 0x06001F4A RID: 8010 RVA: 0x00033004 File Offset: 0x00031204
		public static bool IsValid(CameraEvent value)
		{
			return value >= CameraEvent.BeforeDepthTexture && value <= CameraEvent.AfterHaloAndLensFlares;
		}

		// Token: 0x04000AFA RID: 2810
		private const CameraEvent k_MinimumValue = CameraEvent.BeforeDepthTexture;

		// Token: 0x04000AFB RID: 2811
		private const CameraEvent k_MaximumValue = CameraEvent.AfterHaloAndLensFlares;
	}
}
