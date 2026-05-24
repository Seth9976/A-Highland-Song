using System;

// Token: 0x02000017 RID: 23
public class DebugOptions : MonoSingleton<DebugOptions>
{
	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000082 RID: 130 RVA: 0x00008FAB File Offset: 0x000071AB
	public static DebugOptionsData opts
	{
		get
		{
			return MonoSingleton<DebugOptions>.instance.options;
		}
	}

	// Token: 0x040000B5 RID: 181
	public DebugOptionsData options;
}
