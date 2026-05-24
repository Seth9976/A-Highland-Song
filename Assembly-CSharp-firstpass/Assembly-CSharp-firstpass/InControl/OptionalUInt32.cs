using System;
using System.Globalization;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000076 RID: 118
	[Serializable]
	public struct OptionalUInt32
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x00013B49 File Offset: 0x00011D49
		public OptionalUInt32(uint value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00013B59 File Offset: 0x00011D59
		public bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00013B61 File Offset: 0x00011D61
		public bool HasNoValue
		{
			get
			{
				return !this.hasValue;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00013B6C File Offset: 0x00011D6C
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x00013B87 File Offset: 0x00011D87
		public uint Value
		{
			get
			{
				if (!this.hasValue)
				{
					throw new OptionalTypeHasNoValueException("Trying to get a value from an OptionalUInt32 that has no value.");
				}
				return this.value;
			}
			set
			{
				this.value = value;
				this.hasValue = true;
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00013B97 File Offset: 0x00011D97
		public void Clear()
		{
			this.value = 0U;
			this.hasValue = false;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00013BA7 File Offset: 0x00011DA7
		public uint GetValueOrDefault(uint defaultValue)
		{
			if (!this.hasValue)
			{
				return defaultValue;
			}
			return this.value;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00013BB9 File Offset: 0x00011DB9
		public uint GetValueOrZero()
		{
			if (!this.hasValue)
			{
				return 0U;
			}
			return this.value;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00013BCB File Offset: 0x00011DCB
		public void SetValue(uint value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00013BDB File Offset: 0x00011DDB
		public override bool Equals(object other)
		{
			return (other == null && !this.hasValue) || this.value.Equals(other);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00013BF6 File Offset: 0x00011DF6
		public bool Equals(OptionalUInt32 other)
		{
			return this.hasValue && other.hasValue && this.value == other.value;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00013C18 File Offset: 0x00011E18
		public bool Equals(uint other)
		{
			return this.hasValue && this.value == other;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00013C2D File Offset: 0x00011E2D
		public static bool operator ==(OptionalUInt32 a, OptionalUInt32 b)
		{
			return a.hasValue && b.hasValue && a.value == b.value;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00013C4F File Offset: 0x00011E4F
		public static bool operator !=(OptionalUInt32 a, OptionalUInt32 b)
		{
			return !(a == b);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00013C5B File Offset: 0x00011E5B
		public static bool operator ==(OptionalUInt32 a, uint b)
		{
			return a.hasValue && a.value == b;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00013C70 File Offset: 0x00011E70
		public static bool operator !=(OptionalUInt32 a, uint b)
		{
			return !a.hasValue || a.value != b;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00013C88 File Offset: 0x00011E88
		private static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00013C91 File Offset: 0x00011E91
		public override int GetHashCode()
		{
			return OptionalUInt32.CombineHashCodes(this.hasValue.GetHashCode(), this.value.GetHashCode());
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00013CAE File Offset: 0x00011EAE
		public override string ToString()
		{
			if (!this.hasValue)
			{
				return "";
			}
			return this.value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00013CCE File Offset: 0x00011ECE
		public static implicit operator OptionalUInt32(uint value)
		{
			return new OptionalUInt32(value);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00013CD6 File Offset: 0x00011ED6
		public static explicit operator uint(OptionalUInt32 optional)
		{
			return optional.Value;
		}

		// Token: 0x04000478 RID: 1144
		[SerializeField]
		private bool hasValue;

		// Token: 0x04000479 RID: 1145
		[SerializeField]
		private uint value;
	}
}
