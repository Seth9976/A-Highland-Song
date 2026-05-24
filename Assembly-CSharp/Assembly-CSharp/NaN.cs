using System;
using UnityEngine;

// Token: 0x0200020B RID: 523
public static class NaN
{
	// Token: 0x0600135B RID: 4955 RVA: 0x00088855 File Offset: 0x00086A55
	public static bool Check(float f)
	{
		if (float.IsNaN(f))
		{
			Debug.LogError("NaN.Check failed!");
			return true;
		}
		return false;
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x0008886C File Offset: 0x00086A6C
	public static bool Check(Vector3 v)
	{
		return NaN.Check(v.x) || NaN.Check(v.y) || NaN.Check(v.z);
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x0008889C File Offset: 0x00086A9C
	public static bool Check(Vector2 v)
	{
		return NaN.Check(v.x) || NaN.Check(v.y);
	}
}
