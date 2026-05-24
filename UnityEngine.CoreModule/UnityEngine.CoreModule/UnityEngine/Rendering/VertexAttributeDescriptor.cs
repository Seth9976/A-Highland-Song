using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x020003BF RID: 959
	[UsedByNativeCode]
	public struct VertexAttributeDescriptor : IEquatable<VertexAttributeDescriptor>
	{
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001F5B RID: 8027 RVA: 0x00033174 File Offset: 0x00031374
		// (set) Token: 0x06001F5C RID: 8028 RVA: 0x0003317C File Offset: 0x0003137C
		public VertexAttribute attribute { readonly get; set; }

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001F5D RID: 8029 RVA: 0x00033185 File Offset: 0x00031385
		// (set) Token: 0x06001F5E RID: 8030 RVA: 0x0003318D File Offset: 0x0003138D
		public VertexAttributeFormat format { readonly get; set; }

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001F5F RID: 8031 RVA: 0x00033196 File Offset: 0x00031396
		// (set) Token: 0x06001F60 RID: 8032 RVA: 0x0003319E File Offset: 0x0003139E
		public int dimension { readonly get; set; }

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001F61 RID: 8033 RVA: 0x000331A7 File Offset: 0x000313A7
		// (set) Token: 0x06001F62 RID: 8034 RVA: 0x000331AF File Offset: 0x000313AF
		public int stream { readonly get; set; }

		// Token: 0x06001F63 RID: 8035 RVA: 0x000331B8 File Offset: 0x000313B8
		public VertexAttributeDescriptor(VertexAttribute attribute = VertexAttribute.Position, VertexAttributeFormat format = VertexAttributeFormat.Float32, int dimension = 3, int stream = 0)
		{
			this.attribute = attribute;
			this.format = format;
			this.dimension = dimension;
			this.stream = stream;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x000331DC File Offset: 0x000313DC
		public override string ToString()
		{
			return string.Format("(attr={0} fmt={1} dim={2} stream={3})", new object[] { this.attribute, this.format, this.dimension, this.stream });
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x00033238 File Offset: 0x00031438
		public override int GetHashCode()
		{
			int num = 17;
			num = (int)(num * 23 + this.attribute);
			num = (int)(num * 23 + this.format);
			num = num * 23 + this.dimension;
			return num * 23 + this.stream;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x00033280 File Offset: 0x00031480
		public override bool Equals(object other)
		{
			bool flag = !(other is VertexAttributeDescriptor);
			return !flag && this.Equals((VertexAttributeDescriptor)other);
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x000332B4 File Offset: 0x000314B4
		public bool Equals(VertexAttributeDescriptor other)
		{
			return this.attribute == other.attribute && this.format == other.format && this.dimension == other.dimension && this.stream == other.stream;
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x00033308 File Offset: 0x00031508
		public static bool operator ==(VertexAttributeDescriptor lhs, VertexAttributeDescriptor rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x00033324 File Offset: 0x00031524
		public static bool operator !=(VertexAttributeDescriptor lhs, VertexAttributeDescriptor rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}
