using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000201 RID: 513
public static class CanvasGroupX
{
	// Token: 0x060012ED RID: 4845 RVA: 0x00086EE8 File Offset: 0x000850E8
	public static bool CanvasGroupsAllowInteraction(GameObject gameObject)
	{
		bool flag = true;
		Transform transform = gameObject.transform;
		while (transform != null)
		{
			transform.GetComponents<CanvasGroup>(CanvasGroupX.m_CanvasGroupCache);
			bool flag2 = false;
			for (int i = 0; i < CanvasGroupX.m_CanvasGroupCache.Count; i++)
			{
				if (!CanvasGroupX.m_CanvasGroupCache[i].interactable)
				{
					flag = false;
					flag2 = true;
				}
				if (CanvasGroupX.m_CanvasGroupCache[i].ignoreParentGroups)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				break;
			}
			transform = transform.parent;
		}
		return flag;
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x00086F60 File Offset: 0x00085160
	public static float CanvasGroupsAlpha(GameObject gameObject)
	{
		float num = 1f;
		Transform transform = gameObject.transform;
		while (transform != null)
		{
			transform.GetComponents<CanvasGroup>(CanvasGroupX.m_CanvasGroupCache);
			bool flag = false;
			for (int i = 0; i < CanvasGroupX.m_CanvasGroupCache.Count; i++)
			{
				num *= CanvasGroupX.m_CanvasGroupCache[i].alpha;
				if (CanvasGroupX.m_CanvasGroupCache[i].ignoreParentGroups)
				{
					flag = true;
				}
			}
			if (flag)
			{
				break;
			}
			transform = transform.parent;
		}
		return num;
	}

	// Token: 0x040012A6 RID: 4774
	private static readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();
}
