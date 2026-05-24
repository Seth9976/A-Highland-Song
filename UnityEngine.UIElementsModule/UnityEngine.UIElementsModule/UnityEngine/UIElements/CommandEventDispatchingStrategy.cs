using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001C2 RID: 450
	internal class CommandEventDispatchingStrategy : IEventDispatchingStrategy
	{
		// Token: 0x06000E29 RID: 3625 RVA: 0x0003A04C File Offset: 0x0003824C
		public bool CanDispatchEvent(EventBase evt)
		{
			return evt is ICommandEvent;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0003A068 File Offset: 0x00038268
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
						bool flag4 = !evt.isPropagationStopped && evt.propagateToIMGUI;
						if (flag4)
						{
							evt.skipElements.Add(imguicontainer);
							EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
						}
					}
					else
					{
						evt.target = panel.focusController.GetLeafFocusedElement();
						EventDispatchUtilities.PropagateEvent(evt);
						bool flag5 = !evt.isPropagationStopped && evt.propagateToIMGUI;
						if (flag5)
						{
							EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
						}
					}
				}
				else
				{
					EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
				}
			}
			evt.propagateToIMGUI = false;
			evt.stopDispatch = true;
		}
	}
}
