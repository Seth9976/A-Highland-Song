using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	public struct OptionalInputDeviceTransportType
	{
		// Token: 0x0600051A RID: 1306 RVA: 0x000134E4 File Offset: 0x000116E4
		public OptionalInputDeviceTransportType(InputDeviceTransportType value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x000134F4 File Offset: 0x000116F4
		public bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x000134FC File Offset: 0x000116FC
		public bool HasNoValue
		{
			get
			{
				return !this.hasValue;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00013507 File Offset: 0x00011707
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x00013522 File Offset: 0x00011722
		public InputDeviceTransportType Value
		{
			get
			{
				if (!this.hasValue)
				{
					throw new OptionalTypeHasNoValueException("Trying to get a value from an OptionalInputDeviceTransportType that has no value.");
				}
				return this.value;
			}
			set
			{
				this.value = value;
				this.hasValue = true;
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00013532 File Offset: 0x00011732
		public void Clear()
		{
			this.value = InputDeviceTransportType.Unknown;
			this.hasValue = false;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00013542 File Offset: 0x00011742
		public InputDeviceTransportType GetValueOrDefault(InputDeviceTransportType defaultValue)
		{
			if (!this.hasValue)
			{
				return defaultValue;
			}
			return this.value;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00013554 File Offset: 0x00011754
		public InputDeviceTransportType GetValueOrZero()
		{
			if (!this.hasValue)
			{
				return InputDeviceTransportType.Unknown;
			}
			return this.value;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00013566 File Offset: 0x00011766
		public void SetValue(InputDeviceTransportType value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00013576 File Offset: 0x00011776
		public override bool Equals(object other)
		{
			return (other == null && !this.hasValue) || this.value.Equals(other);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00013597 File Offset: 0x00011797
		public bool Equals(OptionalInputDeviceTransportType other)
		{
			return this.hasValue && other.hasValue && this.value == other.value;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000135B9 File Offset: 0x000117B9
		public bool Equals(InputDeviceTransportType other)
		{
			return this.hasValue && this.value == other;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000135CE File Offset: 0x000117CE
		public static bool operator ==(OptionalInputDeviceTransportType a, OptionalInputDeviceTransportType b)
		{
			return a.hasValue && b.hasValue && a.value == b.value;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000135F0 File Offset: 0x000117F0
		public static bool operator !=(OptionalInputDeviceTransportType a, OptionalInputDeviceTransportType b)
		{
			return !(a == b);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x000135FC File Offset: 0x000117FC
		public static bool operator ==(OptionalInputDeviceTransportType a, InputDeviceTransportType b)
		{
			return a.hasValue && a.value == b;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00013611 File Offset: 0x00011811
		public static bool operator !=(OptionalInputDeviceTransportType a, InputDeviceTransportType b)
		{
			return !a.hasValue || a.value != b;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00013629 File Offset: 0x00011829
		private static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00013632 File Offset: 0x00011832
		public override int GetHashCode()
		{
			return OptionalInputDeviceTransportType.CombineHashCodes(this.hasValue.GetHashCode(), this.value.GetHashCode());
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00013655 File Offset: 0x00011855
		public override string ToString()
		{
			if (!this.hasValue)
			{
				return "";
			}
			return this.value.ToString();
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00013676 File Offset: 0x00011876
		public static implicit operator OptionalInputDeviceTransportType(InputDeviceTransportType value)
		{
			return new OptionalInputDeviceTransportType(value);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001367E File Offset: 0x0001187E
		public static explicit operator InputDeviceTransportType(OptionalInputDeviceTransportType optional)
		{
			return optional.Value;
		}

		// Token: 0x04000470 RID: 1136
		[SerializeField]
		private bool hasValue;

		// Token: 0x04000471 RID: 1137
		[SerializeField]
		private InputDeviceTransportType value;
	}
}
