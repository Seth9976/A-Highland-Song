using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x0200023A RID: 570
	public sealed class ShaderVariantCollection : Object
	{
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001870 RID: 6256
		public extern int shaderCount
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001871 RID: 6257
		public extern int variantCount
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001872 RID: 6258
		public extern bool isWarmedUp
		{
			[NativeName("IsWarmedUp")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x06001873 RID: 6259
		[MethodImpl(4096)]
		private extern bool AddVariant(Shader shader, PassType passType, string[] keywords);

		// Token: 0x06001874 RID: 6260
		[MethodImpl(4096)]
		private extern bool RemoveVariant(Shader shader, PassType passType, string[] keywords);

		// Token: 0x06001875 RID: 6261
		[MethodImpl(4096)]
		private extern bool ContainsVariant(Shader shader, PassType passType, string[] keywords);

		// Token: 0x06001876 RID: 6262
		[NativeName("ClearVariants")]
		[MethodImpl(4096)]
		public extern void Clear();

		// Token: 0x06001877 RID: 6263
		[NativeName("WarmupShaders")]
		[MethodImpl(4096)]
		public extern void WarmUp();

		// Token: 0x06001878 RID: 6264
		[NativeName("CreateFromScript")]
		[MethodImpl(4096)]
		private static extern void Internal_Create([Writable] ShaderVariantCollection svc);

		// Token: 0x06001879 RID: 6265 RVA: 0x0002785B File Offset: 0x00025A5B
		public ShaderVariantCollection()
		{
			ShaderVariantCollection.Internal_Create(this);
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x0002786C File Offset: 0x00025A6C
		public bool Add(ShaderVariantCollection.ShaderVariant variant)
		{
			return this.AddVariant(variant.shader, variant.passType, variant.keywords);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00027898 File Offset: 0x00025A98
		public bool Remove(ShaderVariantCollection.ShaderVariant variant)
		{
			return this.RemoveVariant(variant.shader, variant.passType, variant.keywords);
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x000278C4 File Offset: 0x00025AC4
		public bool Contains(ShaderVariantCollection.ShaderVariant variant)
		{
			return this.ContainsVariant(variant.shader, variant.passType, variant.keywords);
		}

		// Token: 0x0200023B RID: 571
		public struct ShaderVariant
		{
			// Token: 0x0600187D RID: 6269
			[FreeFunction]
			[NativeConditional("UNITY_EDITOR")]
			[MethodImpl(4096)]
			private static extern string CheckShaderVariant(Shader shader, PassType passType, string[] keywords);

			// Token: 0x0600187E RID: 6270 RVA: 0x000278EE File Offset: 0x00025AEE
			public ShaderVariant(Shader shader, PassType passType, params string[] keywords)
			{
				this.shader = shader;
				this.passType = passType;
				this.keywords = keywords;
			}

			// Token: 0x0400083B RID: 2107
			public Shader shader;

			// Token: 0x0400083C RID: 2108
			public PassType passType;

			// Token: 0x0400083D RID: 2109
			public string[] keywords;
		}
	}
}
