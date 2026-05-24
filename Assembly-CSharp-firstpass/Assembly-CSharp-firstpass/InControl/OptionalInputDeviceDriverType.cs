using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000071 RID: 113
	[Serializable]
	public struct OptionalInputDeviceDriverType
	{
		// Token: 0x06000505 RID: 1285 RVA: 0x00013341 File Offset: 0x00011541
		public OptionalInputDeviceDriverType(InputDeviceDriverType value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00013351 File Offset: 0x00011551
		public bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x00013359 File Offset: 0x00011559
		public bool HasNoValue
		{
			get
			{
				return !this.hasValue;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00013364 File Offset: 0x00011564
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x0001337F File Offset: 0x0001157F
		public InputDeviceDriverType Value
		{
			get
			{
				if (!this.hasValue)
				{
					throw new OptionalTypeHasNoValueException("Trying to get a value from an OptionalInputDeviceDriverType that has no value.");
				}
				return this.value;
			}
			set
			{
				this.value = value;
				this.hasValue = true;
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001338F File Offset: 0x0001158F
		public void Clear()
		{
			this.value = InputDeviceDriverType.Unknown;
			this.hasValue = false;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001339F File Offset: 0x0001159F
		public InputDeviceDriverType GetValueOrDefault(InputDeviceDriverType defaultValue)
		{
			if (!this.hasValue)
			{
				return defaultValue;
			}
			return this.value;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000133B1 File Offset: 0x000115B1
		public InputDeviceDriverType GetValueOrZero()
		{
			if (!this.hasValue)
			{
				return InputDeviceDriverType.Unknown;
			}
			return this.value;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000133C3 File Offset: 0x000115C3
		public void SetValue(InputDeviceDriverType value)
		{
			this.value = value;
			this.hasValue = true;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000133D3 File Offset: 0x000115D3
		public override bool Equals(object other)
		{
			return (other == null && !this.hasValue) || this.value.Equals(other);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000133F4 File Offset: 0x000115F4
		public bool Equals(OptionalInputDeviceDriverType other)
		{
			return this.hasValue && other.hasValue && this.value == other.value;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00013416 File Offset: 0x00011616
		public bool Equals(InputDeviceDriverType other)
		{
			return this.hasValue && this.value == other;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001342B File Offset: 0x0001162B
		public static bool operator ==(OptionalInputDeviceDriverType a, OptionalInputDeviceDriverType b)
		{
			return a.hasValue && b.hasValue && a.value == b.value;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001344D File Offset: 0x0001164D
		public static bool operator !=(OptionalInputDeviceDriverType a, OptionalInputDeviceDriverType b)
		{
			return !(a == b);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00013459 File Offset: 0x00011659
		public static bool operator ==(OptionalInputDeviceDriverType a, InputDeviceDriverType b)
		{
			return a.hasValue && a.value == b;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001346E File Offset: 0x0001166E
		public static bool operator !=(OptionalInputDeviceDriverType a, InputDeviceDriverType b)
		{
			return !a.hasValue || a.value != b;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00013486 File Offset: 0x00011686
		private static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001348F File Offset: 0x0001168F
		public override int GetHashCode()
		{
			return OptionalInputDeviceDriverType.CombineHashCodes(this.hasValue.GetHashCode(), this.value.GetHashCode());
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x000134B2 File Offset: 0x000116B2
		public override string ToString()
		{
			if (!this.hasValue)
			{
				return "";
			}
			return this.value.ToString();
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000134D3 File Offset: 0x000116D3
		public static implicit operator OptionalInputDeviceDriverType(InputDeviceDriverType value)
		{
			return new OptionalInputDeviceDriverType(value);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000134DB File Offset: 0x000116DB
		public static explicit operator InputDeviceDriverType(OptionalInputDeviceDriverType optional)
		{
			return optional.Value;
		}

		// Token: 0x0400046E RID: 1134
		[SerializeField]
		private bool hasValue;

		// Token: 0x0400046F RID: 1135
		[SerializeField]
		private InputDeviceDriverType value;
	}
}
