using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F6 RID: 502
	public class WheelEvent : MouseEventBase<WheelEvent>
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x0003DB01 File Offset: 0x0003BD01
		// (set) Token: 0x06000F8F RID: 3983 RVA: 0x0003DB09 File Offset: 0x0003BD09
		public Vector3 delta { get; private set; }

		// Token: 0x06000F90 RID: 3984 RVA: 0x0003DB14 File Offset: 0x0003BD14
		public new static WheelEvent GetPooled(Event systemEvent)
		{
			WheelEvent pooled = MouseEventBase<WheelEvent>.GetPooled(systemEvent);
			pooled.imguiEvent = systemEvent;
			bool flag = systemEvent != null;
			if (flag)
			{
				pooled.delta = systemEvent.delta;
			}
			return pooled;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0003DB54 File Offset: 0x0003BD54
		internal static WheelEvent GetPooled(Vector3 delta, Vector3 mousePosition)
		{
			WheelEvent pooled = EventBase<WheelEvent>.GetPooled();
			pooled.delta = delta;
			pooled.mousePosition = mousePosition;
			return pooled;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0003DB84 File Offset: 0x0003BD84
		internal static WheelEvent GetPooled(Vector3 delta, IPointerEvent pointerEvent)
		{
			WheelEvent pooled = MouseEventBase<WheelEvent>.GetPooled(pointerEvent);
			pooled.delta = delta;
			return pooled;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0003DBA6 File Offset: 0x0003BDA6
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0003DBB7 File Offset: 0x0003BDB7
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
			this.delta = Vector3.zero;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0003DBCF File Offset: 0x0003BDCF
		public WheelEvent()
		{
			this.LocalInit();
		}
	}
}
