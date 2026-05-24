using System;

namespace Shapes
{
	// Token: 0x02000034 RID: 52
	public static class AngularUnitExtensions
	{
		// Token: 0x060009DF RID: 2527 RVA: 0x00023864 File Offset: 0x00021A64
		public static string Suffix(this AngularUnit unit)
		{
			return AngularUnitExtensions.angUnitToSuffix[(int)unit];
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0002386D File Offset: 0x00021A6D
		public static string Name(this AngularUnit unit)
		{
			return AngularUnitExtensions.angUnitNames[(int)unit];
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00023876 File Offset: 0x00021A76
		public static string NameShort(this AngularUnit unit)
		{
			return AngularUnitExtensions.angUnitNamesShort[(int)unit];
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0002387F File Offset: 0x00021A7F
		public static float FromRadians(this AngularUnit unit)
		{
			return 1f / unit.ToRadians();
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0002388D File Offset: 0x00021A8D
		public static float ToRadians(this AngularUnit unit)
		{
			switch (unit)
			{
			case AngularUnit.Radians:
				return 1f;
			case AngularUnit.Degrees:
				return 0.017453292f;
			case AngularUnit.Turns:
				return 6.2831855f;
			default:
				throw new ArgumentOutOfRangeException("unit", unit, null);
			}
		}

		// Token: 0x04000147 RID: 327
		public static string[] angUnitToSuffix = new string[] { "rad", "°", "tr" };

		// Token: 0x04000148 RID: 328
		public static string[] angUnitNames = new string[] { "Radians", "Degrees", "Turns" };

		// Token: 0x04000149 RID: 329
		public static string[] angUnitNamesShort = new string[] { "Rad", "Deg", "Turns" };
	}
}
