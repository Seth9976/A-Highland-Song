using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x020003EE RID: 1006
	[UsedByNativeCode]
	internal struct CoreCameraValues : IEquatable<CoreCameraValues>
	{
		// Token: 0x0600222E RID: 8750 RVA: 0x00038970 File Offset: 0x00036B70
		public bool Equals(CoreCameraValues other)
		{
			return this.filterMode == other.filterMode && this.cullingMask == other.cullingMask && this.instanceID == other.instanceID;
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x000389B0 File Offset: 0x00036BB0
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is CoreCameraValues && this.Equals((CoreCameraValues)obj);
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000389E8 File Offset: 0x00036BE8
		public override int GetHashCode()
		{
			int num = this.filterMode;
			num = (num * 397) ^ (int)this.cullingMask;
			return (num * 397) ^ this.instanceID;
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x00038A24 File Offset: 0x00036C24
		public static bool operator ==(CoreCameraValues left, CoreCameraValues right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x00038A40 File Offset: 0x00036C40
		public static bool operator !=(CoreCameraValues left, CoreCameraValues right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000C76 RID: 3190
		private int filterMode;

		// Token: 0x04000C77 RID: 3191
		private uint cullingMask;

		// Token: 0x04000C78 RID: 3192
		private int instanceID;
	}
}
