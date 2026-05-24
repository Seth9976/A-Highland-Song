using System;

namespace InControl
{
	// Token: 0x0200001D RID: 29
	public class DeviceBindingSourceListener : BindingSourceListener
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x000043D7 File Offset: 0x000025D7
		public void Reset()
		{
			this.detectFound = InputControlType.None;
			this.detectPhase = 0;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000043E8 File Offset: 0x000025E8
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeControllers || device.IsUnknown)
			{
				return null;
			}
			if (this.detectFound != InputControlType.None && !this.IsPressed(this.detectFound, device) && this.detectPhase == 2)
			{
				BindingSource bindingSource = new DeviceBindingSource(this.detectFound);
				this.Reset();
				return bindingSource;
			}
			InputControlType inputControlType = this.ListenForControl(listenOptions, device);
			if (inputControlType != InputControlType.None)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = inputControlType;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000446E File Offset: 0x0000266E
		private bool IsPressed(InputControl control)
		{
			return Utility.AbsoluteIsOverThreshold(control.Value, 0.5f);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004480 File Offset: 0x00002680
		private bool IsPressed(InputControlType control, InputDevice device)
		{
			return this.IsPressed(device.GetControl(control));
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004490 File Offset: 0x00002690
		private InputControlType ListenForControl(BindingListenOptions listenOptions, InputDevice device)
		{
			if (device.IsKnown)
			{
				int count = device.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = device.Controls[i];
					if (inputControl != null && this.IsPressed(inputControl) && (listenOptions.IncludeNonStandardControls || inputControl.IsStandard))
					{
						InputControlType target = inputControl.Target;
						if (target != InputControlType.Command || !listenOptions.IncludeNonStandardControls)
						{
							return target;
						}
					}
				}
			}
			return InputControlType.None;
		}

		// Token: 0x040000AA RID: 170
		private InputControlType detectFound;

		// Token: 0x040000AB RID: 171
		private int detectPhase;
	}
}
