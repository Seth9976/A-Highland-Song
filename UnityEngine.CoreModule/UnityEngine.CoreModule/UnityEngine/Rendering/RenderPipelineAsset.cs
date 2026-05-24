using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000404 RID: 1028
	public abstract class RenderPipelineAsset : ScriptableObject
	{
		// Token: 0x060022FA RID: 8954 RVA: 0x0003ADE8 File Offset: 0x00038FE8
		internal RenderPipeline InternalCreatePipeline()
		{
			RenderPipeline renderPipeline = null;
			try
			{
				renderPipeline = this.CreatePipeline();
			}
			catch (Exception ex)
			{
				bool flag = !ex.Data.Contains("InvalidImport") || !(ex.Data["InvalidImport"] is int) || (int)ex.Data["InvalidImport"] != 1;
				if (flag)
				{
					Debug.LogException(ex);
				}
			}
			return renderPipeline;
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual string[] renderingLayerMaskNames
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060022FC RID: 8956 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual string[] prefixedRenderingLayerMaskNames
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Material defaultMaterial
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060022FE RID: 8958 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Shader autodeskInteractiveShader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060022FF RID: 8959 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Shader autodeskInteractiveTransparentShader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002300 RID: 8960 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Shader autodeskInteractiveMaskedShader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002301 RID: 8961 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Shader terrainDetailLitShader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Shader terrainDetailGrassShader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Shader terrainDetailGrassBillboardShader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Material defaultParticleMaterial
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06002305 RID: 8965 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Material defaultLineMaterial
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Material defaultTerrainMaterial
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002307 RID: 8967 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Material defaultUIMaterial
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Material defaultUIOverdrawMaterial
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002309 RID: 8969 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Material defaultUIETC1SupportedMaterial
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x0600230A RID: 8970 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Material default2DMaterial
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x0600230B RID: 8971 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Material default2DMaskMaterial
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Shader defaultShader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600230D RID: 8973 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Shader defaultSpeedTree7Shader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x00030F9A File Offset: 0x0002F19A
		public virtual Shader defaultSpeedTree8Shader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600230F RID: 8975
		protected abstract RenderPipeline CreatePipeline();

		// Token: 0x06002310 RID: 8976 RVA: 0x0003AE70 File Offset: 0x00039070
		protected virtual void OnValidate()
		{
			bool flag = RenderPipelineManager.s_CurrentPipelineAsset == this;
			if (flag)
			{
				RenderPipelineManager.CleanupRenderPipeline();
				RenderPipelineManager.PrepareRenderPipeline(this);
			}
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0003AE9C File Offset: 0x0003909C
		protected virtual void OnDisable()
		{
			RenderPipelineManager.CleanupRenderPipeline();
		}
	}
}
