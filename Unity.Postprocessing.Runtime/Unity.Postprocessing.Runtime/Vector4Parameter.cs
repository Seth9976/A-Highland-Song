using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	public sealed class Vector4Parameter : ParameterOverride<Vector4>
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00009780 File Offset: 0x00007980
		public override void Interp(Vector4 from, Vector4 to, float t)
		{
			this.value.x = from.x + (to.x - from.x) * t;
			this.value.y = from.y + (to.y - from.y) * t;
			this.value.z = from.z + (to.z - from.z) * t;
			this.value.w = from.w + (to.w - from.w) * t;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00009811 File Offset: 0x00007A11
		public static implicit operator Vector2(Vector4Parameter prop)
		{
			return prop.value;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000981E File Offset: 0x00007A1E
		public static implicit operator Vector3(Vector4Parameter prop)
		{
			return prop.value;
		}
	}
}
