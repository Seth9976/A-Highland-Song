using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000194 RID: 404
	[NativeHeader("Runtime/Graphics/Mesh/MeshRenderer.h")]
	public class MeshRenderer : Renderer
	{
		// Token: 0x06000EE3 RID: 3811 RVA: 0x00004557 File Offset: 0x00002757
		[RequiredByNativeCode]
		private void DontStripMeshRenderer()
		{
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000EE4 RID: 3812
		// (set) Token: 0x06000EE5 RID: 3813
		public extern Mesh additionalVertexStreams
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000EE6 RID: 3814
		// (set) Token: 0x06000EE7 RID: 3815
		public extern Mesh enlightenVertexStream
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000EE8 RID: 3816
		public extern int subMeshStartIndex
		{
			[NativeName("GetSubMeshStartIndex")]
			[MethodImpl(4096)]
			get;
		}
	}
}
