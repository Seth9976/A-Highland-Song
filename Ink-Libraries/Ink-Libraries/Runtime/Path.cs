using System;
using System.Collections.Generic;
using System.Linq;

namespace Ink.Runtime
{
	// Token: 0x02000023 RID: 35
	public class Path : IEquatable<Path>
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000C99B File Offset: 0x0000AB9B
		public Path.Component GetComponent(int index)
		{
			return this._components[index];
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000C9A9 File Offset: 0x0000ABA9
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000C9B1 File Offset: 0x0000ABB1
		public bool isRelative { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000C9BA File Offset: 0x0000ABBA
		public Path.Component head
		{
			get
			{
				if (this._components.Count > 0)
				{
					return this._components.First<Path.Component>();
				}
				return null;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000C9D7 File Offset: 0x0000ABD7
		public Path tail
		{
			get
			{
				if (this._components.Count >= 2)
				{
					return new Path(this._components.GetRange(1, this._components.Count - 1), false);
				}
				return Path.self;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000CA0C File Offset: 0x0000AC0C
		public int length
		{
			get
			{
				return this._components.Count;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000CA1C File Offset: 0x0000AC1C
		public Path.Component lastComponent
		{
			get
			{
				int num = this._components.Count - 1;
				if (num >= 0)
				{
					return this._components[num];
				}
				return null;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000CA4C File Offset: 0x0000AC4C
		public bool containsNamedComponent
		{
			get
			{
				using (List<Path.Component>.Enumerator enumerator = this._components.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.isIndex)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000CAA8 File Offset: 0x0000ACA8
		public Path()
		{
			this._components = new List<Path.Component>();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000CABB File Offset: 0x0000ACBB
		public Path(Path.Component head, Path tail)
			: this()
		{
			this._components.Add(head);
			this._components.AddRange(tail._components);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000CAE0 File Offset: 0x0000ACE0
		public Path(IEnumerable<Path.Component> components, bool relative = false)
			: this()
		{
			this._components.AddRange(components);
			this.isRelative = relative;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000CAFB File Offset: 0x0000ACFB
		public Path(string componentsString)
			: this()
		{
			this.componentsString = componentsString;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000CB0A File Offset: 0x0000AD0A
		public static Path self
		{
			get
			{
				return new Path
				{
					isRelative = true
				};
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000CB18 File Offset: 0x0000AD18
		public Path PathByAppendingPath(Path pathToAppend)
		{
			Path path = new Path();
			int num = 0;
			int num2 = 0;
			while (num2 < pathToAppend._components.Count && pathToAppend._components[num2].isParent)
			{
				num++;
				num2++;
			}
			for (int i = 0; i < this._components.Count - num; i++)
			{
				path._components.Add(this._components[i]);
			}
			for (int j = num; j < pathToAppend._components.Count; j++)
			{
				path._components.Add(pathToAppend._components[j]);
			}
			return path;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000CBBC File Offset: 0x0000ADBC
		public Path PathByAppendingComponent(Path.Component c)
		{
			Path path = new Path();
			path._components.AddRange(this._components);
			path._components.Add(c);
			return path;
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000CC30 File Offset: 0x0000AE30
		public string componentsString
		{
			get
			{
				if (this._componentsString == null)
				{
					this._componentsString = StringExt.Join<Path.Component>(".", this._components);
					if (this.isRelative)
					{
						this._componentsString = "." + this._componentsString;
					}
				}
				return this._componentsString;
			}
			private set
			{
				this._components.Clear();
				this._componentsString = value;
				if (string.IsNullOrEmpty(this._componentsString))
				{
					return;
				}
				if (this._componentsString[0] == '.')
				{
					this.isRelative = true;
					this._componentsString = this._componentsString.Substring(1);
				}
				else
				{
					this.isRelative = false;
				}
				foreach (string text in this._componentsString.Split('.', StringSplitOptions.None))
				{
					int num;
					if (int.TryParse(text, out num))
					{
						this._components.Add(new Path.Component(num));
					}
					else
					{
						this._components.Add(new Path.Component(text));
					}
				}
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000CCDE File Offset: 0x0000AEDE
		public override string ToString()
		{
			return this.componentsString;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000CCE6 File Offset: 0x0000AEE6
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Path);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000CCF4 File Offset: 0x0000AEF4
		public bool Equals(Path otherPath)
		{
			return otherPath != null && otherPath._components.Count == this._components.Count && otherPath.isRelative == this.isRelative && otherPath._components.SequenceEqual(this._components);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000CD41 File Offset: 0x0000AF41
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x040000A4 RID: 164
		private static string parentId = "^";

		// Token: 0x040000A6 RID: 166
		private string _componentsString;

		// Token: 0x040000A7 RID: 167
		private List<Path.Component> _components;

		// Token: 0x02000090 RID: 144
		public class Component : IEquatable<Path.Component>
		{
			// Token: 0x1700013D RID: 317
			// (get) Token: 0x06000593 RID: 1427 RVA: 0x0001BB4F File Offset: 0x00019D4F
			// (set) Token: 0x06000594 RID: 1428 RVA: 0x0001BB57 File Offset: 0x00019D57
			public int index { get; private set; }

			// Token: 0x1700013E RID: 318
			// (get) Token: 0x06000595 RID: 1429 RVA: 0x0001BB60 File Offset: 0x00019D60
			// (set) Token: 0x06000596 RID: 1430 RVA: 0x0001BB68 File Offset: 0x00019D68
			public string name { get; private set; }

			// Token: 0x1700013F RID: 319
			// (get) Token: 0x06000597 RID: 1431 RVA: 0x0001BB71 File Offset: 0x00019D71
			public bool isIndex
			{
				get
				{
					return this.index >= 0;
				}
			}

			// Token: 0x17000140 RID: 320
			// (get) Token: 0x06000598 RID: 1432 RVA: 0x0001BB7F File Offset: 0x00019D7F
			public bool isParent
			{
				get
				{
					return this.name == Path.parentId;
				}
			}

			// Token: 0x06000599 RID: 1433 RVA: 0x0001BB91 File Offset: 0x00019D91
			public Component(int index)
			{
				this.index = index;
				this.name = null;
			}

			// Token: 0x0600059A RID: 1434 RVA: 0x0001BBA7 File Offset: 0x00019DA7
			public Component(string name)
			{
				this.name = name;
				this.index = -1;
			}

			// Token: 0x0600059B RID: 1435 RVA: 0x0001BBBD File Offset: 0x00019DBD
			public static Path.Component ToParent()
			{
				return new Path.Component(Path.parentId);
			}

			// Token: 0x0600059C RID: 1436 RVA: 0x0001BBCC File Offset: 0x00019DCC
			public override string ToString()
			{
				if (this.isIndex)
				{
					return this.index.ToString();
				}
				return this.name;
			}

			// Token: 0x0600059D RID: 1437 RVA: 0x0001BBF6 File Offset: 0x00019DF6
			public override bool Equals(object obj)
			{
				return this.Equals(obj as Path.Component);
			}

			// Token: 0x0600059E RID: 1438 RVA: 0x0001BC04 File Offset: 0x00019E04
			public bool Equals(Path.Component otherComp)
			{
				if (otherComp == null || otherComp.isIndex != this.isIndex)
				{
					return false;
				}
				if (this.isIndex)
				{
					return this.index == otherComp.index;
				}
				return this.name == otherComp.name;
			}

			// Token: 0x0600059F RID: 1439 RVA: 0x0001BC41 File Offset: 0x00019E41
			public override int GetHashCode()
			{
				if (this.isIndex)
				{
					return this.index;
				}
				return this.name.GetHashCode();
			}
		}
	}
}
