using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001E6 RID: 486
	internal class KeyboardEventDispatchingStrategy : IEventDispatchingStrategy
	{
		// Token: 0x06000F0D RID: 3853 RVA: 0x0003C8F8 File Offset: 0x0003AAF8
		public bool CanDispatchEvent(EventBase evt)
		{
			return evt is IKeyboardEvent;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0003C914 File Offset: 0x0003AB14
		public void DispatchEvent(EventBase evt, IPanel panel)
		{
			bool flag = panel != null;
			if (flag)
			{
				Focusable leafFocusedElement = panel.focusController.GetLeafFocusedElement();
				bool flag2 = leafFocusedElement != null;
				if (flag2)
				{
					bool isIMGUIContainer = leafFocusedElement.isIMGUIContainer;
					if (isIMGUIContainer)
					{
						IMGUIContainer imguicontainer = (IMGUIContainer)leafFocusedElement;
						bool flag3 = !evt.Skip(imguicontainer) && imguicontainer.SendEventToIMGUI(evt, true, true);
						if (flag3)
						{
							evt.StopPropagation();
							evt.PreventDefault();
						}
					}
					else
					{
						evt.target = leafFocusedElement;
						EventDispatchUtilities.PropagateEvent(evt);
					}
				}
				else
				{
					evt.target = panel.visualTree;
					EventDispatchUtilities.PropagateEvent(evt);
					bool flag4 = !evt.isPropagationStopped;
					if (flag4)
					{
						EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
					}
				}
			}
			evt.propagateToIMGUI = false;
			evt.stopDispatch = true;
		}
	}
}
