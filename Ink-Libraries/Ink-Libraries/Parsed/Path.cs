using System;
using System.Collections.Generic;
using System.Linq;

namespace Ink.Parsed
{
	// Token: 0x0200005A RID: 90
	public class Path
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x0001831C File Offset: 0x0001651C
		public FlowLevel baseTargetLevel
		{
			get
			{
				if (this.baseLevelIsAmbiguous)
				{
					return FlowLevel.Story;
				}
				return this._baseTargetLevel.Value;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00018333 File Offset: 0x00016533
		public bool baseLevelIsAmbiguous
		{
			get
			{
				return this._baseTargetLevel == null;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00018343 File Offset: 0x00016543
		public string firstComponent
		{
			get
			{
				if (this.components == null || this.components.Count == 0)
				{
					return null;
				}
				return this.components[0].name;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001836D File Offset: 0x0001656D
		public int numberOfComponents
		{
			get
			{
				return this.components.Count;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0001837C File Offset: 0x0001657C
		public string dotSeparatedComponents
		{
			get
			{
				if (this._dotSeparatedComponents == null)
				{
					this._dotSeparatedComponents = string.Join(".", this.components.Select(delegate(Identifier c)
					{
						if (c == null)
						{
							return null;
						}
						return c.name;
					}));
				}
				return this._dotSeparatedComponents;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x000183D1 File Offset: 0x000165D1
		public List<Identifier> components { get; }

		// Token: 0x06000493 RID: 1171 RVA: 0x000183D9 File Offset: 0x000165D9
		public Path(FlowLevel baseFlowLevel, List<Identifier> components)
		{
			this._baseTargetLevel = new FlowLevel?(baseFlowLevel);
			this.components = components;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x000183F4 File Offset: 0x000165F4
		public Path(List<Identifier> components)
		{
			this._baseTargetLevel = null;
			this.components = components;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0001840F File Offset: 0x0001660F
		public Path(Identifier ambiguousName)
		{
			this._baseTargetLevel = null;
			this.components = new List<Identifier>();
			this.components.Add(ambiguousName);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001843A File Offset: 0x0001663A
		public override string ToString()
		{
			if (this.components != null && this.components.Count != 0)
			{
				return "-> " + this.dotSeparatedComponents;
			}
			if (this.baseTargetLevel == FlowLevel.WeavePoint)
			{
				return "-> <next gather point>";
			}
			return "<invalid Path>";
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00018478 File Offset: 0x00016678
		public Object ResolveFromContext(Object context)
		{
			if (this.components == null || this.components.Count == 0)
			{
				return null;
			}
			Object @object = this.ResolveBaseTarget(context);
			if (@object == null)
			{
				return null;
			}
			if (this.components.Count > 1)
			{
				return this.ResolveTailComponents(@object);
			}
			return @object;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000184C8 File Offset: 0x000166C8
		private Object ResolveBaseTarget(Object originalContext)
		{
			string firstComponent = this.firstComponent;
			Object @object = originalContext;
			while (@object != null)
			{
				bool flag = @object == originalContext;
				Object object2 = this.TryGetChildFromContext(@object, firstComponent, null, flag);
				if (object2 != null)
				{
					return object2;
				}
				@object = @object.parent;
			}
			return null;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00018518 File Offset: 0x00016718
		private Object ResolveTailComponents(Object rootTarget)
		{
			Object @object = rootTarget;
			for (int i = 1; i < this.components.Count; i++)
			{
				string name = this.components[i].name;
				FlowBase flowBase = @object as FlowBase;
				FlowLevel flowLevel;
				if (flowBase != null)
				{
					flowLevel = flowBase.flowLevel + 1;
				}
				else
				{
					flowLevel = FlowLevel.WeavePoint;
				}
				@object = this.TryGetChildFromContext(@object, name, new FlowLevel?(flowLevel), false);
				if (@object == null)
				{
					break;
				}
			}
			return @object;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001858C File Offset: 0x0001678C
		private Object TryGetChildFromContext(Object context, string childName, FlowLevel? minimumLevel, bool forceDeepSearch = false)
		{
			bool flag = minimumLevel == null;
			Weave weave = context as Weave;
			if (weave != null)
			{
				if (!flag)
				{
					FlowLevel? flowLevel = minimumLevel;
					FlowLevel flowLevel2 = FlowLevel.WeavePoint;
					if (!((flowLevel.GetValueOrDefault() == flowLevel2) & (flowLevel != null)))
					{
						goto IL_0045;
					}
				}
				return (Object)weave.WeavePointNamed(childName);
			}
			IL_0045:
			FlowBase flowBase = context as FlowBase;
			if (flowBase != null)
			{
				bool flag2 = forceDeepSearch || flowBase.flowLevel == FlowLevel.Knot;
				return flowBase.ContentWithNameAtLevel(childName, minimumLevel, flag2);
			}
			return null;
		}

		// Token: 0x04000175 RID: 373
		private string _dotSeparatedComponents;

		// Token: 0x04000177 RID: 375
		private FlowLevel? _baseTargetLevel;
	}
}
