using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000239 RID: 569
	[Serializable]
	internal class EventDebuggerEventRecord
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x00041E02 File Offset: 0x00040002
		// (set) Token: 0x060010E0 RID: 4320 RVA: 0x00041E0A File Offset: 0x0004000A
		public string eventBaseName { get; private set; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x00041E13 File Offset: 0x00040013
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x00041E1B File Offset: 0x0004001B
		public long eventTypeId { get; private set; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x00041E24 File Offset: 0x00040024
		// (set) Token: 0x060010E4 RID: 4324 RVA: 0x00041E2C File Offset: 0x0004002C
		public ulong eventId { get; private set; }

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x00041E35 File Offset: 0x00040035
		// (set) Token: 0x060010E6 RID: 4326 RVA: 0x00041E3D File Offset: 0x0004003D
		private ulong triggerEventId { get; set; }

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x00041E46 File Offset: 0x00040046
		// (set) Token: 0x060010E8 RID: 4328 RVA: 0x00041E4E File Offset: 0x0004004E
		internal long timestamp { get; private set; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x00041E57 File Offset: 0x00040057
		// (set) Token: 0x060010EA RID: 4330 RVA: 0x00041E5F File Offset: 0x0004005F
		public IEventHandler target { get; set; }

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x00041E68 File Offset: 0x00040068
		// (set) Token: 0x060010EC RID: 4332 RVA: 0x00041E70 File Offset: 0x00040070
		private List<IEventHandler> skipElements { get; set; }

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x00041E79 File Offset: 0x00040079
		// (set) Token: 0x060010EE RID: 4334 RVA: 0x00041E81 File Offset: 0x00040081
		public bool hasUnderlyingPhysicalEvent { get; private set; }

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x00041E8A File Offset: 0x0004008A
		// (set) Token: 0x060010F0 RID: 4336 RVA: 0x00041E92 File Offset: 0x00040092
		private bool isPropagationStopped { get; set; }

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00041E9B File Offset: 0x0004009B
		// (set) Token: 0x060010F2 RID: 4338 RVA: 0x00041EA3 File Offset: 0x000400A3
		private bool isImmediatePropagationStopped { get; set; }

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x00041EAC File Offset: 0x000400AC
		// (set) Token: 0x060010F4 RID: 4340 RVA: 0x00041EB4 File Offset: 0x000400B4
		private bool isDefaultPrevented { get; set; }

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00041EBD File Offset: 0x000400BD
		// (set) Token: 0x060010F6 RID: 4342 RVA: 0x00041EC5 File Offset: 0x000400C5
		public PropagationPhase propagationPhase { get; private set; }

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x00041ECE File Offset: 0x000400CE
		// (set) Token: 0x060010F8 RID: 4344 RVA: 0x00041ED6 File Offset: 0x000400D6
		private IEventHandler currentTarget { get; set; }

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x00041EDF File Offset: 0x000400DF
		// (set) Token: 0x060010FA RID: 4346 RVA: 0x00041EE7 File Offset: 0x000400E7
		private bool dispatch { get; set; }

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x00041EF0 File Offset: 0x000400F0
		// (set) Token: 0x060010FC RID: 4348 RVA: 0x00041EF8 File Offset: 0x000400F8
		private Vector2 originalMousePosition { get; set; }

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060010FD RID: 4349 RVA: 0x00041F01 File Offset: 0x00040101
		// (set) Token: 0x060010FE RID: 4350 RVA: 0x00041F09 File Offset: 0x00040109
		public EventModifiers modifiers { get; private set; }

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x00041F12 File Offset: 0x00040112
		// (set) Token: 0x06001100 RID: 4352 RVA: 0x00041F1A File Offset: 0x0004011A
		public Vector2 mousePosition { get; private set; }

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x00041F23 File Offset: 0x00040123
		// (set) Token: 0x06001102 RID: 4354 RVA: 0x00041F2B File Offset: 0x0004012B
		public int clickCount { get; private set; }

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x00041F34 File Offset: 0x00040134
		// (set) Token: 0x06001104 RID: 4356 RVA: 0x00041F3C File Offset: 0x0004013C
		public int button { get; private set; }

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x00041F45 File Offset: 0x00040145
		// (set) Token: 0x06001106 RID: 4358 RVA: 0x00041F4D File Offset: 0x0004014D
		public int pressedButtons { get; private set; }

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x00041F56 File Offset: 0x00040156
		// (set) Token: 0x06001108 RID: 4360 RVA: 0x00041F5E File Offset: 0x0004015E
		public Vector3 delta { get; private set; }

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x00041F67 File Offset: 0x00040167
		// (set) Token: 0x0600110A RID: 4362 RVA: 0x00041F6F File Offset: 0x0004016F
		public char character { get; private set; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x00041F78 File Offset: 0x00040178
		// (set) Token: 0x0600110C RID: 4364 RVA: 0x00041F80 File Offset: 0x00040180
		public KeyCode keyCode { get; private set; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x00041F89 File Offset: 0x00040189
		// (set) Token: 0x0600110E RID: 4366 RVA: 0x00041F91 File Offset: 0x00040191
		public string commandName { get; private set; }

		// Token: 0x0600110F RID: 4367 RVA: 0x00041F9C File Offset: 0x0004019C
		private void Init(EventBase evt)
		{
			Type type = evt.GetType();
			this.eventBaseName = EventDebugger.GetTypeDisplayName(type);
			this.eventTypeId = evt.eventTypeId;
			this.eventId = evt.eventId;
			this.triggerEventId = evt.triggerEventId;
			this.timestamp = evt.timestamp;
			this.target = evt.target;
			this.skipElements = evt.skipElements;
			this.isPropagationStopped = evt.isPropagationStopped;
			this.isImmediatePropagationStopped = evt.isImmediatePropagationStopped;
			this.isDefaultPrevented = evt.isDefaultPrevented;
			IMouseEvent mouseEvent = evt as IMouseEvent;
			IMouseEventInternal mouseEventInternal = evt as IMouseEventInternal;
			this.hasUnderlyingPhysicalEvent = mouseEvent != null && mouseEventInternal != null && mouseEventInternal.triggeredByOS;
			this.propagationPhase = evt.propagationPhase;
			this.originalMousePosition = evt.originalMousePosition;
			this.currentTarget = evt.currentTarget;
			this.dispatch = evt.dispatch;
			bool flag = mouseEvent != null;
			if (flag)
			{
				this.modifiers = mouseEvent.modifiers;
				this.mousePosition = mouseEvent.mousePosition;
				this.button = mouseEvent.button;
				this.pressedButtons = mouseEvent.pressedButtons;
				this.clickCount = mouseEvent.clickCount;
				WheelEvent wheelEvent = mouseEvent as WheelEvent;
				bool flag2 = wheelEvent != null;
				if (flag2)
				{
					this.delta = wheelEvent.delta;
				}
			}
			IPointerEvent pointerEvent = evt as IPointerEvent;
			bool flag3 = pointerEvent != null;
			if (flag3)
			{
				IPointerEventInternal pointerEventInternal = evt as IPointerEventInternal;
				this.hasUnderlyingPhysicalEvent = pointerEvent != null && pointerEventInternal != null && pointerEventInternal.triggeredByOS;
				this.modifiers = pointerEvent.modifiers;
				this.mousePosition = pointerEvent.position;
				this.button = pointerEvent.button;
				this.pressedButtons = pointerEvent.pressedButtons;
				this.clickCount = pointerEvent.clickCount;
			}
			IKeyboardEvent keyboardEvent = evt as IKeyboardEvent;
			bool flag4 = keyboardEvent != null;
			if (flag4)
			{
				this.modifiers = keyboardEvent.modifiers;
				this.character = keyboardEvent.character;
				this.keyCode = keyboardEvent.keyCode;
			}
			ICommandEvent commandEvent = evt as ICommandEvent;
			bool flag5 = commandEvent != null;
			if (flag5)
			{
				this.commandName = commandEvent.commandName;
			}
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x000421D6 File Offset: 0x000403D6
		public EventDebuggerEventRecord(EventBase evt)
		{
			this.Init(evt);
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x000421E8 File Offset: 0x000403E8
		public string TimestampString()
		{
			long num = (long)((float)this.timestamp / 1000f * 10000000f);
			return new DateTime(num).ToString("HH:mm:ss.ffffff");
		}
	}
}
