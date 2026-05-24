using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	// Token: 0x0200013A RID: 314
	internal readonly struct RenderInstancedDataLayout
	{
		// Token: 0x06000A0C RID: 2572 RVA: 0x0000F154 File Offset: 0x0000D354
		public RenderInstancedDataLayout(Type t)
		{
			this.size = Marshal.SizeOf(t);
			this.offsetObjectToWorld = ((t == typeof(Matrix4x4)) ? 0 : Marshal.OffsetOf(t, "objectToWorld").ToInt32());
			try
			{
				this.offsetPrevObjectToWorld = Marshal.OffsetOf(t, "prevObjectToWorld").ToInt32();
			}
			catch (ArgumentException)
			{
				this.offsetPrevObjectToWorld = -1;
			}
			try
			{
				this.offsetRenderingLayerMask = Marshal.OffsetOf(t, "renderingLayerMask").ToInt32();
			}
			catch (ArgumentException)
			{
				this.offsetRenderingLayerMask = -1;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0000F208 File Offset: 0x0000D408
		public int size { get; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0000F210 File Offset: 0x0000D410
		public int offsetObjectToWorld { get; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0000F218 File Offset: 0x0000D418
		public int offsetPrevObjectToWorld { get; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0000F220 File Offset: 0x0000D420
		public int offsetRenderingLayerMask { get; }
	}
}
