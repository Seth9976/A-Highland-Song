using System;

// Token: 0x02000213 RID: 531
[Serializable]
public struct Version
{
	// Token: 0x06001378 RID: 4984 RVA: 0x00088F28 File Offset: 0x00087128
	public string ToBasicVersionString()
	{
		return string.Format("{0}.{1}.{2}", this.major, this.minor, this.build);
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x00088F58 File Offset: 0x00087158
	public string ToBasicWithSHAString()
	{
		return string.Format("{0}.{1}.{2} {3}", new object[] { this.major, this.minor, this.build, this.gitCommitSHA });
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x00088FA8 File Offset: 0x000871A8
	public override string ToString()
	{
		return string.Format("Version {0}.{1}.{2}{3} {4} {5}", new object[]
		{
			this.major,
			this.minor,
			this.build,
			string.IsNullOrWhiteSpace(this.buildType) ? "" : (" (" + this.buildType + ")"),
			this.gitBranch,
			this.gitCommitSHA
		});
	}

	// Token: 0x040012B5 RID: 4789
	public int major;

	// Token: 0x040012B6 RID: 4790
	public int minor;

	// Token: 0x040012B7 RID: 4791
	public int build;

	// Token: 0x040012B8 RID: 4792
	[Info("For demos or other special versions")]
	[Disable]
	public string buildType;

	// Token: 0x040012B9 RID: 4793
	[Disable]
	public string platform;

	// Token: 0x040012BA RID: 4794
	[Disable]
	public string buildTarget;

	// Token: 0x040012BB RID: 4795
	[Disable]
	public bool isDevelopment;

	// Token: 0x040012BC RID: 4796
	[Disable]
	public string gitBranch;

	// Token: 0x040012BD RID: 4797
	[Disable]
	public string gitCommitSHA;

	// Token: 0x040012BE RID: 4798
	[Disable]
	public string buildDateTimeString;

	// Token: 0x040012BF RID: 4799
	[Disable]
	public string inkCompileDateTimeString;
}
