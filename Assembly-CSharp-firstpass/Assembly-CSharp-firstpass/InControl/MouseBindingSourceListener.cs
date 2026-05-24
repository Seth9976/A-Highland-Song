using System;

namespace InControl
{
	// Token: 0x02000024 RID: 36
	public class MouseBindingSourceListener : BindingSourceListener
	{
		// Token: 0x060000EB RID: 235 RVA: 0x00004EDA File Offset: 0x000030DA
		public void Reset()
		{
			this.detectFound = Mouse.None;
			this.detectPhase = 0;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004EEC File Offset: 0x000030EC
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (this.detectFound != Mouse.None && !this.IsPressed(this.detectFound) && this.detectPhase == 2)
			{
				BindingSource bindingSource = new MouseBindingSource(this.detectFound);
				this.Reset();
				return bindingSource;
			}
			Mouse mouse = this.ListenForControl(listenOptions);
			if (mouse != Mouse.None)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = mouse;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004F5E File Offset: 0x0000315E
		private bool IsPressed(Mouse control)
		{
			if (control == Mouse.PositiveScrollWheel)
			{
				return MouseBindingSource.PositiveScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold);
			}
			if (control == Mouse.NegativeScrollWheel)
			{
				return MouseBindingSource.NegativeScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold);
			}
			return MouseBindingSource.ButtonIsPressed(control);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004F88 File Offset: 0x00003188
		private Mouse ListenForControl(BindingListenOptions listenOptions)
		{
			if (listenOptions.IncludeMouseButtons)
			{
				for (Mouse mouse = Mouse.None; mouse <= Mouse.Button7; mouse++)
				{
					if (MouseBindingSource.ButtonIsPressed(mouse))
					{
						return mouse;
					}
				}
			}
			if (listenOptions.IncludeMouseScrollWheel)
			{
				if (MouseBindingSource.NegativeScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold))
				{
					return Mouse.NegativeScrollWheel;
				}
				if (MouseBindingSource.PositiveScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold))
				{
					return Mouse.PositiveScrollWheel;
				}
			}
			return Mouse.None;
		}

		// Token: 0x04000151 RID: 337
		public static float ScrollWheelThreshold = 0.001f;

		// Token: 0x04000152 RID: 338
		private Mouse detectFound;

		// Token: 0x04000153 RID: 339
		private int detectPhase;
	}
}
