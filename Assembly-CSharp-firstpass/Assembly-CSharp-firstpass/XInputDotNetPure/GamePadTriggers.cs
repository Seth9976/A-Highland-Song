using System;

namespace XInputDotNetPure
{
	// Token: 0x02000013 RID: 19
	public struct GamePadTriggers
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00003DD5 File Offset: 0x00001FD5
		internal GamePadTriggers(float left, float right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003DE5 File Offset: 0x00001FE5
		public float Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003DED File Offset: 0x00001FED
		public float Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x04000080 RID: 128
		private float left;

		// Token: 0x04000081 RID: 129
		private float right;
	}
}
