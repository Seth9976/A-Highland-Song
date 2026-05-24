using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000127 RID: 295
	[NativeType("Runtime/Graphics/RefreshRate.h")]
	public struct RefreshRate : IEquatable<RefreshRate>
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0000C4C0 File Offset: 0x0000A6C0
		public double value
		{
			[MethodImpl(256)]
			get
			{
				return this.numerator / this.denominator;
			}
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0000C4D4 File Offset: 0x0000A6D4
		[MethodImpl(256)]
		public bool Equals(RefreshRate other)
		{
			return this.numerator == other.numerator && this.denominator == other.denominator;
		}

		// Token: 0x040003B8 RID: 952
		[RequiredMember]
		public uint numerator;

		// Token: 0x040003B9 RID: 953
		[RequiredMember]
		public uint denominator;
	}
}
