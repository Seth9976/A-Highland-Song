using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x0200047C RID: 1148
	[NativeHeader("Runtime/Shaders/RayTracingAccelerationStructure.h")]
	[NativeHeader("Runtime/Export/Graphics/RayTracingAccelerationStructure.bindings.h")]
	[Flags]
	[UsedByNativeCode]
	public enum RayTracingSubMeshFlags
	{
		// Token: 0x04000F83 RID: 3971
		Disabled = 0,
		// Token: 0x04000F84 RID: 3972
		Enabled = 1,
		// Token: 0x04000F85 RID: 3973
		ClosestHitOnly = 2,
		// Token: 0x04000F86 RID: 3974
		UniqueAnyHitCalls = 4
	}
}
