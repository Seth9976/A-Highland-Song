using System;

namespace UnityEngine.Sprites
{
	// Token: 0x0200026D RID: 621
	public sealed class DataUtility
	{
		// Token: 0x06001B0D RID: 6925 RVA: 0x0002B64C File Offset: 0x0002984C
		public static Vector4 GetInnerUV(Sprite sprite)
		{
			return sprite.GetInnerUVs();
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0002B664 File Offset: 0x00029864
		public static Vector4 GetOuterUV(Sprite sprite)
		{
			return sprite.GetOuterUVs();
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0002B67C File Offset: 0x0002987C
		public static Vector4 GetPadding(Sprite sprite)
		{
			return sprite.GetPadding();
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x0002B694 File Offset: 0x00029894
		public static Vector2 GetMinSize(Sprite sprite)
		{
			Vector2 vector;
			vector.x = sprite.border.x + sprite.border.z;
			vector.y = sprite.border.y + sprite.border.w;
			return vector;
		}
	}
}
