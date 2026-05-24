using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001DC RID: 476
public class UIPrimitiveBase : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
{
	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x0007A360 File Offset: 0x00078560
	// (set) Token: 0x0600106A RID: 4202 RVA: 0x0007A368 File Offset: 0x00078568
	public Sprite sprite
	{
		get
		{
			return this.m_Sprite;
		}
		set
		{
			if (SetPropertyUtility.SetClass<Sprite>(ref this.m_Sprite, value))
			{
				this.SetAllDirty();
			}
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x0600106B RID: 4203 RVA: 0x0007A37E File Offset: 0x0007857E
	// (set) Token: 0x0600106C RID: 4204 RVA: 0x0007A39B File Offset: 0x0007859B
	public Sprite overrideSprite
	{
		get
		{
			if (!(this.m_OverrideSprite == null))
			{
				return this.m_OverrideSprite;
			}
			return this.sprite;
		}
		set
		{
			if (SetPropertyUtility.SetClass<Sprite>(ref this.m_OverrideSprite, value))
			{
				this.SetAllDirty();
			}
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x0600106D RID: 4205 RVA: 0x0007A3B1 File Offset: 0x000785B1
	// (set) Token: 0x0600106E RID: 4206 RVA: 0x0007A3B9 File Offset: 0x000785B9
	public float eventAlphaThreshold
	{
		get
		{
			return this.m_EventAlphaThreshold;
		}
		set
		{
			this.m_EventAlphaThreshold = value;
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x0600106F RID: 4207 RVA: 0x0007A3C2 File Offset: 0x000785C2
	// (set) Token: 0x06001070 RID: 4208 RVA: 0x0007A3CA File Offset: 0x000785CA
	public ResolutionMode ImproveResolution
	{
		get
		{
			return this.m_improveResolution;
		}
		set
		{
			this.m_improveResolution = value;
			this.SetAllDirty();
		}
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06001071 RID: 4209 RVA: 0x0007A3D9 File Offset: 0x000785D9
	// (set) Token: 0x06001072 RID: 4210 RVA: 0x0007A3E1 File Offset: 0x000785E1
	public float Resoloution
	{
		get
		{
			return this.m_Resolution;
		}
		set
		{
			this.m_Resolution = value;
			this.SetAllDirty();
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06001073 RID: 4211 RVA: 0x0007A3F0 File Offset: 0x000785F0
	public override Texture mainTexture
	{
		get
		{
			if (!(this.overrideSprite == null))
			{
				return this.overrideSprite.texture;
			}
			if (this.material != null && this.material.mainTexture != null)
			{
				return this.material.mainTexture;
			}
			return Graphic.s_WhiteTexture;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06001074 RID: 4212 RVA: 0x0007A44C File Offset: 0x0007864C
	public float pixelsPerUnit
	{
		get
		{
			float num = 100f;
			if (this.sprite)
			{
				num = this.sprite.pixelsPerUnit;
			}
			float num2 = 100f;
			if (base.canvas)
			{
				num2 = base.canvas.referencePixelsPerUnit;
			}
			return num / num2;
		}
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x0007A49C File Offset: 0x0007869C
	protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
	{
		UIVertex[] array = new UIVertex[4];
		for (int i = 0; i < vertices.Length; i++)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = this.color;
			simpleVert.position = vertices[i];
			simpleVert.uv0 = uvs[i];
			array[i] = simpleVert;
		}
		return array;
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x0007A508 File Offset: 0x00078708
	protected Vector2[] IncreaseResolution(Vector2[] input)
	{
		List<Vector2> list = new List<Vector2>();
		ResolutionMode improveResolution = this.ImproveResolution;
		if (improveResolution != ResolutionMode.PerSegment)
		{
			if (improveResolution == ResolutionMode.PerLine)
			{
				float num = 0f;
				for (int i = 0; i < input.Length - 1; i++)
				{
					num += Vector2.Distance(input[i], input[i + 1]);
				}
				float num2 = num / this.m_Resolution;
				int num3 = 0;
				for (int j = 0; j < input.Length - 1; j++)
				{
					Vector2 vector = input[j];
					list.Add(vector);
					Vector2 vector2 = input[j + 1];
					float num4 = Vector2.Distance(vector, vector2) / num2;
					float num5 = 1f / num4;
					int num6 = 0;
					while ((float)num6 < num4)
					{
						list.Add(Vector2.Lerp(vector, vector2, (float)num6 * num5));
						num3++;
						num6++;
					}
					list.Add(vector2);
				}
			}
		}
		else
		{
			for (int k = 0; k < input.Length - 1; k++)
			{
				Vector2 vector3 = input[k];
				list.Add(vector3);
				Vector2 vector4 = input[k + 1];
				float num2 = 1f / this.m_Resolution;
				for (float num7 = 1f; num7 < this.m_Resolution; num7 += 1f)
				{
					list.Add(Vector2.Lerp(vector3, vector4, num2 * num7));
				}
				list.Add(vector4);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x0007A673 File Offset: 0x00078873
	public virtual void CalculateLayoutInputHorizontal()
	{
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x0007A675 File Offset: 0x00078875
	public virtual void CalculateLayoutInputVertical()
	{
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06001079 RID: 4217 RVA: 0x0007A677 File Offset: 0x00078877
	public virtual float minWidth
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x0600107A RID: 4218 RVA: 0x0007A680 File Offset: 0x00078880
	public virtual float preferredWidth
	{
		get
		{
			if (this.overrideSprite == null)
			{
				return 0f;
			}
			return this.overrideSprite.rect.size.x / this.pixelsPerUnit;
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x0600107B RID: 4219 RVA: 0x0007A6C0 File Offset: 0x000788C0
	public virtual float flexibleWidth
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x0600107C RID: 4220 RVA: 0x0007A6C7 File Offset: 0x000788C7
	public virtual float minHeight
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x0600107D RID: 4221 RVA: 0x0007A6D0 File Offset: 0x000788D0
	public virtual float preferredHeight
	{
		get
		{
			if (this.overrideSprite == null)
			{
				return 0f;
			}
			return this.overrideSprite.rect.size.y / this.pixelsPerUnit;
		}
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x0600107E RID: 4222 RVA: 0x0007A710 File Offset: 0x00078910
	public virtual float flexibleHeight
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x0600107F RID: 4223 RVA: 0x0007A717 File Offset: 0x00078917
	public virtual int layoutPriority
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x0007A71C File Offset: 0x0007891C
	public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		if (this.m_EventAlphaThreshold >= 1f)
		{
			return true;
		}
		Sprite overrideSprite = this.overrideSprite;
		if (overrideSprite == null)
		{
			return true;
		}
		Vector2 vector;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, out vector);
		Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
		vector.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
		vector.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
		vector = this.MapCoordinate(vector, pixelAdjustedRect);
		Rect textureRect = overrideSprite.textureRect;
		Vector2 vector2 = new Vector2(vector.x / textureRect.width, vector.y / textureRect.height);
		float num = Mathf.Lerp(textureRect.x, textureRect.xMax, vector2.x) / (float)overrideSprite.texture.width;
		float num2 = Mathf.Lerp(textureRect.y, textureRect.yMax, vector2.y) / (float)overrideSprite.texture.height;
		bool flag;
		try
		{
			flag = overrideSprite.texture.GetPixelBilinear(num, num2).a >= this.m_EventAlphaThreshold;
		}
		catch (UnityException ex)
		{
			Debug.LogError("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite.", this);
			flag = true;
		}
		return flag;
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x0007A884 File Offset: 0x00078A84
	private Vector2 MapCoordinate(Vector2 local, Rect rect)
	{
		Rect rect2 = this.sprite.rect;
		return new Vector2(local.x * rect.width, local.y * rect.height);
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x0007A8B4 File Offset: 0x00078AB4
	private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
	{
		for (int i = 0; i <= 1; i++)
		{
			float num = border[i] + border[i + 2];
			if (rect.size[i] < num && num != 0f)
			{
				float num2 = rect.size[i] / num;
				ref Vector4 ptr = ref border;
				int num3 = i;
				ptr[num3] *= num2;
				ptr = ref border;
				num3 = i + 2;
				ptr[num3] *= num2;
			}
		}
		return border;
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x0007A94B File Offset: 0x00078B4B
	protected override void OnEnable()
	{
		base.OnEnable();
		this.SetAllDirty();
	}

	// Token: 0x04001237 RID: 4663
	[SerializeField]
	private Sprite m_Sprite;

	// Token: 0x04001238 RID: 4664
	[NonSerialized]
	private Sprite m_OverrideSprite;

	// Token: 0x04001239 RID: 4665
	internal float m_EventAlphaThreshold = 1f;

	// Token: 0x0400123A RID: 4666
	[SerializeField]
	private ResolutionMode m_improveResolution;

	// Token: 0x0400123B RID: 4667
	[SerializeField]
	private float m_Resolution;
}
