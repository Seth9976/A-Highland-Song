using System;

namespace InControl
{
	// Token: 0x02000035 RID: 53
	public struct InputControlState
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x000076FC File Offset: 0x000058FC
		public void Reset()
		{
			this.State = false;
			this.Value = 0f;
			this.RawValue = 0f;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000771B File Offset: 0x0000591B
		public void Set(float value)
		{
			this.Value = value;
			this.State = Utility.IsNotZero(value);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00007730 File Offset: 0x00005930
		public void Set(float value, float threshold)
		{
			this.Value = value;
			this.State = Utility.AbsoluteIsOverThreshold(value, threshold);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00007746 File Offset: 0x00005946
		public void Set(bool state)
		{
			this.State = state;
			this.Value = (state ? 1f : 0f);
			this.RawValue = this.Value;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00007770 File Offset: 0x00005970
		public static implicit operator bool(InputControlState state)
		{
			return state.State;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00007778 File Offset: 0x00005978
		public static implicit operator float(InputControlState state)
		{
			return state.Value;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00007780 File Offset: 0x00005980
		public static bool operator ==(InputControlState a, InputControlState b)
		{
			return a.State == b.State && Utility.Approximately(a.Value, b.Value);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000077A3 File Offset: 0x000059A3
		public static bool operator !=(InputControlState a, InputControlState b)
		{
			return a.State != b.State || !Utility.Approximately(a.Value, b.Value);
		}

		// Token: 0x040001C5 RID: 453
		public bool State;

		// Token: 0x040001C6 RID: 454
		public float Value;

		// Token: 0x040001C7 RID: 455
		public float RawValue;
	}
}
