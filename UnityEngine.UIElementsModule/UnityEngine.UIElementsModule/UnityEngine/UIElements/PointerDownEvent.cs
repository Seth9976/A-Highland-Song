using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000217 RID: 535
	public sealed class PointerDownEvent : PointerEventBase<PointerDownEvent>
	{
		// Token: 0x0600104B RID: 4171 RVA: 0x0003FB23 File Offset: 0x0003DD23
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0003FB34 File Offset: 0x0003DD34
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
			((IPointerEventInternal)this).triggeredByOS = true;
			((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0003FB50 File Offset: 0x0003DD50
		public PointerDownEvent()
		{
			this.LocalInit();
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0003FB64 File Offset: 0x0003DD64
		protected internal override void PostDispatch(IPanel panel)
		{
			bool flag = !base.isDefaultPrevented;
			if (flag)
			{
				bool flag2 = panel.ShouldSendCompatibilityMouseEvents(this);
				if (flag2)
				{
					using (MouseDownEvent pooled = MouseDownEvent.GetPooled(this))
					{
						pooled.target = base.target;
						pooled.target.SendEvent(pooled);
					}
				}
			}
			else
			{
				panel.PreventCompatibilityMouseEvents(base.pointerId);
			}
			base.PostDispatch(panel);
		}
	}
}
