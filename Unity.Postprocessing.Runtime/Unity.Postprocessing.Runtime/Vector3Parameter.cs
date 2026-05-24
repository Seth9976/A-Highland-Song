using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000045 RID: 69
	[Serializable]
	public sealed class Vector3Parameter : ParameterOverride<Vector3>
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x000096EC File Offset: 0x000078EC
		public override void Interp(Vector3 from, Vector3 to, float t)
		{
			this.value.x = from.x + (to.x - from.x) * t;
			this.value.y = from.y + (to.y - from.y) * t;
			this.value.z = from.z + (to.z - from.z) * t;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000975C File Offset: 0x0000795C
		public static implicit operator Vector2(Vector3Parameter prop)
		{
			return prop.value;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00009769 File Offset: 0x00007969
		public static implicit operator Vector4(Vector3Parameter prop)
		{
			return prop.value;
		}
	}
}
