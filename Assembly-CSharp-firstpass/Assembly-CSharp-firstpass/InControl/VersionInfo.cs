using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200007C RID: 124
	[Serializable]
	public struct VersionInfo : IComparable<VersionInfo>
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x00014CC3 File Offset: 0x00012EC3
		public VersionInfo(int major, int minor, int patch, int build)
		{
			this.major = major;
			this.minor = minor;
			this.patch = patch;
			this.build = build;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00014CE4 File Offset: 0x00012EE4
		public static VersionInfo InControlVersion()
		{
			return new VersionInfo
			{
				major = 1,
				minor = 8,
				patch = 9,
				build = 9376
			};
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00014D20 File Offset: 0x00012F20
		public static VersionInfo UnityVersion()
		{
			Match match = Regex.Match(Application.unityVersion, "^(\\d+)\\.(\\d+)\\.(\\d+)[a-zA-Z](\\d+)");
			return new VersionInfo
			{
				major = Convert.ToInt32(match.Groups[1].Value),
				minor = Convert.ToInt32(match.Groups[2].Value),
				patch = Convert.ToInt32(match.Groups[3].Value),
				build = Convert.ToInt32(match.Groups[4].Value)
			};
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x00014DBA File Offset: 0x00012FBA
		public static VersionInfo Min
		{
			get
			{
				return new VersionInfo(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x00014DD5 File Offset: 0x00012FD5
		public static VersionInfo Max
		{
			get
			{
				return new VersionInfo(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x00014DF0 File Offset: 0x00012FF0
		public VersionInfo Next
		{
			get
			{
				return new VersionInfo(this.major, this.minor, this.patch, this.build + 1);
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00014E11 File Offset: 0x00013011
		public int Build
		{
			get
			{
				return this.build;
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00014E1C File Offset: 0x0001301C
		public int CompareTo(VersionInfo other)
		{
			if (this.major < other.major)
			{
				return -1;
			}
			if (this.major > other.major)
			{
				return 1;
			}
			if (this.minor < other.minor)
			{
				return -1;
			}
			if (this.minor > other.minor)
			{
				return 1;
			}
			if (this.patch < other.patch)
			{
				return -1;
			}
			if (this.patch > other.patch)
			{
				return 1;
			}
			if (this.build < other.build)
			{
				return -1;
			}
			if (this.build > other.build)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00014EAA File Offset: 0x000130AA
		public static bool operator ==(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) == 0;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00014EB7 File Offset: 0x000130B7
		public static bool operator !=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) != 0;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00014EC4 File Offset: 0x000130C4
		public static bool operator <=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) <= 0;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00014ED4 File Offset: 0x000130D4
		public static bool operator >=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) >= 0;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00014EE4 File Offset: 0x000130E4
		public static bool operator <(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) < 0;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00014EF1 File Offset: 0x000130F1
		public static bool operator >(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) > 0;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00014EFE File Offset: 0x000130FE
		public override bool Equals(object other)
		{
			return other is VersionInfo && this == (VersionInfo)other;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00014F1B File Offset: 0x0001311B
		public override int GetHashCode()
		{
			return this.major.GetHashCode() ^ this.minor.GetHashCode() ^ this.patch.GetHashCode() ^ this.build.GetHashCode();
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00014F4C File Offset: 0x0001314C
		public override string ToString()
		{
			if (this.build == 0)
			{
				return string.Format("{0}.{1}.{2}", this.major, this.minor, this.patch);
			}
			return string.Format("{0}.{1}.{2} build {3}", new object[] { this.major, this.minor, this.patch, this.build });
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00014FD8 File Offset: 0x000131D8
		public string ToShortString()
		{
			if (this.build == 0)
			{
				return string.Format("{0}.{1}.{2}", this.major, this.minor, this.patch);
			}
			return string.Format("{0}.{1}.{2}b{3}", new object[] { this.major, this.minor, this.patch, this.build });
		}

		// Token: 0x04000484 RID: 1156
		[SerializeField]
		private int major;

		// Token: 0x04000485 RID: 1157
		[SerializeField]
		private int minor;

		// Token: 0x04000486 RID: 1158
		[SerializeField]
		private int patch;

		// Token: 0x04000487 RID: 1159
		[SerializeField]
		private int build;
	}
}
