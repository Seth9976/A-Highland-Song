using System;
using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
	// Token: 0x0200027A RID: 634
	[UsedByNativeCode]
	[NativeHeader("Runtime/Profiler/ScriptBindings/Sampler.bindings.h")]
	public class Sampler
	{
		// Token: 0x06001BAB RID: 7083 RVA: 0x00008CAF File Offset: 0x00006EAF
		internal Sampler()
		{
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x0002C5E7 File Offset: 0x0002A7E7
		internal Sampler(IntPtr ptr)
		{
			this.m_Ptr = ptr;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001BAD RID: 7085 RVA: 0x0002C5F8 File Offset: 0x0002A7F8
		public bool isValid
		{
			get
			{
				return this.m_Ptr != IntPtr.Zero;
			}
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x0002C61C File Offset: 0x0002A81C
		public Recorder GetRecorder()
		{
			ProfilerRecorderHandle profilerRecorderHandle = new ProfilerRecorderHandle((ulong)this.m_Ptr.ToInt64());
			return new Recorder(profilerRecorderHandle);
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x0002C648 File Offset: 0x0002A848
		public static Sampler Get(string name)
		{
			IntPtr marker = ProfilerUnsafeUtility.GetMarker(name);
			bool flag = marker == IntPtr.Zero;
			Sampler sampler;
			if (flag)
			{
				sampler = Sampler.s_InvalidSampler;
			}
			else
			{
				sampler = new Sampler(marker);
			}
			return sampler;
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x0002C680 File Offset: 0x0002A880
		public static int GetNames(List<string> names)
		{
			List<ProfilerRecorderHandle> list = new List<ProfilerRecorderHandle>();
			ProfilerRecorderHandle.GetAvailable(list);
			bool flag = names != null;
			if (flag)
			{
				bool flag2 = names.Count < list.Count;
				if (flag2)
				{
					names.Capacity = list.Count;
					for (int i = names.Count; i < list.Count; i++)
					{
						names.Add(null);
					}
				}
				int num = 0;
				foreach (ProfilerRecorderHandle profilerRecorderHandle in list)
				{
					names[num] = ProfilerRecorderHandle.GetDescription(profilerRecorderHandle).Name;
					num++;
				}
			}
			return list.Count;
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001BB1 RID: 7089 RVA: 0x0002C75C File Offset: 0x0002A95C
		public string name
		{
			get
			{
				return ProfilerUnsafeUtility.Internal_GetName(this.m_Ptr);
			}
		}

		// Token: 0x0400090D RID: 2317
		internal IntPtr m_Ptr;

		// Token: 0x0400090E RID: 2318
		internal static Sampler s_InvalidSampler = new Sampler();
	}
}
