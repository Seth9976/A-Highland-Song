using System;

namespace Ink.Runtime
{
	// Token: 0x02000024 RID: 36
	public struct Pointer
	{
		// Token: 0x0600021F RID: 543 RVA: 0x0000CD5A File Offset: 0x0000AF5A
		public Pointer(Container container, int index)
		{
			this.container = container;
			this.index = index;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000CD6C File Offset: 0x0000AF6C
		public Object Resolve()
		{
			if (this.index < 0)
			{
				return this.container;
			}
			if (this.container == null)
			{
				return null;
			}
			if (this.container.content.Count == 0)
			{
				return this.container;
			}
			if (this.index >= this.container.content.Count)
			{
				return null;
			}
			return this.container.content[this.index];
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000CDE2 File Offset: 0x0000AFE2
		public bool isNull
		{
			get
			{
				return this.container == null;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
		public Path path
		{
			get
			{
				if (this.isNull)
				{
					return null;
				}
				if (this.index >= 0)
				{
					return this.container.path.PathByAppendingComponent(new Path.Component(this.index));
				}
				return this.container.path;
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000CE2C File Offset: 0x0000B02C
		public override string ToString()
		{
			if (this.container == null)
			{
				return "Ink Pointer (null)";
			}
			return "Ink Pointer -> " + this.container.path.ToString() + " -- index " + this.index.ToString();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public static Pointer StartOf(Container container)
		{
			return new Pointer
			{
				container = container,
				index = 0
			};
		}

		// Token: 0x040000A8 RID: 168
		public Container container;

		// Token: 0x040000A9 RID: 169
		public int index;

		// Token: 0x040000AA RID: 170
		public static Pointer Null = new Pointer
		{
			container = null,
			index = -1
		};
	}
}
