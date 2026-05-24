using System;

namespace InControl
{
	// Token: 0x02000020 RID: 32
	public class KeyBindingSourceListener : BindingSourceListener
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00004684 File Offset: 0x00002884
		public void Reset()
		{
			this.detectFound.Clear();
			this.detectPhase = 0;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004698 File Offset: 0x00002898
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeKeys)
			{
				return null;
			}
			if (this.detectFound.IncludeCount > 0 && !this.detectFound.IsPressed && this.detectPhase == 2)
			{
				BindingSource bindingSource = new KeyBindingSource(this.detectFound);
				this.Reset();
				return bindingSource;
			}
			KeyCombo keyCombo = KeyCombo.Detect(listenOptions.IncludeModifiersAsFirstClassKeys);
			if (keyCombo.IncludeCount > 0)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = keyCombo;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x04000132 RID: 306
		private KeyCombo detectFound;

		// Token: 0x04000133 RID: 307
		private int detectPhase;
	}
}
