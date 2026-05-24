using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000044 RID: 68
	[Serializable]
	public sealed class Vector2Parameter : ParameterOverride<Vector2>
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00009678 File Offset: 0x00007878
		public override void Interp(Vector2 from, Vector2 to, float t)
		{
			this.value.x = from.x + (to.x - from.x) * t;
			this.value.y = from.y + (to.y - from.y) * t;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000096C7 File Offset: 0x000078C7
		public static implicit operator Vector3(Vector2Parameter prop)
		{
			return prop.value;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000096D4 File Offset: 0x000078D4
		public static implicit operator Vector4(Vector2Parameter prop)
		{
			return prop.value;
		}
	}
}
