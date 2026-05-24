using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003D2 RID: 978
	[Flags]
	public enum CopyTextureSupport
	{
		// Token: 0x04000BE7 RID: 3047
		None = 0,
		// Token: 0x04000BE8 RID: 3048
		Basic = 1,
		// Token: 0x04000BE9 RID: 3049
		Copy3D = 2,
		// Token: 0x04000BEA RID: 3050
		DifferentTypes = 4,
		// Token: 0x04000BEB RID: 3051
		TextureToRT = 8,
		// Token: 0x04000BEC RID: 3052
		RTToTexture = 16
	}
}
