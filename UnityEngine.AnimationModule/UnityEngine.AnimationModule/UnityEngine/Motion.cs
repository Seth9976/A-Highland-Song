using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200003B RID: 59
	[NativeHeader("Modules/Animation/Motion.h")]
	public class Motion : Object
	{
		// Token: 0x0600027F RID: 639 RVA: 0x000039EB File Offset: 0x00001BEB
		protected Motion()
		{
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000280 RID: 640
		public extern float averageDuration
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000281 RID: 641
		public extern float averageAngularSpeed
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000282 RID: 642 RVA: 0x00004334 File Offset: 0x00002534
		public Vector3 averageSpeed
		{
			get
			{
				Vector3 vector;
				this.get_averageSpeed_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000283 RID: 643
		public extern float apparentSpeed
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000284 RID: 644
		public extern bool isLooping
		{
			[NativeMethod("IsLooping")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000285 RID: 645
		public extern bool legacy
		{
			[NativeMethod("IsLegacy")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000286 RID: 646
		public extern bool isHumanMotion
		{
			[NativeMethod("IsHumanMotion")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000434C File Offset: 0x0000254C
		[EditorBrowsable(1)]
		[Obsolete("ValidateIfRetargetable is not supported anymore, please use isHumanMotion instead.", true)]
		public bool ValidateIfRetargetable(bool val)
		{
			return false;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000435F File Offset: 0x0000255F
		[EditorBrowsable(1)]
		[Obsolete("isAnimatorMotion is not supported anymore, please use !legacy instead.", true)]
		public bool isAnimatorMotion { get; }

		// Token: 0x06000289 RID: 649
		[MethodImpl(4096)]
		private extern void get_averageSpeed_Injected(out Vector3 ret);
	}
}
