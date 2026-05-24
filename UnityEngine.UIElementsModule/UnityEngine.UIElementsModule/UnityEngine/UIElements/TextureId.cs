using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.UIElements
{
	// Token: 0x02000260 RID: 608
	internal struct TextureId
	{
		// Token: 0x06001246 RID: 4678 RVA: 0x00047CE8 File Offset: 0x00045EE8
		public TextureId(int index)
		{
			this.m_Index = index + 1;
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x00047CF4 File Offset: 0x00045EF4
		public int index
		{
			get
			{
				return this.m_Index - 1;
			}
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00047D00 File Offset: 0x00045F00
		public float ConvertToGpu()
		{
			return (float)this.index;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00047D1C File Offset: 0x00045F1C
		public override bool Equals(object obj)
		{
			bool flag = !(obj is TextureId);
			return !flag && (TextureId)obj == this;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00047D54 File Offset: 0x00045F54
		[MethodImpl(256)]
		public bool Equals(TextureId other)
		{
			return this.m_Index == other.m_Index;
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00047D74 File Offset: 0x00045F74
		[MethodImpl(256)]
		public override int GetHashCode()
		{
			return this.m_Index.GetHashCode();
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00047D94 File Offset: 0x00045F94
		[MethodImpl(256)]
		public static bool operator ==(TextureId left, TextureId right)
		{
			return left.m_Index == right.m_Index;
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00047DB4 File Offset: 0x00045FB4
		[MethodImpl(256)]
		public static bool operator !=(TextureId left, TextureId right)
		{
			return !(left == right);
		}

		// Token: 0x04000867 RID: 2151
		private readonly int m_Index;

		// Token: 0x04000868 RID: 2152
		public static readonly TextureId invalid = new TextureId(-1);
	}
}
