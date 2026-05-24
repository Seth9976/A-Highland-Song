using System;

namespace XInputDotNetPure
{
	// Token: 0x02000014 RID: 20
	public struct GamePadState
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00003DF8 File Offset: 0x00001FF8
		internal GamePadState(bool isConnected, GamePadState.RawState rawState)
		{
			this.isConnected = isConnected;
			if (!isConnected)
			{
				rawState.dwPacketNumber = 0U;
				rawState.Gamepad.dwButtons = 0;
				rawState.Gamepad.bLeftTrigger = 0;
				rawState.Gamepad.bRightTrigger = 0;
				rawState.Gamepad.sThumbLX = 0;
				rawState.Gamepad.sThumbLY = 0;
				rawState.Gamepad.sThumbRX = 0;
				rawState.Gamepad.sThumbRY = 0;
			}
			this.packetNumber = rawState.dwPacketNumber;
			this.buttons = new GamePadButtons(((rawState.Gamepad.dwButtons & 16) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 32) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 64) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 128) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 256) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 512) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 4096) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 8192) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 16384) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 32768) != 0) ? ButtonState.Pressed : ButtonState.Released);
			this.dPad = new GamePadDPad(((rawState.Gamepad.dwButtons & 1) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 2) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 4) != 0) ? ButtonState.Pressed : ButtonState.Released, ((rawState.Gamepad.dwButtons & 8) != 0) ? ButtonState.Pressed : ButtonState.Released);
			this.thumbSticks = new GamePadThumbSticks(new GamePadThumbSticks.StickValue((float)rawState.Gamepad.sThumbLX / 32767f, (float)rawState.Gamepad.sThumbLY / 32767f), new GamePadThumbSticks.StickValue((float)rawState.Gamepad.sThumbRX / 32767f, (float)rawState.Gamepad.sThumbRY / 32767f));
			this.triggers = new GamePadTriggers((float)rawState.Gamepad.bLeftTrigger / 255f, (float)rawState.Gamepad.bRightTrigger / 255f);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00004049 File Offset: 0x00002249
		public uint PacketNumber
		{
			get
			{
				return this.packetNumber;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00004051 File Offset: 0x00002251
		public bool IsConnected
		{
			get
			{
				return this.isConnected;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00004059 File Offset: 0x00002259
		public GamePadButtons Buttons
		{
			get
			{
				return this.buttons;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00004061 File Offset: 0x00002261
		public GamePadDPad DPad
		{
			get
			{
				return this.dPad;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00004069 File Offset: 0x00002269
		public GamePadTriggers Triggers
		{
			get
			{
				return this.triggers;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00004071 File Offset: 0x00002271
		public GamePadThumbSticks ThumbSticks
		{
			get
			{
				return this.thumbSticks;
			}
		}

		// Token: 0x04000082 RID: 130
		private bool isConnected;

		// Token: 0x04000083 RID: 131
		private uint packetNumber;

		// Token: 0x04000084 RID: 132
		private GamePadButtons buttons;

		// Token: 0x04000085 RID: 133
		private GamePadDPad dPad;

		// Token: 0x04000086 RID: 134
		private GamePadThumbSticks thumbSticks;

		// Token: 0x04000087 RID: 135
		private GamePadTriggers triggers;

		// Token: 0x02000217 RID: 535
		internal struct RawState
		{
			// Token: 0x040004A8 RID: 1192
			public uint dwPacketNumber;

			// Token: 0x040004A9 RID: 1193
			public GamePadState.RawState.GamePad Gamepad;

			// Token: 0x0200022B RID: 555
			public struct GamePad
			{
				// Token: 0x04000536 RID: 1334
				public ushort dwButtons;

				// Token: 0x04000537 RID: 1335
				public byte bLeftTrigger;

				// Token: 0x04000538 RID: 1336
				public byte bRightTrigger;

				// Token: 0x04000539 RID: 1337
				public short sThumbLX;

				// Token: 0x0400053A RID: 1338
				public short sThumbLY;

				// Token: 0x0400053B RID: 1339
				public short sThumbRX;

				// Token: 0x0400053C RID: 1340
				public short sThumbRY;
			}
		}

		// Token: 0x02000218 RID: 536
		private enum ButtonsConstants
		{
			// Token: 0x040004AB RID: 1195
			DPadUp = 1,
			// Token: 0x040004AC RID: 1196
			DPadDown,
			// Token: 0x040004AD RID: 1197
			DPadLeft = 4,
			// Token: 0x040004AE RID: 1198
			DPadRight = 8,
			// Token: 0x040004AF RID: 1199
			Start = 16,
			// Token: 0x040004B0 RID: 1200
			Back = 32,
			// Token: 0x040004B1 RID: 1201
			LeftThumb = 64,
			// Token: 0x040004B2 RID: 1202
			RightThumb = 128,
			// Token: 0x040004B3 RID: 1203
			LeftShoulder = 256,
			// Token: 0x040004B4 RID: 1204
			RightShoulder = 512,
			// Token: 0x040004B5 RID: 1205
			A = 4096,
			// Token: 0x040004B6 RID: 1206
			B = 8192,
			// Token: 0x040004B7 RID: 1207
			X = 16384,
			// Token: 0x040004B8 RID: 1208
			Y = 32768
		}
	}
}
