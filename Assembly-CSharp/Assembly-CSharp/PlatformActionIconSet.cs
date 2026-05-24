using System;
using InControl;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class PlatformActionIconSet : ScriptableObject
{
	// Token: 0x06000E89 RID: 3721 RVA: 0x000725D8 File Offset: 0x000707D8
	public PlatformActionIcon GetPlatformActionIcon(InputControlType controlType)
	{
		switch (controlType)
		{
		case InputControlType.DPadUp:
			return this.dPadUp;
		case InputControlType.DPadDown:
			return this.dPadDown;
		case InputControlType.DPadLeft:
			return this.dPadLeft;
		case InputControlType.DPadRight:
			return this.dPadRight;
		case InputControlType.LeftTrigger:
			return this.leftTrigger;
		case InputControlType.RightTrigger:
			return this.rightTrigger;
		case InputControlType.LeftBumper:
			return this.leftBumper;
		case InputControlType.RightBumper:
			return this.rightBumper;
		case InputControlType.Action1:
			return this.action1;
		case InputControlType.Action2:
			return this.action2;
		case InputControlType.Action3:
			return this.action3;
		case InputControlType.Action4:
			return this.action4;
		default:
			switch (controlType)
			{
			case InputControlType.Start:
			case InputControlType.Options:
			case InputControlType.Pause:
			case InputControlType.Menu:
			case InputControlType.Plus:
				return this.start;
			case InputControlType.Select:
			case InputControlType.Share:
			case InputControlType.View:
			case InputControlType.Minus:
				return this.select;
			}
			return null;
		}
	}

	// Token: 0x04001166 RID: 4454
	public PlatformActionIcon action1;

	// Token: 0x04001167 RID: 4455
	public PlatformActionIcon action2;

	// Token: 0x04001168 RID: 4456
	public PlatformActionIcon action3;

	// Token: 0x04001169 RID: 4457
	public PlatformActionIcon action4;

	// Token: 0x0400116A RID: 4458
	public PlatformActionIcon dPadUp;

	// Token: 0x0400116B RID: 4459
	public PlatformActionIcon dPadDown;

	// Token: 0x0400116C RID: 4460
	public PlatformActionIcon dPadLeft;

	// Token: 0x0400116D RID: 4461
	public PlatformActionIcon dPadRight;

	// Token: 0x0400116E RID: 4462
	public PlatformActionIcon leftTrigger;

	// Token: 0x0400116F RID: 4463
	public PlatformActionIcon leftBumper;

	// Token: 0x04001170 RID: 4464
	public PlatformActionIcon rightTrigger;

	// Token: 0x04001171 RID: 4465
	public PlatformActionIcon rightBumper;

	// Token: 0x04001172 RID: 4466
	[Info("Maps to TouchPadButton on PS, View on Xbox and Minus on Switch")]
	public PlatformActionIcon select;

	// Token: 0x04001173 RID: 4467
	[Info("Maps to Options on PS, Menu on Xbox and Plus on Switch")]
	public PlatformActionIcon start;

	// Token: 0x04001174 RID: 4468
	[Space]
	public Sprite touchPad;

	// Token: 0x04001175 RID: 4469
	[Space]
	public Sprite switchMinus;

	// Token: 0x04001176 RID: 4470
	public Sprite switchPlus;

	// Token: 0x04001177 RID: 4471
	[Space]
	public Sprite dpadLeftRight;

	// Token: 0x04001178 RID: 4472
	public Sprite dpadLeft;

	// Token: 0x04001179 RID: 4473
	public Sprite dpadRight;

	// Token: 0x0400117A RID: 4474
	public Sprite dpadUp;

	// Token: 0x0400117B RID: 4475
	public Sprite dpadDown;
}
