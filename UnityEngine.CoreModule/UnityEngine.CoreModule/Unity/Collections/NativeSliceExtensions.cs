using System;

namespace Unity.Collections
{
	// Token: 0x02000099 RID: 153
	public static class NativeSliceExtensions
	{
		// Token: 0x060002AD RID: 685 RVA: 0x00005134 File Offset: 0x00003334
		public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray) where T : struct
		{
			return new NativeSlice<T>(thisArray);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000514C File Offset: 0x0000334C
		public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray, int start) where T : struct
		{
			return new NativeSlice<T>(thisArray, start);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00005168 File Offset: 0x00003368
		public static NativeSlice<T> Slice<T>(this NativeArray<T> thisArray, int start, int length) where T : struct
		{
			return new NativeSlice<T>(thisArray, start, length);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00005184 File Offset: 0x00003384
		public static NativeSlice<T> Slice<T>(this NativeSlice<T> thisSlice) where T : struct
		{
			return thisSlice;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00005198 File Offset: 0x00003398
		public static NativeSlice<T> Slice<T>(this NativeSlice<T> thisSlice, int start) where T : struct
		{
			return new NativeSlice<T>(thisSlice, start);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000051B4 File Offset: 0x000033B4
		public static NativeSlice<T> Slice<T>(this NativeSlice<T> thisSlice, int start, int length) where T : struct
		{
			return new NativeSlice<T>(thisSlice, start, length);
		}
	}
}
