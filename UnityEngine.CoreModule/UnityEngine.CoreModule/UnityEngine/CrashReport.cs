using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000107 RID: 263
	[NativeHeader("Runtime/Export/CrashReport/CrashReport.bindings.h")]
	public sealed class CrashReport
	{
		// Token: 0x06000620 RID: 1568 RVA: 0x000084C0 File Offset: 0x000066C0
		private static int Compare(CrashReport c1, CrashReport c2)
		{
			long ticks = c1.time.Ticks;
			long ticks2 = c2.time.Ticks;
			bool flag = ticks > ticks2;
			int num;
			if (flag)
			{
				num = 1;
			}
			else
			{
				bool flag2 = ticks < ticks2;
				if (flag2)
				{
					num = -1;
				}
				else
				{
					num = 0;
				}
			}
			return num;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00008510 File Offset: 0x00006710
		private static void PopulateReports()
		{
			object obj = CrashReport.reportsLock;
			lock (obj)
			{
				bool flag = CrashReport.internalReports != null;
				if (!flag)
				{
					string[] reports = CrashReport.GetReports();
					CrashReport.internalReports = new List<CrashReport>(reports.Length);
					foreach (string text in reports)
					{
						double num;
						string reportData = CrashReport.GetReportData(text, out num);
						DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(num);
						CrashReport.internalReports.Add(new CrashReport(text, dateTime, reportData));
					}
					CrashReport.internalReports.Sort(new Comparison<CrashReport>(CrashReport.Compare));
				}
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x000085DC File Offset: 0x000067DC
		public static CrashReport[] reports
		{
			get
			{
				CrashReport.PopulateReports();
				object obj = CrashReport.reportsLock;
				CrashReport[] array;
				lock (obj)
				{
					array = CrashReport.internalReports.ToArray();
				}
				return array;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x00008624 File Offset: 0x00006824
		public static CrashReport lastReport
		{
			get
			{
				CrashReport.PopulateReports();
				object obj = CrashReport.reportsLock;
				lock (obj)
				{
					bool flag = CrashReport.internalReports.Count > 0;
					if (flag)
					{
						return CrashReport.internalReports[CrashReport.internalReports.Count - 1];
					}
				}
				return null;
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00008694 File Offset: 0x00006894
		public static void RemoveAll()
		{
			foreach (CrashReport crashReport in CrashReport.reports)
			{
				crashReport.Remove();
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000086C2 File Offset: 0x000068C2
		private CrashReport(string id, DateTime time, string text)
		{
			this.id = id;
			this.time = time;
			this.text = text;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000086E4 File Offset: 0x000068E4
		public void Remove()
		{
			bool flag = CrashReport.RemoveReport(this.id);
			if (flag)
			{
				object obj = CrashReport.reportsLock;
				lock (obj)
				{
					CrashReport.internalReports.Remove(this);
				}
			}
		}

		// Token: 0x06000627 RID: 1575
		[FreeFunction(Name = "CrashReport_Bindings::GetReports", IsThreadSafe = true)]
		[MethodImpl(4096)]
		private static extern string[] GetReports();

		// Token: 0x06000628 RID: 1576
		[FreeFunction(Name = "CrashReport_Bindings::GetReportData", IsThreadSafe = true)]
		[MethodImpl(4096)]
		private static extern string GetReportData(string id, out double secondsSinceUnixEpoch);

		// Token: 0x06000629 RID: 1577
		[FreeFunction(Name = "CrashReport_Bindings::RemoveReport", IsThreadSafe = true)]
		[MethodImpl(4096)]
		private static extern bool RemoveReport(string id);

		// Token: 0x04000378 RID: 888
		private static List<CrashReport> internalReports;

		// Token: 0x04000379 RID: 889
		private static object reportsLock = new object();

		// Token: 0x0400037A RID: 890
		private readonly string id;

		// Token: 0x0400037B RID: 891
		public readonly DateTime time;

		// Token: 0x0400037C RID: 892
		public readonly string text;
	}
}
