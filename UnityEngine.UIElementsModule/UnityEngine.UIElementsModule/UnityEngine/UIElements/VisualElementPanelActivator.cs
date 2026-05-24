using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000EF RID: 239
	internal class VisualElementPanelActivator
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0001B134 File Offset: 0x00019334
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x0001B13C File Offset: 0x0001933C
		public bool isActive { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x0001B145 File Offset: 0x00019345
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x0001B14D File Offset: 0x0001934D
		public bool isDetaching { get; private set; }

		// Token: 0x06000772 RID: 1906 RVA: 0x0001B156 File Offset: 0x00019356
		public VisualElementPanelActivator(IVisualElementPanelActivatable activatable)
		{
			this.m_Activatable = activatable;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001B168 File Offset: 0x00019368
		public void SetActive(bool action)
		{
			bool flag = this.isActive != action;
			if (flag)
			{
				this.isActive = action;
				bool isActive = this.isActive;
				if (isActive)
				{
					this.m_Activatable.element.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnEnter), TrickleDown.NoTrickleDown);
					this.m_Activatable.element.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnLeave), TrickleDown.NoTrickleDown);
					this.SendActivation();
				}
				else
				{
					this.m_Activatable.element.UnregisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnEnter), TrickleDown.NoTrickleDown);
					this.m_Activatable.element.UnregisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnLeave), TrickleDown.NoTrickleDown);
					this.SendDeactivation();
				}
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001B22C File Offset: 0x0001942C
		public void SendActivation()
		{
			bool flag = this.m_Activatable.CanBeActivated();
			if (flag)
			{
				this.m_Activatable.OnPanelActivate();
			}
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001B258 File Offset: 0x00019458
		public void SendDeactivation()
		{
			bool flag = this.m_Activatable.CanBeActivated();
			if (flag)
			{
				this.m_Activatable.OnPanelDeactivate();
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001B284 File Offset: 0x00019484
		private void OnEnter(AttachToPanelEvent evt)
		{
			bool isActive = this.isActive;
			if (isActive)
			{
				this.SendActivation();
			}
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0001B2A8 File Offset: 0x000194A8
		private void OnLeave(DetachFromPanelEvent evt)
		{
			bool isActive = this.isActive;
			if (isActive)
			{
				this.isDetaching = true;
				try
				{
					this.SendDeactivation();
				}
				finally
				{
					this.isDetaching = false;
				}
			}
		}

		// Token: 0x04000300 RID: 768
		private IVisualElementPanelActivatable m_Activatable;
	}
}
