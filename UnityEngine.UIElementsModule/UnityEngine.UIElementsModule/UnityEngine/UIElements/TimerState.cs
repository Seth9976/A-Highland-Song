using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000065 RID: 101
	public struct TimerState : IEquatable<TimerState>
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000AA1A File Offset: 0x00008C1A
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000AA22 File Offset: 0x00008C22
		public long start { readonly get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000AA2B File Offset: 0x00008C2B
		// (set) Token: 0x060002DC RID: 732 RVA: 0x0000AA33 File Offset: 0x00008C33
		public long now { readonly get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000AA3C File Offset: 0x00008C3C
		public long deltaTime
		{
			get
			{
				return this.now - this.start;
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000AA5C File Offset: 0x00008C5C
		public override bool Equals(object obj)
		{
			return obj is TimerState && this.Equals((TimerState)obj);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000AA88 File Offset: 0x00008C88
		public bool Equals(TimerState other)
		{
			return this.start == other.start && this.now == other.now && this.deltaTime == other.deltaTime;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000AACC File Offset: 0x00008CCC
		public override int GetHashCode()
		{
			int num = 540054806;
			num = num * -1521134295 + this.start.GetHashCode();
			num = num * -1521134295 + this.now.GetHashCode();
			return num * -1521134295 + this.deltaTime.GetHashCode();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000AB2C File Offset: 0x00008D2C
		public static bool operator ==(TimerState state1, TimerState state2)
		{
			return state1.Equals(state2);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000AB48 File Offset: 0x00008D48
		public static bool operator !=(TimerState state1, TimerState state2)
		{
			return !(state1 == state2);
		}
	}
}
