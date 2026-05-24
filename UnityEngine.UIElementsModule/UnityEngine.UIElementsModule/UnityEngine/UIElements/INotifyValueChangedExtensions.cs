using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000149 RID: 329
	public static class INotifyValueChangedExtensions
	{
		// Token: 0x06000A88 RID: 2696 RVA: 0x000299C4 File Offset: 0x00027BC4
		public static bool RegisterValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
		{
			CallbackEventHandler callbackEventHandler = control as CallbackEventHandler;
			bool flag = callbackEventHandler != null;
			bool flag2;
			if (flag)
			{
				callbackEventHandler.RegisterCallback<ChangeEvent<T>>(callback, TrickleDown.NoTrickleDown);
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x000299F4 File Offset: 0x00027BF4
		public static bool UnregisterValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
		{
			CallbackEventHandler callbackEventHandler = control as CallbackEventHandler;
			bool flag = callbackEventHandler != null;
			bool flag2;
			if (flag)
			{
				callbackEventHandler.UnregisterCallback<ChangeEvent<T>>(callback, TrickleDown.NoTrickleDown);
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}
	}
}
