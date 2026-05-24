using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F1 RID: 497
	public abstract class MouseEventBase<T> : EventBase<T>, IMouseEvent, IMouseEventInternal where T : MouseEventBase<T>, new()
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x0003D17D File Offset: 0x0003B37D
		// (set) Token: 0x06000F54 RID: 3924 RVA: 0x0003D185 File Offset: 0x0003B385
		public EventModifiers modifiers { get; protected set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0003D18E File Offset: 0x0003B38E
		// (set) Token: 0x06000F56 RID: 3926 RVA: 0x0003D196 File Offset: 0x0003B396
		public Vector2 mousePosition { get; protected set; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000F57 RID: 3927 RVA: 0x0003D19F File Offset: 0x0003B39F
		// (set) Token: 0x06000F58 RID: 3928 RVA: 0x0003D1A7 File Offset: 0x0003B3A7
		public Vector2 localMousePosition { get; internal set; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0003D1B0 File Offset: 0x0003B3B0
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x0003D1B8 File Offset: 0x0003B3B8
		public Vector2 mouseDelta { get; protected set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x0003D1C1 File Offset: 0x0003B3C1
		// (set) Token: 0x06000F5C RID: 3932 RVA: 0x0003D1C9 File Offset: 0x0003B3C9
		public int clickCount { get; protected set; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0003D1D2 File Offset: 0x0003B3D2
		// (set) Token: 0x06000F5E RID: 3934 RVA: 0x0003D1DA File Offset: 0x0003B3DA
		public int button { get; protected set; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0003D1E3 File Offset: 0x0003B3E3
		// (set) Token: 0x06000F60 RID: 3936 RVA: 0x0003D1EB File Offset: 0x0003B3EB
		public int pressedButtons { get; protected set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0003D1F4 File Offset: 0x0003B3F4
		public bool shiftKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Shift) > EventModifiers.None;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0003D214 File Offset: 0x0003B414
		public bool ctrlKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Control) > EventModifiers.None;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0003D234 File Offset: 0x0003B434
		public bool commandKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Command) > EventModifiers.None;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000F64 RID: 3940 RVA: 0x0003D254 File Offset: 0x0003B454
		public bool altKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Alt) > EventModifiers.None;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0003D274 File Offset: 0x0003B474
		public bool actionKey
		{
			get
			{
				bool flag = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
				bool flag2;
				if (flag)
				{
					flag2 = this.commandKey;
				}
				else
				{
					flag2 = this.ctrlKey;
				}
				return flag2;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x0003D2AD File Offset: 0x0003B4AD
		// (set) Token: 0x06000F67 RID: 3943 RVA: 0x0003D2B5 File Offset: 0x0003B4B5
		bool IMouseEventInternal.triggeredByOS { get; set; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0003D2BE File Offset: 0x0003B4BE
		// (set) Token: 0x06000F69 RID: 3945 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
		bool IMouseEventInternal.recomputeTopElementUnderMouse { get; set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0003D2CF File Offset: 0x0003B4CF
		// (set) Token: 0x06000F6B RID: 3947 RVA: 0x0003D2D7 File Offset: 0x0003B4D7
		IPointerEvent IMouseEventInternal.sourcePointerEvent { get; set; }

		// Token: 0x06000F6C RID: 3948 RVA: 0x0003D2E0 File Offset: 0x0003B4E0
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0003D2F4 File Offset: 0x0003B4F4
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable;
			this.modifiers = EventModifiers.None;
			this.mousePosition = Vector2.zero;
			this.localMousePosition = Vector2.zero;
			this.mouseDelta = Vector2.zero;
			this.clickCount = 0;
			this.button = 0;
			this.pressedButtons = 0;
			((IMouseEventInternal)this).triggeredByOS = false;
			((IMouseEventInternal)this).recomputeTopElementUnderMouse = true;
			((IMouseEventInternal)this).sourcePointerEvent = null;
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000F6E RID: 3950 RVA: 0x0003D368 File Offset: 0x0003B568
		// (set) Token: 0x06000F6F RID: 3951 RVA: 0x0003D380 File Offset: 0x0003B580
		public override IEventHandler currentTarget
		{
			get
			{
				return base.currentTarget;
			}
			internal set
			{
				base.currentTarget = value;
				VisualElement visualElement = this.currentTarget as VisualElement;
				bool flag = visualElement != null;
				if (flag)
				{
					this.localMousePosition = visualElement.WorldToLocal(this.mousePosition);
				}
				else
				{
					this.localMousePosition = this.mousePosition;
				}
			}
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0003D3D0 File Offset: 0x0003B5D0
		protected internal override void PreDispatch(IPanel panel)
		{
			base.PreDispatch(panel);
			bool triggeredByOS = ((IMouseEventInternal)this).triggeredByOS;
			if (triggeredByOS)
			{
				PointerDeviceState.SavePointerPosition(PointerId.mousePointerId, this.mousePosition, panel, panel.contextType);
			}
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0003D40C File Offset: 0x0003B60C
		protected internal override void PostDispatch(IPanel panel)
		{
			EventBase eventBase = ((IMouseEventInternal)this).sourcePointerEvent as EventBase;
			bool flag = eventBase != null;
			if (flag)
			{
				Debug.Assert(eventBase.processed);
				BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
				if (baseVisualElementPanel != null)
				{
					baseVisualElementPanel.CommitElementUnderPointers();
				}
				bool isPropagationStopped = base.isPropagationStopped;
				if (isPropagationStopped)
				{
					eventBase.StopPropagation();
				}
				bool isImmediatePropagationStopped = base.isImmediatePropagationStopped;
				if (isImmediatePropagationStopped)
				{
					eventBase.StopImmediatePropagation();
				}
				bool isDefaultPrevented = base.isDefaultPrevented;
				if (isDefaultPrevented)
				{
					eventBase.PreventDefault();
				}
				eventBase.processedByFocusController |= base.processedByFocusController;
			}
			base.PostDispatch(panel);
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0003D4A8 File Offset: 0x0003B6A8
		public static T GetPooled(Event systemEvent)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.imguiEvent = systemEvent;
			bool flag = systemEvent != null;
			if (flag)
			{
				pooled.modifiers = systemEvent.modifiers;
				pooled.mousePosition = systemEvent.mousePosition;
				pooled.localMousePosition = systemEvent.mousePosition;
				pooled.mouseDelta = systemEvent.delta;
				pooled.button = systemEvent.button;
				pooled.pressedButtons = PointerDeviceState.GetPressedButtons(PointerId.mousePointerId);
				pooled.clickCount = systemEvent.clickCount;
				pooled.triggeredByOS = true;
				pooled.recomputeTopElementUnderMouse = true;
			}
			return pooled;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0003D578 File Offset: 0x0003B778
		public static T GetPooled(Vector2 position, int button, int clickCount, Vector2 delta, EventModifiers modifiers = EventModifiers.None)
		{
			return MouseEventBase<T>.GetPooled(position, button, clickCount, delta, modifiers, false);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0003D598 File Offset: 0x0003B798
		internal static T GetPooled(Vector2 position, int button, int clickCount, Vector2 delta, EventModifiers modifiers, bool fromOS)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.modifiers = modifiers;
			pooled.mousePosition = position;
			pooled.localMousePosition = position;
			pooled.mouseDelta = delta;
			pooled.button = button;
			pooled.pressedButtons = PointerDeviceState.GetPressedButtons(PointerId.mousePointerId);
			pooled.clickCount = clickCount;
			pooled.triggeredByOS = fromOS;
			pooled.recomputeTopElementUnderMouse = true;
			return pooled;
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0003D634 File Offset: 0x0003B834
		internal static T GetPooled(IMouseEvent triggerEvent, Vector2 mousePosition, bool recomputeTopElementUnderMouse)
		{
			bool flag = triggerEvent != null;
			T t;
			if (flag)
			{
				t = MouseEventBase<T>.GetPooled(triggerEvent);
			}
			else
			{
				T pooled = EventBase<T>.GetPooled();
				pooled.mousePosition = mousePosition;
				pooled.localMousePosition = mousePosition;
				pooled.recomputeTopElementUnderMouse = recomputeTopElementUnderMouse;
				t = pooled;
			}
			return t;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0003D688 File Offset: 0x0003B888
		public static T GetPooled(IMouseEvent triggerEvent)
		{
			T pooled = EventBase<T>.GetPooled(triggerEvent as EventBase);
			bool flag = triggerEvent != null;
			if (flag)
			{
				pooled.modifiers = triggerEvent.modifiers;
				pooled.mousePosition = triggerEvent.mousePosition;
				pooled.localMousePosition = triggerEvent.mousePosition;
				pooled.mouseDelta = triggerEvent.mouseDelta;
				pooled.button = triggerEvent.button;
				pooled.pressedButtons = triggerEvent.pressedButtons;
				pooled.clickCount = triggerEvent.clickCount;
				IMouseEventInternal mouseEventInternal = triggerEvent as IMouseEventInternal;
				bool flag2 = mouseEventInternal != null;
				if (flag2)
				{
					pooled.triggeredByOS = mouseEventInternal.triggeredByOS;
					pooled.recomputeTopElementUnderMouse = false;
				}
			}
			return pooled;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0003D764 File Offset: 0x0003B964
		protected static T GetPooled(IPointerEvent pointerEvent)
		{
			T pooled = EventBase<T>.GetPooled();
			EventBase eventBase = pooled;
			EventBase eventBase2 = pointerEvent as EventBase;
			eventBase.target = ((eventBase2 != null) ? eventBase2.target : null);
			EventBase eventBase3 = pooled;
			EventBase eventBase4 = pointerEvent as EventBase;
			eventBase3.imguiEvent = ((eventBase4 != null) ? eventBase4.imguiEvent : null);
			EventBase eventBase5 = pointerEvent as EventBase;
			bool flag = ((eventBase5 != null) ? eventBase5.path : null) != null;
			if (flag)
			{
				pooled.path = (pointerEvent as EventBase).path;
			}
			pooled.modifiers = pointerEvent.modifiers;
			pooled.mousePosition = pointerEvent.position;
			pooled.localMousePosition = pointerEvent.position;
			pooled.mouseDelta = pointerEvent.deltaPosition;
			pooled.button = ((pointerEvent.button == -1) ? 0 : pointerEvent.button);
			pooled.pressedButtons = pointerEvent.pressedButtons;
			pooled.clickCount = pointerEvent.clickCount;
			IPointerEventInternal pointerEventInternal = pointerEvent as IPointerEventInternal;
			bool flag2 = pointerEventInternal != null;
			if (flag2)
			{
				pooled.triggeredByOS = pointerEventInternal.triggeredByOS;
				pooled.recomputeTopElementUnderMouse = true;
				pooled.sourcePointerEvent = pointerEvent;
			}
			return pooled;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0003D8C1 File Offset: 0x0003BAC1
		protected MouseEventBase()
		{
			this.LocalInit();
		}
	}
}
