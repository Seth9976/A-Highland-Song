using System;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public static class AudioUtils
{
	// Token: 0x060010F1 RID: 4337 RVA: 0x0007DAD5 File Offset: 0x0007BCD5
	public static float LinearVolumeToDBVolume(float linearVolume)
	{
		return Mathf.Clamp(20f * Mathf.Log10(linearVolume), -80f, 0f);
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x0007DAF2 File Offset: 0x0007BCF2
	public static float DBVolumeToLinearVolume(float dbVolume)
	{
		return Mathf.Pow(10f, dbVolume / 20f);
	}

	// Token: 0x04001254 RID: 4692
	public const float c = 20f;
}
