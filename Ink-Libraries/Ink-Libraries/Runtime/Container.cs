using System;
using System.Collections.Generic;
using System.Text;

namespace Ink.Runtime
{
	// Token: 0x02000015 RID: 21
	public class Container : Object, INamedContent
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00007E64 File Offset: 0x00006064
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00007E6C File Offset: 0x0000606C
		public string name { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00007E75 File Offset: 0x00006075
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00007E7D File Offset: 0x0000607D
		public List<Object> content
		{
			get
			{
				return this._content;
			}
			set
			{
				this.AddContent(value);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00007E86 File Offset: 0x00006086
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00007E8E File Offset: 0x0000608E
		public Dictionary<string, INamedContent> namedContent { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00007E98 File Offset: 0x00006098
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00007F6C File Offset: 0x0000616C
		public Dictionary<string, Object> namedOnlyContent
		{
			get
			{
				Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
				foreach (KeyValuePair<string, INamedContent> keyValuePair in this.namedContent)
				{
					dictionary[keyValuePair.Key] = (Object)keyValuePair.Value;
				}
				foreach (Object @object in this.content)
				{
					INamedContent namedContent = @object as INamedContent;
					if (namedContent != null && namedContent.hasValidName)
					{
						dictionary.Remove(namedContent.name);
					}
				}
				if (dictionary.Count == 0)
				{
					dictionary = null;
				}
				return dictionary;
			}
			set
			{
				Dictionary<string, Object> namedOnlyContent = this.namedOnlyContent;
				if (namedOnlyContent != null)
				{
					foreach (KeyValuePair<string, Object> keyValuePair in namedOnlyContent)
					{
						this.namedContent.Remove(keyValuePair.Key);
					}
				}
				if (value == null)
				{
					return;
				}
				foreach (KeyValuePair<string, Object> keyValuePair2 in value)
				{
					INamedContent namedContent = keyValuePair2.Value as INamedContent;
					if (namedContent != null)
					{
						this.AddToNamedContentOnly(namedContent);
					}
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00008024 File Offset: 0x00006224
		// (set) Token: 0x0600013E RID: 318 RVA: 0x0000802C File Offset: 0x0000622C
		public bool visitsShouldBeCounted { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00008035 File Offset: 0x00006235
		// (set) Token: 0x06000140 RID: 320 RVA: 0x0000803D File Offset: 0x0000623D
		public bool turnIndexShouldBeCounted { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00008046 File Offset: 0x00006246
		// (set) Token: 0x06000142 RID: 322 RVA: 0x0000804E File Offset: 0x0000624E
		public bool countingAtStartOnly { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00008058 File Offset: 0x00006258
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00008092 File Offset: 0x00006292
		public int countFlags
		{
			get
			{
				Container.CountFlags countFlags = (Container.CountFlags)0;
				if (this.visitsShouldBeCounted)
				{
					countFlags |= Container.CountFlags.Visits;
				}
				if (this.turnIndexShouldBeCounted)
				{
					countFlags |= Container.CountFlags.Turns;
				}
				if (this.countingAtStartOnly)
				{
					countFlags |= Container.CountFlags.CountStartOnly;
				}
				if (countFlags == Container.CountFlags.CountStartOnly)
				{
					countFlags = (Container.CountFlags)0;
				}
				return (int)countFlags;
			}
			set
			{
				if ((value & 1) > 0)
				{
					this.visitsShouldBeCounted = true;
				}
				if ((value & 2) > 0)
				{
					this.turnIndexShouldBeCounted = true;
				}
				if ((value & 4) > 0)
				{
					this.countingAtStartOnly = true;
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000080BB File Offset: 0x000062BB
		public bool hasValidName
		{
			get
			{
				return this.name != null && this.name.Length > 0;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000080D5 File Offset: 0x000062D5
		public Path pathToFirstLeafContent
		{
			get
			{
				if (this._pathToFirstLeafContent == null)
				{
					this._pathToFirstLeafContent = base.path.PathByAppendingPath(this.internalPathToFirstLeafContent);
				}
				return this._pathToFirstLeafContent;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000080FC File Offset: 0x000062FC
		private Path internalPathToFirstLeafContent
		{
			get
			{
				List<Path.Component> list = new List<Path.Component>();
				Container container = this;
				while (container != null)
				{
					if (container.content.Count > 0)
					{
						list.Add(new Path.Component(0));
						container = container.content[0] as Container;
					}
				}
				return new Path(list, false);
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000814F File Offset: 0x0000634F
		public Container()
		{
			this._content = new List<Object>();
			this.namedContent = new Dictionary<string, INamedContent>();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00008170 File Offset: 0x00006370
		public void AddContent(Object contentObj)
		{
			this.content.Add(contentObj);
			if (contentObj.parent)
			{
				string text = "content is already in ";
				Object parent = contentObj.parent;
				throw new Exception(text + ((parent != null) ? parent.ToString() : null));
			}
			contentObj.parent = this;
			this.TryAddNamedContent(contentObj);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000081C8 File Offset: 0x000063C8
		public void AddContent(IList<Object> contentList)
		{
			foreach (Object @object in contentList)
			{
				this.AddContent(@object);
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00008210 File Offset: 0x00006410
		public void InsertContent(Object contentObj, int index)
		{
			this.content.Insert(index, contentObj);
			if (contentObj.parent)
			{
				string text = "content is already in ";
				Object parent = contentObj.parent;
				throw new Exception(text + ((parent != null) ? parent.ToString() : null));
			}
			contentObj.parent = this;
			this.TryAddNamedContent(contentObj);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00008268 File Offset: 0x00006468
		public void TryAddNamedContent(Object contentObj)
		{
			INamedContent namedContent = contentObj as INamedContent;
			if (namedContent != null && namedContent.hasValidName)
			{
				this.AddToNamedContentOnly(namedContent);
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000828E File Offset: 0x0000648E
		public void AddToNamedContentOnly(INamedContent namedContentObj)
		{
			((Object)namedContentObj).parent = this;
			this.namedContent[namedContentObj.name] = namedContentObj;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000082B0 File Offset: 0x000064B0
		public void AddContentsOfContainer(Container otherContainer)
		{
			this.content.AddRange(otherContainer.content);
			foreach (Object @object in otherContainer.content)
			{
				@object.parent = this;
				this.TryAddNamedContent(@object);
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000831C File Offset: 0x0000651C
		protected Object ContentWithPathComponent(Path.Component component)
		{
			if (component.isIndex)
			{
				if (component.index >= 0 && component.index < this.content.Count)
				{
					return this.content[component.index];
				}
				return null;
			}
			else
			{
				if (component.isParent)
				{
					return base.parent;
				}
				INamedContent namedContent = null;
				if (this.namedContent.TryGetValue(component.name, out namedContent))
				{
					return (Object)namedContent;
				}
				return null;
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00008390 File Offset: 0x00006590
		public SearchResult ContentAtPath(Path path, int partialPathStart = 0, int partialPathLength = -1)
		{
			if (partialPathLength == -1)
			{
				partialPathLength = path.length;
			}
			SearchResult searchResult = default(SearchResult);
			searchResult.approximate = false;
			Container container = this;
			Object @object = this;
			for (int i = partialPathStart; i < partialPathLength; i++)
			{
				Path.Component component = path.GetComponent(i);
				if (container == null)
				{
					searchResult.approximate = true;
					break;
				}
				Object object2 = container.ContentWithPathComponent(component);
				if (object2 == null)
				{
					searchResult.approximate = true;
					break;
				}
				Container container2 = object2 as Container;
				if (i < partialPathLength - 1 && container2 == null)
				{
					searchResult.approximate = true;
					break;
				}
				@object = object2;
				container = container2;
			}
			searchResult.obj = @object;
			return searchResult;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00008438 File Offset: 0x00006638
		public void BuildStringOfHierarchy(StringBuilder sb, int indentation, Object pointedObj)
		{
			Action action = delegate
			{
				for (int j = 0; j < 4 * indentation; j++)
				{
					sb.Append(" ");
				}
			};
			action();
			sb.Append("[");
			if (this.hasValidName)
			{
				sb.AppendFormat(" ({0})", this.name);
			}
			if (this == pointedObj)
			{
				sb.Append("  <---");
			}
			sb.AppendLine();
			int num = indentation;
			indentation = num + 1;
			for (int i = 0; i < this.content.Count; i++)
			{
				Object @object = this.content[i];
				if (@object is Container)
				{
					((Container)@object).BuildStringOfHierarchy(sb, indentation, pointedObj);
				}
				else
				{
					action();
					if (@object is StringValue)
					{
						sb.Append("\"");
						sb.Append(@object.ToString().Replace("\n", "\\n"));
						sb.Append("\"");
					}
					else
					{
						sb.Append(@object.ToString());
					}
				}
				if (i != this.content.Count - 1)
				{
					sb.Append(",");
				}
				if (!(@object is Container) && @object == pointedObj)
				{
					sb.Append("  <---");
				}
				sb.AppendLine();
			}
			Dictionary<string, INamedContent> dictionary = new Dictionary<string, INamedContent>();
			foreach (KeyValuePair<string, INamedContent> keyValuePair in this.namedContent)
			{
				if (!this.content.Contains((Object)keyValuePair.Value))
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			if (dictionary.Count > 0)
			{
				action();
				sb.AppendLine("-- named: --");
				foreach (KeyValuePair<string, INamedContent> keyValuePair2 in dictionary)
				{
					((Container)keyValuePair2.Value).BuildStringOfHierarchy(sb, indentation, pointedObj);
					sb.AppendLine();
				}
			}
			num = indentation;
			indentation = num - 1;
			action();
			sb.Append("]");
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000086FC File Offset: 0x000068FC
		public virtual string BuildStringOfHierarchy()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.BuildStringOfHierarchy(stringBuilder, 0, null);
			return stringBuilder.ToString();
		}

		// Token: 0x04000059 RID: 89
		private List<Object> _content;

		// Token: 0x0400005E RID: 94
		private Path _pathToFirstLeafContent;

		// Token: 0x02000089 RID: 137
		[Flags]
		public enum CountFlags
		{
			// Token: 0x040001F2 RID: 498
			Visits = 1,
			// Token: 0x040001F3 RID: 499
			Turns = 2,
			// Token: 0x040001F4 RID: 500
			CountStartOnly = 4
		}
	}
}
