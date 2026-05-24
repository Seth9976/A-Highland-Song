using System;

namespace XInputDotNetPure
{
	// Token: 0x02000010 RID: 16
	public struct GamePadButtons
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00003CCC File Offset: 0x00001ECC
		internal GamePadButtons(ButtonState start, ButtonState back, ButtonState leftStick, ButtonState rightStick, ButtonState leftShoulder, ButtonState rightShoulder, ButtonState a, ButtonState b, ButtonState x, ButtonState y)
		{
			this.start = start;
			this.back = back;
			this.leftStick = leftStick;
			this.rightStick = rightStick;
			this.leftShoulder = leftShoulder;
			this.rightShoulder = rightShoulder;
			this.a = a;
			this.b = b;
			this.x = x;
			this.y = y;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003D26 File Offset: 0x00001F26
		public ButtonState Start
		{
			get
			{
				return this.start;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003D2E File Offset: 0x00001F2E
		public ButtonState Back
		{
			get
			{
				return this.back;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003D36 File Offset: 0x00001F36
		public ButtonState LeftStick
		{
			get
			{
				return this.leftStick;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003D3E File Offset: 0x00001F3E
		public ButtonState RightStick
		{
			get
			{
				return this.rightStick;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00003D46 File Offset: 0x00001F46
		public ButtonState LeftShoulder
		{
			get
			{
				return this.leftShoulder;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003D4E File Offset: 0x00001F4E
		public ButtonState RightShoulder
		{
			get
			{
				return this.rightShoulder;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003D56 File Offset: 0x00001F56
		public ButtonState A
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003D5E File Offset: 0x00001F5E
		public ButtonState B
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003D66 File Offset: 0x00001F66
		public ButtonState X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003D6E File Offset: 0x00001F6E
		public ButtonState Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x04000070 RID: 112
		private ButtonState start;

		// Token: 0x04000071 RID: 113
		private ButtonState back;

		// Token: 0x04000072 RID: 114
		private ButtonState leftStick;

		// Token: 0x04000073 RID: 115
		private ButtonState rightStick;

		// Token: 0x04000074 RID: 116
		private ButtonState leftShoulder;

		// Token: 0x04000075 RID: 117
		private ButtonState rightShoulder;

		// Token: 0x04000076 RID: 118
		private ButtonState a;

		// Token: 0x04000077 RID: 119
		private ButtonState b;

		// Token: 0x04000078 RID: 120
		private ButtonState x;

		// Token: 0x04000079 RID: 121
		private ButtonState y;
	}
}
