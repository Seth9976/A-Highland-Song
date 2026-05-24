using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Windows
{
	// Token: 0x02000288 RID: 648
	[NativeHeader("PlatformDependent/Win/Bindings/InputBindings.h")]
	public static class Input
	{
		// Token: 0x06001C1D RID: 7197
		[NativeName("ForwardRawInput")]
		[StaticAccessor("", StaticAccessorType.DoubleColon)]
		[ThreadSafe]
		[MethodImpl(4096)]
		private unsafe static extern void ForwardRawInputImpl(uint* rawInputHeaderIndices, uint* rawInputDataIndices, uint indicesCount, byte* rawInputData, uint rawInputDataSize);

		// Token: 0x06001C1E RID: 7198 RVA: 0x0002D17D File Offset: 0x0002B37D
		public unsafe static void ForwardRawInput(IntPtr rawInputHeaderIndices, IntPtr rawInputDataIndices, uint indicesCount, IntPtr rawInputData, uint rawInputDataSize)
		{
			Input.ForwardRawInput((uint*)(void*)rawInputHeaderIndices, (uint*)(void*)rawInputDataIndices, indicesCount, (byte*)(void*)rawInputData, rawInputDataSize);
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x0002D1A0 File Offset: 0x0002B3A0
		public unsafe static void ForwardRawInput(uint* rawInputHeaderIndices, uint* rawInputDataIndices, uint indicesCount, byte* rawInputData, uint rawInputDataSize)
		{
			bool flag = rawInputHeaderIndices == null;
			if (flag)
			{
				throw new ArgumentNullException("rawInputHeaderIndices");
			}
			bool flag2 = rawInputDataIndices == null;
			if (flag2)
			{
				throw new ArgumentNullException("rawInputDataIndices");
			}
			bool flag3 = rawInputData == null;
			if (flag3)
			{
				throw new ArgumentNullException("rawInputData");
			}
			Input.ForwardRawInputImpl(rawInputHeaderIndices, rawInputDataIndices, indicesCount, rawInputData, rawInputDataSize);
		}
	}
}
