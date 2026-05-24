using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200040C RID: 1036
	public struct ScopedSubPass : IDisposable
	{
		// Token: 0x06002367 RID: 9063 RVA: 0x0003BC70 File Offset: 0x00039E70
		internal ScopedSubPass(ScriptableRenderContext context)
		{
			this.m_Context = context;
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0003BC7C File Offset: 0x00039E7C
		public void Dispose()
		{
			try
			{
				this.m_Context.EndSubPass();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("The ScopedSubPass instance is not valid. This can happen if it was constructed using the default constructor.", ex);
			}
		}

		// Token: 0x04000D30 RID: 3376
		private ScriptableRenderContext m_Context;
	}
}
