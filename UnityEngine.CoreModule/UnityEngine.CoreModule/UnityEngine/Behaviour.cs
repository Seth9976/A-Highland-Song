using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001FA RID: 506
	[NativeHeader("Runtime/Mono/MonoBehaviour.h")]
	[UsedByNativeCode]
	public class Behaviour : Component
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001666 RID: 5734
		// (set) Token: 0x06001667 RID: 5735
		[NativeProperty]
		[RequiredByNativeCode]
		public extern bool enabled
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001668 RID: 5736
		[NativeProperty]
		public extern bool isActiveAndEnabled
		{
			[NativeMethod("IsAddedToManager")]
			[MethodImpl(4096)]
			get;
		}
	}
}
