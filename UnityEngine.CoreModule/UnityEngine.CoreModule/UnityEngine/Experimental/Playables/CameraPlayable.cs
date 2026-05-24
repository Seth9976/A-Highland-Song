using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Playables
{
	// Token: 0x02000467 RID: 1127
	[RequiredByNativeCode]
	[StaticAccessor("CameraPlayableBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	[NativeHeader("Runtime/Export/Director/CameraPlayable.bindings.h")]
	[NativeHeader("Runtime/Camera//Director/CameraPlayable.h")]
	public struct CameraPlayable : IPlayable, IEquatable<CameraPlayable>
	{
		// Token: 0x060027E0 RID: 10208 RVA: 0x00042644 File Offset: 0x00040844
		public static CameraPlayable Create(PlayableGraph graph, Camera camera)
		{
			PlayableHandle playableHandle = CameraPlayable.CreateHandle(graph, camera);
			return new CameraPlayable(playableHandle);
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x00042664 File Offset: 0x00040864
		private static PlayableHandle CreateHandle(PlayableGraph graph, Camera camera)
		{
			PlayableHandle @null = PlayableHandle.Null;
			bool flag = !CameraPlayable.InternalCreateCameraPlayable(ref graph, camera, ref @null);
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

		// Token: 0x060027E2 RID: 10210 RVA: 0x00042698 File Offset: 0x00040898
		internal CameraPlayable(PlayableHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !handle.IsPlayableOfType<CameraPlayable>();
				if (flag2)
				{
					throw new InvalidCastException("Can't set handle: the playable is not an CameraPlayable.");
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000426D4 File Offset: 0x000408D4
		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000426EC File Offset: 0x000408EC
		public static implicit operator Playable(CameraPlayable playable)
		{
			return new Playable(playable.GetHandle());
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x0004270C File Offset: 0x0004090C
		public static explicit operator CameraPlayable(Playable playable)
		{
			return new CameraPlayable(playable.GetHandle());
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x0004272C File Offset: 0x0004092C
		public bool Equals(CameraPlayable other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x00042750 File Offset: 0x00040950
		public Camera GetCamera()
		{
			return CameraPlayable.GetCameraInternal(ref this.m_Handle);
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x0004276D File Offset: 0x0004096D
		public void SetCamera(Camera value)
		{
			CameraPlayable.SetCameraInternal(ref this.m_Handle, value);
		}

		// Token: 0x060027E9 RID: 10217
		[NativeThrows]
		[MethodImpl(4096)]
		private static extern Camera GetCameraInternal(ref PlayableHandle hdl);

		// Token: 0x060027EA RID: 10218
		[NativeThrows]
		[MethodImpl(4096)]
		private static extern void SetCameraInternal(ref PlayableHandle hdl, Camera camera);

		// Token: 0x060027EB RID: 10219
		[NativeThrows]
		[MethodImpl(4096)]
		private static extern bool InternalCreateCameraPlayable(ref PlayableGraph graph, Camera camera, ref PlayableHandle handle);

		// Token: 0x060027EC RID: 10220
		[NativeThrows]
		[MethodImpl(4096)]
		private static extern bool ValidateType(ref PlayableHandle hdl);

		// Token: 0x04000EC0 RID: 3776
		private PlayableHandle m_Handle;
	}
}
