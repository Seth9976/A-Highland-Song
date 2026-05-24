using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class Decayer
	{
		// Token: 0x0600003C RID: 60 RVA: 0x000033B6 File Offset: 0x000015B6
		public void SetT(float v)
		{
			this.t = v;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000033C0 File Offset: 0x000015C0
		public void Update()
		{
			this.t = Mathf.Max(0f, this.t - this.decaySpeed * Time.deltaTime);
			float num = ((this.curve.keys.Length != 0) ? this.curve.Evaluate(1f - this.t) : this.t);
			this.value = num * this.magnitude;
			this.valueInv = (1f - num) * this.magnitude;
		}

		// Token: 0x04000044 RID: 68
		public float decaySpeed;

		// Token: 0x04000045 RID: 69
		public float magnitude;

		// Token: 0x04000046 RID: 70
		public AnimationCurve curve;

		// Token: 0x04000047 RID: 71
		[NonSerialized]
		public float value;

		// Token: 0x04000048 RID: 72
		[NonSerialized]
		public float valueInv;

		// Token: 0x04000049 RID: 73
		[NonSerialized]
		public float t;
	}
}
