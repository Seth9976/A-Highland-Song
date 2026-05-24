using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000045 RID: 69
	[Serializable]
	public struct InputDeviceMatcher
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000A160 File Offset: 0x00008360
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000A168 File Offset: 0x00008368
		public OptionalUInt16 VendorID
		{
			get
			{
				return this.vendorID;
			}
			set
			{
				this.vendorID = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000A171 File Offset: 0x00008371
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000A179 File Offset: 0x00008379
		public OptionalUInt16 ProductID
		{
			get
			{
				return this.productID;
			}
			set
			{
				this.productID = value;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000A182 File Offset: 0x00008382
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000A18A File Offset: 0x0000838A
		public OptionalUInt32 VersionNumber
		{
			get
			{
				return this.versionNumber;
			}
			set
			{
				this.versionNumber = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000A193 File Offset: 0x00008393
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000A19B File Offset: 0x0000839B
		public OptionalInputDeviceDriverType DriverType
		{
			get
			{
				return this.driverType;
			}
			set
			{
				this.driverType = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000A1A4 File Offset: 0x000083A4
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000A1AC File Offset: 0x000083AC
		public OptionalInputDeviceTransportType TransportType
		{
			get
			{
				return this.transportType;
			}
			set
			{
				this.transportType = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000A1B5 File Offset: 0x000083B5
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x0000A1BD File Offset: 0x000083BD
		public string NameLiteral
		{
			get
			{
				return this.nameLiteral;
			}
			set
			{
				this.nameLiteral = value;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000A1C6 File Offset: 0x000083C6
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x0000A1CE File Offset: 0x000083CE
		public string NamePattern
		{
			get
			{
				return this.namePattern;
			}
			set
			{
				this.namePattern = value;
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000A1D8 File Offset: 0x000083D8
		internal bool Matches(InputDeviceInfo deviceInfo)
		{
			return (!this.VendorID.HasValue || this.VendorID.Value == deviceInfo.vendorID) && (!this.ProductID.HasValue || this.ProductID.Value == deviceInfo.productID) && (!this.VersionNumber.HasValue || this.VersionNumber.Value == deviceInfo.versionNumber) && (!this.DriverType.HasValue || this.DriverType.Value == deviceInfo.driverType) && (!this.TransportType.HasValue || this.TransportType.Value == deviceInfo.transportType) && (this.NameLiteral == null || string.Equals(deviceInfo.name, this.NameLiteral, StringComparison.OrdinalIgnoreCase)) && (this.NamePattern == null || Regex.IsMatch(deviceInfo.name, this.NamePattern, RegexOptions.IgnoreCase));
		}

		// Token: 0x040002FF RID: 767
		[SerializeField]
		[Hexadecimal]
		private OptionalUInt16 vendorID;

		// Token: 0x04000300 RID: 768
		[SerializeField]
		private OptionalUInt16 productID;

		// Token: 0x04000301 RID: 769
		[SerializeField]
		[Hexadecimal]
		private OptionalUInt32 versionNumber;

		// Token: 0x04000302 RID: 770
		[SerializeField]
		private OptionalInputDeviceDriverType driverType;

		// Token: 0x04000303 RID: 771
		[SerializeField]
		private OptionalInputDeviceTransportType transportType;

		// Token: 0x04000304 RID: 772
		[SerializeField]
		private string nameLiteral;

		// Token: 0x04000305 RID: 773
		[SerializeField]
		private string namePattern;
	}
}
