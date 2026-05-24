using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200023D RID: 573
	internal class EventDebuggerPathTrace : EventDebuggerTrace
	{
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x00042342 File Offset: 0x00040542
		public PropagationPaths paths { get; }

		// Token: 0x06001122 RID: 4386 RVA: 0x0004234A File Offset: 0x0004054A
		public EventDebuggerPathTrace(IPanel panel, EventBase evt, PropagationPaths paths)
			: base(panel, evt, -1L, null)
		{
			this.paths = paths;
		}
	}
}
