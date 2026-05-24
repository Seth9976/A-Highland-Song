using System;

namespace Shapes
{
	// Token: 0x0200004C RID: 76
	internal static class PolylineJoinsExtensions
	{
		// Token: 0x06000A04 RID: 2564 RVA: 0x00023C9A File Offset: 0x00021E9A
		public static bool HasJoinMesh(this PolylineJoins join)
		{
			switch (join)
			{
			case PolylineJoins.Simple:
				return false;
			case PolylineJoins.Miter:
				return false;
			case PolylineJoins.Round:
				return true;
			case PolylineJoins.Bevel:
				return true;
			default:
				throw new ArgumentOutOfRangeException("join", join, null);
			}
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00023CCD File Offset: 0x00021ECD
		public static bool HasSimpleJoin(this PolylineJoins join)
		{
			switch (join)
			{
			case PolylineJoins.Simple:
				return false;
			case PolylineJoins.Miter:
				return false;
			case PolylineJoins.Round:
				return false;
			case PolylineJoins.Bevel:
				return true;
			default:
				throw new ArgumentOutOfRangeException("join", join, null);
			}
		}
	}
}
