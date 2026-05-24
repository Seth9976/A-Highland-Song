using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200004F RID: 79
	public abstract class PostProcessEffectRenderer
	{
		// Token: 0x06000108 RID: 264 RVA: 0x0000A4A8 File Offset: 0x000086A8
		public virtual void Init()
		{
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000A4AA File Offset: 0x000086AA
		public virtual DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.None;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000A4AD File Offset: 0x000086AD
		public virtual void ResetHistory()
		{
			this.m_ResetHistory = true;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000A4B6 File Offset: 0x000086B6
		public virtual void Release()
		{
			this.ResetHistory();
		}

		// Token: 0x0600010C RID: 268
		public abstract void Render(PostProcessRenderContext context);

		// Token: 0x0600010D RID: 269
		internal abstract void SetSettings(PostProcessEffectSettings settings);

		// Token: 0x0400012A RID: 298
		protected bool m_ResetHistory = true;
	}
}
