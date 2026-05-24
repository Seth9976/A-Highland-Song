using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000423 RID: 1059
	[UsedByNativeCode]
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	public struct ShaderKeyword
	{
		// Token: 0x060024DD RID: 9437
		[FreeFunction("ShaderScripting::GetGlobalKeywordCount")]
		[MethodImpl(4096)]
		internal static extern uint GetGlobalKeywordCount();

		// Token: 0x060024DE RID: 9438
		[FreeFunction("ShaderScripting::GetGlobalKeywordIndex")]
		[MethodImpl(4096)]
		internal static extern uint GetGlobalKeywordIndex(string keyword);

		// Token: 0x060024DF RID: 9439
		[FreeFunction("ShaderScripting::GetKeywordCount")]
		[MethodImpl(4096)]
		internal static extern uint GetKeywordCount(Shader shader);

		// Token: 0x060024E0 RID: 9440
		[FreeFunction("ShaderScripting::GetKeywordIndex")]
		[MethodImpl(4096)]
		internal static extern uint GetKeywordIndex(Shader shader, string keyword);

		// Token: 0x060024E1 RID: 9441
		[FreeFunction("ShaderScripting::GetKeywordCount")]
		[MethodImpl(4096)]
		internal static extern uint GetComputeShaderKeywordCount(ComputeShader shader);

		// Token: 0x060024E2 RID: 9442
		[FreeFunction("ShaderScripting::GetKeywordIndex")]
		[MethodImpl(4096)]
		internal static extern uint GetComputeShaderKeywordIndex(ComputeShader shader, string keyword);

		// Token: 0x060024E3 RID: 9443
		[FreeFunction("ShaderScripting::CreateGlobalKeyword")]
		[MethodImpl(4096)]
		internal static extern void CreateGlobalKeyword(string keyword);

		// Token: 0x060024E4 RID: 9444
		[FreeFunction("ShaderScripting::GetKeywordType")]
		[MethodImpl(4096)]
		internal static extern ShaderKeywordType GetGlobalShaderKeywordType(uint keyword);

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060024E5 RID: 9445 RVA: 0x0003E660 File Offset: 0x0003C860
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x0003E678 File Offset: 0x0003C878
		public static ShaderKeywordType GetGlobalKeywordType(ShaderKeyword index)
		{
			bool flag = index.IsValid();
			ShaderKeywordType shaderKeywordType;
			if (flag)
			{
				shaderKeywordType = ShaderKeyword.GetGlobalShaderKeywordType(index.m_Index);
			}
			else
			{
				shaderKeywordType = ShaderKeywordType.UserDefined;
			}
			return shaderKeywordType;
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x0003E6A8 File Offset: 0x0003C8A8
		public ShaderKeyword(string keywordName)
		{
			this.m_Name = keywordName;
			this.m_Index = ShaderKeyword.GetGlobalKeywordIndex(keywordName);
			bool flag = this.m_Index >= ShaderKeyword.GetGlobalKeywordCount();
			if (flag)
			{
				ShaderKeyword.CreateGlobalKeyword(keywordName);
				this.m_Index = ShaderKeyword.GetGlobalKeywordIndex(keywordName);
			}
			this.m_IsValid = true;
			this.m_IsLocal = false;
			this.m_IsCompute = false;
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x0003E707 File Offset: 0x0003C907
		public ShaderKeyword(Shader shader, string keywordName)
		{
			this.m_Name = keywordName;
			this.m_Index = ShaderKeyword.GetKeywordIndex(shader, keywordName);
			this.m_IsValid = this.m_Index < ShaderKeyword.GetKeywordCount(shader);
			this.m_IsLocal = true;
			this.m_IsCompute = false;
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x0003E740 File Offset: 0x0003C940
		public ShaderKeyword(ComputeShader shader, string keywordName)
		{
			this.m_Name = keywordName;
			this.m_Index = ShaderKeyword.GetComputeShaderKeywordIndex(shader, keywordName);
			this.m_IsValid = this.m_Index < ShaderKeyword.GetComputeShaderKeywordCount(shader);
			this.m_IsLocal = true;
			this.m_IsCompute = true;
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x0003E77C File Offset: 0x0003C97C
		public static bool IsKeywordLocal(ShaderKeyword keyword)
		{
			return keyword.m_IsLocal;
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x0003E794 File Offset: 0x0003C994
		public bool IsValid()
		{
			return this.m_IsValid;
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x0003E7AC File Offset: 0x0003C9AC
		public bool IsValid(ComputeShader shader)
		{
			return this.m_IsValid;
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x0003E7C4 File Offset: 0x0003C9C4
		public bool IsValid(Shader shader)
		{
			return this.m_IsValid;
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060024EE RID: 9454 RVA: 0x0003E7DC File Offset: 0x0003C9DC
		public int index
		{
			get
			{
				return (int)this.m_Index;
			}
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x0003E7F4 File Offset: 0x0003C9F4
		public override string ToString()
		{
			return this.m_Name;
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x0003E80C File Offset: 0x0003CA0C
		[Obsolete("GetKeywordType is deprecated. Only global keywords can have a type. This method always returns ShaderKeywordType.UserDefined.")]
		public static ShaderKeywordType GetKeywordType(Shader shader, ShaderKeyword index)
		{
			return ShaderKeywordType.UserDefined;
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x0003E820 File Offset: 0x0003CA20
		[Obsolete("GetKeywordType is deprecated. Only global keywords can have a type. This method always returns ShaderKeywordType.UserDefined.")]
		public static ShaderKeywordType GetKeywordType(ComputeShader shader, ShaderKeyword index)
		{
			return ShaderKeywordType.UserDefined;
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x0003E834 File Offset: 0x0003CA34
		[Obsolete("GetGlobalKeywordName is deprecated. Use the ShaderKeyword.name property instead.")]
		public static string GetGlobalKeywordName(ShaderKeyword index)
		{
			return index.m_Name;
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x0003E84C File Offset: 0x0003CA4C
		[Obsolete("GetKeywordName is deprecated. Use the ShaderKeyword.name property instead.")]
		public static string GetKeywordName(Shader shader, ShaderKeyword index)
		{
			return index.m_Name;
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x0003E864 File Offset: 0x0003CA64
		[Obsolete("GetKeywordName is deprecated. Use the ShaderKeyword.name property instead.")]
		public static string GetKeywordName(ComputeShader shader, ShaderKeyword index)
		{
			return index.m_Name;
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x0003E87C File Offset: 0x0003CA7C
		[Obsolete("GetKeywordType is deprecated. Use ShaderKeyword.name instead.")]
		public ShaderKeywordType GetKeywordType()
		{
			return ShaderKeyword.GetGlobalKeywordType(this);
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x0003E89C File Offset: 0x0003CA9C
		[Obsolete("GetKeywordName is deprecated. Use ShaderKeyword.name instead.")]
		public string GetKeywordName()
		{
			return ShaderKeyword.GetGlobalKeywordName(this);
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x0003E8BC File Offset: 0x0003CABC
		[Obsolete("GetName() has been deprecated. Use ShaderKeyword.name instead.")]
		public string GetName()
		{
			return this.GetKeywordName();
		}

		// Token: 0x04000DAF RID: 3503
		internal string m_Name;

		// Token: 0x04000DB0 RID: 3504
		internal uint m_Index;

		// Token: 0x04000DB1 RID: 3505
		internal bool m_IsLocal;

		// Token: 0x04000DB2 RID: 3506
		internal bool m_IsCompute;

		// Token: 0x04000DB3 RID: 3507
		internal bool m_IsValid;
	}
}
