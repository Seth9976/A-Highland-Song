using System;

namespace UnityEngine.Playables
{
	// Token: 0x0200044A RID: 1098
	public static class ScriptPlayableBinding
	{
		// Token: 0x060026D5 RID: 9941 RVA: 0x00040CA0 File Offset: 0x0003EEA0
		public static PlayableBinding Create(string name, Object key, Type type)
		{
			return PlayableBinding.CreateInternal(name, key, type, new PlayableBinding.CreateOutputMethod(ScriptPlayableBinding.CreateScriptOutput));
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x00040CC8 File Offset: 0x0003EEC8
		private static PlayableOutput CreateScriptOutput(PlayableGraph graph, string name)
		{
			return ScriptPlayableOutput.Create(graph, name);
		}
	}
}
