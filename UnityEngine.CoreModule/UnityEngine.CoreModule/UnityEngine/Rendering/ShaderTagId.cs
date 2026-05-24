using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200040E RID: 1038
	public struct ShaderTagId : IEquatable<ShaderTagId>
	{
		// Token: 0x060023C7 RID: 9159 RVA: 0x0003C4D6 File Offset: 0x0003A6D6
		public ShaderTagId(string name)
		{
			this.m_Id = Shader.TagToID(name);
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x0003C4E8 File Offset: 0x0003A6E8
		// (set) Token: 0x060023C9 RID: 9161 RVA: 0x0003C500 File Offset: 0x0003A700
		internal int id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060023CA RID: 9162 RVA: 0x0003C50C File Offset: 0x0003A70C
		public string name
		{
			get
			{
				return Shader.IDToTag(this.id);
			}
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0003C52C File Offset: 0x0003A72C
		public override bool Equals(object obj)
		{
			return obj is ShaderTagId && this.Equals((ShaderTagId)obj);
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x0003C558 File Offset: 0x0003A758
		public bool Equals(ShaderTagId other)
		{
			return this.m_Id == other.m_Id;
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x0003C578 File Offset: 0x0003A778
		public override int GetHashCode()
		{
			int num = 2079669542;
			return num * -1521134295 + this.m_Id.GetHashCode();
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x0003C5A8 File Offset: 0x0003A7A8
		public static bool operator ==(ShaderTagId tag1, ShaderTagId tag2)
		{
			return tag1.Equals(tag2);
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x0003C5C4 File Offset: 0x0003A7C4
		public static bool operator !=(ShaderTagId tag1, ShaderTagId tag2)
		{
			return !(tag1 == tag2);
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x0003C5E0 File Offset: 0x0003A7E0
		public static explicit operator ShaderTagId(string name)
		{
			return new ShaderTagId(name);
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0003C5F8 File Offset: 0x0003A7F8
		public static explicit operator string(ShaderTagId tagId)
		{
			return tagId.name;
		}

		// Token: 0x04000D33 RID: 3379
		public static readonly ShaderTagId none;

		// Token: 0x04000D34 RID: 3380
		private int m_Id;
	}
}
