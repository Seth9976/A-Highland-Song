using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x02000482 RID: 1154
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	public static class ShaderWarmup
	{
		// Token: 0x06002890 RID: 10384 RVA: 0x00043128 File Offset: 0x00041328
		[FreeFunction(Name = "ShaderWarmupScripting::WarmupShader")]
		public static void WarmupShader(Shader shader, ShaderWarmupSetup setup)
		{
			ShaderWarmup.WarmupShader_Injected(shader, ref setup);
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x00043132 File Offset: 0x00041332
		[FreeFunction(Name = "ShaderWarmupScripting::WarmupShaderFromCollection")]
		public static void WarmupShaderFromCollection(ShaderVariantCollection collection, Shader shader, ShaderWarmupSetup setup)
		{
			ShaderWarmup.WarmupShaderFromCollection_Injected(collection, shader, ref setup);
		}

		// Token: 0x06002892 RID: 10386
		[MethodImpl(4096)]
		private static extern void WarmupShader_Injected(Shader shader, ref ShaderWarmupSetup setup);

		// Token: 0x06002893 RID: 10387
		[MethodImpl(4096)]
		private static extern void WarmupShaderFromCollection_Injected(ShaderVariantCollection collection, Shader shader, ref ShaderWarmupSetup setup);
	}
}
