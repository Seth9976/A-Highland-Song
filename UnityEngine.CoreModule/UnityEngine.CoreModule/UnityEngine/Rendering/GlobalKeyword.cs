using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200041D RID: 1053
	[UsedByNativeCode]
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	public readonly struct GlobalKeyword
	{
		// Token: 0x060024A4 RID: 9380
		[FreeFunction("ShaderScripting::GetGlobalKeywordCount")]
		[MethodImpl(4096)]
		private static extern uint GetGlobalKeywordCount();

		// Token: 0x060024A5 RID: 9381
		[FreeFunction("ShaderScripting::GetGlobalKeywordIndex")]
		[MethodImpl(4096)]
		private static extern uint GetGlobalKeywordIndex(string keyword);

		// Token: 0x060024A6 RID: 9382
		[FreeFunction("ShaderScripting::CreateGlobalKeyword")]
		[MethodImpl(4096)]
		private static extern void CreateGlobalKeyword(string keyword);

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060024A7 RID: 9383 RVA: 0x0003E0B8 File Offset: 0x0003C2B8
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x0003E0D0 File Offset: 0x0003C2D0
		public static GlobalKeyword Create(string name)
		{
			GlobalKeyword.CreateGlobalKeyword(name);
			return new GlobalKeyword(name);
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x0003E0F0 File Offset: 0x0003C2F0
		public GlobalKeyword(string name)
		{
			this.m_Name = name;
			this.m_Index = GlobalKeyword.GetGlobalKeywordIndex(name);
			bool flag = this.m_Index >= GlobalKeyword.GetGlobalKeywordCount();
			if (flag)
			{
				Debug.LogErrorFormat("Global keyword {0} doesn't exist.", new object[] { name });
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x0003E13C File Offset: 0x0003C33C
		public override string ToString()
		{
			return this.m_Name;
		}

		// Token: 0x04000D9E RID: 3486
		internal readonly string m_Name;

		// Token: 0x04000D9F RID: 3487
		internal readonly uint m_Index;
	}
}
