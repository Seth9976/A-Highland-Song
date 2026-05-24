using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000050 RID: 80
	public abstract class PostProcessEffectRenderer<T> : PostProcessEffectRenderer where T : PostProcessEffectSettings
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000A4CD File Offset: 0x000086CD
		// (set) Token: 0x06000110 RID: 272 RVA: 0x0000A4D5 File Offset: 0x000086D5
		public T settings { get; internal set; }

		// Token: 0x06000111 RID: 273 RVA: 0x0000A4DE File Offset: 0x000086DE
		internal override void SetSettings(PostProcessEffectSettings settings)
		{
			this.settings = (T)((object)settings);
		}
	}
}
