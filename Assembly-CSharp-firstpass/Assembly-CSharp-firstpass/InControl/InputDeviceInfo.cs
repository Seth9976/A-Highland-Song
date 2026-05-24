using System;
using System.Runtime.InteropServices;

namespace InControl
{
	// Token: 0x02000042 RID: 66
	public struct InputDeviceInfo
	{
		// Token: 0x060002BE RID: 702 RVA: 0x0000A0CF File Offset: 0x000082CF
		public bool HasSameVendorID(InputDeviceInfo deviceInfo)
		{
			return this.vendorID == deviceInfo.vendorID;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000A0DF File Offset: 0x000082DF
		public bool HasSameProductID(InputDeviceInfo deviceInfo)
		{
			return this.productID == deviceInfo.productID;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000A0EF File Offset: 0x000082EF
		public bool HasSameVersionNumber(InputDeviceInfo deviceInfo)
		{
			return this.versionNumber == deviceInfo.versionNumber;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000A0FF File Offset: 0x000082FF
		public bool HasSameLocation(InputDeviceInfo deviceInfo)
		{
			return !string.IsNullOrEmpty(this.location) && this.location == deviceInfo.location;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000A121 File Offset: 0x00008321
		public bool HasSameSerialNumber(InputDeviceInfo deviceInfo)
		{
			return !string.IsNullOrEmpty(this.serialNumber) && this.serialNumber == deviceInfo.serialNumber;
		}

		// Token: 0x040002F4 RID: 756
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string name;

		// Token: 0x040002F5 RID: 757
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string location;

		// Token: 0x040002F6 RID: 758
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string serialNumber;

		// Token: 0x040002F7 RID: 759
		public ushort vendorID;

		// Token: 0x040002F8 RID: 760
		public ushort productID;

		// Token: 0x040002F9 RID: 761
		public uint versionNumber;

		// Token: 0x040002FA RID: 762
		public InputDeviceDriverType driverType;

		// Token: 0x040002FB RID: 763
		public InputDeviceTransportType transportType;

		// Token: 0x040002FC RID: 764
		public uint numButtons;

		// Token: 0x040002FD RID: 765
		public uint numAnalogs;
	}
}
