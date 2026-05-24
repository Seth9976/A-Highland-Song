using System;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x0200046F RID: 1135
	public abstract class ScriptableRuntimeReflectionSystem : IScriptableRuntimeReflectionSystem, IDisposable
	{
		// Token: 0x0600281A RID: 10266 RVA: 0x00042BA8 File Offset: 0x00040DA8
		public virtual bool TickRealtimeProbes()
		{
			return false;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x00004557 File Offset: 0x00002757
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x00042BBB File Offset: 0x00040DBB
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
