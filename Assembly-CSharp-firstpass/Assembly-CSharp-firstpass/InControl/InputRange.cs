using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public struct InputRange
	{
		// Token: 0x060001DB RID: 475 RVA: 0x000077C9 File Offset: 0x000059C9
		private InputRange(float value0, float value1, InputRangeType type)
		{
			this.Value0 = value0;
			this.Value1 = value1;
			this.Type = type;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000077E0 File Offset: 0x000059E0
		public InputRange(InputRangeType type)
		{
			this.Value0 = InputRange.typeToRange[(int)type].Value0;
			this.Value1 = InputRange.typeToRange[(int)type].Value1;
			this.Type = type;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007815 File Offset: 0x00005A15
		public bool Includes(float value)
		{
			return !this.Excludes(value);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00007821 File Offset: 0x00005A21
		private bool Excludes(float value)
		{
			return this.Type == InputRangeType.None || value < Mathf.Min(this.Value0, this.Value1) || value > Mathf.Max(this.Value0, this.Value1);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00007857 File Offset: 0x00005A57
		public static bool Excludes(InputRangeType rangeType, float value)
		{
			return InputRange.typeToRange[(int)rangeType].Excludes(value);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000786C File Offset: 0x00005A6C
		private static float Remap(float value, InputRange sourceRange, InputRange targetRange)
		{
			if (sourceRange.Excludes(value))
			{
				return 0f;
			}
			float num = Mathf.InverseLerp(sourceRange.Value0, sourceRange.Value1, value);
			return Mathf.Lerp(targetRange.Value0, targetRange.Value1, num);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000078B0 File Offset: 0x00005AB0
		public static float Remap(float value, InputRangeType sourceRangeType, InputRangeType targetRangeType)
		{
			InputRange inputRange = InputRange.typeToRange[(int)sourceRangeType];
			InputRange inputRange2 = InputRange.typeToRange[(int)targetRangeType];
			return InputRange.Remap(value, inputRange, inputRange2);
		}

		// Token: 0x04000262 RID: 610
		public static readonly InputRange None = new InputRange(0f, 0f, InputRangeType.None);

		// Token: 0x04000263 RID: 611
		public static readonly InputRange MinusOneToOne = new InputRange(-1f, 1f, InputRangeType.MinusOneToOne);

		// Token: 0x04000264 RID: 612
		public static readonly InputRange OneToMinusOne = new InputRange(1f, -1f, InputRangeType.OneToMinusOne);

		// Token: 0x04000265 RID: 613
		public static readonly InputRange ZeroToOne = new InputRange(0f, 1f, InputRangeType.ZeroToOne);

		// Token: 0x04000266 RID: 614
		public static readonly InputRange ZeroToMinusOne = new InputRange(0f, -1f, InputRangeType.ZeroToMinusOne);

		// Token: 0x04000267 RID: 615
		public static readonly InputRange OneToZero = new InputRange(1f, 0f, InputRangeType.OneToZero);

		// Token: 0x04000268 RID: 616
		public static readonly InputRange MinusOneToZero = new InputRange(-1f, 0f, InputRangeType.MinusOneToZero);

		// Token: 0x04000269 RID: 617
		private static readonly InputRange[] typeToRange = new InputRange[]
		{
			InputRange.None,
			InputRange.MinusOneToOne,
			InputRange.OneToMinusOne,
			InputRange.ZeroToOne,
			InputRange.ZeroToMinusOne,
			InputRange.OneToZero,
			InputRange.MinusOneToZero
		};

		// Token: 0x0400026A RID: 618
		public readonly float Value0;

		// Token: 0x0400026B RID: 619
		public readonly float Value1;

		// Token: 0x0400026C RID: 620
		public readonly InputRangeType Type;
	}
}
