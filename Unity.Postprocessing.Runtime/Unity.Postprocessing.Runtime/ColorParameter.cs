using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000043 RID: 67
	[Serializable]
	public sealed class ColorParameter : ParameterOverride<Color>
	{
		// Token: 0x060000CE RID: 206 RVA: 0x000095D0 File Offset: 0x000077D0
		public override void Interp(Color from, Color to, float t)
		{
			this.value.r = from.r + (to.r - from.r) * t;
			this.value.g = from.g + (to.g - from.g) * t;
			this.value.b = from.b + (to.b - from.b) * t;
			this.value.a = from.a + (to.a - from.a) * t;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00009661 File Offset: 0x00007861
		public static implicit operator Vector4(ColorParameter prop)
		{
			return prop.value;
		}
	}
}
