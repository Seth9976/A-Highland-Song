using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200005F RID: 95
	internal static class PostProcessEffectRendererExtensions
	{
		// Token: 0x060001C7 RID: 455 RVA: 0x0000E850 File Offset: 0x0000CA50
		public static Exception RenderOrLog(this PostProcessEffectRenderer self, PostProcessRenderContext context)
		{
			try
			{
				self.Render(context);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				return ex;
			}
			return null;
		}
	}
}
