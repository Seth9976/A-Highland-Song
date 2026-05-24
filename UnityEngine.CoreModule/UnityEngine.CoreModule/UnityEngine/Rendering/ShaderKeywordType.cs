using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000422 RID: 1058
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	[UsedByNativeCode]
	public enum ShaderKeywordType
	{
		// Token: 0x04000DA9 RID: 3497
		None,
		// Token: 0x04000DAA RID: 3498
		BuiltinDefault = 2,
		// Token: 0x04000DAB RID: 3499
		[Obsolete("Shader keyword type BuiltinExtra is no longer used. Use BuiltinDefault instead. (UnityUpgradable) -> BuiltinDefault")]
		BuiltinExtra = 6,
		// Token: 0x04000DAC RID: 3500
		[Obsolete("Shader keyword type BuiltinAutoStripped is no longer used. Use BuiltinDefault instead. (UnityUpgradable) -> BuiltinDefault")]
		BuiltinAutoStripped = 10,
		// Token: 0x04000DAD RID: 3501
		UserDefined = 16,
		// Token: 0x04000DAE RID: 3502
		Plugin = 32
	}
}
