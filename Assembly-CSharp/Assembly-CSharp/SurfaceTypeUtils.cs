using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public static class SurfaceTypeUtils
{
	// Token: 0x060007D9 RID: 2009 RVA: 0x00045D00 File Offset: 0x00043F00
	static SurfaceTypeUtils()
	{
		foreach (object obj in Enum.GetValues(typeof(SurfaceType)))
		{
			SurfaceType surfaceType = (SurfaceType)obj;
			SurfaceTypeUtils.spriteTagLookup.Add(surfaceType.ToString(), surfaceType);
		}
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00045D84 File Offset: 0x00043F84
	public static SurfaceType GetSurfaceTypeFromAssetLabel(string label, SurfaceType fallbackSurfaceType = SurfaceType.NONE, bool warnIfNotFound = false)
	{
		SurfaceType surfaceType = fallbackSurfaceType;
		if (!SurfaceTypeUtils.spriteTagLookup.TryGetValue(label, out surfaceType) && warnIfNotFound)
		{
			Debug.LogWarning("No surface type found with label " + label);
		}
		return surfaceType;
	}

	// Token: 0x040009D4 RID: 2516
	private static Dictionary<string, SurfaceType> spriteTagLookup = new Dictionary<string, SurfaceType>(StringComparer.OrdinalIgnoreCase);
}
