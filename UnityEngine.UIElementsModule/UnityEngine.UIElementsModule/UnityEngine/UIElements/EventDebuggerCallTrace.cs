using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200023B RID: 571
	internal class EventDebuggerCallTrace : EventDebuggerTrace
	{
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x0004229B File Offset: 0x0004049B
		public int callbackHashCode { get; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x000422A3 File Offset: 0x000404A3
		public string callbackName { get; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x0600111A RID: 4378 RVA: 0x000422AB File Offset: 0x000404AB
		public bool propagationHasStopped { get; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x000422B3 File Offset: 0x000404B3
		public bool immediatePropagationHasStopped { get; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x0600111C RID: 4380 RVA: 0x000422BB File Offset: 0x000404BB
		public bool defaultHasBeenPrevented { get; }

		// Token: 0x0600111D RID: 4381 RVA: 0x000422C3 File Offset: 0x000404C3
		public EventDebuggerCallTrace(IPanel panel, EventBase evt, int cbHashCode, string cbName, bool propagationHasStopped, bool immediatePropagationHasStopped, bool defaultHasBeenPrevented, long duration, IEventHandler mouseCapture)
			: base(panel, evt, duration, mouseCapture)
		{
			this.callbackHashCode = cbHashCode;
			this.callbackName = cbName;
			this.propagationHasStopped = propagationHasStopped;
			this.immediatePropagationHasStopped = immediatePropagationHasStopped;
			this.defaultHasBeenPrevented = defaultHasBeenPrevented;
		}
	}
}
