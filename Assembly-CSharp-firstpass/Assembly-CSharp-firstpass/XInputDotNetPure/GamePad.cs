using System;
using System.Runtime.InteropServices;

namespace XInputDotNetPure
{
	// Token: 0x02000016 RID: 22
	public class GamePad
	{
		// Token: 0x06000074 RID: 116 RVA: 0x0000407C File Offset: 0x0000227C
		public static GamePadState GetState(PlayerIndex playerIndex)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GamePadState.RawState)));
			int num = (int)Imports.XInputGamePadGetState((uint)playerIndex, intPtr);
			GamePadState.RawState rawState = (GamePadState.RawState)Marshal.PtrToStructure(intPtr, typeof(GamePadState.RawState));
			return new GamePadState(num == 0, rawState);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000040C4 File Offset: 0x000022C4
		public static void SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
		{
			Imports.XInputGamePadSetState((uint)playerIndex, leftMotor, rightMotor);
		}
	}
}
