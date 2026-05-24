using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000BD RID: 189
	[AttributeUsage(64, AllowMultiple = true)]
	public sealed class ContractAnnotationAttribute : Attribute
	{
		// Token: 0x06000348 RID: 840 RVA: 0x00005CD5 File Offset: 0x00003ED5
		public ContractAnnotationAttribute([NotNull] string contract)
			: this(contract, false)
		{
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00005CE1 File Offset: 0x00003EE1
		public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
		{
			this.Contract = contract;
			this.ForceFullStates = forceFullStates;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00005CF9 File Offset: 0x00003EF9
		[NotNull]
		public string Contract { get; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00005D01 File Offset: 0x00003F01
		public bool ForceFullStates { get; }
	}
}
