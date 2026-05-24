using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000446 RID: 1094
	[RequiredByNativeCode]
	public struct PlayableOutput : IPlayableOutput, IEquatable<PlayableOutput>
	{
		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002683 RID: 9859 RVA: 0x00040428 File Offset: 0x0003E628
		public static PlayableOutput Null
		{
			get
			{
				return PlayableOutput.m_NullPlayableOutput;
			}
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0004043F File Offset: 0x0003E63F
		[VisibleToOtherModules]
		internal PlayableOutput(PlayableOutputHandle handle)
		{
			this.m_Handle = handle;
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x0004044C File Offset: 0x0003E64C
		public PlayableOutputHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x00040464 File Offset: 0x0003E664
		public bool IsPlayableOutputOfType<T>() where T : struct, IPlayableOutput
		{
			return this.GetHandle().IsPlayableOutputOfType<T>();
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x00040484 File Offset: 0x0003E684
		public Type GetPlayableOutputType()
		{
			return this.GetHandle().GetPlayableOutputType();
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x000404A4 File Offset: 0x0003E6A4
		public bool Equals(PlayableOutput other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x04000E28 RID: 3624
		private PlayableOutputHandle m_Handle;

		// Token: 0x04000E29 RID: 3625
		private static readonly PlayableOutput m_NullPlayableOutput = new PlayableOutput(PlayableOutputHandle.Null);
	}
}
