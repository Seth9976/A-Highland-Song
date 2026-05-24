using System;
using System.Collections.Generic;

namespace Ink.Runtime
{
	// Token: 0x02000022 RID: 34
	public class Object
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000C60E File Offset: 0x0000A80E
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000C616 File Offset: 0x0000A816
		public Object parent { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000C61F File Offset: 0x0000A81F
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000C648 File Offset: 0x0000A848
		public DebugMetadata debugMetadata
		{
			get
			{
				if (this._debugMetadata == null && this.parent)
				{
					return this.parent.debugMetadata;
				}
				return this._debugMetadata;
			}
			set
			{
				this._debugMetadata = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000C651 File Offset: 0x0000A851
		public DebugMetadata ownDebugMetadata
		{
			get
			{
				return this._debugMetadata;
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000C65C File Offset: 0x0000A85C
		public int? DebugLineNumberOfPath(Path path)
		{
			if (path == null)
			{
				return null;
			}
			Container rootContentContainer = this.rootContentContainer;
			if (rootContentContainer)
			{
				Object obj = rootContentContainer.ContentAtPath(path, 0, -1).obj;
				if (obj)
				{
					DebugMetadata debugMetadata = obj.debugMetadata;
					if (debugMetadata != null)
					{
						return new int?(debugMetadata.startLineNumber);
					}
				}
			}
			return null;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000C6BC File Offset: 0x0000A8BC
		public Path path
		{
			get
			{
				if (this._path == null)
				{
					if (this.parent == null)
					{
						this._path = new Path();
					}
					else
					{
						Stack<Path.Component> stack = new Stack<Path.Component>();
						Object @object = this;
						Container container = @object.parent as Container;
						while (container)
						{
							INamedContent namedContent = @object as INamedContent;
							if (namedContent != null && namedContent.hasValidName)
							{
								stack.Push(new Path.Component(namedContent.name));
							}
							else
							{
								stack.Push(new Path.Component(container.content.IndexOf(@object)));
							}
							@object = container;
							container = container.parent as Container;
						}
						this._path = new Path(stack, false);
					}
				}
				return this._path;
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000C76C File Offset: 0x0000A96C
		public SearchResult ResolvePath(Path path)
		{
			if (path.isRelative)
			{
				Container container = this as Container;
				if (!container)
				{
					container = this.parent as Container;
					path = path.tail;
				}
				return container.ContentAtPath(path, 0, -1);
			}
			return this.rootContentContainer.ContentAtPath(path, 0, -1);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000C7BC File Offset: 0x0000A9BC
		public Path ConvertPathToRelative(Path globalPath)
		{
			Path path = this.path;
			int num = Math.Min(globalPath.length, path.length);
			int num2 = -1;
			for (int i = 0; i < num; i++)
			{
				Path.Component component = path.GetComponent(i);
				Path.Component component2 = globalPath.GetComponent(i);
				if (!component.Equals(component2))
				{
					break;
				}
				num2 = i;
			}
			if (num2 == -1)
			{
				return globalPath;
			}
			int num3 = path.length - 1 - num2;
			List<Path.Component> list = new List<Path.Component>();
			for (int j = 0; j < num3; j++)
			{
				list.Add(Path.Component.ToParent());
			}
			for (int k = num2 + 1; k < globalPath.length; k++)
			{
				list.Add(globalPath.GetComponent(k));
			}
			return new Path(list, true);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000C874 File Offset: 0x0000AA74
		public string CompactPathString(Path otherPath)
		{
			string text;
			string text2;
			if (otherPath.isRelative)
			{
				text = otherPath.componentsString;
				text2 = this.path.PathByAppendingPath(otherPath).componentsString;
			}
			else
			{
				text = this.ConvertPathToRelative(otherPath).componentsString;
				text2 = otherPath.componentsString;
			}
			if (text.Length < text2.Length)
			{
				return text;
			}
			return text2;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		public Container rootContentContainer
		{
			get
			{
				Object @object = this;
				while (@object.parent)
				{
					@object = @object.parent;
				}
				return @object as Container;
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000C903 File Offset: 0x0000AB03
		public virtual Object Copy()
		{
			throw new NotImplementedException(base.GetType().Name + " doesn't support copying");
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000C920 File Offset: 0x0000AB20
		public void SetChild<T>(ref T obj, T value) where T : Object
		{
			if (obj)
			{
				obj.parent = null;
			}
			obj = value;
			if (obj)
			{
				obj.parent = this;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000C972 File Offset: 0x0000AB72
		public static implicit operator bool(Object obj)
		{
			return obj != null;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000C97B File Offset: 0x0000AB7B
		public static bool operator ==(Object a, Object b)
		{
			return a == b;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000C981 File Offset: 0x0000AB81
		public static bool operator !=(Object a, Object b)
		{
			return !(a == b);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000C98D File Offset: 0x0000AB8D
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000C993 File Offset: 0x0000AB93
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040000A2 RID: 162
		private DebugMetadata _debugMetadata;

		// Token: 0x040000A3 RID: 163
		private Path _path;
	}
}
