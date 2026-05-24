using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200041E RID: 1054
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	[UsedByNativeCode]
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	public readonly struct LocalKeyword : IEquatable<LocalKeyword>
	{
		// Token: 0x060024AB RID: 9387 RVA: 0x0003E154 File Offset: 0x0003C354
		[FreeFunction("keywords::IsKeywordOverridable")]
		private static bool IsOverridable(LocalKeyword kw)
		{
			return LocalKeyword.IsOverridable_Injected(ref kw);
		}

		// Token: 0x060024AC RID: 9388
		[FreeFunction("ShaderScripting::GetKeywordCount")]
		[MethodImpl(4096)]
		private static extern uint GetShaderKeywordCount(Shader shader);

		// Token: 0x060024AD RID: 9389
		[FreeFunction("ShaderScripting::GetKeywordIndex")]
		[MethodImpl(4096)]
		private static extern uint GetShaderKeywordIndex(Shader shader, string keyword);

		// Token: 0x060024AE RID: 9390
		[FreeFunction("ShaderScripting::GetKeywordCount")]
		[MethodImpl(4096)]
		private static extern uint GetComputeShaderKeywordCount(ComputeShader shader);

		// Token: 0x060024AF RID: 9391
		[FreeFunction("ShaderScripting::GetKeywordIndex")]
		[MethodImpl(4096)]
		private static extern uint GetComputeShaderKeywordIndex(ComputeShader shader, string keyword);

		// Token: 0x060024B0 RID: 9392 RVA: 0x0003E15D File Offset: 0x0003C35D
		[FreeFunction("keywords::GetKeywordType")]
		private static ShaderKeywordType GetKeywordType(LocalKeywordSpace spaceInfo, uint keyword)
		{
			return LocalKeyword.GetKeywordType_Injected(ref spaceInfo, keyword);
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x0003E167 File Offset: 0x0003C367
		[FreeFunction("keywords::IsKeywordValid")]
		private static bool IsValid(LocalKeywordSpace spaceInfo, uint keyword)
		{
			return LocalKeyword.IsValid_Injected(ref spaceInfo, keyword);
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060024B2 RID: 9394 RVA: 0x0003E174 File Offset: 0x0003C374
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060024B3 RID: 9395 RVA: 0x0003E18C File Offset: 0x0003C38C
		public bool isOverridable
		{
			get
			{
				return LocalKeyword.IsOverridable(this);
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060024B4 RID: 9396 RVA: 0x0003E1AC File Offset: 0x0003C3AC
		public bool isValid
		{
			get
			{
				return LocalKeyword.IsValid(this.m_SpaceInfo, this.m_Index);
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060024B5 RID: 9397 RVA: 0x0003E1D0 File Offset: 0x0003C3D0
		public ShaderKeywordType type
		{
			get
			{
				return LocalKeyword.GetKeywordType(this.m_SpaceInfo, this.m_Index);
			}
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x0003E1F4 File Offset: 0x0003C3F4
		public LocalKeyword(Shader shader, string name)
		{
			bool flag = shader == null;
			if (flag)
			{
				Debug.LogError("Cannot initialize a LocalKeyword with a null Shader.");
			}
			this.m_SpaceInfo = shader.keywordSpace;
			this.m_Name = name;
			this.m_Index = LocalKeyword.GetShaderKeywordIndex(shader, name);
			bool flag2 = this.m_Index >= LocalKeyword.GetShaderKeywordCount(shader);
			if (flag2)
			{
				Debug.LogErrorFormat("Local keyword {0} doesn't exist in the shader.", new object[] { name });
			}
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x0003E264 File Offset: 0x0003C464
		public LocalKeyword(ComputeShader shader, string name)
		{
			bool flag = shader == null;
			if (flag)
			{
				Debug.LogError("Cannot initialize a LocalKeyword with a null ComputeShader.");
			}
			this.m_SpaceInfo = shader.keywordSpace;
			this.m_Name = name;
			this.m_Index = LocalKeyword.GetComputeShaderKeywordIndex(shader, name);
			bool flag2 = this.m_Index >= LocalKeyword.GetComputeShaderKeywordCount(shader);
			if (flag2)
			{
				Debug.LogErrorFormat("Local keyword {0} doesn't exist in the compute shader.", new object[] { name });
			}
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x0003E2D4 File Offset: 0x0003C4D4
		public override string ToString()
		{
			return this.m_Name;
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x0003E2EC File Offset: 0x0003C4EC
		public override bool Equals(object o)
		{
			bool flag;
			if (o is LocalKeyword)
			{
				LocalKeyword localKeyword = (LocalKeyword)o;
				flag = this.Equals(localKeyword);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x0003E318 File Offset: 0x0003C518
		public bool Equals(LocalKeyword rhs)
		{
			return this.m_SpaceInfo == rhs.m_SpaceInfo && this.m_Index == rhs.m_Index;
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x0003E350 File Offset: 0x0003C550
		public static bool operator ==(LocalKeyword lhs, LocalKeyword rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x0003E36C File Offset: 0x0003C56C
		public static bool operator !=(LocalKeyword lhs, LocalKeyword rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0003E388 File Offset: 0x0003C588
		public override int GetHashCode()
		{
			return this.m_Index.GetHashCode() ^ this.m_SpaceInfo.GetHashCode();
		}

		// Token: 0x060024BE RID: 9406
		[MethodImpl(4096)]
		private static extern bool IsOverridable_Injected(ref LocalKeyword kw);

		// Token: 0x060024BF RID: 9407
		[MethodImpl(4096)]
		private static extern ShaderKeywordType GetKeywordType_Injected(ref LocalKeywordSpace spaceInfo, uint keyword);

		// Token: 0x060024C0 RID: 9408
		[MethodImpl(4096)]
		private static extern bool IsValid_Injected(ref LocalKeywordSpace spaceInfo, uint keyword);

		// Token: 0x04000DA0 RID: 3488
		internal readonly LocalKeywordSpace m_SpaceInfo;

		// Token: 0x04000DA1 RID: 3489
		internal readonly string m_Name;

		// Token: 0x04000DA2 RID: 3490
		internal readonly uint m_Index;
	}
}
