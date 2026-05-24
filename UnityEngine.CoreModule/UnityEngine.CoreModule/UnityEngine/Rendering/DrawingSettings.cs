using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Rendering
{
	// Token: 0x020003FA RID: 1018
	public struct DrawingSettings : IEquatable<DrawingSettings>
	{
		// Token: 0x0600229F RID: 8863 RVA: 0x0003A150 File Offset: 0x00038350
		public unsafe DrawingSettings(ShaderTagId shaderPassName, SortingSettings sortingSettings)
		{
			this.m_SortingSettings = sortingSettings;
			this.m_PerObjectData = PerObjectData.None;
			this.m_Flags = DrawRendererFlags.EnableInstancing;
			this.m_OverrideMaterialInstanceId = 0;
			this.m_OverrideMaterialPassIndex = 0;
			this.m_fallbackMaterialInstanceId = 0;
			this.m_MainLightIndex = -1;
			fixed (int* ptr = &this.shaderPassNames.FixedElementField)
			{
				int* ptr2 = ptr;
				*ptr2 = shaderPassName.id;
				for (int i = 1; i < DrawingSettings.maxShaderPasses; i++)
				{
					ptr2[i] = -1;
				}
			}
			this.m_PerObjectData = PerObjectData.None;
			this.m_Flags = DrawRendererFlags.EnableInstancing;
			this.m_UseSrpBatcher = 0;
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x0003A1E0 File Offset: 0x000383E0
		// (set) Token: 0x060022A1 RID: 8865 RVA: 0x0003A1F8 File Offset: 0x000383F8
		public SortingSettings sortingSettings
		{
			get
			{
				return this.m_SortingSettings;
			}
			set
			{
				this.m_SortingSettings = value;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060022A2 RID: 8866 RVA: 0x0003A204 File Offset: 0x00038404
		// (set) Token: 0x060022A3 RID: 8867 RVA: 0x0003A21C File Offset: 0x0003841C
		public PerObjectData perObjectData
		{
			get
			{
				return this.m_PerObjectData;
			}
			set
			{
				this.m_PerObjectData = value;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x0003A228 File Offset: 0x00038428
		// (set) Token: 0x060022A5 RID: 8869 RVA: 0x0003A248 File Offset: 0x00038448
		public bool enableDynamicBatching
		{
			get
			{
				return (this.m_Flags & DrawRendererFlags.EnableDynamicBatching) > DrawRendererFlags.None;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= DrawRendererFlags.EnableDynamicBatching;
				}
				else
				{
					this.m_Flags &= ~DrawRendererFlags.EnableDynamicBatching;
				}
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060022A6 RID: 8870 RVA: 0x0003A27C File Offset: 0x0003847C
		// (set) Token: 0x060022A7 RID: 8871 RVA: 0x0003A29C File Offset: 0x0003849C
		public bool enableInstancing
		{
			get
			{
				return (this.m_Flags & DrawRendererFlags.EnableInstancing) > DrawRendererFlags.None;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= DrawRendererFlags.EnableInstancing;
				}
				else
				{
					this.m_Flags &= ~DrawRendererFlags.EnableInstancing;
				}
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060022A8 RID: 8872 RVA: 0x0003A2D0 File Offset: 0x000384D0
		// (set) Token: 0x060022A9 RID: 8873 RVA: 0x0003A2FD File Offset: 0x000384FD
		public Material overrideMaterial
		{
			get
			{
				return (this.m_OverrideMaterialInstanceId != 0) ? (Object.FindObjectFromInstanceID(this.m_OverrideMaterialInstanceId) as Material) : null;
			}
			set
			{
				this.m_OverrideMaterialInstanceId = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060022AA RID: 8874 RVA: 0x0003A314 File Offset: 0x00038514
		// (set) Token: 0x060022AB RID: 8875 RVA: 0x0003A32C File Offset: 0x0003852C
		public int overrideMaterialPassIndex
		{
			get
			{
				return this.m_OverrideMaterialPassIndex;
			}
			set
			{
				this.m_OverrideMaterialPassIndex = value;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060022AC RID: 8876 RVA: 0x0003A338 File Offset: 0x00038538
		// (set) Token: 0x060022AD RID: 8877 RVA: 0x0003A365 File Offset: 0x00038565
		public Material fallbackMaterial
		{
			get
			{
				return (this.m_fallbackMaterialInstanceId != 0) ? (Object.FindObjectFromInstanceID(this.m_fallbackMaterialInstanceId) as Material) : null;
			}
			set
			{
				this.m_fallbackMaterialInstanceId = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060022AE RID: 8878 RVA: 0x0003A37C File Offset: 0x0003857C
		// (set) Token: 0x060022AF RID: 8879 RVA: 0x0003A394 File Offset: 0x00038594
		public int mainLightIndex
		{
			get
			{
				return this.m_MainLightIndex;
			}
			set
			{
				this.m_MainLightIndex = value;
			}
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x0003A3A0 File Offset: 0x000385A0
		public unsafe ShaderTagId GetShaderPassName(int index)
		{
			bool flag = index >= DrawingSettings.maxShaderPasses || index < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Index should range from 0 to DrawSettings.maxShaderPasses ({0}), was {1}", DrawingSettings.maxShaderPasses, index));
			}
			fixed (int* ptr = &this.shaderPassNames.FixedElementField)
			{
				int* ptr2 = ptr;
				return new ShaderTagId
				{
					id = ptr2[index]
				};
			}
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x0003A414 File Offset: 0x00038614
		public unsafe void SetShaderPassName(int index, ShaderTagId shaderPassName)
		{
			bool flag = index >= DrawingSettings.maxShaderPasses || index < 0;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Index should range from 0 to DrawSettings.maxShaderPasses ({0}), was {1}", DrawingSettings.maxShaderPasses, index));
			}
			fixed (int* ptr = &this.shaderPassNames.FixedElementField)
			{
				int* ptr2 = ptr;
				ptr2[index] = shaderPassName.id;
			}
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x0003A47C File Offset: 0x0003867C
		public bool Equals(DrawingSettings other)
		{
			for (int i = 0; i < DrawingSettings.maxShaderPasses; i++)
			{
				bool flag = !this.GetShaderPassName(i).Equals(other.GetShaderPassName(i));
				if (flag)
				{
					return false;
				}
			}
			return this.m_SortingSettings.Equals(other.m_SortingSettings) && this.m_PerObjectData == other.m_PerObjectData && this.m_Flags == other.m_Flags && this.m_OverrideMaterialInstanceId == other.m_OverrideMaterialInstanceId && this.m_OverrideMaterialPassIndex == other.m_OverrideMaterialPassIndex && this.m_fallbackMaterialInstanceId == other.m_fallbackMaterialInstanceId && this.m_UseSrpBatcher == other.m_UseSrpBatcher;
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x0003A538 File Offset: 0x00038738
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is DrawingSettings && this.Equals((DrawingSettings)obj);
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x0003A570 File Offset: 0x00038770
		public override int GetHashCode()
		{
			int num = this.m_SortingSettings.GetHashCode();
			num = (num * 397) ^ (int)this.m_PerObjectData;
			num = (num * 397) ^ (int)this.m_Flags;
			num = (num * 397) ^ this.m_OverrideMaterialInstanceId;
			num = (num * 397) ^ this.m_OverrideMaterialPassIndex;
			num = (num * 397) ^ this.m_fallbackMaterialInstanceId;
			return (num * 397) ^ this.m_UseSrpBatcher;
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x0003A5F0 File Offset: 0x000387F0
		public static bool operator ==(DrawingSettings left, DrawingSettings right)
		{
			return left.Equals(right);
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x0003A60C File Offset: 0x0003880C
		public static bool operator !=(DrawingSettings left, DrawingSettings right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000CD0 RID: 3280
		private const int kMaxShaderPasses = 16;

		// Token: 0x04000CD1 RID: 3281
		public static readonly int maxShaderPasses = 16;

		// Token: 0x04000CD2 RID: 3282
		private SortingSettings m_SortingSettings;

		// Token: 0x04000CD3 RID: 3283
		[FixedBuffer(typeof(int), 16)]
		internal DrawingSettings.<shaderPassNames>e__FixedBuffer shaderPassNames;

		// Token: 0x04000CD4 RID: 3284
		private PerObjectData m_PerObjectData;

		// Token: 0x04000CD5 RID: 3285
		private DrawRendererFlags m_Flags;

		// Token: 0x04000CD6 RID: 3286
		private int m_OverrideMaterialInstanceId;

		// Token: 0x04000CD7 RID: 3287
		private int m_OverrideMaterialPassIndex;

		// Token: 0x04000CD8 RID: 3288
		private int m_fallbackMaterialInstanceId;

		// Token: 0x04000CD9 RID: 3289
		private int m_MainLightIndex;

		// Token: 0x04000CDA RID: 3290
		private int m_UseSrpBatcher;

		// Token: 0x020003FB RID: 1019
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 64)]
		public struct <shaderPassNames>e__FixedBuffer
		{
			// Token: 0x04000CDB RID: 3291
			public int FixedElementField;
		}
	}
}
