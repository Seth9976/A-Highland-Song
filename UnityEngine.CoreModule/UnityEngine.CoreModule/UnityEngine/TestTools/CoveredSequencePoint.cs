using System;
using System.Reflection;
using UnityEngine.Bindings;

namespace UnityEngine.TestTools
{
	// Token: 0x0200048C RID: 1164
	[NativeType(CodegenOptions.Custom, "ManagedCoveredSequencePoint", Header = "Runtime/Scripting/ScriptingCoverage.bindings.h")]
	public struct CoveredSequencePoint
	{
		// Token: 0x04000FA1 RID: 4001
		public MethodBase method;

		// Token: 0x04000FA2 RID: 4002
		public uint ilOffset;

		// Token: 0x04000FA3 RID: 4003
		public uint hitCount;

		// Token: 0x04000FA4 RID: 4004
		public string filename;

		// Token: 0x04000FA5 RID: 4005
		public uint line;

		// Token: 0x04000FA6 RID: 4006
		public uint column;
	}
}
