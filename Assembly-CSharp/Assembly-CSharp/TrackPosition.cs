using System;
using System.Runtime.CompilerServices;

// Token: 0x02000084 RID: 132
[NullableContext(2)]
[Nullable(0)]
public struct TrackPosition
{
	// Token: 0x17000113 RID: 275
	// (get) Token: 0x060003E3 RID: 995 RVA: 0x0001FE60 File Offset: 0x0001E060
	public bool isValid
	{
		get
		{
			return !float.IsInfinity(this.x);
		}
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x0001FE70 File Offset: 0x0001E070
	[NullableContext(1)]
	public override string ToString()
	{
		string text = ((this.slope != null) ? this.slope.name : "NULL");
		return string.Format("(Slope={0}, x={1})", text, this.x);
	}

	// Token: 0x04000519 RID: 1305
	public Chunk chunk;

	// Token: 0x0400051A RID: 1306
	public Slope slope;

	// Token: 0x0400051B RID: 1307
	public float x;

	// Token: 0x0400051C RID: 1308
	public static TrackPosition none = new TrackPosition
	{
		x = float.PositiveInfinity
	};
}
