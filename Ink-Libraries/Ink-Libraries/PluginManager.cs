using System;
using System.Collections.Generic;
using Ink.Parsed;
using Ink.Runtime;

namespace Ink
{
	// Token: 0x0200000C RID: 12
	public class PluginManager
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00006570 File Offset: 0x00004770
		public PluginManager(List<string> pluginNames)
		{
			this._plugins = new List<IPlugin>();
			using (List<string>.Enumerator enumerator = pluginNames.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					throw new Exception("Plugin not found");
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000065D4 File Offset: 0x000047D4
		public void PostParse(Ink.Parsed.Story parsedStory)
		{
			foreach (IPlugin plugin in this._plugins)
			{
				plugin.PostParse(parsedStory);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006628 File Offset: 0x00004828
		public void PostExport(Ink.Parsed.Story parsedStory, Ink.Runtime.Story runtimeStory)
		{
			foreach (IPlugin plugin in this._plugins)
			{
				plugin.PostExport(parsedStory, runtimeStory);
			}
		}

		// Token: 0x04000033 RID: 51
		private List<IPlugin> _plugins;
	}
}
