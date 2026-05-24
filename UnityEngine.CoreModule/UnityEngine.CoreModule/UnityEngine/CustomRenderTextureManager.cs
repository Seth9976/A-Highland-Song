using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000122 RID: 290
	[NativeHeader("Runtime/Graphics/CustomRenderTextureManager.h")]
	public static class CustomRenderTextureManager
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060007FA RID: 2042 RVA: 0x0000BF28 File Offset: 0x0000A128
		// (remove) Token: 0x060007FB RID: 2043 RVA: 0x0000BF5C File Offset: 0x0000A15C
		[field: DebuggerBrowsable(0)]
		public static event Action<CustomRenderTexture> textureLoaded;

		// Token: 0x060007FC RID: 2044 RVA: 0x0000BF8F File Offset: 0x0000A18F
		[RequiredByNativeCode]
		private static void InvokeOnTextureLoaded_Internal(CustomRenderTexture source)
		{
			Action<CustomRenderTexture> action = CustomRenderTextureManager.textureLoaded;
			if (action != null)
			{
				action.Invoke(source);
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060007FD RID: 2045 RVA: 0x0000BFA4 File Offset: 0x0000A1A4
		// (remove) Token: 0x060007FE RID: 2046 RVA: 0x0000BFD8 File Offset: 0x0000A1D8
		[field: DebuggerBrowsable(0)]
		public static event Action<CustomRenderTexture> textureUnloaded;

		// Token: 0x060007FF RID: 2047 RVA: 0x0000C00B File Offset: 0x0000A20B
		[RequiredByNativeCode]
		private static void InvokeOnTextureUnloaded_Internal(CustomRenderTexture source)
		{
			Action<CustomRenderTexture> action = CustomRenderTextureManager.textureUnloaded;
			if (action != null)
			{
				action.Invoke(source);
			}
		}

		// Token: 0x06000800 RID: 2048
		[FreeFunction(Name = "CustomRenderTextureManagerScripting::GetAllCustomRenderTextures", HasExplicitThis = false)]
		[MethodImpl(4096)]
		public static extern void GetAllCustomRenderTextures(List<CustomRenderTexture> currentCustomRenderTextures);

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000801 RID: 2049 RVA: 0x0000C020 File Offset: 0x0000A220
		// (remove) Token: 0x06000802 RID: 2050 RVA: 0x0000C054 File Offset: 0x0000A254
		[field: DebuggerBrowsable(0)]
		public static event Action<CustomRenderTexture, int> updateTriggered;

		// Token: 0x06000803 RID: 2051 RVA: 0x0000C087 File Offset: 0x0000A287
		internal static void InvokeTriggerUpdate(CustomRenderTexture crt, int updateCount)
		{
			Action<CustomRenderTexture, int> action = CustomRenderTextureManager.updateTriggered;
			if (action != null)
			{
				action.Invoke(crt, updateCount);
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000804 RID: 2052 RVA: 0x0000C09C File Offset: 0x0000A29C
		// (remove) Token: 0x06000805 RID: 2053 RVA: 0x0000C0D0 File Offset: 0x0000A2D0
		[field: DebuggerBrowsable(0)]
		public static event Action<CustomRenderTexture> initializeTriggered;

		// Token: 0x06000806 RID: 2054 RVA: 0x0000C103 File Offset: 0x0000A303
		internal static void InvokeTriggerInitialize(CustomRenderTexture crt)
		{
			Action<CustomRenderTexture> action = CustomRenderTextureManager.initializeTriggered;
			if (action != null)
			{
				action.Invoke(crt);
			}
		}
	}
}
