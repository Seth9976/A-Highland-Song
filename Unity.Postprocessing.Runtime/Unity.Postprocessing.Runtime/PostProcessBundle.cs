using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200004A RID: 74
	public sealed class PostProcessBundle
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00009A66 File Offset: 0x00007C66
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00009A6E File Offset: 0x00007C6E
		public PostProcessAttribute attribute { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00009A77 File Offset: 0x00007C77
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00009A7F File Offset: 0x00007C7F
		public PostProcessEffectSettings settings { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00009A88 File Offset: 0x00007C88
		internal PostProcessEffectRenderer renderer
		{
			get
			{
				if (this.m_Renderer == null)
				{
					Type renderer = this.attribute.renderer;
					this.m_Renderer = (PostProcessEffectRenderer)Activator.CreateInstance(renderer);
					this.m_Renderer.SetSettings(this.settings);
					this.m_Renderer.Init();
				}
				return this.m_Renderer;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00009ADC File Offset: 0x00007CDC
		internal PostProcessBundle(PostProcessEffectSettings settings)
		{
			this.settings = settings;
			this.attribute = settings.GetType().GetAttribute<PostProcessAttribute>();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00009AFC File Offset: 0x00007CFC
		internal void Release()
		{
			if (this.m_Renderer != null)
			{
				this.m_Renderer.Release();
			}
			RuntimeUtilities.Destroy(this.settings);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00009B1C File Offset: 0x00007D1C
		internal void ResetHistory()
		{
			if (this.m_Renderer != null)
			{
				this.m_Renderer.ResetHistory();
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00009B31 File Offset: 0x00007D31
		internal T CastSettings<T>() where T : PostProcessEffectSettings
		{
			return (T)((object)this.settings);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00009B3E File Offset: 0x00007D3E
		internal T CastRenderer<T>() where T : PostProcessEffectRenderer
		{
			return (T)((object)this.renderer);
		}

		// Token: 0x04000105 RID: 261
		private PostProcessEffectRenderer m_Renderer;
	}
}
