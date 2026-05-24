using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001C4 RID: 452
	public abstract class CommandEventBase<T> : EventBase<T>, ICommandEvent where T : CommandEventBase<T>, new()
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x0003A178 File Offset: 0x00038378
		// (set) Token: 0x06000E2E RID: 3630 RVA: 0x0003A1B7 File Offset: 0x000383B7
		public string commandName
		{
			get
			{
				bool flag = this.m_CommandName == null && base.imguiEvent != null;
				string text;
				if (flag)
				{
					text = base.imguiEvent.commandName;
				}
				else
				{
					text = this.m_CommandName;
				}
				return text;
			}
			protected set
			{
				this.m_CommandName = value;
			}
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0003A1C1 File Offset: 0x000383C1
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0003A1D2 File Offset: 0x000383D2
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable;
			this.commandName = null;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0003A1E8 File Offset: 0x000383E8
		public static T GetPooled(Event systemEvent)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.imguiEvent = systemEvent;
			return pooled;
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0003A210 File Offset: 0x00038410
		public static T GetPooled(string commandName)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.commandName = commandName;
			return pooled;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0003A236 File Offset: 0x00038436
		protected CommandEventBase()
		{
			this.LocalInit();
		}

		// Token: 0x0400067E RID: 1662
		private string m_CommandName;
	}
}
