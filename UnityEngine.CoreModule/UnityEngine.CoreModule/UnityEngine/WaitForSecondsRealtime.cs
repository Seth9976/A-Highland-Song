using System;

namespace UnityEngine
{
	// Token: 0x0200022D RID: 557
	public class WaitForSecondsRealtime : CustomYieldInstruction
	{
		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060017E8 RID: 6120 RVA: 0x00026D15 File Offset: 0x00024F15
		// (set) Token: 0x060017E9 RID: 6121 RVA: 0x00026D1D File Offset: 0x00024F1D
		public float waitTime { get; set; }

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060017EA RID: 6122 RVA: 0x00026D28 File Offset: 0x00024F28
		public override bool keepWaiting
		{
			get
			{
				bool flag = this.m_WaitUntilTime < 0f;
				if (flag)
				{
					this.m_WaitUntilTime = Time.realtimeSinceStartup + this.waitTime;
				}
				bool flag2 = Time.realtimeSinceStartup < this.m_WaitUntilTime;
				bool flag3 = !flag2;
				if (flag3)
				{
					this.Reset();
				}
				return flag2;
			}
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00026D7F File Offset: 0x00024F7F
		public WaitForSecondsRealtime(float time)
		{
			this.waitTime = time;
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00026D9C File Offset: 0x00024F9C
		public override void Reset()
		{
			this.m_WaitUntilTime = -1f;
		}

		// Token: 0x04000833 RID: 2099
		private float m_WaitUntilTime = -1f;
	}
}
