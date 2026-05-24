using System;
using UnityEngine.Playables;

namespace UnityEngine.Experimental.Playables
{
	// Token: 0x0200046A RID: 1130
	public static class TexturePlayableBinding
	{
		// Token: 0x06002806 RID: 10246 RVA: 0x000429FC File Offset: 0x00040BFC
		public static PlayableBinding Create(string name, Object key)
		{
			return PlayableBinding.CreateInternal(name, key, typeof(RenderTexture), new PlayableBinding.CreateOutputMethod(TexturePlayableBinding.CreateTextureOutput));
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x00042A2C File Offset: 0x00040C2C
		private static PlayableOutput CreateTextureOutput(PlayableGraph graph, string name)
		{
			return TexturePlayableOutput.Create(graph, name, null);
		}
	}
}
