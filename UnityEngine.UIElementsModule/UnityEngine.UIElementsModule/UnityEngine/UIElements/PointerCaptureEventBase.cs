using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001B9 RID: 441
	public abstract class PointerCaptureEventBase<T> : EventBase<T>, IPointerCaptureEvent, IPointerCaptureEventInternal where T : PointerCaptureEventBase<T>, new()
	{
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E11 RID: 3601 RVA: 0x00039EA4 File Offset: 0x000380A4
		// (set) Token: 0x06000E12 RID: 3602 RVA: 0x00039EAC File Offset: 0x000380AC
		public IEventHandler relatedTarget { get; private set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x00039EB5 File Offset: 0x000380B5
		// (set) Token: 0x06000E14 RID: 3604 RVA: 0x00039EBD File Offset: 0x000380BD
		public int pointerId { get; private set; }

		// Token: 0x06000E15 RID: 3605 RVA: 0x00039EC6 File Offset: 0x000380C6
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00039ED7 File Offset: 0x000380D7
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
			this.relatedTarget = null;
			this.pointerId = PointerId.invalidPointerId;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00039EF8 File Offset: 0x000380F8
		public static T GetPooled(IEventHandler target, IEventHandler relatedTarget, int pointerId)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.target = target;
			pooled.relatedTarget = relatedTarget;
			pooled.pointerId = pointerId;
			return pooled;
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00039F38 File Offset: 0x00038138
		protected PointerCaptureEventBase()
		{
			this.LocalInit();
		}
	}
}
