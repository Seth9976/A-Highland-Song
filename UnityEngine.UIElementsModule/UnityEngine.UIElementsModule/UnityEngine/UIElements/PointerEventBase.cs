using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000216 RID: 534
	public abstract class PointerEventBase<T> : EventBase<T>, IPointerEvent, IPointerEventInternal where T : PointerEventBase<T>, new()
	{
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001012 RID: 4114 RVA: 0x0003EF2E File Offset: 0x0003D12E
		// (set) Token: 0x06001013 RID: 4115 RVA: 0x0003EF36 File Offset: 0x0003D136
		public int pointerId { get; protected set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001014 RID: 4116 RVA: 0x0003EF3F File Offset: 0x0003D13F
		// (set) Token: 0x06001015 RID: 4117 RVA: 0x0003EF47 File Offset: 0x0003D147
		public string pointerType { get; protected set; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001016 RID: 4118 RVA: 0x0003EF50 File Offset: 0x0003D150
		// (set) Token: 0x06001017 RID: 4119 RVA: 0x0003EF58 File Offset: 0x0003D158
		public bool isPrimary { get; protected set; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001018 RID: 4120 RVA: 0x0003EF61 File Offset: 0x0003D161
		// (set) Token: 0x06001019 RID: 4121 RVA: 0x0003EF69 File Offset: 0x0003D169
		public int button { get; protected set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x0600101A RID: 4122 RVA: 0x0003EF72 File Offset: 0x0003D172
		// (set) Token: 0x0600101B RID: 4123 RVA: 0x0003EF7A File Offset: 0x0003D17A
		public int pressedButtons { get; protected set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600101C RID: 4124 RVA: 0x0003EF83 File Offset: 0x0003D183
		// (set) Token: 0x0600101D RID: 4125 RVA: 0x0003EF8B File Offset: 0x0003D18B
		public Vector3 position { get; protected set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x0003EF94 File Offset: 0x0003D194
		// (set) Token: 0x0600101F RID: 4127 RVA: 0x0003EF9C File Offset: 0x0003D19C
		public Vector3 localPosition { get; protected set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x0003EFA5 File Offset: 0x0003D1A5
		// (set) Token: 0x06001021 RID: 4129 RVA: 0x0003EFAD File Offset: 0x0003D1AD
		public Vector3 deltaPosition { get; protected set; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001022 RID: 4130 RVA: 0x0003EFB6 File Offset: 0x0003D1B6
		// (set) Token: 0x06001023 RID: 4131 RVA: 0x0003EFBE File Offset: 0x0003D1BE
		public float deltaTime { get; protected set; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x0003EFC7 File Offset: 0x0003D1C7
		// (set) Token: 0x06001025 RID: 4133 RVA: 0x0003EFCF File Offset: 0x0003D1CF
		public int clickCount { get; protected set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x0003EFD8 File Offset: 0x0003D1D8
		// (set) Token: 0x06001027 RID: 4135 RVA: 0x0003EFE0 File Offset: 0x0003D1E0
		public float pressure { get; protected set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001028 RID: 4136 RVA: 0x0003EFE9 File Offset: 0x0003D1E9
		// (set) Token: 0x06001029 RID: 4137 RVA: 0x0003EFF1 File Offset: 0x0003D1F1
		public float tangentialPressure { get; protected set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x0003EFFA File Offset: 0x0003D1FA
		// (set) Token: 0x0600102B RID: 4139 RVA: 0x0003F002 File Offset: 0x0003D202
		public float altitudeAngle { get; protected set; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x0003F00B File Offset: 0x0003D20B
		// (set) Token: 0x0600102D RID: 4141 RVA: 0x0003F013 File Offset: 0x0003D213
		public float azimuthAngle { get; protected set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x0003F01C File Offset: 0x0003D21C
		// (set) Token: 0x0600102F RID: 4143 RVA: 0x0003F024 File Offset: 0x0003D224
		public float twist { get; protected set; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x0003F02D File Offset: 0x0003D22D
		// (set) Token: 0x06001031 RID: 4145 RVA: 0x0003F035 File Offset: 0x0003D235
		public Vector2 radius { get; protected set; }

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x0003F03E File Offset: 0x0003D23E
		// (set) Token: 0x06001033 RID: 4147 RVA: 0x0003F046 File Offset: 0x0003D246
		public Vector2 radiusVariance { get; protected set; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0003F04F File Offset: 0x0003D24F
		// (set) Token: 0x06001035 RID: 4149 RVA: 0x0003F057 File Offset: 0x0003D257
		public EventModifiers modifiers { get; protected set; }

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x0003F060 File Offset: 0x0003D260
		public bool shiftKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Shift) > EventModifiers.None;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x0003F080 File Offset: 0x0003D280
		public bool ctrlKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Control) > EventModifiers.None;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x0003F0A0 File Offset: 0x0003D2A0
		public bool commandKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Command) > EventModifiers.None;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x0003F0C0 File Offset: 0x0003D2C0
		public bool altKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Alt) > EventModifiers.None;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x0003F0E0 File Offset: 0x0003D2E0
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

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x0003F119 File Offset: 0x0003D319
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x0003F121 File Offset: 0x0003D321
		bool IPointerEventInternal.triggeredByOS { get; set; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x0003F12A File Offset: 0x0003D32A
		// (set) Token: 0x0600103E RID: 4158 RVA: 0x0003F132 File Offset: 0x0003D332
		bool IPointerEventInternal.recomputeTopElementUnderPointer { get; set; }

		// Token: 0x0600103F RID: 4159 RVA: 0x0003F13B File Offset: 0x0003D33B
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0003F14C File Offset: 0x0003D34C
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable;
			base.propagateToIMGUI = false;
			this.pointerId = 0;
			this.pointerType = PointerType.unknown;
			this.isPrimary = false;
			this.button = -1;
			this.pressedButtons = 0;
			this.position = Vector3.zero;
			this.localPosition = Vector3.zero;
			this.deltaPosition = Vector3.zero;
			this.deltaTime = 0f;
			this.clickCount = 0;
			this.pressure = 0f;
			this.tangentialPressure = 0f;
			this.altitudeAngle = 0f;
			this.azimuthAngle = 0f;
			this.twist = 0f;
			this.radius = Vector2.zero;
			this.radiusVariance = Vector2.zero;
			this.modifiers = EventModifiers.None;
			((IPointerEventInternal)this).triggeredByOS = false;
			((IPointerEventInternal)this).recomputeTopElementUnderPointer = false;
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0003F23C File Offset: 0x0003D43C
		// (set) Token: 0x06001042 RID: 4162 RVA: 0x0003F254 File Offset: 0x0003D454
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
					this.localPosition = visualElement.WorldToLocal(this.position);
				}
				else
				{
					this.localPosition = this.position;
				}
			}
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0003F2B0 File Offset: 0x0003D4B0
		private static bool IsMouse(Event systemEvent)
		{
			EventType rawType = systemEvent.rawType;
			return rawType == EventType.MouseMove || rawType == EventType.MouseDown || rawType == EventType.MouseUp || rawType == EventType.MouseDrag || rawType == EventType.ContextClick || rawType == EventType.MouseEnterWindow || rawType == EventType.MouseLeaveWindow;
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0003F2EC File Offset: 0x0003D4EC
		public static T GetPooled(Event systemEvent)
		{
			T pooled = EventBase<T>.GetPooled();
			bool flag = !PointerEventBase<T>.IsMouse(systemEvent) && systemEvent.rawType != EventType.DragUpdated;
			if (flag)
			{
				Debug.Assert(false, string.Concat(new string[]
				{
					"Unexpected event type: ",
					systemEvent.rawType.ToString(),
					" (",
					systemEvent.type.ToString(),
					")"
				}));
			}
			PointerType pointerType = systemEvent.pointerType;
			PointerType pointerType2 = pointerType;
			if (pointerType2 != PointerType.Touch)
			{
				if (pointerType2 != PointerType.Pen)
				{
					pooled.pointerType = PointerType.mouse;
					pooled.pointerId = PointerId.mousePointerId;
				}
				else
				{
					pooled.pointerType = PointerType.pen;
					pooled.pointerId = PointerId.penPointerIdBase;
				}
			}
			else
			{
				pooled.pointerType = PointerType.touch;
				pooled.pointerId = PointerId.touchPointerIdBase;
			}
			pooled.isPrimary = true;
			pooled.altitudeAngle = 0f;
			pooled.azimuthAngle = 0f;
			pooled.twist = 0f;
			pooled.radius = Vector2.zero;
			pooled.radiusVariance = Vector2.zero;
			pooled.imguiEvent = systemEvent;
			bool flag2 = systemEvent.rawType == EventType.MouseDown;
			if (flag2)
			{
				PointerDeviceState.PressButton(PointerId.mousePointerId, systemEvent.button);
				pooled.button = systemEvent.button;
			}
			else
			{
				bool flag3 = systemEvent.rawType == EventType.MouseUp;
				if (flag3)
				{
					PointerDeviceState.ReleaseButton(PointerId.mousePointerId, systemEvent.button);
					pooled.button = systemEvent.button;
				}
				else
				{
					bool flag4 = systemEvent.rawType == EventType.MouseMove;
					if (flag4)
					{
						pooled.button = -1;
					}
				}
			}
			pooled.pressedButtons = PointerDeviceState.GetPressedButtons(pooled.pointerId);
			pooled.position = systemEvent.mousePosition;
			pooled.localPosition = systemEvent.mousePosition;
			pooled.deltaPosition = systemEvent.delta;
			pooled.clickCount = systemEvent.clickCount;
			pooled.modifiers = systemEvent.modifiers;
			PointerType pointerType3 = systemEvent.pointerType;
			PointerType pointerType4 = pointerType3;
			if (pointerType4 - PointerType.Touch > 1)
			{
				pooled.pressure = ((pooled.pressedButtons == 0) ? 0f : 0.5f);
			}
			else
			{
				pooled.pressure = systemEvent.pressure;
			}
			pooled.tangentialPressure = 0f;
			pooled.triggeredByOS = true;
			return pooled;
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0003F5EC File Offset: 0x0003D7EC
		public static T GetPooled(Touch touch, EventModifiers modifiers = EventModifiers.None)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.pointerId = touch.fingerId + PointerId.touchPointerIdBase;
			pooled.pointerType = PointerType.touch;
			bool flag = false;
			for (int i = PointerId.touchPointerIdBase; i < PointerId.touchPointerIdBase + PointerId.touchPointerCount; i++)
			{
				bool flag2 = i != pooled.pointerId && PointerDeviceState.GetPressedButtons(i) != 0;
				if (flag2)
				{
					flag = true;
					break;
				}
			}
			pooled.isPrimary = !flag;
			bool flag3 = touch.phase == TouchPhase.Began;
			if (flag3)
			{
				PointerDeviceState.PressButton(pooled.pointerId, 0);
				pooled.button = 0;
			}
			else
			{
				bool flag4 = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
				if (flag4)
				{
					PointerDeviceState.ReleaseButton(pooled.pointerId, 0);
					pooled.button = 0;
				}
				else
				{
					pooled.button = -1;
				}
			}
			pooled.pressedButtons = PointerDeviceState.GetPressedButtons(pooled.pointerId);
			pooled.position = touch.position;
			pooled.localPosition = touch.position;
			pooled.deltaPosition = touch.deltaPosition;
			pooled.deltaTime = touch.deltaTime;
			pooled.clickCount = touch.tapCount;
			pooled.pressure = ((Mathf.Abs(touch.maximumPossiblePressure) > 1E-30f) ? (touch.pressure / touch.maximumPossiblePressure) : 1f);
			pooled.tangentialPressure = 0f;
			pooled.altitudeAngle = touch.altitudeAngle;
			pooled.azimuthAngle = touch.azimuthAngle;
			pooled.twist = 0f;
			pooled.radius = new Vector2(touch.radius, touch.radius);
			pooled.radiusVariance = new Vector2(touch.radiusVariance, touch.radiusVariance);
			pooled.modifiers = modifiers;
			pooled.triggeredByOS = true;
			return pooled;
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0003F86C File Offset: 0x0003DA6C
		internal static T GetPooled(IPointerEvent triggerEvent, Vector2 position, int pointerId)
		{
			bool flag = triggerEvent != null;
			T t;
			if (flag)
			{
				t = PointerEventBase<T>.GetPooled(triggerEvent);
			}
			else
			{
				T pooled = EventBase<T>.GetPooled();
				pooled.position = position;
				pooled.localPosition = position;
				pooled.pointerId = pointerId;
				pooled.pointerType = PointerType.GetPointerType(pointerId);
				t = pooled;
			}
			return t;
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0003F8DC File Offset: 0x0003DADC
		public static T GetPooled(IPointerEvent triggerEvent)
		{
			T pooled = EventBase<T>.GetPooled();
			bool flag = triggerEvent != null;
			if (flag)
			{
				pooled.pointerId = triggerEvent.pointerId;
				pooled.pointerType = triggerEvent.pointerType;
				pooled.isPrimary = triggerEvent.isPrimary;
				pooled.button = triggerEvent.button;
				pooled.pressedButtons = triggerEvent.pressedButtons;
				pooled.position = triggerEvent.position;
				pooled.localPosition = triggerEvent.localPosition;
				pooled.deltaPosition = triggerEvent.deltaPosition;
				pooled.deltaTime = triggerEvent.deltaTime;
				pooled.clickCount = triggerEvent.clickCount;
				pooled.pressure = triggerEvent.pressure;
				pooled.tangentialPressure = triggerEvent.tangentialPressure;
				pooled.altitudeAngle = triggerEvent.altitudeAngle;
				pooled.azimuthAngle = triggerEvent.azimuthAngle;
				pooled.twist = triggerEvent.twist;
				pooled.radius = triggerEvent.radius;
				pooled.radiusVariance = triggerEvent.radiusVariance;
				pooled.modifiers = triggerEvent.modifiers;
				IPointerEventInternal pointerEventInternal = triggerEvent as IPointerEventInternal;
				bool flag2 = pointerEventInternal != null;
				if (flag2)
				{
					pooled.triggeredByOS |= pointerEventInternal.triggeredByOS;
				}
			}
			return pooled;
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0003FA74 File Offset: 0x0003DC74
		protected internal override void PreDispatch(IPanel panel)
		{
			base.PreDispatch(panel);
			bool triggeredByOS = ((IPointerEventInternal)this).triggeredByOS;
			if (triggeredByOS)
			{
				PointerDeviceState.SavePointerPosition(this.pointerId, this.position, panel, panel.contextType);
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0003FAB4 File Offset: 0x0003DCB4
		protected internal override void PostDispatch(IPanel panel)
		{
			for (int i = 0; i < PointerId.maxPointers; i++)
			{
				panel.ProcessPointerCapture(i);
			}
			bool flag = !panel.ShouldSendCompatibilityMouseEvents(this) && ((IPointerEventInternal)this).triggeredByOS;
			if (flag)
			{
				BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
				if (baseVisualElementPanel != null)
				{
					baseVisualElementPanel.CommitElementUnderPointers();
				}
			}
			base.PostDispatch(panel);
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0003FB12 File Offset: 0x0003DD12
		protected PointerEventBase()
		{
			this.LocalInit();
		}
	}
}
