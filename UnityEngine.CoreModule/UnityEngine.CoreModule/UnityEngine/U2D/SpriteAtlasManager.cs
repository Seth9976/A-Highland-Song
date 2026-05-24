using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.U2D
{
	// Token: 0x02000275 RID: 629
	[NativeHeader("Runtime/2D/SpriteAtlas/SpriteAtlasManager.h")]
	[NativeHeader("Runtime/2D/SpriteAtlas/SpriteAtlas.h")]
	[StaticAccessor("GetSpriteAtlasManager()", StaticAccessorType.Dot)]
	public class SpriteAtlasManager
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06001B4D RID: 6989 RVA: 0x0002BC88 File Offset: 0x00029E88
		// (remove) Token: 0x06001B4E RID: 6990 RVA: 0x0002BCBC File Offset: 0x00029EBC
		[field: DebuggerBrowsable(0)]
		public static event Action<string, Action<SpriteAtlas>> atlasRequested;

		// Token: 0x06001B4F RID: 6991 RVA: 0x0002BCF0 File Offset: 0x00029EF0
		[RequiredByNativeCode]
		private static bool RequestAtlas(string tag)
		{
			bool flag = SpriteAtlasManager.atlasRequested != null;
			bool flag2;
			if (flag)
			{
				SpriteAtlasManager.atlasRequested.Invoke(tag, new Action<SpriteAtlas>(SpriteAtlasManager.Register));
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06001B50 RID: 6992 RVA: 0x0002BD2C File Offset: 0x00029F2C
		// (remove) Token: 0x06001B51 RID: 6993 RVA: 0x0002BD60 File Offset: 0x00029F60
		[field: DebuggerBrowsable(0)]
		public static event Action<SpriteAtlas> atlasRegistered;

		// Token: 0x06001B52 RID: 6994 RVA: 0x0002BD93 File Offset: 0x00029F93
		[RequiredByNativeCode]
		private static void PostRegisteredAtlas(SpriteAtlas spriteAtlas)
		{
			Action<SpriteAtlas> action = SpriteAtlasManager.atlasRegistered;
			if (action != null)
			{
				action.Invoke(spriteAtlas);
			}
		}

		// Token: 0x06001B53 RID: 6995
		[MethodImpl(4096)]
		internal static extern void Register(SpriteAtlas spriteAtlas);
	}
}
