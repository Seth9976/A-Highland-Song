using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000211 RID: 529
public static class TransformX
{
	// Token: 0x06001370 RID: 4976 RVA: 0x00088D20 File Offset: 0x00086F20
	public static Transform FindInChildren(this Transform current, string name)
	{
		return current.FindInChildren(name);
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x00088D2C File Offset: 0x00086F2C
	public static T FindInChildren<T>(this Transform current, string name = null) where T : Component
	{
		return current.FindInChildren((T t) => t.name == name);
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x00088D58 File Offset: 0x00086F58
	public static T FindInChildren<T>(this Transform current, Predicate<T> predicate = null) where T : Component
	{
		if (TransformX._objectQueue == null)
		{
			TransformX._objectQueue = new Queue<Transform>();
		}
		foreach (object obj in current)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.activeInHierarchy)
			{
				TransformX._objectQueue.Enqueue(transform);
			}
		}
		T t = TransformX.FindInChildrenFromMainQueue<T>(predicate);
		TransformX._objectQueue.Clear();
		return t;
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x00088DE0 File Offset: 0x00086FE0
	private static T FindInChildrenFromMainQueue<T>(Predicate<T> predicate) where T : Component
	{
		while (TransformX._objectQueue.Count > 0)
		{
			Transform transform = TransformX._objectQueue.Dequeue();
			T component = transform.GetComponent<T>();
			if (component && (predicate == null || predicate(component)))
			{
				return component;
			}
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				TransformX._objectQueue.Enqueue(transform2);
			}
		}
		return default(T);
	}

	// Token: 0x040012B2 RID: 4786
	private static Queue<Transform> _objectQueue;
}
