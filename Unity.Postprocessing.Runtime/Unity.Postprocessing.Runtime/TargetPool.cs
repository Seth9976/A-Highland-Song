using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000065 RID: 101
	internal class TargetPool
	{
		// Token: 0x06000216 RID: 534 RVA: 0x000102E2 File Offset: 0x0000E4E2
		internal TargetPool()
		{
			this.m_Pool = new List<int>();
			this.Get();
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000102FC File Offset: 0x0000E4FC
		internal int Get()
		{
			int num = this.Get(this.m_Current);
			this.m_Current++;
			return num;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00010318 File Offset: 0x0000E518
		private int Get(int i)
		{
			int num;
			if (this.m_Pool.Count > i)
			{
				num = this.m_Pool[i];
			}
			else
			{
				while (this.m_Pool.Count <= i)
				{
					this.m_Pool.Add(Shader.PropertyToID("_TargetPool" + i.ToString()));
				}
				num = this.m_Pool[i];
			}
			return num;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0001037F File Offset: 0x0000E57F
		internal void Reset()
		{
			this.m_Current = 0;
		}

		// Token: 0x0400023D RID: 573
		private readonly List<int> m_Pool;

		// Token: 0x0400023E RID: 574
		private int m_Current;
	}
}
