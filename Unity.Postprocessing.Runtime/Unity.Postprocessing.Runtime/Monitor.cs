using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200003B RID: 59
	public abstract class Monitor
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00008EAF File Offset: 0x000070AF
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00008EB7 File Offset: 0x000070B7
		public RenderTexture output { get; protected set; }

		// Token: 0x060000A7 RID: 167 RVA: 0x00008EC0 File Offset: 0x000070C0
		public bool IsRequestedAndSupported(PostProcessRenderContext context)
		{
			return this.requested && SystemInfo.supportsComputeShaders && !RuntimeUtilities.isAndroidOpenGL && this.ShaderResourcesAvailable(context);
		}

		// Token: 0x060000A8 RID: 168
		internal abstract bool ShaderResourcesAvailable(PostProcessRenderContext context);

		// Token: 0x060000A9 RID: 169 RVA: 0x00008EE1 File Offset: 0x000070E1
		internal virtual bool NeedsHalfRes()
		{
			return false;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00008EE4 File Offset: 0x000070E4
		protected void CheckOutput(int width, int height)
		{
			if (this.output == null || !this.output.IsCreated() || this.output.width != width || this.output.height != height)
			{
				RuntimeUtilities.Destroy(this.output);
				this.output = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32)
				{
					anisoLevel = 0,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					useMipMap = false
				};
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00008F5E File Offset: 0x0000715E
		internal virtual void OnEnable()
		{
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00008F60 File Offset: 0x00007160
		internal virtual void OnDisable()
		{
			RuntimeUtilities.Destroy(this.output);
		}

		// Token: 0x060000AD RID: 173
		internal abstract void Render(PostProcessRenderContext context);

		// Token: 0x040000EE RID: 238
		internal bool requested;
	}
}
