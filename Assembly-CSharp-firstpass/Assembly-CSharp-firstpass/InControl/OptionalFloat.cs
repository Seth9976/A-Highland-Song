using System;
using System.Globalization;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000070 RID: 112
	[Serializable]
	public struct OptionalFloat
	{
		// Token: 0x060004EE RID: 1262 RVA: 0x00013134 File Offset: 0x00011334
		public OptionalFloat(float value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00013144 File Offset: 0x00011344
		public bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x0001314C File Offset: 0x0001134C
		public bool HasNoValue
		{
			get
			{
				return !this.hasValue;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00013157 File Offset: 0x00011357
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x00013172 File Offset: 0x00011372
		public float Value
		{
			get
			{
				if (!this.hasValue)
				{
					throw new OptionalTypeHasNoValueException("Trying to get a value from an OptionalFloat that has no value.");
				}
				return this.value;
			}
			set
			{
				this.value = value;
				this.hasValue = true;
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00013182 File Offset: 0x00011382
		public void Clear()
		{
			this.value = 0f;
			this.hasValue = false;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00013196 File Offset: 0x00011396
		public float GetValueOrDefault(float defaultValue)
		{
			if (!this.hasValue)
			{
				return defaultValue;
			}
			return this.value;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000131A8 File Offset: 0x000113A8
		public float GetValueOrZero()
		{
			if (!this.hasValue)
			{
				return 0f;
			}
			return this.value;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000131BE File Offset: 0x000113BE
		public void SetValue(float value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000131CE File Offset: 0x000113CE
		public override bool Equals(object other)
		{
			return (other == null && !this.hasValue) || this.value.Equals(other);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000131E9 File Offset: 0x000113E9
		public bool Equals(OptionalFloat other)
		{
			return this.hasValue && other.hasValue && OptionalFloat.IsApproximatelyEqual(this.value, other.value);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001320E File Offset: 0x0001140E
		public bool Equals(float other)
		{
			return this.hasValue && OptionalFloat.IsApproximatelyEqual(this.value, other);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00013226 File Offset: 0x00011426
		public static bool operator ==(OptionalFloat a, OptionalFloat b)
		{
			return a.hasValue && b.hasValue && OptionalFloat.IsApproximatelyEqual(a.value, b.value);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001324B File Offset: 0x0001144B
		public static bool operator !=(OptionalFloat a, OptionalFloat b)
		{
			return !(a == b);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00013257 File Offset: 0x00011457
		public static bool operator ==(OptionalFloat a, float b)
		{
			return a.hasValue && OptionalFloat.IsApproximatelyEqual(a.value, b);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001326F File Offset: 0x0001146F
		public static bool operator !=(OptionalFloat a, float b)
		{
			return !a.hasValue || !OptionalFloat.IsApproximatelyEqual(a.value, b);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001328A File Offset: 0x0001148A
		private static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00013293 File Offset: 0x00011493
		public override int GetHashCode()
		{
			return OptionalFloat.CombineHashCodes(this.hasValue.GetHashCode(), this.value.GetHashCode());
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000132B0 File Offset: 0x000114B0
		public override string ToString()
		{
			if (!this.hasValue)
			{
				return "";
			}
			return this.value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000132D0 File Offset: 0x000114D0
		public static implicit operator OptionalFloat(float value)
		{
			return new OptionalFloat(value);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x000132D8 File Offset: 0x000114D8
		public static explicit operator float(OptionalFloat optional)
		{
			return optional.Value;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000132E4 File Offset: 0x000114E4
		private static bool IsApproximatelyEqual(float a, float b)
		{
			float num = a - b;
			return num >= -1E-07f && num <= 1E-07f;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001330C File Offset: 0x0001150C
		public bool ApproximatelyEquals(float other)
		{
			if (!this.hasValue)
			{
				return false;
			}
			float num = this.value - other;
			return num >= -1E-07f && num <= 1E-07f;
		}

		// Token: 0x0400046B RID: 1131
		[SerializeField]
		private bool hasValue;

		// Token: 0x0400046C RID: 1132
		[SerializeField]
		private float value;

		// Token: 0x0400046D RID: 1133
		private const float epsilon = 1E-07f;
	}
}
