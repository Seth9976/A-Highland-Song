using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000439 RID: 1081
	[RequiredByNativeCode]
	public struct Playable : IPlayable, IEquatable<Playable>
	{
		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x0003F334 File Offset: 0x0003D534
		public static Playable Null
		{
			get
			{
				return Playable.m_NullPlayable;
			}
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x0003F34C File Offset: 0x0003D54C
		public static Playable Create(PlayableGraph graph, int inputCount = 0)
		{
			Playable playable = new Playable(graph.CreatePlayableHandle());
			playable.SetInputCount(inputCount);
			return playable;
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x0003F375 File Offset: 0x0003D575
		[VisibleToOtherModules]
		internal Playable(PlayableHandle handle)
		{
			this.m_Handle = handle;
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x0003F380 File Offset: 0x0003D580
		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x0003F398 File Offset: 0x0003D598
		public bool IsPlayableOfType<T>() where T : struct, IPlayable
		{
			return this.GetHandle().IsPlayableOfType<T>();
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x0003F3B8 File Offset: 0x0003D5B8
		public Type GetPlayableType()
		{
			return this.GetHandle().GetPlayableType();
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x0003F3D8 File Offset: 0x0003D5D8
		public bool Equals(Playable other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x04000E0A RID: 3594
		private PlayableHandle m_Handle;

		// Token: 0x04000E0B RID: 3595
		private static readonly Playable m_NullPlayable = new Playable(PlayableHandle.Null);
	}
}
