using System;

namespace Ink.Runtime
{
	// Token: 0x0200003A RID: 58
	public class VariableAssignment : Object
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0001344A File Offset: 0x0001164A
		// (set) Token: 0x0600035F RID: 863 RVA: 0x00013452 File Offset: 0x00011652
		public string variableName { get; protected set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0001345B File Offset: 0x0001165B
		// (set) Token: 0x06000361 RID: 865 RVA: 0x00013463 File Offset: 0x00011663
		public bool isNewDeclaration { get; protected set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0001346C File Offset: 0x0001166C
		// (set) Token: 0x06000363 RID: 867 RVA: 0x00013474 File Offset: 0x00011674
		public bool isGlobal { get; set; }

		// Token: 0x06000364 RID: 868 RVA: 0x0001347D File Offset: 0x0001167D
		public VariableAssignment(string variableName, bool isNewDeclaration)
		{
			this.variableName = variableName;
			this.isNewDeclaration = isNewDeclaration;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00013493 File Offset: 0x00011693
		public VariableAssignment()
			: this(null, false)
		{
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0001349D File Offset: 0x0001169D
		public override string ToString()
		{
			return "VarAssign to " + this.variableName;
		}
	}
}
