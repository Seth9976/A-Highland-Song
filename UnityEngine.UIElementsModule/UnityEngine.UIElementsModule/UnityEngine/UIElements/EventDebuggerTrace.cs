using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200023A RID: 570
	internal class EventDebuggerTrace
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x00042222 File Offset: 0x00040422
		public EventDebuggerEventRecord eventBase { get; }

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x0004222A File Offset: 0x0004042A
		public IEventHandler focusedElement { get; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001114 RID: 4372 RVA: 0x00042232 File Offset: 0x00040432
		public IEventHandler mouseCapture { get; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x0004223A File Offset: 0x0004043A
		// (set) Token: 0x06001116 RID: 4374 RVA: 0x00042242 File Offset: 0x00040442
		public long duration { get; set; }

		// Token: 0x06001117 RID: 4375 RVA: 0x0004224C File Offset: 0x0004044C
		public EventDebuggerTrace(IPanel panel, EventBase evt, long duration, IEventHandler mouseCapture)
		{
			this.eventBase = new EventDebuggerEventRecord(evt);
			IEventHandler eventHandler;
			if (panel == null)
			{
				eventHandler = null;
			}
			else
			{
				FocusController focusController = panel.focusController;
				eventHandler = ((focusController != null) ? focusController.focusedElement : null);
			}
			this.focusedElement = eventHandler;
			this.mouseCapture = mouseCapture;
			this.duration = duration;
		}
	}
}
