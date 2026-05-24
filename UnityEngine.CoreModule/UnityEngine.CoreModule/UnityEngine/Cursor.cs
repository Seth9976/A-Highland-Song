using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020001B6 RID: 438
	[NativeHeader("Runtime/Export/Input/Cursor.bindings.h")]
	public class Cursor
	{
		// Token: 0x06001330 RID: 4912 RVA: 0x0001AD9F File Offset: 0x00018F9F
		private static void SetCursor(Texture2D texture, CursorMode cursorMode)
		{
			Cursor.SetCursor(texture, Vector2.zero, cursorMode);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0001ADAF File Offset: 0x00018FAF
		public static void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
		{
			Cursor.SetCursor_Injected(texture, ref hotspot, cursorMode);
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001332 RID: 4914
		// (set) Token: 0x06001333 RID: 4915
		public static extern bool visible
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001334 RID: 4916
		// (set) Token: 0x06001335 RID: 4917
		public static extern CursorLockMode lockState
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06001337 RID: 4919
		[MethodImpl(4096)]
		private static extern void SetCursor_Injected(Texture2D texture, ref Vector2 hotspot, CursorMode cursorMode);
	}
}
