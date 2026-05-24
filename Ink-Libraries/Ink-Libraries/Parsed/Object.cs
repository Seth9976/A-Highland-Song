using System;
using System.Collections.Generic;
using System.Text;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000059 RID: 89
	public abstract class Object
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00017CBD File Offset: 0x00015EBD
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x00017CE6 File Offset: 0x00015EE6
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

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x00017CEF File Offset: 0x00015EEF
		public bool hasOwnDebugMetadata
		{
			get
			{
				return this._debugMetadata != null;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x00017CFA File Offset: 0x00015EFA
		public virtual string typeName
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x00017D07 File Offset: 0x00015F07
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x00017D0F File Offset: 0x00015F0F
		public Object parent { get; set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x00017D18 File Offset: 0x00015F18
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x00017D20 File Offset: 0x00015F20
		public List<Object> content { get; protected set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x00017D2C File Offset: 0x00015F2C
		public Story story
		{
			get
			{
				Object @object = this;
				while (@object.parent)
				{
					@object = @object.parent;
				}
				return @object as Story;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00017D57 File Offset: 0x00015F57
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x00017D97 File Offset: 0x00015F97
		public Object runtimeObject
		{
			get
			{
				if (this._runtimeObject == null)
				{
					this._runtimeObject = this.GenerateRuntimeObject();
					if (this._runtimeObject)
					{
						this._runtimeObject.debugMetadata = this.debugMetadata;
					}
				}
				return this._runtimeObject;
			}
			set
			{
				this._runtimeObject = value;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00017DA0 File Offset: 0x00015FA0
		public virtual Path runtimePath
		{
			get
			{
				return this.runtimeObject.path;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00017DAD File Offset: 0x00015FAD
		public virtual Container containerForCounting
		{
			get
			{
				return this.runtimeObject as Container;
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00017DBC File Offset: 0x00015FBC
		public Path PathRelativeTo(Object otherObj)
		{
			List<Object> ancestry = this.ancestry;
			List<Object> ancestry2 = otherObj.ancestry;
			Object @object = null;
			int num = Math.Min(ancestry.Count, ancestry2.Count);
			for (int i = 0; i < num; i++)
			{
				Object object2 = this.ancestry[i];
				Object object3 = ancestry2[i];
				if (!(object2 == object3))
				{
					break;
				}
				@object = object2;
			}
			FlowBase flowBase = @object as FlowBase;
			if (flowBase == null)
			{
				flowBase = @object.ClosestFlowBase();
			}
			List<Identifier> list = new List<Identifier>();
			bool flag = false;
			FlowLevel flowLevel = FlowLevel.WeavePoint;
			Object object4 = this;
			while (object4 && object4 != flowBase && !(object4 is Story) && !(object4 == flowBase))
			{
				if (!flag)
				{
					IWeavePoint weavePoint = object4 as IWeavePoint;
					if (weavePoint != null && weavePoint.identifier != null)
					{
						list.Add(weavePoint.identifier);
						flag = true;
						continue;
					}
				}
				FlowBase flowBase2 = object4 as FlowBase;
				if (flowBase2)
				{
					list.Add(flowBase2.identifier);
					flowLevel = flowBase2.flowLevel;
				}
				object4 = object4.parent;
			}
			list.Reverse();
			if (list.Count > 0)
			{
				return new Path(flowLevel, list);
			}
			return null;
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x00017EF0 File Offset: 0x000160F0
		public List<Object> ancestry
		{
			get
			{
				List<Object> list = new List<Object>();
				Object @object = this.parent;
				while (@object)
				{
					list.Add(@object);
					@object = @object.parent;
				}
				list.Reverse();
				return list;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00017F2C File Offset: 0x0001612C
		public string descriptionOfScope
		{
			get
			{
				List<string> list = new List<string>();
				Object @object = this;
				while (@object)
				{
					FlowBase flowBase = @object as FlowBase;
					if (flowBase && flowBase.identifier != null)
					{
						List<string> list2 = list;
						string text = "'";
						Identifier identifier = flowBase.identifier;
						list2.Add(text + ((identifier != null) ? identifier.ToString() : null) + "'");
					}
					@object = @object.parent;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (list.Count > 0)
				{
					string text2 = string.Join(", ", list.ToArray());
					stringBuilder.Append(text2);
					stringBuilder.Append(" and ");
				}
				stringBuilder.Append("at top scope");
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00017FD8 File Offset: 0x000161D8
		public T AddContent<T>(T subContent) where T : Object
		{
			if (this.content == null)
			{
				this.content = new List<Object>();
			}
			if (subContent)
			{
				subContent.parent = this;
				this.content.Add(subContent);
			}
			return subContent;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00018018 File Offset: 0x00016218
		public void AddContent<T>(List<T> listContent) where T : Object
		{
			foreach (T t in listContent)
			{
				this.AddContent<T>(t);
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00018068 File Offset: 0x00016268
		public T InsertContent<T>(int index, T subContent) where T : Object
		{
			if (this.content == null)
			{
				this.content = new List<Object>();
			}
			subContent.parent = this;
			this.content.Insert(index, subContent);
			return subContent;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001809C File Offset: 0x0001629C
		public T Find<T>(Object.FindQueryFunc<T> queryFunc = null) where T : class
		{
			T t = this as T;
			if (t != null && (queryFunc == null || queryFunc(t)))
			{
				return t;
			}
			if (this.content == null)
			{
				return default(T);
			}
			foreach (Object @object in this.content)
			{
				T t2 = @object.Find<T>(queryFunc);
				if (t2 != null)
				{
					return t2;
				}
			}
			return default(T);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001813C File Offset: 0x0001633C
		public List<T> FindAll<T>(Object.FindQueryFunc<T> queryFunc = null) where T : class
		{
			List<T> list = new List<T>();
			this.FindAll<T>(queryFunc, list);
			return list;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00018158 File Offset: 0x00016358
		private void FindAll<T>(Object.FindQueryFunc<T> queryFunc, List<T> foundSoFar) where T : class
		{
			T t = this as T;
			if (t != null && (queryFunc == null || queryFunc(t)))
			{
				foundSoFar.Add(t);
			}
			if (this.content == null)
			{
				return;
			}
			foreach (Object @object in this.content)
			{
				@object.FindAll<T>(queryFunc, foundSoFar);
			}
		}

		// Token: 0x06000482 RID: 1154
		public abstract Object GenerateRuntimeObject();

		// Token: 0x06000483 RID: 1155 RVA: 0x000181DC File Offset: 0x000163DC
		public virtual void ResolveReferences(Story context)
		{
			if (this.content != null)
			{
				foreach (Object @object in this.content)
				{
					@object.ResolveReferences(context);
				}
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00018238 File Offset: 0x00016438
		public FlowBase ClosestFlowBase()
		{
			Object @object = this.parent;
			while (@object)
			{
				if (@object is FlowBase)
				{
					return (FlowBase)@object;
				}
				@object = @object.parent;
			}
			return null;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00018270 File Offset: 0x00016470
		public virtual void Error(string message, Object source = null, bool isWarning = false)
		{
			if (source == null)
			{
				source = this;
			}
			if (source._alreadyHadError && !isWarning)
			{
				return;
			}
			if (source._alreadyHadWarning && isWarning)
			{
				return;
			}
			if (!this.parent)
			{
				throw new Exception("No parent object to send error to: " + message);
			}
			this.parent.Error(message, source, isWarning);
			if (isWarning)
			{
				source._alreadyHadWarning = true;
				return;
			}
			source._alreadyHadError = true;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000182E0 File Offset: 0x000164E0
		public void Warning(string message, Object source = null)
		{
			this.Error(message, source, true);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000182EB File Offset: 0x000164EB
		public static implicit operator bool(Object obj)
		{
			return obj != null;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000182F4 File Offset: 0x000164F4
		public static bool operator ==(Object a, Object b)
		{
			return a == b;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000182FA File Offset: 0x000164FA
		public static bool operator !=(Object a, Object b)
		{
			return !(a == b);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00018306 File Offset: 0x00016506
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001830C File Offset: 0x0001650C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400016F RID: 367
		private DebugMetadata _debugMetadata;

		// Token: 0x04000172 RID: 370
		private Object _runtimeObject;

		// Token: 0x04000173 RID: 371
		private bool _alreadyHadError;

		// Token: 0x04000174 RID: 372
		private bool _alreadyHadWarning;

		// Token: 0x020000A9 RID: 169
		// (Invoke) Token: 0x06000608 RID: 1544
		public delegate bool FindQueryFunc<T>(T obj);
	}
}
