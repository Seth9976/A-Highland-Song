using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001CA RID: 458
public class ExtendedButton : Button
{
	// Token: 0x06000F4C RID: 3916 RVA: 0x00075AA7 File Offset: 0x00073CA7
	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onDown.Invoke(eventData);
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x00075ACD File Offset: 0x00073CCD
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onUp.Invoke(eventData);
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x00075AF3 File Offset: 0x00073CF3
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onEnter.Invoke(eventData);
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x00075B19 File Offset: 0x00073D19
	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onExit.Invoke(eventData);
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x00075B3F File Offset: 0x00073D3F
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onSelect.Invoke(eventData);
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x00075B65 File Offset: 0x00073D65
	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		this.onDeselect.Invoke(eventData);
	}

	// Token: 0x040011CC RID: 4556
	public ExtendedButton.ButtonDownEvent onDown = new ExtendedButton.ButtonDownEvent();

	// Token: 0x040011CD RID: 4557
	public ExtendedButton.ButtonUpEvent onUp = new ExtendedButton.ButtonUpEvent();

	// Token: 0x040011CE RID: 4558
	public ExtendedButton.ButtonEnterEvent onEnter = new ExtendedButton.ButtonEnterEvent();

	// Token: 0x040011CF RID: 4559
	public ExtendedButton.ButtonExitEvent onExit = new ExtendedButton.ButtonExitEvent();

	// Token: 0x040011D0 RID: 4560
	public ExtendedButton.ButtonSelectEvent onSelect = new ExtendedButton.ButtonSelectEvent();

	// Token: 0x040011D1 RID: 4561
	public ExtendedButton.ButtonDeselectEvent onDeselect = new ExtendedButton.ButtonDeselectEvent();

	// Token: 0x020003D6 RID: 982
	[Serializable]
	public class ButtonDownEvent : UnityEvent<PointerEventData>
	{
	}

	// Token: 0x020003D7 RID: 983
	[Serializable]
	public class ButtonUpEvent : UnityEvent<PointerEventData>
	{
	}

	// Token: 0x020003D8 RID: 984
	[Serializable]
	public class ButtonEnterEvent : UnityEvent<PointerEventData>
	{
	}

	// Token: 0x020003D9 RID: 985
	[Serializable]
	public class ButtonExitEvent : UnityEvent<PointerEventData>
	{
	}

	// Token: 0x020003DA RID: 986
	[Serializable]
	public class ButtonSelectEvent : UnityEvent<BaseEventData>
	{
	}

	// Token: 0x020003DB RID: 987
	[Serializable]
	public class ButtonDeselectEvent : UnityEvent<BaseEventData>
	{
	}
}
