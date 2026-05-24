using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200023C RID: 572
	internal class EventDebuggerDefaultActionTrace : EventDebuggerTrace
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x0600111E RID: 4382 RVA: 0x000422FA File Offset: 0x000404FA
		public PropagationPhase phase { get; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x00042304 File Offset: 0x00040504
		public string targetName
		{
			get
			{
				return base.eventBase.target.GetType().FullName;
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0004232B File Offset: 0x0004052B
		public EventDebuggerDefaultActionTrace(IPanel panel, EventBase evt, PropagationPhase phase, long duration, IEventHandler mouseCapture)
			: base(panel, evt, duration, mouseCapture)
		{
			this.phase = phase;
		}
	}
}
