using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Playables
{
	// Token: 0x02000469 RID: 1129
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	[StaticAccessor("TextureMixerPlayableBindings", StaticAccessorType.DoubleColon)]
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Export/Director/TextureMixerPlayable.bindings.h")]
	[NativeHeader("Runtime/Graphics/Director/TextureMixerPlayable.h")]
	public struct TextureMixerPlayable : IPlayable, IEquatable<TextureMixerPlayable>
	{
		// Token: 0x060027FE RID: 10238 RVA: 0x000428F0 File Offset: 0x00040AF0
		public static TextureMixerPlayable Create(PlayableGraph graph)
		{
			PlayableHandle playableHandle = TextureMixerPlayable.CreateHandle(graph);
			return new TextureMixerPlayable(playableHandle);
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x00042910 File Offset: 0x00040B10
		private static PlayableHandle CreateHandle(PlayableGraph graph)
		{
			PlayableHandle @null = PlayableHandle.Null;
			bool flag = !TextureMixerPlayable.CreateTextureMixerPlayableInternal(ref graph, ref @null);
			PlayableHandle playableHandle;
			if (flag)
			{
				playableHandle = PlayableHandle.Null;
			}
			else
			{
				playableHandle = @null;
			}
			return playableHandle;
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x00042944 File Offset: 0x00040B44
		internal TextureMixerPlayable(PlayableHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !handle.IsPlayableOfType<TextureMixerPlayable>();
				if (flag2)
				{
					throw new InvalidCastException("Can't set handle: the playable is not an TextureMixerPlayable.");
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x00042980 File Offset: 0x00040B80
		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x00042998 File Offset: 0x00040B98
		public static implicit operator Playable(TextureMixerPlayable playable)
		{
			return new Playable(playable.GetHandle());
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000429B8 File Offset: 0x00040BB8
		public static explicit operator TextureMixerPlayable(Playable playable)
		{
			return new TextureMixerPlayable(playable.GetHandle());
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000429D8 File Offset: 0x00040BD8
		public bool Equals(TextureMixerPlayable other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x06002805 RID: 10245
		[NativeThrows]
		[MethodImpl(4096)]
		private static extern bool CreateTextureMixerPlayableInternal(ref PlayableGraph graph, ref PlayableHandle handle);

		// Token: 0x04000EC2 RID: 3778
		private PlayableHandle m_Handle;
	}
}
