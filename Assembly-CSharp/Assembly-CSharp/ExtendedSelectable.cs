using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001CB RID: 459
public class ExtendedSelectable : Selectable
{
	// Token: 0x06000F53 RID: 3923 RVA: 0x00075BE1 File Offset: 0x00073DE1
	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onDown.Invoke(eventData);
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x00075C07 File Offset: 0x00073E07
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onUp.Invoke(eventData);
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x00075C2D File Offset: 0x00073E2D
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onEnter.Invoke(eventData);
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x00075C53 File Offset: 0x00073E53
	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onExit.Invoke(eventData);
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x00075C79 File Offset: 0x00073E79
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onSelect.Invoke(eventData);
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x00075C9F File Offset: 0x00073E9F
	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onDeselect.Invoke(eventData);
	}

	// Token: 0x040011D2 RID: 4562
	public ExtendedSelectable.ButtonDownEvent onDown = new ExtendedSelectable.ButtonDownEvent();

	// Token: 0x040011D3 RID: 4563
	public ExtendedSelectable.ButtonUpEvent onUp = new ExtendedSelectable.ButtonUpEvent();

	// Token: 0x040011D4 RID: 4564
	public ExtendedSelectable.ButtonEnterEvent onEnter = new ExtendedSelectable.ButtonEnterEvent();

	// Token: 0x040011D5 RID: 4565
	public ExtendedSelectable.ButtonExitEvent onExit = new ExtendedSelectable.ButtonExitEvent();

	// Token: 0x040011D6 RID: 4566
	public ExtendedSelectable.ButtonSelectEvent onSelect = new ExtendedSelectable.ButtonSelectEvent();

	// Token: 0x040011D7 RID: 4567
	public ExtendedSelectable.ButtonDeselectEvent onDeselect = new ExtendedSelectable.ButtonDeselectEvent();

	// Token: 0x020003DC RID: 988
	[Serializable]
	public class ButtonDownEvent : UnityEvent<PointerEventData>
	{
	}

	// Token: 0x020003DD RID: 989
	[Serializable]
	public class ButtonUpEvent : UnityEvent<PointerEventData>
	{
	}

	// Token: 0x020003DE RID: 990
	[Serializable]
	public class ButtonEnterEvent : UnityEvent<PointerEventData>
	{
	}

	// Token: 0x020003DF RID: 991
	[Serializable]
	public class ButtonExitEvent : UnityEvent<PointerEventData>
	{
	}

	// Token: 0x020003E0 RID: 992
	[Serializable]
	public class ButtonSelectEvent : UnityEvent<BaseEventData>
	{
	}

	// Token: 0x020003E1 RID: 993
	[Serializable]
	public class ButtonDeselectEvent : UnityEvent<BaseEventData>
	{
	}
}
