using System;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements
{
	// Token: 0x020002B6 RID: 694
	internal struct TextHandle : ITextHandle
	{
		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001747 RID: 5959 RVA: 0x0005EE56 File Offset: 0x0005D056
		// (set) Token: 0x06001748 RID: 5960 RVA: 0x0005EE63 File Offset: 0x0005D063
		public Vector2 MeasuredSizes
		{
			get
			{
				return this.textHandle.MeasuredSizes;
			}
			set
			{
				this.textHandle.MeasuredSizes = value;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x0005EE72 File Offset: 0x0005D072
		// (set) Token: 0x0600174A RID: 5962 RVA: 0x0005EE7F File Offset: 0x0005D07F
		public Vector2 RoundedSizes
		{
			get
			{
				return this.textHandle.RoundedSizes;
			}
			set
			{
				this.textHandle.RoundedSizes = value;
			}
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x0005EE90 File Offset: 0x0005D090
		public Vector2 GetCursorPosition(CursorPositionStylePainterParameters parms, float scaling)
		{
			return this.textHandle.GetCursorPosition(parms, scaling);
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x0005EEB0 File Offset: 0x0005D0B0
		public float GetLineHeight(int characterIndex, MeshGenerationContextUtils.TextParams textParams, float textScaling, float pixelPerPoint)
		{
			return this.textHandle.GetLineHeight(characterIndex, textParams, textScaling, pixelPerPoint);
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x0005EED4 File Offset: 0x0005D0D4
		public float ComputeTextWidth(MeshGenerationContextUtils.TextParams parms, float scaling)
		{
			return this.textHandle.ComputeTextWidth(parms, scaling);
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x0005EEF4 File Offset: 0x0005D0F4
		public float ComputeTextHeight(MeshGenerationContextUtils.TextParams parms, float scaling)
		{
			return this.textHandle.ComputeTextHeight(parms, scaling);
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x0005EF14 File Offset: 0x0005D114
		public TextInfo Update(MeshGenerationContextUtils.TextParams parms, float pixelsPerPoint)
		{
			return this.textHandle.Update(parms, pixelsPerPoint);
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x0005EF34 File Offset: 0x0005D134
		public int VerticesCount(MeshGenerationContextUtils.TextParams parms, float pixelPerPoint)
		{
			return this.textHandle.VerticesCount(parms, pixelPerPoint);
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x0005EF54 File Offset: 0x0005D154
		public ITextHandle New()
		{
			return this.textHandle.New();
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x0005EF74 File Offset: 0x0005D174
		public bool IsLegacy()
		{
			return this.textHandle.IsLegacy();
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x0005EF91 File Offset: 0x0005D191
		public void SetDirty()
		{
			this.textHandle.SetDirty();
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x0005EFA0 File Offset: 0x0005D1A0
		public bool IsElided()
		{
			return this.textHandle.IsElided();
		}

		// Token: 0x040009F5 RID: 2549
		internal ITextHandle textHandle;
	}
}
