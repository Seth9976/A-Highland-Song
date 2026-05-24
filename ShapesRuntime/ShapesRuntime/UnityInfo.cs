using System;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x02000065 RID: 101
	internal static class UnityInfo
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x00026937 File Offset: 0x00024B37
		public static bool UsingSRP
		{
			get
			{
				return GraphicsSettings.renderPipelineAsset != null;
			}
		}

		// Token: 0x04000249 RID: 585
		public const int INSTANCES_MAX = 1023;
	}
}
