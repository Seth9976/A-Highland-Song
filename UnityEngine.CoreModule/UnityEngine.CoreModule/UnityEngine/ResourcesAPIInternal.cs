using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngineInternal;

namespace UnityEngine
{
	// Token: 0x020001E8 RID: 488
	[NativeHeader("Runtime/Misc/ResourceManagerUtility.h")]
	[NativeHeader("Runtime/Export/Resources/Resources.bindings.h")]
	internal static class ResourcesAPIInternal
	{
		// Token: 0x06001610 RID: 5648
		[FreeFunction("Resources_Bindings::FindObjectsOfTypeAll")]
		[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
		[MethodImpl(4096)]
		public static extern Object[] FindObjectsOfTypeAll(Type type);

		// Token: 0x06001611 RID: 5649
		[FreeFunction("GetShaderNameRegistry().FindShader")]
		[MethodImpl(4096)]
		public static extern Shader FindShaderByName(string name);

		// Token: 0x06001612 RID: 5650
		[NativeThrows]
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
		[FreeFunction("Resources_Bindings::Load")]
		[MethodImpl(4096)]
		public static extern Object Load(string path, [NotNull("ArgumentNullException")] Type systemTypeInstance);

		// Token: 0x06001613 RID: 5651
		[FreeFunction("Resources_Bindings::LoadAll")]
		[NativeThrows]
		[MethodImpl(4096)]
		public static extern Object[] LoadAll([NotNull("ArgumentNullException")] string path, [NotNull("ArgumentNullException")] Type systemTypeInstance);

		// Token: 0x06001614 RID: 5652
		[FreeFunction("Resources_Bindings::LoadAsyncInternal")]
		[MethodImpl(4096)]
		internal static extern ResourceRequest LoadAsyncInternal(string path, Type type);

		// Token: 0x06001615 RID: 5653
		[FreeFunction("Scripting::UnloadAssetFromScripting")]
		[MethodImpl(4096)]
		public static extern void UnloadAsset(Object assetToUnload);
	}
}
