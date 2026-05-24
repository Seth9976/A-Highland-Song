using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000041 RID: 65
	[Serializable]
	public sealed class IntParameter : ParameterOverride<int>
	{
		// Token: 0x060000CB RID: 203 RVA: 0x000095AB File Offset: 0x000077AB
		public override void Interp(int from, int to, float t)
		{
			this.value = (int)((float)from + (float)(to - from) * t);
		}
	}
}
