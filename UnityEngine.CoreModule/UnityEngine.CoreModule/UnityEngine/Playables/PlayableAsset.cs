using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x0200043B RID: 1083
	[RequiredByNativeCode]
	[AssetFileNameExtension("playable", new string[] { })]
	[Serializable]
	public abstract class PlayableAsset : ScriptableObject, IPlayableAsset
	{
		// Token: 0x06002585 RID: 9605
		public abstract Playable CreatePlayable(PlayableGraph graph, GameObject owner);

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x0003F410 File Offset: 0x0003D610
		public virtual double duration
		{
			get
			{
				return PlayableBinding.DefaultDuration;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002587 RID: 9607 RVA: 0x0003F428 File Offset: 0x0003D628
		public virtual IEnumerable<PlayableBinding> outputs
		{
			get
			{
				return PlayableBinding.None;
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x0003F440 File Offset: 0x0003D640
		[RequiredByNativeCode]
		internal unsafe static void Internal_CreatePlayable(PlayableAsset asset, PlayableGraph graph, GameObject go, IntPtr ptr)
		{
			bool flag = asset == null;
			Playable playable;
			if (flag)
			{
				playable = Playable.Null;
			}
			else
			{
				playable = asset.CreatePlayable(graph, go);
			}
			Playable* ptr2 = (Playable*)ptr.ToPointer();
			*ptr2 = playable;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x0003F47C File Offset: 0x0003D67C
		[RequiredByNativeCode]
		internal unsafe static void Internal_GetPlayableAssetDuration(PlayableAsset asset, IntPtr ptrToDouble)
		{
			double duration = asset.duration;
			double* ptr = (double*)ptrToDouble.ToPointer();
			*ptr = duration;
		}
	}
}
