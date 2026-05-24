using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200020A RID: 522
	public abstract class PanelChangedEventBase<T> : EventBase<T>, IPanelChangedEvent where T : PanelChangedEventBase<T>, new()
	{
		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x0003E76F File Offset: 0x0003C96F
		// (set) Token: 0x06000FCB RID: 4043 RVA: 0x0003E777 File Offset: 0x0003C977
		public IPanel originPanel { get; private set; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0003E780 File Offset: 0x0003C980
		// (set) Token: 0x06000FCD RID: 4045 RVA: 0x0003E788 File Offset: 0x0003C988
		public IPanel destinationPanel { get; private set; }

		// Token: 0x06000FCE RID: 4046 RVA: 0x0003E791 File Offset: 0x0003C991
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0003E7A2 File Offset: 0x0003C9A2
		private void LocalInit()
		{
			this.originPanel = null;
			this.destinationPanel = null;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0003E7B8 File Offset: 0x0003C9B8
		public static T GetPooled(IPanel originPanel, IPanel destinationPanel)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.originPanel = originPanel;
			pooled.destinationPanel = destinationPanel;
			return pooled;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0003E7EB File Offset: 0x0003C9EB
		protected PanelChangedEventBase()
		{
			this.LocalInit();
		}
	}
}
