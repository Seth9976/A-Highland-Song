using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000D RID: 13
	public abstract class ShapeRendererFillable : ShapeRenderer
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00005773 File Offset: 0x00003973
		internal ShapeFill Fill
		{
			get
			{
				return this.fill;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000577B File Offset: 0x0000397B
		private int FillTypeShaderInt
		{
			get
			{
				if (!this.useFill)
				{
					return -1;
				}
				return this.fill.GetShaderFillModeInt();
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00005792 File Offset: 0x00003992
		// (set) Token: 0x0600018D RID: 397 RVA: 0x0000579A File Offset: 0x0000399A
		public bool UseFill
		{
			get
			{
				return this.useFill;
			}
			set
			{
				this.useFill = value;
				base.SetIntNow(ShapesMaterialUtils.propFillType, this.FillTypeShaderInt);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000057B4 File Offset: 0x000039B4
		// (set) Token: 0x0600018F RID: 399 RVA: 0x000057C1 File Offset: 0x000039C1
		public FillType FillType
		{
			get
			{
				return this.fill.type;
			}
			set
			{
				this.fill.type = value;
				base.SetIntNow(ShapesMaterialUtils.propFillType, this.FillTypeShaderInt);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000057E0 File Offset: 0x000039E0
		// (set) Token: 0x06000191 RID: 401 RVA: 0x000057F0 File Offset: 0x000039F0
		public FillSpace FillSpace
		{
			get
			{
				return this.fill.space;
			}
			set
			{
				int propFillSpace = ShapesMaterialUtils.propFillSpace;
				this.fill.space = value;
				base.SetIntNow(propFillSpace, (int)value);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00005817 File Offset: 0x00003A17
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00005824 File Offset: 0x00003A24
		public Vector3 FillRadialOrigin
		{
			get
			{
				return this.fill.radialOrigin;
			}
			set
			{
				this.fill.radialOrigin = value;
				base.SetVector4Now(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00005848 File Offset: 0x00003A48
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00005855 File Offset: 0x00003A55
		public float FillRadialRadius
		{
			get
			{
				return this.fill.radialRadius;
			}
			set
			{
				this.fill.radialRadius = value;
				base.SetVector4Now(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00005879 File Offset: 0x00003A79
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00005886 File Offset: 0x00003A86
		public Vector3 FillLinearStart
		{
			get
			{
				return this.fill.linearStart;
			}
			set
			{
				this.fill.linearStart = value;
				base.SetVector4Now(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000058AA File Offset: 0x00003AAA
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000058B8 File Offset: 0x00003AB8
		public Vector3 FillLinearEnd
		{
			get
			{
				return this.fill.linearEnd;
			}
			set
			{
				int propFillEnd = ShapesMaterialUtils.propFillEnd;
				this.fill.linearEnd = value;
				base.SetVector3Now(propFillEnd, value);
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000058DF File Offset: 0x00003ADF
		// (set) Token: 0x0600019B RID: 411 RVA: 0x000058EC File Offset: 0x00003AEC
		public Color FillColorStart
		{
			get
			{
				return this.fill.colorStart;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.fill.colorStart = value;
				base.SetColorNow(propColor, value);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00005913 File Offset: 0x00003B13
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00005920 File Offset: 0x00003B20
		public Color FillColorEnd
		{
			get
			{
				return this.fill.colorEnd;
			}
			set
			{
				int propColorEnd = ShapesMaterialUtils.propColorEnd;
				this.fill.colorEnd = value;
				base.SetColorNow(propColorEnd, value);
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00005948 File Offset: 0x00003B48
		private protected void SetFillProperties()
		{
			if (this.useFill)
			{
				base.SetInt(ShapesMaterialUtils.propFillSpace, (int)this.fill.space);
				base.SetVector4(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
				base.SetVector3(ShapesMaterialUtils.propFillEnd, this.fill.linearEnd);
				base.SetColor(ShapesMaterialUtils.propColor, this.fill.colorStart);
				base.SetColor(ShapesMaterialUtils.propColorEnd, this.fill.colorEnd);
			}
			base.SetInt(ShapesMaterialUtils.propFillType, this.FillTypeShaderInt);
		}

		// Token: 0x04000063 RID: 99
		[SerializeField]
		private protected ShapeFill fill = new ShapeFill();

		// Token: 0x04000064 RID: 100
		[SerializeField]
		private protected bool useFill;
	}
}
