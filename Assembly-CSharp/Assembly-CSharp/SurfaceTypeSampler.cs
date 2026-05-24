using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F1 RID: 241
[Serializable]
public class SurfaceTypeSampler
{
	// Token: 0x17000205 RID: 517
	// (get) Token: 0x060007CE RID: 1998 RVA: 0x00045A7C File Offset: 0x00043C7C
	// (set) Token: 0x060007CF RID: 1999 RVA: 0x00045A84 File Offset: 0x00043C84
	public SurfaceType surfaceType
	{
		get
		{
			return this._surfaceType;
		}
		set
		{
			if (this._surfaceType == value)
			{
				return;
			}
			SurfaceType surfaceType = this._surfaceType;
			this._surfaceType = value;
			if (this.OnChangeSurfaceType != null)
			{
				this.OnChangeSurfaceType(surfaceType, this.surfaceType);
			}
		}
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00045AC4 File Offset: 0x00043CC4
	public void UpdateSurfaceType(Slope slope, Vector3 position)
	{
		if (slope == null)
		{
			return;
		}
		Poly componentInParent = slope.GetComponentInParent<Poly>();
		if (componentInParent != null)
		{
			this.surfaceType = SurfaceTypeSampler.GetSurfaceTypeFromPolyAndPoint(componentInParent, position);
			return;
		}
		this.surfaceType = SurfaceTypeSampler.GetSurfaceTypeFromPolyAndPoint(slope, position);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00045B08 File Offset: 0x00043D08
	public static SurfaceType GetSurfaceTypeFromPolyAndPoint(BasePolyAndSlope poly, Vector3 hitPoint)
	{
		Vector3 surfacePointFromPolyAndPosition = SurfaceTypeSampler.GetSurfacePointFromPolyAndPosition(poly, hitPoint);
		return SurfaceTypeSampler.GetSurfaceTypeFromPolyAndSurfacePoint(poly, surfacePointFromPolyAndPosition);
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00045B24 File Offset: 0x00043D24
	public static SurfaceType GetSurfaceTypeFromPolyAndSurfacePoint(BasePolyAndSlope poly, Vector3 surfacePoint)
	{
		SurfaceTypeSampler.GetValidSplatsAtPointNonAlloc(poly, surfacePoint, SurfaceTypeSampler.validSplats);
		Splat splat = SurfaceTypeSampler.FindBestSplatForDerivingSurfaceType(SurfaceTypeSampler.validSplats);
		return SurfaceTypeSampler.GetSurfaceTypeFromPolyAndSplat(poly, splat);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00045B4F File Offset: 0x00043D4F
	public static SurfaceType GetSurfaceTypeFromPolyAndSplat(BasePolyAndSlope poly, Splat bestSplat)
	{
		if (poly == null)
		{
			return SurfaceType.NONE;
		}
		if (bestSplat != null && !bestSplat.IgnoreSurfaceType)
		{
			return bestSplat.surfaceType;
		}
		return poly.fallbackSurfaceType;
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00045B7C File Offset: 0x00043D7C
	public static Vector3 GetSurfacePointFromPolyAndPosition(BasePolyAndSlope poly, Vector3 hitPoint)
	{
		Vector3 vector = poly.transform.InverseTransformPoint(hitPoint);
		if (poly is Slope)
		{
			Vector2[] array = ((Slope)poly).localPoints.ToArray();
			return poly.transform.TransformPoint(Polygon.FindClosestPointOnPolygon(array, vector, true));
		}
		if (poly is Poly)
		{
			Vector2[] vertices = ((Poly)poly).polygon.vertices;
			return poly.transform.TransformPoint(Polygon.FindClosestPointOnPolygon(vertices, vector, true));
		}
		Debug.LogWarning("Unknown poly type");
		return Vector3.zero;
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00045C14 File Offset: 0x00043E14
	public static void GetValidSplatsAtPointNonAlloc(BasePolyAndSlope poly, Vector3 surfacePoint, List<Splat> validSplats)
	{
		validSplats.Clear();
		if (poly == null)
		{
			return;
		}
		SurfaceTypeSampler._validSplatsCache.Clear();
		SurfaceTypeSampler._validSplatsCache.AddRange(poly.splats);
		for (int i = SurfaceTypeSampler._validSplatsCache.Count - 1; i >= 0; i--)
		{
			Splat splat = SurfaceTypeSampler._validSplatsCache[i];
			if (!splat.IgnoreSurfaceType && splat.ContainsPoint(surfacePoint))
			{
				validSplats.Add(splat);
			}
		}
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00045C8C File Offset: 0x00043E8C
	public static Splat FindBestSplatForDerivingSurfaceType(List<Splat> splats)
	{
		if (splats == null || splats.Count == 0)
		{
			return null;
		}
		float num = float.NegativeInfinity;
		Splat splat = null;
		for (int i = 0; i < splats.Count; i++)
		{
			Splat splat2 = splats[i];
			float num2 = (float)splat2.transform.GetSiblingIndex();
			if (num2 > num)
			{
				num = num2;
				splat = splat2;
			}
		}
		return splat;
	}

	// Token: 0x040009D0 RID: 2512
	[SerializeField]
	[Disable]
	private SurfaceType _surfaceType;

	// Token: 0x040009D1 RID: 2513
	public Action<SurfaceType, SurfaceType> OnChangeSurfaceType;

	// Token: 0x040009D2 RID: 2514
	private static List<Splat> validSplats = new List<Splat>();

	// Token: 0x040009D3 RID: 2515
	private static List<Splat> _validSplatsCache = new List<Splat>();
}
