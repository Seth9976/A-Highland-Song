using System;
using System.Globalization;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public struct OptionalInt16
	{
		// Token: 0x0600052F RID: 1327 RVA: 0x00013687 File Offset: 0x00011887
		public OptionalInt16(short value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00013697 File Offset: 0x00011897
		public bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x0001369F File Offset: 0x0001189F
		public bool HasNoValue
		{
			get
			{
				return !this.hasValue;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x000136AA File Offset: 0x000118AA
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x000136C5 File Offset: 0x000118C5
		public short Value
		{
			get
			{
				if (!this.hasValue)
				{
					throw new OptionalTypeHasNoValueException("Trying to get a value from an OptionalInt16 that has no value.");
				}
				return this.value;
			}
			set
			{
				this.value = value;
				this.hasValue = true;
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000136D5 File Offset: 0x000118D5
		public void Clear()
		{
			this.value = 0;
			this.hasValue = false;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000136E5 File Offset: 0x000118E5
		public short GetValueOrDefault(short defaultValue)
		{
			if (!this.hasValue)
			{
				return defaultValue;
			}
			return this.value;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x000136F7 File Offset: 0x000118F7
		public short GetValueOrZero()
		{
			if (!this.hasValue)
			{
				return 0;
			}
			return this.value;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00013709 File Offset: 0x00011909
		public void SetValue(short value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00013719 File Offset: 0x00011919
		public override bool Equals(object other)
		{
			return (other == null && !this.hasValue) || this.value.Equals(other);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00013734 File Offset: 0x00011934
		public bool Equals(OptionalInt16 other)
		{
			return this.hasValue && other.hasValue && this.value == other.value;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00013756 File Offset: 0x00011956
		public bool Equals(short other)
		{
			return this.hasValue && this.value == other;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001376B File Offset: 0x0001196B
		public static bool operator ==(OptionalInt16 a, OptionalInt16 b)
		{
			return a.hasValue && b.hasValue && a.value == b.value;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001378D File Offset: 0x0001198D
		public static bool operator !=(OptionalInt16 a, OptionalInt16 b)
		{
			return !(a == b);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00013799 File Offset: 0x00011999
		public static bool operator ==(OptionalInt16 a, short b)
		{
			return a.hasValue && a.value == b;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000137AE File Offset: 0x000119AE
		public static bool operator !=(OptionalInt16 a, short b)
		{
			return !a.hasValue || a.value != b;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x000137C6 File Offset: 0x000119C6
		private static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000137CF File Offset: 0x000119CF
		public override int GetHashCode()
		{
			return OptionalInt16.CombineHashCodes(this.hasValue.GetHashCode(), this.value.GetHashCode());
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000137EC File Offset: 0x000119EC
		public override string ToString()
		{
			if (!this.hasValue)
			{
				return "";
			}
			return this.value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001380C File Offset: 0x00011A0C
		public static implicit operator OptionalInt16(short value)
		{
			return new OptionalInt16(value);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00013814 File Offset: 0x00011A14
		public static explicit operator short(OptionalInt16 optional)
		{
			return optional.Value;
		}

		// Token: 0x04000472 RID: 1138
		[SerializeField]
		private bool hasValue;

		// Token: 0x04000473 RID: 1139
		[SerializeField]
		private short value;
	}
}
