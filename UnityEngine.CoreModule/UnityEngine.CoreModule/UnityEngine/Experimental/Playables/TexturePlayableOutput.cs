using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Playables
{
	// Token: 0x0200046C RID: 1132
	[StaticAccessor("TexturePlayableOutputBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Export/Director/TexturePlayableOutput.bindings.h")]
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Graphics/RenderTexture.h")]
	[NativeHeader("Runtime/Graphics/Director/TexturePlayableOutput.h")]
	public struct TexturePlayableOutput : IPlayableOutput
	{
		// Token: 0x06002809 RID: 10249 RVA: 0x00042A4C File Offset: 0x00040C4C
		public static TexturePlayableOutput Create(PlayableGraph graph, string name, RenderTexture target)
		{
			PlayableOutputHandle playableOutputHandle;
			bool flag = !TexturePlayableGraphExtensions.InternalCreateTextureOutput(ref graph, name, out playableOutputHandle);
			TexturePlayableOutput texturePlayableOutput;
			if (flag)
			{
				texturePlayableOutput = TexturePlayableOutput.Null;
			}
			else
			{
				TexturePlayableOutput texturePlayableOutput2 = new TexturePlayableOutput(playableOutputHandle);
				texturePlayableOutput2.SetTarget(target);
				texturePlayableOutput = texturePlayableOutput2;
			}
			return texturePlayableOutput;
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x00042A8C File Offset: 0x00040C8C
		internal TexturePlayableOutput(PlayableOutputHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !handle.IsPlayableOutputOfType<TexturePlayableOutput>();
				if (flag2)
				{
					throw new InvalidCastException("Can't set handle: the playable is not an TexturePlayableOutput.");
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x0600280B RID: 10251 RVA: 0x00042AC8 File Offset: 0x00040CC8
		public static TexturePlayableOutput Null
		{
			get
			{
				return new TexturePlayableOutput(PlayableOutputHandle.Null);
			}
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x00042AE4 File Offset: 0x00040CE4
		public PlayableOutputHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x00042AFC File Offset: 0x00040CFC
		public static implicit operator PlayableOutput(TexturePlayableOutput output)
		{
			return new PlayableOutput(output.GetHandle());
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x00042B1C File Offset: 0x00040D1C
		public static explicit operator TexturePlayableOutput(PlayableOutput output)
		{
			return new TexturePlayableOutput(output.GetHandle());
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x00042B3C File Offset: 0x00040D3C
		public RenderTexture GetTarget()
		{
			return TexturePlayableOutput.InternalGetTarget(ref this.m_Handle);
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x00042B59 File Offset: 0x00040D59
		public void SetTarget(RenderTexture value)
		{
			TexturePlayableOutput.InternalSetTarget(ref this.m_Handle, value);
		}

		// Token: 0x06002811 RID: 10257
		[NativeThrows]
		[MethodImpl(4096)]
		private static extern RenderTexture InternalGetTarget(ref PlayableOutputHandle output);

		// Token: 0x06002812 RID: 10258
		[NativeThrows]
		[MethodImpl(4096)]
		private static extern void InternalSetTarget(ref PlayableOutputHandle output, RenderTexture target);

		// Token: 0x04000EC3 RID: 3779
		private PlayableOutputHandle m_Handle;
	}
}
