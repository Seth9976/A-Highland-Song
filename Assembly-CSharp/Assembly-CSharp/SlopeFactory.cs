using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class SlopeFactory : MonoSingleton<SlopeFactory>
{
	// Token: 0x06000E1C RID: 3612 RVA: 0x0006EF98 File Offset: 0x0006D198
	public static SlopeFactory.Creation Create(List<Vector2> vertices, float z, bool allowReuse, Poly parent = null, Transform group = null)
	{
		Slope slope = null;
		if (allowReuse)
		{
			SlopeFactory.SetupPoolsAndParentsIfNecessary();
			while (slope == null && SlopeFactory._slopeReusePool.Count > 0)
			{
				slope = SlopeFactory._slopeReusePool.Pop();
			}
			if (slope != null)
			{
				slope.gameObject.SetActive(true);
			}
		}
		if (!allowReuse || slope == null)
		{
			GameObject gameObject = new GameObject("Auto-gen slope");
			gameObject.tag = "Auto-gen";
			slope = gameObject.AddComponent<Slope>();
			if (allowReuse)
			{
				gameObject.hideFlags = HideFlags.DontSave;
				slope.transform.SetParent(SlopeFactory._dynamicSlopesParent, false);
				SlopeFactory._dynamicActiveSlopes.Add(slope);
			}
		}
		if (!allowReuse && group == null)
		{
			group = parent.transform.Find("Auto-gen slopes");
			if (group == null)
			{
				GameObject gameObject2 = new GameObject("Auto-gen slopes");
				gameObject2.tag = "Auto-gen";
				gameObject2.transform.SetParent(parent.transform, false);
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = Vector3.one;
				group = gameObject2.transform;
			}
		}
		if (group != null)
		{
			float z2 = group.transform.position.z;
			if (z2 - (float)Mathf.RoundToInt(z2) > 0f)
			{
				Vector3 position = group.transform.position;
				position.z = (float)Mathf.RoundToInt(position.z);
				group.transform.position = position;
			}
		}
		if (!allowReuse)
		{
			slope.transform.SetParent(group, false);
		}
		slope.transform.localPosition = Vector3.zero;
		slope.transform.localRotation = Quaternion.identity;
		slope.transform.localScale = Vector3.one;
		slope.defaultColor = parent.color;
		Vector2 vector = Vector2.zero;
		foreach (Vector2 vector2 in vertices)
		{
			vector += vector2;
		}
		vector /= (float)vertices.Count;
		slope.transform.position = new Vector3(vector.x, vector.y, z);
		slope.SetWorldPoints(vertices);
		return new SlopeFactory.Creation
		{
			slope = slope,
			group = group
		};
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x0006F200 File Offset: 0x0006D400
	public static void Recycle(List<Slope> slopes)
	{
		foreach (Slope slope in slopes)
		{
			if (!(slope == null))
			{
				SlopeFactory._dynamicActiveSlopes.Remove(slope);
				SlopeFactory._slopeReusePool.Push(slope);
				slope.gameObject.SetActive(false);
				slope.chunk = null;
				slope.leftSlopes.Clear();
				slope.rightSlopes.Clear();
				slope.isStatic = false;
			}
		}
		slopes.Clear();
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x0006F2A0 File Offset: 0x0006D4A0
	public static void ClearAll()
	{
		foreach (Slope slope in SlopeFactory._slopeReusePool)
		{
			if (slope != null)
			{
				Object.DestroyImmediate(slope.gameObject);
			}
		}
		SlopeFactory._slopeReusePool.Clear();
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x0006F30C File Offset: 0x0006D50C
	private static void SetupPoolsAndParentsIfNecessary()
	{
		if (SlopeFactory._dynamicSlopesParent == null)
		{
			Transform transform = MonoSingleton<SlopeFactory>.instance.transform.Find("Dynamic Slopes");
			GameObject gameObject = ((transform != null) ? transform.gameObject : null);
			if (gameObject != null)
			{
				foreach (object obj in gameObject.transform)
				{
					Slope component = ((Transform)obj).GetComponent<Slope>();
					if (component == null)
					{
						Object.DestroyImmediate(component.gameObject);
					}
					else
					{
						component.gameObject.SetActive(false);
						SlopeFactory._slopeReusePool.Push(component);
					}
				}
			}
			if (gameObject == null)
			{
				gameObject = new GameObject("Dynamic Slopes");
				gameObject.hideFlags = HideFlags.DontSave;
				gameObject.transform.parent = MonoSingleton<SlopeFactory>.instance.transform;
			}
			SlopeFactory._dynamicSlopesParent = gameObject.transform;
		}
	}

	// Token: 0x040010EC RID: 4332
	private static Stack<Slope> _slopeReusePool = new Stack<Slope>();

	// Token: 0x040010ED RID: 4333
	private static Transform _dynamicSlopesParent;

	// Token: 0x040010EE RID: 4334
	private static List<Slope> _dynamicActiveSlopes = new List<Slope>(256);

	// Token: 0x040010EF RID: 4335
	private const string autoGenTag = "Auto-gen";

	// Token: 0x020003BF RID: 959
	public struct Creation
	{
		// Token: 0x04001A0A RID: 6666
		public Slope slope;

		// Token: 0x04001A0B RID: 6667
		public Transform group;
	}
}
