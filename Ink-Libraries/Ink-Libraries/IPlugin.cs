using System;
using Ink.Parsed;
using Ink.Runtime;

namespace Ink
{
	// Token: 0x0200000B RID: 11
	public interface IPlugin
	{
		// Token: 0x060000A7 RID: 167
		void PostParse(Ink.Parsed.Story parsedStory);

		// Token: 0x060000A8 RID: 168
		void PostExport(Ink.Parsed.Story parsedStory, Ink.Runtime.Story runtimeStory);
	}
}
