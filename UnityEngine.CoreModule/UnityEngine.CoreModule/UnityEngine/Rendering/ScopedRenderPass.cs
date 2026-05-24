using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200040B RID: 1035
	public struct ScopedRenderPass : IDisposable
	{
		// Token: 0x06002365 RID: 9061 RVA: 0x0003BC29 File Offset: 0x00039E29
		internal ScopedRenderPass(ScriptableRenderContext context)
		{
			this.m_Context = context;
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0003BC34 File Offset: 0x00039E34
		public void Dispose()
		{
			try
			{
				this.m_Context.EndRenderPass();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("The ScopedRenderPass instance is not valid. This can happen if it was constructed using the default constructor.", ex);
			}
		}

		// Token: 0x04000D2F RID: 3375
		private ScriptableRenderContext m_Context;
	}
}
