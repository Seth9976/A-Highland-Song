using System;

namespace Ink.Runtime
{
	// Token: 0x0200001B RID: 27
	public interface INamedContent
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000191 RID: 401
		string name { get; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000192 RID: 402
		bool hasValidName { get; }
	}
}
