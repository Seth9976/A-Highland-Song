using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001E9 RID: 489
	public class KeyDownEvent : KeyboardEventBase<KeyDownEvent>
	{
		// Token: 0x06000F28 RID: 3880 RVA: 0x0003CBB8 File Offset: 0x0003ADB8
		internal void GetEquivalentImguiEvent(Event outImguiEvent)
		{
			bool flag = base.imguiEvent != null;
			if (flag)
			{
				outImguiEvent.CopyFrom(base.imguiEvent);
			}
			else
			{
				outImguiEvent.type = EventType.KeyDown;
				outImguiEvent.modifiers = base.modifiers;
				outImguiEvent.character = base.character;
				outImguiEvent.keyCode = base.keyCode;
			}
		}
	}
}
