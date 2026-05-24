using System;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000138 RID: 312
	public struct RenderTargetSetup
	{
		// Token: 0x060009E7 RID: 2535 RVA: 0x0000EE38 File Offset: 0x0000D038
		public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face, RenderBufferLoadAction[] colorLoad, RenderBufferStoreAction[] colorStore, RenderBufferLoadAction depthLoad, RenderBufferStoreAction depthStore)
		{
			this.color = color;
			this.depth = depth;
			this.mipLevel = mip;
			this.cubemapFace = face;
			this.depthSlice = 0;
			this.colorLoad = colorLoad;
			this.colorStore = colorStore;
			this.depthLoad = depthLoad;
			this.depthStore = depthStore;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0000EE8C File Offset: 0x0000D08C
		internal static RenderBufferLoadAction[] LoadActions(RenderBuffer[] buf)
		{
			RenderBufferLoadAction[] array = new RenderBufferLoadAction[buf.Length];
			for (int i = 0; i < buf.Length; i++)
			{
				array[i] = buf[i].loadAction;
				buf[i].loadAction = RenderBufferLoadAction.Load;
			}
			return array;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0000EEDC File Offset: 0x0000D0DC
		internal static RenderBufferStoreAction[] StoreActions(RenderBuffer[] buf)
		{
			RenderBufferStoreAction[] array = new RenderBufferStoreAction[buf.Length];
			for (int i = 0; i < buf.Length; i++)
			{
				array[i] = buf[i].storeAction;
				buf[i].storeAction = RenderBufferStoreAction.Store;
			}
			return array;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0000EF29 File Offset: 0x0000D129
		public RenderTargetSetup(RenderBuffer color, RenderBuffer depth)
		{
			this = new RenderTargetSetup(new RenderBuffer[] { color }, depth);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0000EF42 File Offset: 0x0000D142
		public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel)
		{
			this = new RenderTargetSetup(new RenderBuffer[] { color }, depth, mipLevel);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0000EF5C File Offset: 0x0000D15C
		public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel, CubemapFace face)
		{
			this = new RenderTargetSetup(new RenderBuffer[] { color }, depth, mipLevel, face);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0000EF78 File Offset: 0x0000D178
		public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel, CubemapFace face, int depthSlice)
		{
			this = new RenderTargetSetup(new RenderBuffer[] { color }, depth, mipLevel, face);
			this.depthSlice = depthSlice;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0000EF9C File Offset: 0x0000D19C
		public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth)
		{
			this = new RenderTargetSetup(color, depth, 0, CubemapFace.Unknown);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0000EFAA File Offset: 0x0000D1AA
		public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mipLevel)
		{
			this = new RenderTargetSetup(color, depth, mipLevel, CubemapFace.Unknown);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face)
		{
			this = new RenderTargetSetup(color, depth, mip, face, RenderTargetSetup.LoadActions(color), RenderTargetSetup.StoreActions(color), depth.loadAction, depth.storeAction);
		}

		// Token: 0x040003E6 RID: 998
		public RenderBuffer[] color;

		// Token: 0x040003E7 RID: 999
		public RenderBuffer depth;

		// Token: 0x040003E8 RID: 1000
		public int mipLevel;

		// Token: 0x040003E9 RID: 1001
		public CubemapFace cubemapFace;

		// Token: 0x040003EA RID: 1002
		public int depthSlice;

		// Token: 0x040003EB RID: 1003
		public RenderBufferLoadAction[] colorLoad;

		// Token: 0x040003EC RID: 1004
		public RenderBufferStoreAction[] colorStore;

		// Token: 0x040003ED RID: 1005
		public RenderBufferLoadAction depthLoad;

		// Token: 0x040003EE RID: 1006
		public RenderBufferStoreAction depthStore;
	}
}
