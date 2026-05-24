using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001BD RID: 445
	public abstract class MouseCaptureEventBase<T> : PointerCaptureEventBase<T>, IMouseCaptureEvent where T : MouseCaptureEventBase<T>, new()
	{
		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x00039F5B File Offset: 0x0003815B
		public new IEventHandler relatedTarget
		{
			get
			{
				return base.relatedTarget;
			}
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00039F64 File Offset: 0x00038164
		public static T GetPooled(IEventHandler target, IEventHandler relatedTarget)
		{
			return PointerCaptureEventBase<T>.GetPooled(target, relatedTarget, 0);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00039F80 File Offset: 0x00038180
		protected override void Init()
		{
			base.Init();
		}
	}
}
