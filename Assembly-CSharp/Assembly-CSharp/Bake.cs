using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000010 RID: 16
public static class Bake
{
	// Token: 0x0600004D RID: 77 RVA: 0x00005D90 File Offset: 0x00003F90
	public static void Flip(Transform t, string undoString = null)
	{
		Vector3 localScale = t.localScale;
		localScale.x *= -1f;
		t.localScale = localScale;
		Chunk component = t.GetComponent<Chunk>();
		Bake._splatsScratch.Clear();
		t.GetComponentsInChildren<Splat>(Bake._splatsScratch);
		foreach (Splat splat in Bake._splatsScratch)
		{
			splat.flipX = !splat.flipX;
		}
		Bake._splatsScratch.Clear();
		Bake.BakeAll(t, undoString);
		if (component)
		{
			Slope leftSlope = component.leftSlope;
			component.leftSlope = component.rightSlope;
			component.rightSlope = leftSlope;
			component.validSlopeAbsAngleRange = new Range(-component.validSlopeAbsAngleRange.max, -component.validSlopeAbsAngleRange.min);
			if (component.direction == Chunk.Direction.Westwards)
			{
				component.direction = Chunk.Direction.Eastwards;
			}
			else if (component.direction == Chunk.Direction.Eastwards)
			{
				component.direction = Chunk.Direction.Westwards;
			}
			component.FindStartEndMarkersIfNecessary();
			if (component.startMarker != null)
			{
				component.startMarker.rightwards = !component.startMarker.rightwards;
			}
			if (component.endMarker != null)
			{
				component.endMarker.rightwards = !component.endMarker.rightwards;
			}
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00005EF4 File Offset: 0x000040F4
	public static void BakeAll(Transform topTransform, string undoString = null)
	{
		Bake.<>c__DisplayClass3_0 CS$<>8__locals1;
		CS$<>8__locals1.bakeItems = new List<Bake.BakeItem>();
		Bake.<BakeAll>g__AddForBakingAndRecurse|3_1(topTransform, ref CS$<>8__locals1);
		for (int i = CS$<>8__locals1.bakeItems.Count - 1; i >= 0; i--)
		{
			Bake.BakeItem bakeItem = CS$<>8__locals1.bakeItems[i];
			bakeItem.transform.position = bakeItem.position;
			bakeItem.transform.rotation = bakeItem.rotation;
			bakeItem.transform.localScale = bakeItem.lossyScale;
			Slope component = bakeItem.transform.GetComponent<Slope>();
			if (component)
			{
				component.localPoints.Clear();
				foreach (Vector2 vector in bakeItem.worldPoints)
				{
					Vector3 vector2 = component.transform.InverseTransformPoint(vector);
					component.localPoints.Add(vector2);
				}
				if (bakeItem.flipping)
				{
					List<Slope> leftSlopes = component.leftSlopes;
					component.leftSlopes = component.rightSlopes;
					component.rightSlopes = leftSlopes;
				}
			}
			Poly component2 = bakeItem.transform.GetComponent<Poly>();
			if (component2)
			{
				Vector2[] vertices = component2.polygon.vertices;
				for (int j = 0; j < bakeItem.worldPoints.Count; j++)
				{
					Vector3 vector3 = bakeItem.transform.InverseTransformPoint(bakeItem.worldPoints[j]);
					vertices[j] = vector3;
				}
				if (!component2.polygon.GetIsClockwise())
				{
					component2.polygon.FlipWindingOrder();
				}
			}
		}
		for (int k = CS$<>8__locals1.bakeItems.Count - 1; k >= 0; k--)
		{
			Poly component3 = CS$<>8__locals1.bakeItems[k].transform.GetComponent<Poly>();
			if (component3)
			{
				component3.FullRefresh();
			}
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00006104 File Offset: 0x00004304
	[CompilerGenerated]
	internal static void <BakeAll>g__AddBakeItem|3_0(Transform t, IList<Vector2> localPoints, bool autoReverseSlopeVerts, bool resetTransform, ref Bake.<>c__DisplayClass3_0 A_4)
	{
		Bake.BakeItem bakeItem = new Bake.BakeItem
		{
			transform = t
		};
		bakeItem.position = t.position;
		if (localPoints != null)
		{
			bakeItem.worldPoints = new List<Vector2>(localPoints.Count);
			for (int i = 0; i < localPoints.Count; i++)
			{
				Vector2 vector = t.TransformPoint(localPoints[i]);
				bakeItem.worldPoints.Add(vector);
			}
			if (autoReverseSlopeVerts && bakeItem.worldPoints.Count >= 2 && bakeItem.worldPoints[0].x > bakeItem.worldPoints[1].x)
			{
				bakeItem.worldPoints.Reverse();
				bakeItem.flipping = true;
			}
		}
		if (resetTransform)
		{
			bakeItem.rotation = Quaternion.identity;
			bakeItem.lossyScale = Vector3.one;
		}
		else
		{
			bakeItem.rotation = t.rotation;
			bakeItem.lossyScale = t.lossyScale;
		}
		A_4.bakeItems.Add(bakeItem);
	}

	// Token: 0x06000051 RID: 81 RVA: 0x0000620C File Offset: 0x0000440C
	[CompilerGenerated]
	internal static bool <BakeAll>g__AddForBakingAndRecurse|3_1(Transform t, ref Bake.<>c__DisplayClass3_0 A_1)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (object obj in t)
		{
			bool flag4 = Bake.<BakeAll>g__AddForBakingAndRecurse|3_1((Transform)obj, ref A_1);
			flag3 = flag3 || flag4;
		}
		Poly component = t.GetComponent<Poly>();
		if (component)
		{
			Bake.<BakeAll>g__AddBakeItem|3_0(t, component.polygon.vertices, false, true, ref A_1);
			flag = true;
		}
		else
		{
			Slope component2 = t.GetComponent<Slope>();
			if (component2)
			{
				Bake.<BakeAll>g__AddBakeItem|3_0(t, component2.localPoints, true, true, ref A_1);
				flag = true;
			}
			else
			{
				if (t.GetComponent<Splat>())
				{
					flag2 = true;
				}
				Bake.<BakeAll>g__AddBakeItem|3_0(t, null, false, flag || flag3, ref A_1);
			}
		}
		return flag || flag2 || flag3;
	}

	// Token: 0x04000072 RID: 114
	private static List<Splat> _splatsScratch = new List<Splat>(128);

	// Token: 0x02000245 RID: 581
	private struct BakeItem
	{
		// Token: 0x040013BF RID: 5055
		public Transform transform;

		// Token: 0x040013C0 RID: 5056
		public Vector3 position;

		// Token: 0x040013C1 RID: 5057
		public Quaternion rotation;

		// Token: 0x040013C2 RID: 5058
		public Vector3 lossyScale;

		// Token: 0x040013C3 RID: 5059
		public List<Vector2> worldPoints;

		// Token: 0x040013C4 RID: 5060
		public bool flipping;
	}
}
