using System;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine
{
	// Token: 0x020001AE RID: 430
	public static class HashUtilities
	{
		// Token: 0x06001314 RID: 4884 RVA: 0x00019F34 File Offset: 0x00018134
		public unsafe static void AppendHash(ref Hash128 inHash, ref Hash128 outHash)
		{
			fixed (Hash128* ptr = &outHash)
			{
				Hash128* ptr2 = ptr;
				fixed (Hash128* ptr3 = &inHash)
				{
					Hash128* ptr4 = ptr3;
					HashUnsafeUtilities.ComputeHash128((void*)ptr4, (ulong)((long)sizeof(Hash128)), ptr2);
				}
			}
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00019F68 File Offset: 0x00018168
		public unsafe static void QuantisedMatrixHash(ref Matrix4x4 value, ref Hash128 hash)
		{
			fixed (Hash128* ptr = &hash)
			{
				Hash128* ptr2 = ptr;
				int* ptr3 = stackalloc int[(UIntPtr)64];
				for (int i = 0; i < 16; i++)
				{
					ptr3[i] = (int)(value[i] * 1000f + 0.5f);
				}
				HashUnsafeUtilities.ComputeHash128((void*)ptr3, 64UL, ptr2);
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00019FC4 File Offset: 0x000181C4
		public unsafe static void QuantisedVectorHash(ref Vector3 value, ref Hash128 hash)
		{
			fixed (Hash128* ptr = &hash)
			{
				Hash128* ptr2 = ptr;
				int* ptr3 = stackalloc int[(UIntPtr)12];
				for (int i = 0; i < 3; i++)
				{
					ptr3[i] = (int)(value[i] * 1000f + 0.5f);
				}
				HashUnsafeUtilities.ComputeHash128((void*)ptr3, 12UL, ptr2);
			}
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0001A01C File Offset: 0x0001821C
		public unsafe static void ComputeHash128<T>(ref T value, ref Hash128 hash) where T : struct
		{
			void* ptr = UnsafeUtility.AddressOf<T>(ref value);
			ulong num = (ulong)((long)UnsafeUtility.SizeOf<T>());
			Hash128* ptr2 = (Hash128*)UnsafeUtility.AddressOf<Hash128>(ref hash);
			HashUnsafeUtilities.ComputeHash128(ptr, num, ptr2);
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0001A048 File Offset: 0x00018248
		public unsafe static void ComputeHash128(byte[] value, ref Hash128 hash)
		{
			fixed (byte* ptr = &value[0])
			{
				byte* ptr2 = ptr;
				ulong num = (ulong)((long)value.Length);
				Hash128* ptr3 = (Hash128*)UnsafeUtility.AddressOf<Hash128>(ref hash);
				HashUnsafeUtilities.ComputeHash128((void*)ptr2, num, ptr3);
			}
		}
	}
}
