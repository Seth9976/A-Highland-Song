using System;

namespace UnityEngine
{
	// Token: 0x02000180 RID: 384
	[Flags]
	public enum RenderTextureCreationFlags
	{
		// Token: 0x04000554 RID: 1364
		MipMap = 1,
		// Token: 0x04000555 RID: 1365
		AutoGenerateMips = 2,
		// Token: 0x04000556 RID: 1366
		SRGB = 4,
		// Token: 0x04000557 RID: 1367
		EyeTexture = 8,
		// Token: 0x04000558 RID: 1368
		EnableRandomWrite = 16,
		// Token: 0x04000559 RID: 1369
		CreatedFromScript = 32,
		// Token: 0x0400055A RID: 1370
		AllowVerticalFlip = 128,
		// Token: 0x0400055B RID: 1371
		NoResolvedColorSurface = 256,
		// Token: 0x0400055C RID: 1372
		DynamicallyScalable = 1024,
		// Token: 0x0400055D RID: 1373
		BindMS = 2048
	}
}
