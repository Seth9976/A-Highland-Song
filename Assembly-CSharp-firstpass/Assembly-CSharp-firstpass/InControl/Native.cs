using System;
using System.Runtime.InteropServices;

namespace InControl
{
	// Token: 0x02000051 RID: 81
	internal static class Native
	{
		// Token: 0x060003C4 RID: 964
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_Init")]
		public static extern void Init(NativeInputOptions options);

		// Token: 0x060003C5 RID: 965
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_Stop")]
		public static extern void Stop();

		// Token: 0x060003C6 RID: 966
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_GetVersionInfo")]
		public static extern void GetVersionInfo(out NativeVersionInfo versionInfo);

		// Token: 0x060003C7 RID: 967
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_GetDeviceInfo")]
		public static extern bool GetDeviceInfo(uint handle, out InputDeviceInfo deviceInfo);

		// Token: 0x060003C8 RID: 968
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_GetDeviceState")]
		public static extern bool GetDeviceState(uint handle, out IntPtr deviceState);

		// Token: 0x060003C9 RID: 969
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_GetDeviceEvents")]
		public static extern int GetDeviceEvents(out IntPtr deviceEvents);

		// Token: 0x060003CA RID: 970
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_SetHapticState")]
		public static extern void SetHapticState(uint handle, byte lowFrequency, byte highFrequency);

		// Token: 0x060003CB RID: 971
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_SetTriggersHapticState")]
		public static extern void SetTriggersHapticState(uint handle, byte leftTrigger, byte rightTrigger);

		// Token: 0x060003CC RID: 972
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_SetLightColor")]
		public static extern void SetLightColor(uint handle, byte red, byte green, byte blue);

		// Token: 0x060003CD RID: 973
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_SetLightFlash")]
		public static extern void SetLightFlash(uint handle, byte flashOnDuration, byte flashOffDuration);

		// Token: 0x060003CE RID: 974
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_GetAnalogGlyphName")]
		public static extern uint GetAnalogGlyphName(uint handle, uint index, out IntPtr glyphName);

		// Token: 0x060003CF RID: 975
		[DllImport("InControlNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InControl_GetButtonGlyphName")]
		public static extern uint GetButtonGlyphName(uint handle, uint index, out IntPtr glyphName);

		// Token: 0x0400038A RID: 906
		private const string libraryName = "InControlNative";

		// Token: 0x0400038B RID: 907
		private const CallingConvention callingConvention = CallingConvention.Cdecl;
	}
}
