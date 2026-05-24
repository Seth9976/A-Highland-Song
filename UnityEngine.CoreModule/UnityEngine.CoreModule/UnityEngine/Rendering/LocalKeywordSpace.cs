using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x0200041F RID: 1055
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	public readonly struct LocalKeywordSpace : IEquatable<LocalKeywordSpace>
	{
		// Token: 0x060024C1 RID: 9409 RVA: 0x0003E3BA File Offset: 0x0003C5BA
		[FreeFunction("keywords::GetKeywords", HasExplicitThis = true)]
		private LocalKeyword[] GetKeywords()
		{
			return LocalKeywordSpace.GetKeywords_Injected(ref this);
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x0003E3C2 File Offset: 0x0003C5C2
		[FreeFunction("keywords::GetKeywordNames", HasExplicitThis = true)]
		private string[] GetKeywordNames()
		{
			return LocalKeywordSpace.GetKeywordNames_Injected(ref this);
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x0003E3CA File Offset: 0x0003C5CA
		[FreeFunction("keywords::GetKeywordCount", HasExplicitThis = true)]
		private uint GetKeywordCount()
		{
			return LocalKeywordSpace.GetKeywordCount_Injected(ref this);
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x0003E3D4 File Offset: 0x0003C5D4
		[FreeFunction("keywords::GetKeyword", HasExplicitThis = true)]
		private LocalKeyword GetKeyword(string name)
		{
			LocalKeyword localKeyword;
			LocalKeywordSpace.GetKeyword_Injected(ref this, name, out localKeyword);
			return localKeyword;
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060024C5 RID: 9413 RVA: 0x0003E3EC File Offset: 0x0003C5EC
		public LocalKeyword[] keywords
		{
			get
			{
				return this.GetKeywords();
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060024C6 RID: 9414 RVA: 0x0003E404 File Offset: 0x0003C604
		public string[] keywordNames
		{
			get
			{
				return this.GetKeywordNames();
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060024C7 RID: 9415 RVA: 0x0003E41C File Offset: 0x0003C61C
		public uint keywordCount
		{
			get
			{
				return this.GetKeywordCount();
			}
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x0003E434 File Offset: 0x0003C634
		public LocalKeyword FindKeyword(string name)
		{
			return this.GetKeyword(name);
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x0003E450 File Offset: 0x0003C650
		public override bool Equals(object o)
		{
			bool flag;
			if (o is LocalKeywordSpace)
			{
				LocalKeywordSpace localKeywordSpace = (LocalKeywordSpace)o;
				flag = this.Equals(localKeywordSpace);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x0003E47C File Offset: 0x0003C67C
		public bool Equals(LocalKeywordSpace rhs)
		{
			return this.m_KeywordSpace == rhs.m_KeywordSpace;
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x0003E4A0 File Offset: 0x0003C6A0
		public static bool operator ==(LocalKeywordSpace lhs, LocalKeywordSpace rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x0003E4BC File Offset: 0x0003C6BC
		public static bool operator !=(LocalKeywordSpace lhs, LocalKeywordSpace rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x0003E4D8 File Offset: 0x0003C6D8
		public override int GetHashCode()
		{
			return this.m_KeywordSpace.GetHashCode();
		}

		// Token: 0x060024CE RID: 9422
		[MethodImpl(4096)]
		private static extern LocalKeyword[] GetKeywords_Injected(ref LocalKeywordSpace _unity_self);

		// Token: 0x060024CF RID: 9423
		[MethodImpl(4096)]
		private static extern string[] GetKeywordNames_Injected(ref LocalKeywordSpace _unity_self);

		// Token: 0x060024D0 RID: 9424
		[MethodImpl(4096)]
		private static extern uint GetKeywordCount_Injected(ref LocalKeywordSpace _unity_self);

		// Token: 0x060024D1 RID: 9425
		[MethodImpl(4096)]
		private static extern void GetKeyword_Injected(ref LocalKeywordSpace _unity_self, string name, out LocalKeyword ret);

		// Token: 0x04000DA3 RID: 3491
		private readonly IntPtr m_KeywordSpace;
	}
}
