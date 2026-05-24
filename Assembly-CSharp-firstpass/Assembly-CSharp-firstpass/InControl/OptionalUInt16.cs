using System;
using System.Globalization;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000075 RID: 117
	[Serializable]
	public struct OptionalUInt16
	{
		// Token: 0x06000559 RID: 1369 RVA: 0x000139B3 File Offset: 0x00011BB3
		public OptionalUInt16(ushort value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000139C3 File Offset: 0x00011BC3
		public bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x000139CB File Offset: 0x00011BCB
		public bool HasNoValue
		{
			get
			{
				return !this.hasValue;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x000139D6 File Offset: 0x00011BD6
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x000139F1 File Offset: 0x00011BF1
		public ushort Value
		{
			get
			{
				if (!this.hasValue)
				{
					throw new OptionalTypeHasNoValueException("Trying to get a value from an OptionalUInt16 that has no value.");
				}
				return this.value;
			}
			set
			{
				this.value = value;
				this.hasValue = true;
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00013A01 File Offset: 0x00011C01
		public void Clear()
		{
			this.value = 0;
			this.hasValue = false;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00013A11 File Offset: 0x00011C11
		public ushort GetValueOrDefault(ushort defaultValue)
		{
			if (!this.hasValue)
			{
				return defaultValue;
			}
			return this.value;
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00013A23 File Offset: 0x00011C23
		public ushort GetValueOrZero()
		{
			if (!this.hasValue)
			{
				return 0;
			}
			return this.value;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00013A35 File Offset: 0x00011C35
		public void SetValue(ushort value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00013A45 File Offset: 0x00011C45
		public override bool Equals(object other)
		{
			return (other == null && !this.hasValue) || this.value.Equals(other);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00013A60 File Offset: 0x00011C60
		public bool Equals(OptionalUInt16 other)
		{
			return this.hasValue && other.hasValue && this.value == other.value;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00013A82 File Offset: 0x00011C82
		public bool Equals(ushort other)
		{
			return this.hasValue && this.value == other;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00013A97 File Offset: 0x00011C97
		public static bool operator ==(OptionalUInt16 a, OptionalUInt16 b)
		{
			return a.hasValue && b.hasValue && a.value == b.value;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00013AB9 File Offset: 0x00011CB9
		public static bool operator !=(OptionalUInt16 a, OptionalUInt16 b)
		{
			return !(a == b);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00013AC5 File Offset: 0x00011CC5
		public static bool operator ==(OptionalUInt16 a, ushort b)
		{
			return a.hasValue && a.value == b;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00013ADA File Offset: 0x00011CDA
		public static bool operator !=(OptionalUInt16 a, ushort b)
		{
			return !a.hasValue || a.value != b;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00013AF2 File Offset: 0x00011CF2
		private static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00013AFB File Offset: 0x00011CFB
		public override int GetHashCode()
		{
			return OptionalUInt16.CombineHashCodes(this.hasValue.GetHashCode(), this.value.GetHashCode());
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00013B18 File Offset: 0x00011D18
		public override string ToString()
		{
			if (!this.hasValue)
			{
				return "";
			}
			return this.value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00013B38 File Offset: 0x00011D38
		public static implicit operator OptionalUInt16(ushort value)
		{
			return new OptionalUInt16(value);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00013B40 File Offset: 0x00011D40
		public static explicit operator ushort(OptionalUInt16 optional)
		{
			return optional.Value;
		}

		// Token: 0x04000476 RID: 1142
		[SerializeField]
		private bool hasValue;

		// Token: 0x04000477 RID: 1143
		[SerializeField]
		private ushort value;
	}
}
