using System;

namespace Unity.Collections
{
	// Token: 0x02000098 RID: 152
	internal sealed class NativeArrayReadOnlyDebugView<T> where T : struct
	{
		// Token: 0x060002AB RID: 683 RVA: 0x00005116 File Offset: 0x00003316
		public NativeArrayReadOnlyDebugView(NativeArray<T>.ReadOnly array)
		{
			this.m_Array = array;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00005127 File Offset: 0x00003327
		public T[] Items
		{
			get
			{
				return this.m_Array.ToArray();
			}
		}

		// Token: 0x04000233 RID: 563
		private NativeArray<T>.ReadOnly m_Array;
	}
}
