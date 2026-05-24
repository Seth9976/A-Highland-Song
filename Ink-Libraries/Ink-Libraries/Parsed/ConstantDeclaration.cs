using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000042 RID: 66
	public class ConstantDeclaration : Object
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00014928 File Offset: 0x00012B28
		public string constantName
		{
			get
			{
				Identifier constantIdentifier = this.constantIdentifier;
				if (constantIdentifier == null)
				{
					return null;
				}
				return constantIdentifier.name;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0001493B File Offset: 0x00012B3B
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x00014943 File Offset: 0x00012B43
		public Identifier constantIdentifier { get; protected set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0001494C File Offset: 0x00012B4C
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x00014954 File Offset: 0x00012B54
		public Expression expression { get; protected set; }

		// Token: 0x060003C8 RID: 968 RVA: 0x0001495D File Offset: 0x00012B5D
		public ConstantDeclaration(Identifier name, Expression assignedExpression)
		{
			this.constantIdentifier = name;
			if (assignedExpression)
			{
				this.expression = base.AddContent<Expression>(assignedExpression);
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00014981 File Offset: 0x00012B81
		public override Object GenerateRuntimeObject()
		{
			return null;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00014984 File Offset: 0x00012B84
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			context.CheckForNamingCollisions(this, this.constantIdentifier, Story.SymbolType.Var, null);
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0001499C File Offset: 0x00012B9C
		public override string typeName
		{
			get
			{
				return "Constant";
			}
		}
	}
}
