using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000200 RID: 512
	internal class NavigationEventDispatchingStrategy : IEventDispatchingStrategy
	{
		// Token: 0x06000FB4 RID: 4020 RVA: 0x0003E540 File Offset: 0x0003C740
		public bool CanDispatchEvent(EventBase evt)
		{
			return evt is INavigationEvent;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0003E55C File Offset: 0x0003C75C
		public void DispatchEvent(EventBase evt, IPanel panel)
		{
			bool flag = panel != null;
			if (flag)
			{
				evt.target = panel.focusController.GetLeafFocusedElement() ?? panel.visualTree;
				EventDispatchUtilities.PropagateEvent(evt);
			}
			evt.propagateToIMGUI = false;
			evt.stopDispatch = true;
		}
	}
}
