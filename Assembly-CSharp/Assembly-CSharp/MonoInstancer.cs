using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public abstract class MonoInstancer<T> : MonoBehaviour where T : MonoInstancer<T>
{
	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06000EEF RID: 3823 RVA: 0x000738D4 File Offset: 0x00071AD4
	public static List<T> all
	{
		get
		{
			return MonoInstancer<T>._all;
		}
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x000738DB File Offset: 0x00071ADB
	protected virtual void OnEnable()
	{
		MonoInstancer<T>._all.Add((T)((object)this));
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x000738ED File Offset: 0x00071AED
	protected virtual void OnDisable()
	{
		MonoInstancer<T>._all.Remove((T)((object)this));
	}

	// Token: 0x040011AC RID: 4524
	private static List<T> _all = new List<T>();
}
