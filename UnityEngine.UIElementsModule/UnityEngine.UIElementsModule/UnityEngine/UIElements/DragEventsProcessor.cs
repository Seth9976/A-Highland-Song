using System;
using UnityEngine.Assertions;

namespace UnityEngine.UIElements
{
	// Token: 0x020001A5 RID: 421
	internal abstract class DragEventsProcessor
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x000383FA File Offset: 0x000365FA
		internal bool isRegistered
		{
			get
			{
				return this.m_IsRegistered;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0000AC4F File Offset: 0x00008E4F
		internal virtual bool supportsDragEvents
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x00038402 File Offset: 0x00036602
		internal bool useDragEvents
		{
			get
			{
				return this.isEditorContext && this.supportsDragEvents;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00038418 File Offset: 0x00036618
		private bool isEditorContext
		{
			get
			{
				Assert.IsNotNull<VisualElement>(this.m_Target);
				Assert.IsNotNull<VisualElement>(this.m_Target.parent);
				return this.m_Target.panel.contextType == ContextType.Editor;
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0003845C File Offset: 0x0003665C
		internal DragEventsProcessor(VisualElement target)
		{
			this.m_Target = target;
			this.m_Target.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.RegisterCallbacksFromTarget), TrickleDown.NoTrickleDown);
			this.m_Target.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.UnregisterCallbacksFromTarget), TrickleDown.NoTrickleDown);
			this.RegisterCallbacksFromTarget();
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x000384B1 File Offset: 0x000366B1
		private void RegisterCallbacksFromTarget(AttachToPanelEvent evt)
		{
			this.RegisterCallbacksFromTarget();
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x000384BC File Offset: 0x000366BC
		private void RegisterCallbacksFromTarget()
		{
			bool isRegistered = this.m_IsRegistered;
			if (!isRegistered)
			{
				this.m_IsRegistered = true;
				this.m_Target.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDownEvent), TrickleDown.TrickleDown);
				this.m_Target.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUpEvent), TrickleDown.TrickleDown);
				this.m_Target.RegisterCallback<PointerLeaveEvent>(new EventCallback<PointerLeaveEvent>(this.OnPointerLeaveEvent), TrickleDown.NoTrickleDown);
				this.m_Target.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMoveEvent), TrickleDown.NoTrickleDown);
				this.m_Target.RegisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancelEvent), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0003855D File Offset: 0x0003675D
		private void UnregisterCallbacksFromTarget(DetachFromPanelEvent evt)
		{
			this.UnregisterCallbacksFromTarget(false);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00038568 File Offset: 0x00036768
		internal void UnregisterCallbacksFromTarget(bool unregisterPanelEvents = false)
		{
			this.m_IsRegistered = false;
			this.m_Target.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDownEvent), TrickleDown.TrickleDown);
			this.m_Target.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUpEvent), TrickleDown.TrickleDown);
			this.m_Target.UnregisterCallback<PointerLeaveEvent>(new EventCallback<PointerLeaveEvent>(this.OnPointerLeaveEvent), TrickleDown.NoTrickleDown);
			this.m_Target.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMoveEvent), TrickleDown.NoTrickleDown);
			this.m_Target.UnregisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancelEvent), TrickleDown.NoTrickleDown);
			if (unregisterPanelEvents)
			{
				this.m_Target.UnregisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.RegisterCallbacksFromTarget), TrickleDown.NoTrickleDown);
				this.m_Target.UnregisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.UnregisterCallbacksFromTarget), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x06000DBD RID: 3517
		protected abstract bool CanStartDrag(Vector3 pointerPosition);

		// Token: 0x06000DBE RID: 3518
		protected abstract StartDragArgs StartDrag(Vector3 pointerPosition);

		// Token: 0x06000DBF RID: 3519
		protected abstract DragVisualMode UpdateDrag(Vector3 pointerPosition);

		// Token: 0x06000DC0 RID: 3520
		protected abstract void OnDrop(Vector3 pointerPosition);

		// Token: 0x06000DC1 RID: 3521
		protected abstract void ClearDragAndDropUI();

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00038634 File Offset: 0x00036834
		private void OnPointerDownEvent(PointerDownEvent evt)
		{
			bool flag;
			if (evt.button == 0)
			{
				VisualElement visualElement = evt.leafTarget as VisualElement;
				flag = visualElement != null && visualElement.isIMGUIContainer;
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.m_DragState = DragEventsProcessor.DragState.None;
			}
			else
			{
				bool flag3 = this.CanStartDrag(evt.position);
				if (flag3)
				{
					this.m_DragState = DragEventsProcessor.DragState.CanStartDrag;
					this.m_Start = evt.position;
				}
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00038698 File Offset: 0x00036898
		internal void OnPointerUpEvent(PointerUpEvent evt)
		{
			bool flag = !this.useDragEvents;
			if (flag)
			{
				bool flag2 = this.m_DragState == DragEventsProcessor.DragState.Dragging;
				if (flag2)
				{
					this.m_Target.ReleasePointer(evt.pointerId);
					this.OnDrop(evt.position);
					this.ClearDragAndDropUI();
					evt.StopPropagation();
				}
			}
			this.m_DragState = DragEventsProcessor.DragState.None;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x000386F8 File Offset: 0x000368F8
		private void OnPointerLeaveEvent(PointerLeaveEvent evt)
		{
			bool flag = evt.target == this.m_Target;
			if (flag)
			{
				this.ClearDragAndDropUI();
			}
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00038720 File Offset: 0x00036920
		private void OnPointerCancelEvent(PointerCancelEvent evt)
		{
			bool flag = !this.useDragEvents;
			if (flag)
			{
				this.ClearDragAndDropUI();
			}
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00038744 File Offset: 0x00036944
		private void OnPointerMoveEvent(PointerMoveEvent evt)
		{
			bool useDragEvents = this.useDragEvents;
			if (useDragEvents)
			{
				bool flag = this.m_DragState != DragEventsProcessor.DragState.CanStartDrag;
				if (flag)
				{
					return;
				}
			}
			else
			{
				bool flag2 = this.m_DragState == DragEventsProcessor.DragState.Dragging;
				if (flag2)
				{
					this.UpdateDrag(evt.position);
					return;
				}
				bool flag3 = this.m_DragState != DragEventsProcessor.DragState.CanStartDrag;
				if (flag3)
				{
					return;
				}
			}
			bool flag4 = Mathf.Abs(this.m_Start.x - evt.position.x) > 5f || Mathf.Abs(this.m_Start.y - evt.position.y) > 5f;
			if (flag4)
			{
				StartDragArgs startDragArgs = this.StartDrag(this.m_Start);
				bool useDragEvents2 = this.useDragEvents;
				if (useDragEvents2)
				{
					bool flag5 = Event.current.type != EventType.MouseDown && Event.current.type != EventType.MouseDrag;
					if (flag5)
					{
						return;
					}
					DragAndDropUtility.dragAndDrop.StartDrag(startDragArgs);
				}
				else
				{
					this.m_Target.CapturePointer(evt.pointerId);
					evt.StopPropagation();
					this.dragAndDropClient = new DefaultDragAndDropClient();
					this.dragAndDropClient.StartDrag(startDragArgs);
				}
				this.m_DragState = DragEventsProcessor.DragState.Dragging;
			}
		}

		// Token: 0x0400064C RID: 1612
		private bool m_IsRegistered;

		// Token: 0x0400064D RID: 1613
		internal DragEventsProcessor.DragState m_DragState;

		// Token: 0x0400064E RID: 1614
		private Vector3 m_Start;

		// Token: 0x0400064F RID: 1615
		internal readonly VisualElement m_Target;

		// Token: 0x04000650 RID: 1616
		private const int k_DistanceToActivation = 5;

		// Token: 0x04000651 RID: 1617
		internal DefaultDragAndDropClient dragAndDropClient;

		// Token: 0x020001A6 RID: 422
		internal enum DragState
		{
			// Token: 0x04000653 RID: 1619
			None,
			// Token: 0x04000654 RID: 1620
			CanStartDrag,
			// Token: 0x04000655 RID: 1621
			Dragging
		}
	}
}
