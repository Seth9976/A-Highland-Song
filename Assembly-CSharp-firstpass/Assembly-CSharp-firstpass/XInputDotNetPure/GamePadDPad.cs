using System;

namespace XInputDotNetPure
{
	// Token: 0x02000011 RID: 17
	public struct GamePadDPad
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003D76 File Offset: 0x00001F76
		internal GamePadDPad(ButtonState up, ButtonState down, ButtonState left, ButtonState right)
		{
			this.up = up;
			this.down = down;
			this.left = left;
			this.right = right;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003D95 File Offset: 0x00001F95
		public ButtonState Up
		{
			get
			{
				return this.up;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003D9D File Offset: 0x00001F9D
		public ButtonState Down
		{
			get
			{
				return this.down;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00003DA5 File Offset: 0x00001FA5
		public ButtonState Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003DAD File Offset: 0x00001FAD
		public ButtonState Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x0400007A RID: 122
		private ButtonState up;

		// Token: 0x0400007B RID: 123
		private ButtonState down;

		// Token: 0x0400007C RID: 124
		private ButtonState left;

		// Token: 0x0400007D RID: 125
		private ButtonState right;
	}
}
