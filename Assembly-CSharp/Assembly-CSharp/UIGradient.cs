using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000157 RID: 343
[AddComponentMenu("UI/Effects/Extensions/UI Gradient")]
public class UIGradient : BaseMeshEffect
{
	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x0005D3F8 File Offset: 0x0005B5F8
	// (set) Token: 0x06000BA2 RID: 2978 RVA: 0x0005D400 File Offset: 0x0005B600
	public UIGradient.Blend BlendMode
	{
		get
		{
			return this._blendMode;
		}
		set
		{
			this._blendMode = value;
			base.graphic.SetVerticesDirty();
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0005D414 File Offset: 0x0005B614
	// (set) Token: 0x06000BA4 RID: 2980 RVA: 0x0005D41C File Offset: 0x0005B61C
	public Gradient EffectGradient
	{
		get
		{
			return this._effectGradient;
		}
		set
		{
			this._effectGradient = value;
			base.graphic.SetVerticesDirty();
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0005D430 File Offset: 0x0005B630
	// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x0005D438 File Offset: 0x0005B638
	public UIGradient.Type GradientType
	{
		get
		{
			return this._gradientType;
		}
		set
		{
			this._gradientType = value;
			base.graphic.SetVerticesDirty();
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0005D44C File Offset: 0x0005B64C
	// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x0005D454 File Offset: 0x0005B654
	public bool ModifyVertices
	{
		get
		{
			return this._modifyVertices;
		}
		set
		{
			this._modifyVertices = value;
			base.graphic.SetVerticesDirty();
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x0005D468 File Offset: 0x0005B668
	// (set) Token: 0x06000BAA RID: 2986 RVA: 0x0005D470 File Offset: 0x0005B670
	public float Offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			this._offset = value;
			base.graphic.SetVerticesDirty();
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06000BAB RID: 2987 RVA: 0x0005D484 File Offset: 0x0005B684
	// (set) Token: 0x06000BAC RID: 2988 RVA: 0x0005D48C File Offset: 0x0005B68C
	public float Zoom
	{
		get
		{
			return this._zoom;
		}
		set
		{
			this._zoom = value;
			base.graphic.SetVerticesDirty();
		}
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x0005D4A0 File Offset: 0x0005B6A0
	public void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys)
	{
		this._effectGradient.SetKeys(colorKeys, alphaKeys);
		base.graphic.SetVerticesDirty();
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0005D4BC File Offset: 0x0005B6BC
	public override void ModifyMesh(VertexHelper helper)
	{
		if (!this.IsActive() || helper.currentVertCount == 0)
		{
			return;
		}
		helper.GetUIVertexStream(this._vertexList);
		int count = this._vertexList.Count;
		switch (this.GradientType)
		{
		case UIGradient.Type.Horizontal:
		case UIGradient.Type.Vertical:
		{
			Rect bounds = this.GetBounds(this._vertexList);
			float num = bounds.xMin;
			float num2 = bounds.width;
			Func<UIVertex, float> func = (UIVertex v) => v.position.x;
			if (this.GradientType == UIGradient.Type.Vertical)
			{
				num = bounds.yMin;
				num2 = bounds.height;
				func = (UIVertex v) => v.position.y;
			}
			float num3 = ((num2 == 0f) ? 0f : (1f / num2 / this.Zoom));
			float num4 = (1f - 1f / this.Zoom) * 0.5f;
			float num5 = this.Offset * (1f - num4) - num4;
			if (this.ModifyVertices)
			{
				this.SplitTrianglesAtGradientStops(this._vertexList, bounds, num4, helper);
			}
			UIVertex uivertex = default(UIVertex);
			for (int i = 0; i < helper.currentVertCount; i++)
			{
				helper.PopulateUIVertex(ref uivertex, i);
				uivertex.color = this.BlendColor(uivertex.color, this.EffectGradient.Evaluate((func(uivertex) - num) * num3 - num5));
				helper.SetUIVertex(uivertex, i);
			}
			return;
		}
		case UIGradient.Type.Radial:
		{
			Rect bounds2 = this.GetBounds(this._vertexList);
			float num6 = 1f / bounds2.width / this.Zoom;
			float num7 = 1f / bounds2.height / this.Zoom;
			if (this.ModifyVertices)
			{
				helper.Clear();
				float num8 = bounds2.width / 2f;
				float num9 = bounds2.height / 2f;
				UIVertex uivertex2 = default(UIVertex);
				uivertex2.position = Vector3.right * bounds2.center.x + Vector3.up * bounds2.center.y + Vector3.forward * this._vertexList[0].position.z;
				uivertex2.normal = this._vertexList[0].normal;
				uivertex2.uv0 = new Vector2(0.5f, 0.5f);
				uivertex2.color = this._vertexList[0].color;
				int num10 = 64;
				for (int j = 0; j < num10; j++)
				{
					UIVertex uivertex3 = default(UIVertex);
					float num11 = (float)j * 360f / (float)num10;
					float num12 = Mathf.Cos(0.017453292f * num11);
					float num13 = Mathf.Sin(0.017453292f * num11);
					uivertex3.position = Vector3.right * num12 * num8 + Vector3.up * num13 * num9 + Vector3.forward * this._vertexList[0].position.z;
					uivertex3.normal = this._vertexList[0].normal;
					uivertex3.uv0 = new Vector2((num12 + 1f) * 0.5f, (num13 + 1f) * 0.5f);
					uivertex3.color = this._vertexList[0].color;
					helper.AddVert(uivertex3);
				}
				helper.AddVert(uivertex2);
				for (int k = 1; k < num10; k++)
				{
					helper.AddTriangle(k - 1, k, num10);
				}
				helper.AddTriangle(0, num10 - 1, num10);
			}
			UIVertex uivertex4 = default(UIVertex);
			for (int l = 0; l < helper.currentVertCount; l++)
			{
				helper.PopulateUIVertex(ref uivertex4, l);
				uivertex4.color = this.BlendColor(uivertex4.color, this.EffectGradient.Evaluate(Mathf.Sqrt(Mathf.Pow(Mathf.Abs(uivertex4.position.x - bounds2.center.x) * num6, 2f) + Mathf.Pow(Mathf.Abs(uivertex4.position.y - bounds2.center.y) * num7, 2f)) * 2f - this.Offset));
				helper.SetUIVertex(uivertex4, l);
			}
			return;
		}
		case UIGradient.Type.Diamond:
		{
			Rect bounds3 = this.GetBounds(this._vertexList);
			float num14 = 1f / bounds3.height / this.Zoom;
			float num15 = bounds3.center.y / 2f;
			Vector3 vector = (Vector3.right + Vector3.up) * num15 + Vector3.forward * this._vertexList[0].position.z;
			if (this.ModifyVertices)
			{
				helper.Clear();
				for (int m = 0; m < count; m++)
				{
					helper.AddVert(this._vertexList[m]);
				}
				helper.AddVert(new UIVertex
				{
					position = vector,
					normal = this._vertexList[0].normal,
					uv0 = new Vector2(0.5f, 0.5f),
					color = this._vertexList[0].color
				});
				for (int n = 1; n < count; n++)
				{
					helper.AddTriangle(n - 1, n, count);
				}
				helper.AddTriangle(0, count - 1, count);
			}
			UIVertex uivertex5 = default(UIVertex);
			for (int num16 = 0; num16 < helper.currentVertCount; num16++)
			{
				helper.PopulateUIVertex(ref uivertex5, num16);
				uivertex5.color = this.BlendColor(uivertex5.color, this.EffectGradient.Evaluate(Vector3.Distance(uivertex5.position, vector) * num14 - this.Offset));
				helper.SetUIVertex(uivertex5, num16);
			}
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x0005DB18 File Offset: 0x0005BD18
	private Rect GetBounds(List<UIVertex> vertices)
	{
		float num = vertices[0].position.x;
		float num2 = num;
		float num3 = vertices[0].position.y;
		float num4 = num3;
		for (int i = vertices.Count - 1; i >= 1; i--)
		{
			Vector3 position = vertices[i].position;
			float x = position.x;
			float y = position.y;
			if (x > num2)
			{
				num2 = x;
			}
			else if (x < num)
			{
				num = x;
			}
			if (y > num4)
			{
				num4 = y;
			}
			else if (y < num3)
			{
				num3 = y;
			}
		}
		return new Rect(num, num3, num2 - num, num4 - num3);
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x0005DBB4 File Offset: 0x0005BDB4
	private void SplitTrianglesAtGradientStops(List<UIVertex> _vertexList, Rect bounds, float zoomOffset, VertexHelper helper)
	{
		List<float> list = this.FindStops(zoomOffset, bounds);
		if (list.Count > 0)
		{
			helper.Clear();
			int count = _vertexList.Count;
			for (int i = 0; i < count; i += 3)
			{
				float[] positions = this.GetPositions(_vertexList, i);
				List<int> list2 = new List<int>(3);
				List<UIVertex> list3 = new List<UIVertex>(3);
				List<UIVertex> list4 = new List<UIVertex>(2);
				for (int j = 0; j < list.Count; j++)
				{
					int currentVertCount = helper.currentVertCount;
					bool flag = list4.Count > 0;
					bool flag2 = false;
					for (int k = 0; k < 3; k++)
					{
						if (!list2.Contains(k) && positions[k] < list[j])
						{
							int num = (k + 1) % 3;
							UIVertex uivertex = _vertexList[k + i];
							if (positions[num] > list[j])
							{
								list2.Insert(0, k);
								list3.Insert(0, uivertex);
								flag2 = true;
							}
							else
							{
								list2.Add(k);
								list3.Add(uivertex);
							}
						}
					}
					if (list2.Count != 0)
					{
						if (list2.Count == 3)
						{
							break;
						}
						foreach (UIVertex uivertex2 in list3)
						{
							helper.AddVert(uivertex2);
						}
						list4.Clear();
						foreach (int num2 in list2)
						{
							int num3 = (num2 + 1) % 3;
							if (positions[num3] < list[j])
							{
								num3 = (num3 + 1) % 3;
							}
							list4.Add(this.CreateSplitVertex(_vertexList[num2 + i], _vertexList[num3 + i], list[j]));
						}
						if (list4.Count == 1)
						{
							int num4 = (list2[0] + 2) % 3;
							list4.Add(this.CreateSplitVertex(_vertexList[list2[0] + i], _vertexList[num4 + i], list[j]));
						}
						foreach (UIVertex uivertex3 in list4)
						{
							helper.AddVert(uivertex3);
						}
						if (flag)
						{
							helper.AddTriangle(currentVertCount - 2, currentVertCount, currentVertCount + 1);
							helper.AddTriangle(currentVertCount - 2, currentVertCount + 1, currentVertCount - 1);
							if (list3.Count > 0)
							{
								if (flag2)
								{
									helper.AddTriangle(currentVertCount - 2, currentVertCount + 3, currentVertCount);
								}
								else
								{
									helper.AddTriangle(currentVertCount + 1, currentVertCount + 3, currentVertCount - 1);
								}
							}
						}
						else
						{
							int currentVertCount2 = helper.currentVertCount;
							helper.AddTriangle(currentVertCount, currentVertCount2 - 2, currentVertCount2 - 1);
							if (list3.Count > 1)
							{
								helper.AddTriangle(currentVertCount, currentVertCount2 - 1, currentVertCount + 1);
							}
						}
						list3.Clear();
					}
				}
				if (list4.Count > 0)
				{
					if (list3.Count == 0)
					{
						for (int l = 0; l < 3; l++)
						{
							if (!list2.Contains(l) && positions[l] > list[list.Count - 1])
							{
								int num5 = (l + 1) % 3;
								UIVertex uivertex4 = _vertexList[l + i];
								if (positions[num5] > list[list.Count - 1])
								{
									list3.Insert(0, uivertex4);
								}
								else
								{
									list3.Add(uivertex4);
								}
							}
						}
					}
					foreach (UIVertex uivertex5 in list3)
					{
						helper.AddVert(uivertex5);
					}
					int currentVertCount3 = helper.currentVertCount;
					if (list3.Count > 1)
					{
						helper.AddTriangle(currentVertCount3 - 4, currentVertCount3 - 2, currentVertCount3 - 1);
						helper.AddTriangle(currentVertCount3 - 4, currentVertCount3 - 1, currentVertCount3 - 3);
					}
					else if (list3.Count > 0)
					{
						helper.AddTriangle(currentVertCount3 - 3, currentVertCount3 - 1, currentVertCount3 - 2);
					}
				}
				else
				{
					helper.AddVert(_vertexList[i]);
					helper.AddVert(_vertexList[i + 1]);
					helper.AddVert(_vertexList[i + 2]);
					int currentVertCount4 = helper.currentVertCount;
					helper.AddTriangle(currentVertCount4 - 3, currentVertCount4 - 2, currentVertCount4 - 1);
				}
			}
		}
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x0005E034 File Offset: 0x0005C234
	private float[] GetPositions(List<UIVertex> _vertexList, int index)
	{
		float[] array = new float[3];
		if (this.GradientType == UIGradient.Type.Horizontal)
		{
			array[0] = _vertexList[index].position.x;
			array[1] = _vertexList[index + 1].position.x;
			array[2] = _vertexList[index + 2].position.x;
		}
		else
		{
			array[0] = _vertexList[index].position.y;
			array[1] = _vertexList[index + 1].position.y;
			array[2] = _vertexList[index + 2].position.y;
		}
		return array;
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x0005E0D4 File Offset: 0x0005C2D4
	private List<float> FindStops(float zoomOffset, Rect bounds)
	{
		List<float> list = new List<float>();
		float num = this.Offset * (1f - zoomOffset);
		float num2 = zoomOffset - num;
		float num3 = 1f - zoomOffset - num;
		foreach (GradientColorKey gradientColorKey in this.EffectGradient.colorKeys)
		{
			if (gradientColorKey.time >= num3)
			{
				break;
			}
			if (gradientColorKey.time > num2)
			{
				list.Add((gradientColorKey.time - num2) * this.Zoom);
			}
		}
		foreach (GradientAlphaKey gradientAlphaKey in this.EffectGradient.alphaKeys)
		{
			if (gradientAlphaKey.time >= num3)
			{
				break;
			}
			if (gradientAlphaKey.time > num2)
			{
				list.Add((gradientAlphaKey.time - num2) * this.Zoom);
			}
		}
		float num4 = bounds.xMin;
		float num5 = bounds.width;
		if (this.GradientType == UIGradient.Type.Vertical)
		{
			num4 = bounds.yMin;
			num5 = bounds.height;
		}
		list.Sort();
		for (int j = 0; j < list.Count; j++)
		{
			list[j] = list[j] * num5 + num4;
			if (j > 0 && Math.Abs(list[j] - list[j - 1]) < 2f)
			{
				list.RemoveAt(j);
				j--;
			}
		}
		return list;
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x0005E240 File Offset: 0x0005C440
	private UIVertex CreateSplitVertex(UIVertex vertex1, UIVertex vertex2, float stop)
	{
		if (this.GradientType == UIGradient.Type.Horizontal)
		{
			float num = vertex1.position.x - stop;
			float num2 = vertex1.position.x - vertex2.position.x;
			float num3 = vertex1.position.y - vertex2.position.y;
			float num4 = vertex1.uv0.x - vertex2.uv0.x;
			float num5 = vertex1.uv0.y - vertex2.uv0.y;
			float num6 = num / num2;
			float num7 = vertex1.position.y - num3 * num6;
			return new UIVertex
			{
				position = new Vector3(stop, num7, vertex1.position.z),
				normal = vertex1.normal,
				uv0 = new Vector2(vertex1.uv0.x - num4 * num6, vertex1.uv0.y - num5 * num6),
				color = vertex1.color
			};
		}
		float num8 = vertex1.position.y - stop;
		float num9 = vertex1.position.y - vertex2.position.y;
		float num10 = vertex1.position.x - vertex2.position.x;
		float num11 = vertex1.uv0.x - vertex2.uv0.x;
		float num12 = vertex1.uv0.y - vertex2.uv0.y;
		float num13 = num8 / num9;
		float num14 = vertex1.position.x - num10 * num13;
		return new UIVertex
		{
			position = new Vector3(num14, stop, vertex1.position.z),
			normal = vertex1.normal,
			uv0 = new Vector2(vertex1.uv0.x - num11 * num13, vertex1.uv0.y - num12 * num13),
			color = vertex1.color
		};
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x0005E448 File Offset: 0x0005C648
	private Color32 BlendColor(Color32 colorA, Color32 colorB)
	{
		if (this.strength == 0f)
		{
			return colorA;
		}
		UIGradient.Blend blendMode = this.BlendMode;
		if (blendMode != UIGradient.Blend.Add)
		{
			if (blendMode != UIGradient.Blend.Multiply)
			{
				if (this.strength == 1f)
				{
					return colorB;
				}
				return Color32.Lerp(colorA, colorB, this.strength);
			}
			else
			{
				Color32 color = colorA * colorB;
				if (this.strength == 1f)
				{
					return color;
				}
				return Color32.Lerp(colorA, color, this.strength);
			}
		}
		else
		{
			Color32 color2 = colorA + colorB;
			if (this.strength == 1f)
			{
				return color2;
			}
			return Color32.Lerp(colorA, color2, this.strength);
		}
	}

	// Token: 0x04000DCF RID: 3535
	[Range(0f, 1f)]
	public float strength = 1f;

	// Token: 0x04000DD0 RID: 3536
	[SerializeField]
	private UIGradient.Type _gradientType;

	// Token: 0x04000DD1 RID: 3537
	[SerializeField]
	private UIGradient.Blend _blendMode = UIGradient.Blend.Multiply;

	// Token: 0x04000DD2 RID: 3538
	[SerializeField]
	[Tooltip("Add vertices to display complex gradients. Turn off if your shape is already very complex, like text.")]
	private bool _modifyVertices = true;

	// Token: 0x04000DD3 RID: 3539
	[SerializeField]
	[Range(-1f, 1f)]
	private float _offset;

	// Token: 0x04000DD4 RID: 3540
	[SerializeField]
	[Range(0.1f, 10f)]
	private float _zoom = 1f;

	// Token: 0x04000DD5 RID: 3541
	[SerializeField]
	private Gradient _effectGradient = new Gradient
	{
		colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(Color.black, 0f),
			new GradientColorKey(Color.white, 1f)
		}
	};

	// Token: 0x04000DD6 RID: 3542
	private List<UIVertex> _vertexList = new List<UIVertex>();

	// Token: 0x0200038B RID: 907
	public enum Type
	{
		// Token: 0x04001935 RID: 6453
		Horizontal,
		// Token: 0x04001936 RID: 6454
		Vertical,
		// Token: 0x04001937 RID: 6455
		Radial,
		// Token: 0x04001938 RID: 6456
		Diamond
	}

	// Token: 0x0200038C RID: 908
	public enum Blend
	{
		// Token: 0x0400193A RID: 6458
		Override,
		// Token: 0x0400193B RID: 6459
		Add,
		// Token: 0x0400193C RID: 6460
		Multiply
	}
}
