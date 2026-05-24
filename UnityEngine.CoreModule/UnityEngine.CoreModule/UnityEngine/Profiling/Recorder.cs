using System;
using Unity.Profiling;
using Unity.Profiling.LowLevel;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
	// Token: 0x02000279 RID: 633
	[UsedByNativeCode]
	public sealed class Recorder
	{
		// Token: 0x06001B9C RID: 7068 RVA: 0x00008CAF File Offset: 0x00006EAF
		internal Recorder()
		{
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x0002C2CC File Offset: 0x0002A4CC
		internal Recorder(ProfilerRecorderHandle handle)
		{
			bool flag = !handle.Valid;
			if (!flag)
			{
				this.m_RecorderCPU = new ProfilerRecorder(handle, 1, (ProfilerRecorderOptions)153);
				bool flag2 = (ProfilerRecorderHandle.GetDescription(handle).Flags & MarkerFlags.SampleGPU) > MarkerFlags.Default;
				if (flag2)
				{
					this.m_RecorderGPU = new ProfilerRecorder(handle, 1, (ProfilerRecorderOptions)217);
				}
			}
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x0002C330 File Offset: 0x0002A530
		~Recorder()
		{
			this.m_RecorderCPU.Dispose();
			this.m_RecorderGPU.Dispose();
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x0002C374 File Offset: 0x0002A574
		public static Recorder Get(string samplerName)
		{
			ProfilerRecorderHandle profilerRecorderHandle = ProfilerRecorderHandle.Get(ProfilerCategory.Any, samplerName);
			bool flag = !profilerRecorderHandle.Valid;
			Recorder recorder;
			if (flag)
			{
				recorder = Recorder.s_InvalidRecorder;
			}
			else
			{
				recorder = new Recorder(profilerRecorderHandle);
			}
			return recorder;
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001BA0 RID: 7072 RVA: 0x0002C3B0 File Offset: 0x0002A5B0
		public bool isValid
		{
			get
			{
				return this.m_RecorderCPU.handle > 0UL;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001BA1 RID: 7073 RVA: 0x0002C3D4 File Offset: 0x0002A5D4
		// (set) Token: 0x06001BA2 RID: 7074 RVA: 0x0002C3F1 File Offset: 0x0002A5F1
		public bool enabled
		{
			get
			{
				return this.m_RecorderCPU.IsRunning;
			}
			set
			{
				this.SetEnabled(value);
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001BA3 RID: 7075 RVA: 0x0002C3FC File Offset: 0x0002A5FC
		public long elapsedNanoseconds
		{
			get
			{
				bool flag = !this.m_RecorderCPU.Valid;
				long num;
				if (flag)
				{
					num = 0L;
				}
				else
				{
					num = this.m_RecorderCPU.LastValue;
				}
				return num;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001BA4 RID: 7076 RVA: 0x0002C430 File Offset: 0x0002A630
		public long gpuElapsedNanoseconds
		{
			get
			{
				bool flag = !this.m_RecorderGPU.Valid;
				long num;
				if (flag)
				{
					num = 0L;
				}
				else
				{
					num = this.m_RecorderGPU.LastValue;
				}
				return num;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001BA5 RID: 7077 RVA: 0x0002C464 File Offset: 0x0002A664
		public int sampleBlockCount
		{
			get
			{
				bool flag = !this.m_RecorderCPU.Valid;
				int num;
				if (flag)
				{
					num = 0;
				}
				else
				{
					bool flag2 = this.m_RecorderCPU.Count != 1;
					if (flag2)
					{
						num = 0;
					}
					else
					{
						num = (int)this.m_RecorderCPU.GetSample(0).Count;
					}
				}
				return num;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001BA6 RID: 7078 RVA: 0x0002C4BC File Offset: 0x0002A6BC
		public int gpuSampleBlockCount
		{
			get
			{
				bool flag = !this.m_RecorderGPU.Valid;
				int num;
				if (flag)
				{
					num = 0;
				}
				else
				{
					bool flag2 = this.m_RecorderGPU.Count != 1;
					if (flag2)
					{
						num = 0;
					}
					else
					{
						num = (int)this.m_RecorderGPU.GetSample(0).Count;
					}
				}
				return num;
			}
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0002C514 File Offset: 0x0002A714
		public void FilterToCurrentThread()
		{
			bool flag = !this.m_RecorderCPU.Valid;
			if (!flag)
			{
				this.m_RecorderCPU.FilterToCurrentThread();
			}
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0002C544 File Offset: 0x0002A744
		public void CollectFromAllThreads()
		{
			bool flag = !this.m_RecorderCPU.Valid;
			if (!flag)
			{
				this.m_RecorderCPU.CollectFromAllThreads();
			}
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0002C574 File Offset: 0x0002A774
		private void SetEnabled(bool state)
		{
			if (state)
			{
				this.m_RecorderCPU.Start();
				bool valid = this.m_RecorderGPU.Valid;
				if (valid)
				{
					this.m_RecorderGPU.Start();
				}
			}
			else
			{
				this.m_RecorderCPU.Stop();
				bool valid2 = this.m_RecorderGPU.Valid;
				if (valid2)
				{
					this.m_RecorderGPU.Stop();
				}
			}
		}

		// Token: 0x04000909 RID: 2313
		private const ProfilerRecorderOptions s_RecorderDefaultOptions = (ProfilerRecorderOptions)153;

		// Token: 0x0400090A RID: 2314
		internal static Recorder s_InvalidRecorder = new Recorder();

		// Token: 0x0400090B RID: 2315
		private ProfilerRecorder m_RecorderCPU;

		// Token: 0x0400090C RID: 2316
		private ProfilerRecorder m_RecorderGPU;
	}
}
