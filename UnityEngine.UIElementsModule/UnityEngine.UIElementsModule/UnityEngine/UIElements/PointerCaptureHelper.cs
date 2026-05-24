using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000061 RID: 97
	public static class PointerCaptureHelper
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000A408 File Offset: 0x00008608
		private static PointerDispatchState GetStateFor(IEventHandler handler)
		{
			VisualElement visualElement = handler as VisualElement;
			PointerDispatchState pointerDispatchState;
			if (visualElement == null)
			{
				pointerDispatchState = null;
			}
			else
			{
				IPanel panel = visualElement.panel;
				if (panel == null)
				{
					pointerDispatchState = null;
				}
				else
				{
					EventDispatcher dispatcher = panel.dispatcher;
					pointerDispatchState = ((dispatcher != null) ? dispatcher.pointerState : null);
				}
			}
			return pointerDispatchState;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000A448 File Offset: 0x00008648
		public static bool HasPointerCapture(this IEventHandler handler, int pointerId)
		{
			PointerDispatchState stateFor = PointerCaptureHelper.GetStateFor(handler);
			return stateFor != null && stateFor.HasPointerCapture(handler, pointerId);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000A46E File Offset: 0x0000866E
		public static void CapturePointer(this IEventHandler handler, int pointerId)
		{
			PointerDispatchState stateFor = PointerCaptureHelper.GetStateFor(handler);
			if (stateFor != null)
			{
				stateFor.CapturePointer(handler, pointerId);
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000A485 File Offset: 0x00008685
		public static void ReleasePointer(this IEventHandler handler, int pointerId)
		{
			PointerDispatchState stateFor = PointerCaptureHelper.GetStateFor(handler);
			if (stateFor != null)
			{
				stateFor.ReleasePointer(handler, pointerId);
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000A49C File Offset: 0x0000869C
		public static IEventHandler GetCapturingElement(this IPanel panel, int pointerId)
		{
			IEventHandler eventHandler;
			if (panel == null)
			{
				eventHandler = null;
			}
			else
			{
				EventDispatcher dispatcher = panel.dispatcher;
				eventHandler = ((dispatcher != null) ? dispatcher.pointerState.GetCapturingElement(pointerId) : null);
			}
			return eventHandler;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000A4CC File Offset: 0x000086CC
		public static void ReleasePointer(this IPanel panel, int pointerId)
		{
			if (panel != null)
			{
				EventDispatcher dispatcher = panel.dispatcher;
				if (dispatcher != null)
				{
					dispatcher.pointerState.ReleasePointer(pointerId);
				}
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000A4EC File Offset: 0x000086EC
		internal static void ActivateCompatibilityMouseEvents(this IPanel panel, int pointerId)
		{
			if (panel != null)
			{
				EventDispatcher dispatcher = panel.dispatcher;
				if (dispatcher != null)
				{
					dispatcher.pointerState.ActivateCompatibilityMouseEvents(pointerId);
				}
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000A50C File Offset: 0x0000870C
		internal static void PreventCompatibilityMouseEvents(this IPanel panel, int pointerId)
		{
			if (panel != null)
			{
				EventDispatcher dispatcher = panel.dispatcher;
				if (dispatcher != null)
				{
					dispatcher.pointerState.PreventCompatibilityMouseEvents(pointerId);
				}
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000A52C File Offset: 0x0000872C
		internal static bool ShouldSendCompatibilityMouseEvents(this IPanel panel, IPointerEvent evt)
		{
			Nullable<bool> nullable;
			if (panel == null)
			{
				nullable = default(bool?);
			}
			else
			{
				EventDispatcher dispatcher = panel.dispatcher;
				nullable = ((dispatcher != null) ? new bool?(dispatcher.pointerState.ShouldSendCompatibilityMouseEvents(evt)) : default(bool?));
			}
			return nullable ?? true;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000A585 File Offset: 0x00008785
		internal static void ProcessPointerCapture(this IPanel panel, int pointerId)
		{
			if (panel != null)
			{
				EventDispatcher dispatcher = panel.dispatcher;
				if (dispatcher != null)
				{
					dispatcher.pointerState.ProcessPointerCapture(pointerId);
				}
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000A5A5 File Offset: 0x000087A5
		internal static void ResetPointerDispatchState(this IPanel panel)
		{
			if (panel != null)
			{
				EventDispatcher dispatcher = panel.dispatcher;
				if (dispatcher != null)
				{
					dispatcher.pointerState.Reset();
				}
			}
		}
	}
}
