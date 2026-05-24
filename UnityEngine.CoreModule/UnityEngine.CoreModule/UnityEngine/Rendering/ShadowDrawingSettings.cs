using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200040F RID: 1039
	[UsedByNativeCode]
	public struct ShadowDrawingSettings : IEquatable<ShadowDrawingSettings>
	{
		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x0003C614 File Offset: 0x0003A814
		// (set) Token: 0x060023D3 RID: 9171 RVA: 0x0003C62C File Offset: 0x0003A82C
		public CullingResults cullingResults
		{
			get
			{
				return this.m_CullingResults;
			}
			set
			{
				this.m_CullingResults = value;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060023D4 RID: 9172 RVA: 0x0003C638 File Offset: 0x0003A838
		// (set) Token: 0x060023D5 RID: 9173 RVA: 0x0003C650 File Offset: 0x0003A850
		public int lightIndex
		{
			get
			{
				return this.m_LightIndex;
			}
			set
			{
				this.m_LightIndex = value;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x0003C65C File Offset: 0x0003A85C
		// (set) Token: 0x060023D7 RID: 9175 RVA: 0x0003C677 File Offset: 0x0003A877
		public bool useRenderingLayerMaskTest
		{
			get
			{
				return this.m_UseRenderingLayerMaskTest != 0;
			}
			set
			{
				this.m_UseRenderingLayerMaskTest = (value ? 1 : 0);
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x0003C688 File Offset: 0x0003A888
		// (set) Token: 0x060023D9 RID: 9177 RVA: 0x0003C6A0 File Offset: 0x0003A8A0
		public ShadowSplitData splitData
		{
			get
			{
				return this.m_SplitData;
			}
			set
			{
				this.m_SplitData = value;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060023DA RID: 9178 RVA: 0x0003C6AC File Offset: 0x0003A8AC
		// (set) Token: 0x060023DB RID: 9179 RVA: 0x0003C6C4 File Offset: 0x0003A8C4
		public ShadowObjectsFilter objectsFilter
		{
			get
			{
				return this.m_ObjectsFilter;
			}
			set
			{
				this.m_ObjectsFilter = value;
			}
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x0003C6CE File Offset: 0x0003A8CE
		public ShadowDrawingSettings(CullingResults cullingResults, int lightIndex)
		{
			this.m_CullingResults = cullingResults;
			this.m_LightIndex = lightIndex;
			this.m_UseRenderingLayerMaskTest = 0;
			this.m_SplitData = default(ShadowSplitData);
			this.m_SplitData.shadowCascadeBlendCullingFactor = 1f;
			this.m_ObjectsFilter = ShadowObjectsFilter.AllObjects;
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x0003C70C File Offset: 0x0003A90C
		public bool Equals(ShadowDrawingSettings other)
		{
			return this.m_CullingResults.Equals(other.m_CullingResults) && this.m_LightIndex == other.m_LightIndex && this.m_SplitData.Equals(other.m_SplitData) && this.m_UseRenderingLayerMaskTest.Equals(other.m_UseRenderingLayerMaskTest) && this.m_ObjectsFilter.Equals(other.m_ObjectsFilter);
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x0003C784 File Offset: 0x0003A984
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is ShadowDrawingSettings && this.Equals((ShadowDrawingSettings)obj);
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x0003C7BC File Offset: 0x0003A9BC
		public override int GetHashCode()
		{
			int num = this.m_CullingResults.GetHashCode();
			num = (num * 397) ^ this.m_LightIndex;
			num = (num * 397) ^ this.m_UseRenderingLayerMaskTest;
			num = (num * 397) ^ this.m_SplitData.GetHashCode();
			return (num * 397) ^ (int)this.m_ObjectsFilter;
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0003C82C File Offset: 0x0003AA2C
		public static bool operator ==(ShadowDrawingSettings left, ShadowDrawingSettings right)
		{
			return left.Equals(right);
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x0003C848 File Offset: 0x0003AA48
		public static bool operator !=(ShadowDrawingSettings left, ShadowDrawingSettings right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000D35 RID: 3381
		private CullingResults m_CullingResults;

		// Token: 0x04000D36 RID: 3382
		private int m_LightIndex;

		// Token: 0x04000D37 RID: 3383
		private int m_UseRenderingLayerMaskTest;

		// Token: 0x04000D38 RID: 3384
		private ShadowSplitData m_SplitData;

		// Token: 0x04000D39 RID: 3385
		private ShadowObjectsFilter m_ObjectsFilter;
	}
}
