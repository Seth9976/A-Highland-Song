using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200021B RID: 539
	public sealed class PointerCancelEvent : PointerEventBase<PointerCancelEvent>
	{
		// Token: 0x0600105C RID: 4188 RVA: 0x0003FE7C File Offset: 0x0003E07C
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0003FE8D File Offset: 0x0003E08D
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.SkipDisabledElements;
			((IPointerEventInternal)this).triggeredByOS = true;
			((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0003FEA9 File Offset: 0x0003E0A9
		public PointerCancelEvent()
		{
			this.LocalInit();
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0003FEBC File Offset: 0x0003E0BC
		protected internal override void PostDispatch(IPanel panel)
		{
			bool flag = PointerType.IsDirectManipulationDevice(base.pointerType);
			if (flag)
			{
				panel.ReleasePointer(base.pointerId);
				BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
				if (baseVisualElementPanel != null)
				{
					baseVisualElementPanel.ClearCachedElementUnderPointer(base.pointerId, this);
				}
			}
			bool flag2 = panel.ShouldSendCompatibilityMouseEvents(this);
			if (flag2)
			{
				using (MouseUpEvent pooled = MouseUpEvent.GetPooled(this))
				{
					pooled.target = base.target;
					base.target.SendEvent(pooled);
				}
			}
			base.PostDispatch(panel);
			panel.ActivateCompatibilityMouseEvents(base.pointerId);
		}
	}
}
