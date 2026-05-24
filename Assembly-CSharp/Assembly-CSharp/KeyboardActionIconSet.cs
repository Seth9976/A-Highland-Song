using System;
using InControl;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class KeyboardActionIconSet : ScriptableObject
{
	// Token: 0x06000E85 RID: 3717 RVA: 0x00072538 File Offset: 0x00070738
	public PlatformActionIcon GetPlatformActionIcon(Key key)
	{
		if (key > Key.Control)
		{
			if (key != Key.Return)
			{
				switch (key)
				{
				case Key.LeftArrow:
					return this.leftArrow;
				case Key.RightArrow:
					return this.rightArrow;
				case Key.UpArrow:
					return this.upArrow;
				case Key.DownArrow:
					return this.downArrow;
				default:
					if (key != Key.PadEnter)
					{
						goto IL_0064;
					}
					break;
				}
			}
			return this.enter;
		}
		if (key == Key.Shift)
		{
			return this.shift;
		}
		if (key == Key.Control)
		{
			return this.control;
		}
		IL_0064:
		return null;
	}

	// Token: 0x04001158 RID: 4440
	public PlatformActionIcon upArrow;

	// Token: 0x04001159 RID: 4441
	public PlatformActionIcon downArrow;

	// Token: 0x0400115A RID: 4442
	public PlatformActionIcon leftArrow;

	// Token: 0x0400115B RID: 4443
	public PlatformActionIcon rightArrow;

	// Token: 0x0400115C RID: 4444
	public PlatformActionIcon space;

	// Token: 0x0400115D RID: 4445
	public PlatformActionIcon enter;

	// Token: 0x0400115E RID: 4446
	public PlatformActionIcon shift;

	// Token: 0x0400115F RID: 4447
	public PlatformActionIcon control;
}
