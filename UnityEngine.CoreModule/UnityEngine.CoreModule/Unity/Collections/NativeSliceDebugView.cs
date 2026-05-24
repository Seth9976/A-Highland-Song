using System;

namespace Unity.Collections
{
	// Token: 0x0200009C RID: 156
	internal sealed class NativeSliceDebugView<T> where T : struct
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x00005679 File Offset: 0x00003879
		public NativeSliceDebugView(NativeSlice<T> array)
		{
			this.m_Array = array;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000568C File Offset: 0x0000388C
		public T[] Items
		{
			get
			{
				return this.m_Array.ToArray();
			}
		}

		// Token: 0x04000239 RID: 569
		private NativeSlice<T> m_Array;
	}
}
