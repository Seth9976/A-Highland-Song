using System;
using System.Runtime.CompilerServices;

// Token: 0x0200020C RID: 524
[NullableContext(1)]
[Nullable(0)]
public struct Presume<T> where T : class
{
	// Token: 0x0600135E RID: 4958 RVA: 0x000888C0 File Offset: 0x00086AC0
	[return: Nullable(new byte[] { 0, 1 })]
	public static Presume<T> NonNull([CallerFilePath] string filepath = "", [CallerLineNumber] int lineNumber = 0)
	{
		return new Presume<T>
		{
			filepath = filepath,
			lineNumber = lineNumber
		};
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x000888E8 File Offset: 0x00086AE8
	public static implicit operator T([Nullable(new byte[] { 0, 1 })] Presume<T> presumption)
	{
		return default(T);
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x00088900 File Offset: 0x00086B00
	public override bool Equals(object obj)
	{
		if (!(obj is Presume<T>))
		{
			return false;
		}
		Presume<T> presume = (Presume<T>)obj;
		return presume.lineNumber == this.lineNumber && presume.filepath == this.filepath;
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x0008893F File Offset: 0x00086B3F
	public override int GetHashCode()
	{
		return (17 * 31 + this.filepath.GetHashCode()) * 31 + this.lineNumber;
	}

	// Token: 0x040012AE RID: 4782
	public string filepath;

	// Token: 0x040012AF RID: 4783
	public int lineNumber;
}
