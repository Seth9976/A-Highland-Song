using System;
using System.Globalization;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000074 RID: 116
	[Serializable]
	public struct OptionalInt32
	{
		// Token: 0x06000544 RID: 1348 RVA: 0x0001381D File Offset: 0x00011A1D
		public OptionalInt32(int value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x0001382D File Offset: 0x00011A2D
		public bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00013835 File Offset: 0x00011A35
		public bool HasNoValue
		{
			get
			{
				return !this.hasValue;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x00013840 File Offset: 0x00011A40
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x0001385B File Offset: 0x00011A5B
		public int Value
		{
			get
			{
				if (!this.hasValue)
				{
					throw new OptionalTypeHasNoValueException("Trying to get a value from an OptionalInt32 that has no value.");
				}
				return this.value;
			}
			set
			{
				this.value = value;
				this.hasValue = true;
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001386B File Offset: 0x00011A6B
		public void Clear()
		{
			this.value = 0;
			this.hasValue = false;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001387B File Offset: 0x00011A7B
		public int GetValueOrDefault(int defaultValue)
		{
			if (!this.hasValue)
			{
				return defaultValue;
			}
			return this.value;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001388D File Offset: 0x00011A8D
		public int GetValueOrZero()
		{
			if (!this.hasValue)
			{
				return 0;
			}
			return this.value;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001389F File Offset: 0x00011A9F
		public void SetValue(int value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000138AF File Offset: 0x00011AAF
		public override bool Equals(object other)
		{
			return (other == null && !this.hasValue) || this.value.Equals(other);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x000138CA File Offset: 0x00011ACA
		public bool Equals(OptionalInt32 other)
		{
			return this.hasValue && other.hasValue && this.value == other.value;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x000138EC File Offset: 0x00011AEC
		public bool Equals(int other)
		{
			return this.hasValue && this.value == other;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00013901 File Offset: 0x00011B01
		public static bool operator ==(OptionalInt32 a, OptionalInt32 b)
		{
			return a.hasValue && b.hasValue && a.value == b.value;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00013923 File Offset: 0x00011B23
		public static bool operator !=(OptionalInt32 a, OptionalInt32 b)
		{
			return !(a == b);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001392F File Offset: 0x00011B2F
		public static bool operator ==(OptionalInt32 a, int b)
		{
			return a.hasValue && a.value == b;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00013944 File Offset: 0x00011B44
		public static bool operator !=(OptionalInt32 a, int b)
		{
			return !a.hasValue || a.value != b;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001395C File Offset: 0x00011B5C
		private static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00013965 File Offset: 0x00011B65
		public override int GetHashCode()
		{
			return OptionalInt32.CombineHashCodes(this.hasValue.GetHashCode(), this.value.GetHashCode());
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00013982 File Offset: 0x00011B82
		public override string ToString()
		{
			if (!this.hasValue)
			{
				return "";
			}
			return this.value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000139A2 File Offset: 0x00011BA2
		public static implicit operator OptionalInt32(int value)
		{
			return new OptionalInt32(value);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x000139AA File Offset: 0x00011BAA
		public static explicit operator int(OptionalInt32 optional)
		{
			return optional.Value;
		}

		// Token: 0x04000474 RID: 1140
		[SerializeField]
		private bool hasValue;

		// Token: 0x04000475 RID: 1141
		[SerializeField]
		private int value;
	}
}
