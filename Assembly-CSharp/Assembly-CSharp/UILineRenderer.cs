using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001DA RID: 474
[AddComponentMenu("UI/Extensions/Primitives/UILineRenderer")]
public class UILineRenderer : UIPrimitiveBase
{
	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x0600105F RID: 4191 RVA: 0x00079AB8 File Offset: 0x00077CB8
	// (set) Token: 0x06001060 RID: 4192 RVA: 0x00079AC0 File Offset: 0x00077CC0
	public Rect uvRect
	{
		get
		{
			return this.m_UVRect;
		}
		set
		{
			if (this.m_UVRect == value)
			{
				return;
			}
			this.m_UVRect = value;
			this.SetVerticesDirty();
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x06001061 RID: 4193 RVA: 0x00079ADE File Offset: 0x00077CDE
	// (set) Token: 0x06001062 RID: 4194 RVA: 0x00079AE6 File Offset: 0x00077CE6
	public Vector2[] Points
	{
		get
		{
			return this.m_points;
		}
		set
		{
			this.m_points = value;
			this.SetAllDirty();
		}
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x00079AF5 File Offset: 0x00077CF5
	protected override void Awake()
	{
		base.Awake();
		base.useLegacyMeshGeneration = false;
	}

	// Token: 0x06001064 RID: 4196 RVA: 0x00079B04 File Offset: 0x00077D04
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		if (this.m_points == null)
		{
			return;
		}
		Vector2[] array = this.m_points;
		if (this.BezierMode != UILineRenderer.BezierType.None && this.m_points.Length > 3)
		{
			BezierPath bezierPath = new BezierPath();
			bezierPath.SetControlPoints(array);
			bezierPath.SegmentsPerCurve = this.BezierSegmentsPerCurve;
			UILineRenderer.BezierType bezierMode = this.BezierMode;
			List<Vector2> list;
			if (bezierMode != UILineRenderer.BezierType.Basic)
			{
				if (bezierMode != UILineRenderer.BezierType.Improved)
				{
					list = bezierPath.GetDrawingPoints2();
				}
				else
				{
					list = bezierPath.GetDrawingPoints1();
				}
			}
			else
			{
				list = bezierPath.GetDrawingPoints0();
			}
			array = list.ToArray();
		}
		float num = base.rectTransform.rect.width;
		float num2 = base.rectTransform.rect.height;
		float num3 = 0f;
		float num4 = 0f;
		if (!this.relativeSize)
		{
			num = 1f;
			num2 = 1f;
		}
		if (this.UseMargins)
		{
			num -= this.Margin.x;
			num2 -= this.Margin.y;
			num3 += this.Margin.x / 2f;
			num4 += this.Margin.y / 2f;
		}
		vh.Clear();
		List<UIVertex[]> list2 = new List<UIVertex[]>();
		if (this.LineList)
		{
			for (int i = 1; i < array.Length; i += 2)
			{
				Vector2 vector = array[i - 1];
				Vector2 vector2 = array[i];
				vector = new Vector2(vector.x * num + num3, vector.y * num2 + num4);
				vector2 = new Vector2(vector2.x * num + num3, vector2.y * num2 + num4);
				if (this.LineCaps)
				{
					list2.Add(this.CreateLineCap(vector, vector2, UILineRenderer.SegmentType.Start));
				}
				list2.Add(this.CreateLineSegment(vector, vector2, UILineRenderer.SegmentType.Middle));
				if (this.LineCaps)
				{
					list2.Add(this.CreateLineCap(vector, vector2, UILineRenderer.SegmentType.End));
				}
			}
		}
		else
		{
			for (int j = 1; j < array.Length; j++)
			{
				Vector2 vector3 = array[j - 1];
				Vector2 vector4 = array[j];
				vector3 = new Vector2(vector3.x * num + num3, vector3.y * num2 + num4);
				vector4 = new Vector2(vector4.x * num + num3, vector4.y * num2 + num4);
				if (this.LineCaps && j == 1)
				{
					list2.Add(this.CreateLineCap(vector3, vector4, UILineRenderer.SegmentType.Start));
				}
				list2.Add(this.CreateLineSegment(vector3, vector4, UILineRenderer.SegmentType.Middle));
				if (this.LineCaps && j == array.Length - 1)
				{
					list2.Add(this.CreateLineCap(vector3, vector4, UILineRenderer.SegmentType.End));
				}
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			if (!this.LineList && k < list2.Count - 1)
			{
				Vector3 vector5 = list2[k][1].position - list2[k][2].position;
				Vector3 vector6 = list2[k + 1][2].position - list2[k + 1][1].position;
				float num5 = Vector2.Angle(vector5, vector6) * 0.017453292f;
				float num6 = Mathf.Sign(Vector3.Cross(vector5.normalized, vector6.normalized).z);
				float num7 = this.LineThickness / (2f * Mathf.Tan(num5 / 2f));
				Vector3 vector7 = list2[k][2].position - vector5.normalized * num7 * num6;
				Vector3 vector8 = list2[k][3].position + vector5.normalized * num7 * num6;
				UILineRenderer.JoinType joinType = this.LineJoins;
				if (joinType == UILineRenderer.JoinType.Miter)
				{
					if (num7 < vector5.magnitude / 2f && num7 < vector6.magnitude / 2f && num5 > 0.2617994f)
					{
						list2[k][2].position = vector7;
						list2[k][3].position = vector8;
						list2[k + 1][0].position = vector8;
						list2[k + 1][1].position = vector7;
					}
					else
					{
						joinType = UILineRenderer.JoinType.Bevel;
					}
				}
				if (joinType == UILineRenderer.JoinType.Bevel)
				{
					if (num7 < vector5.magnitude / 2f && num7 < vector6.magnitude / 2f && num5 > 0.5235988f)
					{
						if (num6 < 0f)
						{
							list2[k][2].position = vector7;
							list2[k + 1][1].position = vector7;
						}
						else
						{
							list2[k][3].position = vector8;
							list2[k + 1][0].position = vector8;
						}
					}
					UIVertex[] array2 = new UIVertex[]
					{
						list2[k][2],
						list2[k][3],
						list2[k + 1][0],
						list2[k + 1][1]
					};
					vh.AddUIVertexQuad(array2);
				}
			}
			vh.AddUIVertexQuad(list2[k]);
		}
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x0007A0B4 File Offset: 0x000782B4
	private UIVertex[] CreateLineCap(Vector2 start, Vector2 end, UILineRenderer.SegmentType type)
	{
		if (type == UILineRenderer.SegmentType.Start)
		{
			Vector2 vector = start - (end - start).normalized * this.LineThickness / 2f;
			return this.CreateLineSegment(vector, start, UILineRenderer.SegmentType.Start);
		}
		if (type == UILineRenderer.SegmentType.End)
		{
			Vector2 vector2 = end + (end - start).normalized * this.LineThickness / 2f;
			return this.CreateLineSegment(end, vector2, UILineRenderer.SegmentType.End);
		}
		Debug.LogError("Bad SegmentType passed in to CreateLineCap. Must be SegmentType.Start or SegmentType.End");
		return null;
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x0007A140 File Offset: 0x00078340
	private UIVertex[] CreateLineSegment(Vector2 start, Vector2 end, UILineRenderer.SegmentType type)
	{
		Vector2[] array = UILineRenderer.middleUvs;
		if (type == UILineRenderer.SegmentType.Start)
		{
			array = UILineRenderer.startUvs;
		}
		else if (type == UILineRenderer.SegmentType.End)
		{
			array = UILineRenderer.endUvs;
		}
		Vector2 vector = new Vector2(start.y - end.y, end.x - start.x).normalized * this.LineThickness / 2f;
		Vector2 vector2 = start - vector;
		Vector2 vector3 = start + vector;
		Vector2 vector4 = end + vector;
		Vector2 vector5 = end - vector;
		return base.SetVbo(new Vector2[] { vector2, vector3, vector4, vector5 }, array);
	}

	// Token: 0x0400121D RID: 4637
	private const float MIN_MITER_JOIN = 0.2617994f;

	// Token: 0x0400121E RID: 4638
	private const float MIN_BEVEL_NICE_JOIN = 0.5235988f;

	// Token: 0x0400121F RID: 4639
	private static readonly Vector2 UV_TOP_LEFT = Vector2.zero;

	// Token: 0x04001220 RID: 4640
	private static readonly Vector2 UV_BOTTOM_LEFT = new Vector2(0f, 1f);

	// Token: 0x04001221 RID: 4641
	private static readonly Vector2 UV_TOP_CENTER = new Vector2(0.5f, 0f);

	// Token: 0x04001222 RID: 4642
	private static readonly Vector2 UV_BOTTOM_CENTER = new Vector2(0.5f, 1f);

	// Token: 0x04001223 RID: 4643
	private static readonly Vector2 UV_TOP_RIGHT = new Vector2(1f, 0f);

	// Token: 0x04001224 RID: 4644
	private static readonly Vector2 UV_BOTTOM_RIGHT = new Vector2(1f, 1f);

	// Token: 0x04001225 RID: 4645
	private static readonly Vector2[] startUvs = new Vector2[]
	{
		UILineRenderer.UV_TOP_LEFT,
		UILineRenderer.UV_BOTTOM_LEFT,
		UILineRenderer.UV_BOTTOM_CENTER,
		UILineRenderer.UV_TOP_CENTER
	};

	// Token: 0x04001226 RID: 4646
	private static readonly Vector2[] middleUvs = new Vector2[]
	{
		UILineRenderer.UV_TOP_CENTER,
		UILineRenderer.UV_BOTTOM_CENTER,
		UILineRenderer.UV_BOTTOM_CENTER,
		UILineRenderer.UV_TOP_CENTER
	};

	// Token: 0x04001227 RID: 4647
	private static readonly Vector2[] endUvs = new Vector2[]
	{
		UILineRenderer.UV_TOP_CENTER,
		UILineRenderer.UV_BOTTOM_CENTER,
		UILineRenderer.UV_BOTTOM_RIGHT,
		UILineRenderer.UV_TOP_RIGHT
	};

	// Token: 0x04001228 RID: 4648
	[SerializeField]
	private Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x04001229 RID: 4649
	[SerializeField]
	private Vector2[] m_points;

	// Token: 0x0400122A RID: 4650
	public float LineThickness = 2f;

	// Token: 0x0400122B RID: 4651
	public bool UseMargins;

	// Token: 0x0400122C RID: 4652
	public Vector2 Margin;

	// Token: 0x0400122D RID: 4653
	public bool relativeSize;

	// Token: 0x0400122E RID: 4654
	public bool LineList;

	// Token: 0x0400122F RID: 4655
	public bool LineCaps;

	// Token: 0x04001230 RID: 4656
	public UILineRenderer.JoinType LineJoins;

	// Token: 0x04001231 RID: 4657
	public UILineRenderer.BezierType BezierMode;

	// Token: 0x04001232 RID: 4658
	public int BezierSegmentsPerCurve = 10;

	// Token: 0x020003EC RID: 1004
	private enum SegmentType
	{
		// Token: 0x04001A58 RID: 6744
		Start,
		// Token: 0x04001A59 RID: 6745
		Middle,
		// Token: 0x04001A5A RID: 6746
		End
	}

	// Token: 0x020003ED RID: 1005
	public enum JoinType
	{
		// Token: 0x04001A5C RID: 6748
		Bevel,
		// Token: 0x04001A5D RID: 6749
		Miter
	}

	// Token: 0x020003EE RID: 1006
	public enum BezierType
	{
		// Token: 0x04001A5F RID: 6751
		None,
		// Token: 0x04001A60 RID: 6752
		Quick,
		// Token: 0x04001A61 RID: 6753
		Basic,
		// Token: 0x04001A62 RID: 6754
		Improved
	}
}
