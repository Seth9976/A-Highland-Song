using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x0200004C RID: 76
	public abstract class FlowBase : Object, INamedContent
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x000160A2 File Offset: 0x000142A2
		public string name
		{
			get
			{
				Identifier identifier = this.identifier;
				if (identifier == null)
				{
					return null;
				}
				return identifier.name;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x000160B5 File Offset: 0x000142B5
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x000160BD File Offset: 0x000142BD
		public Identifier identifier { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000160C6 File Offset: 0x000142C6
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x000160CE File Offset: 0x000142CE
		public List<FlowBase.Argument> arguments { get; protected set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x000160D7 File Offset: 0x000142D7
		public bool hasParameters
		{
			get
			{
				return this.arguments != null && this.arguments.Count > 0;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600041C RID: 1052
		public abstract FlowLevel flowLevel { get; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x000160F1 File Offset: 0x000142F1
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x000160F9 File Offset: 0x000142F9
		public bool isFunction { get; protected set; }

		// Token: 0x0600041F RID: 1055 RVA: 0x00016104 File Offset: 0x00014304
		public FlowBase(Identifier name = null, List<Object> topLevelObjects = null, List<FlowBase.Argument> arguments = null, bool isFunction = false, bool isIncludedStory = false)
		{
			this.identifier = name;
			if (topLevelObjects == null)
			{
				topLevelObjects = new List<Object>();
			}
			this.PreProcessTopLevelObjects(topLevelObjects);
			topLevelObjects = this.SplitWeaveAndSubFlowContent(topLevelObjects, this is Story && !isIncludedStory);
			base.AddContent<Object>(topLevelObjects);
			this.arguments = arguments;
			this.isFunction = isFunction;
			this.variableDeclarations = new Dictionary<string, VariableAssignment>();
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001616C File Offset: 0x0001436C
		private List<Object> SplitWeaveAndSubFlowContent(List<Object> contentObjs, bool isRootStory)
		{
			List<Object> list = new List<Object>();
			List<Object> list2 = new List<Object>();
			this._subFlowsByName = new Dictionary<string, FlowBase>();
			foreach (Object @object in contentObjs)
			{
				FlowBase flowBase = @object as FlowBase;
				if (flowBase)
				{
					if (this._firstChildFlow == null)
					{
						this._firstChildFlow = flowBase;
					}
					list2.Add(@object);
					Dictionary<string, FlowBase> subFlowsByName = this._subFlowsByName;
					Identifier identifier = flowBase.identifier;
					subFlowsByName[(identifier != null) ? identifier.name : null] = flowBase;
				}
				else
				{
					list.Add(@object);
				}
			}
			if (isRootStory)
			{
				list.Add(new Gather(null, 1));
				list.Add(new Divert(new Path(Identifier.Done), null));
			}
			List<Object> list3 = new List<Object>();
			if (list.Count > 0)
			{
				this._rootWeave = new Weave(list, 0);
				list3.Add(this._rootWeave);
			}
			if (list2.Count > 0)
			{
				list3.AddRange(list2);
			}
			return list3;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00016284 File Offset: 0x00014484
		protected virtual void PreProcessTopLevelObjects(List<Object> topLevelObjects)
		{
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00016288 File Offset: 0x00014488
		public FlowBase.VariableResolveResult ResolveVariableWithName(string varName, Object fromNode)
		{
			FlowBase.VariableResolveResult variableResolveResult = default(FlowBase.VariableResolveResult);
			FlowBase flowBase = ((fromNode == null) ? this : fromNode.ClosestFlowBase());
			if (flowBase.arguments != null)
			{
				using (List<FlowBase.Argument>.Enumerator enumerator = flowBase.arguments.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.identifier.name.Equals(varName))
						{
							variableResolveResult.found = true;
							variableResolveResult.isArgument = true;
							variableResolveResult.ownerFlow = flowBase;
							return variableResolveResult;
						}
					}
				}
			}
			Story story = base.story;
			if (flowBase != story && flowBase.variableDeclarations.ContainsKey(varName))
			{
				variableResolveResult.found = true;
				variableResolveResult.ownerFlow = flowBase;
				variableResolveResult.isTemporary = true;
				return variableResolveResult;
			}
			if (story.variableDeclarations.ContainsKey(varName))
			{
				variableResolveResult.found = true;
				variableResolveResult.ownerFlow = story;
				variableResolveResult.isGlobal = true;
				return variableResolveResult;
			}
			variableResolveResult.found = false;
			return variableResolveResult;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00016394 File Offset: 0x00014594
		public void TryAddNewVariableDeclaration(VariableAssignment varDecl)
		{
			string variableName = varDecl.variableName;
			if (this.variableDeclarations.ContainsKey(variableName))
			{
				string text = "";
				if (this.variableDeclarations[variableName].debugMetadata != null)
				{
					string text2 = " (";
					DebugMetadata debugMetadata = this.variableDeclarations[variableName].debugMetadata;
					text = text2 + ((debugMetadata != null) ? debugMetadata.ToString() : null) + ")";
				}
				this.Error("found declaration variable '" + variableName + "' that was already declared" + text, varDecl, false);
				return;
			}
			this.variableDeclarations[varDecl.variableName] = varDecl;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00016428 File Offset: 0x00014628
		public void ResolveWeavePointNaming()
		{
			if (this._rootWeave)
			{
				this._rootWeave.ResolveWeavePointNaming();
			}
			if (this._subFlowsByName != null)
			{
				foreach (KeyValuePair<string, FlowBase> keyValuePair in this._subFlowsByName)
				{
					keyValuePair.Value.ResolveWeavePointNaming();
				}
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x000164A0 File Offset: 0x000146A0
		public override Object GenerateRuntimeObject()
		{
			Return @return = null;
			if (this.isFunction)
			{
				this.CheckForDisallowedFunctionFlowControl();
			}
			else if (this.flowLevel == FlowLevel.Knot || this.flowLevel == FlowLevel.Stitch)
			{
				@return = base.Find<Return>(null);
				if (@return != null)
				{
					string text = "Return statements can only be used in knots that are declared as functions: == function ";
					Identifier identifier = this.identifier;
					this.Error(text + ((identifier != null) ? identifier.ToString() : null) + " ==", @return, false);
				}
			}
			Container container = new Container();
			Container container2 = container;
			Identifier identifier2 = this.identifier;
			container2.name = ((identifier2 != null) ? identifier2.name : null);
			if (base.story.countAllVisits)
			{
				container.visitsShouldBeCounted = true;
			}
			this.GenerateArgumentVariableAssignments(container);
			int num = 0;
			while (base.content != null && num < base.content.Count)
			{
				Object @object = base.content[num];
				if (@object is FlowBase)
				{
					FlowBase flowBase = (FlowBase)@object;
					Object runtimeObject = flowBase.runtimeObject;
					if (num == 0 && !flowBase.hasParameters && this.flowLevel == FlowLevel.Knot)
					{
						this._startingSubFlowDivert = new Divert();
						container.AddContent(this._startingSubFlowDivert);
						this._startingSubFlowRuntime = runtimeObject;
					}
					INamedContent namedContent = (INamedContent)runtimeObject;
					INamedContent namedContent2 = null;
					if (container.namedContent.TryGetValue(namedContent.name, out namedContent2))
					{
						string text2 = string.Format("{0} already contains flow named '{1}' (at {2})", base.GetType().Name, namedContent.name, (namedContent2 as Object).debugMetadata);
						this.Error(text2, flowBase, false);
					}
					container.AddToNamedContentOnly(namedContent);
				}
				else
				{
					container.AddContent(@object.runtimeObject);
				}
				num++;
			}
			if (this.flowLevel != FlowLevel.Story && !this.isFunction && this._rootWeave != null && @return == null)
			{
				this._rootWeave.ValidateTermination(new Weave.BadTerminationHandler(this.WarningInTermination));
			}
			return container;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00016674 File Offset: 0x00014874
		private void GenerateArgumentVariableAssignments(Container container)
		{
			if (this.arguments == null || this.arguments.Count == 0)
			{
				return;
			}
			for (int i = this.arguments.Count - 1; i >= 0; i--)
			{
				Identifier identifier = this.arguments[i].identifier;
				VariableAssignment variableAssignment = new VariableAssignment((identifier != null) ? identifier.name : null, true);
				container.AddContent(variableAssignment);
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000166DC File Offset: 0x000148DC
		public Object ContentWithNameAtLevel(string name, FlowLevel? level = null, bool deepSearch = false)
		{
			FlowLevel? flowLevel = level;
			FlowLevel flowLevel2 = this.flowLevel;
			if (((flowLevel.GetValueOrDefault() == flowLevel2) & (flowLevel != null)) || level == null)
			{
				Identifier identifier = this.identifier;
				if (name == ((identifier != null) ? identifier.name : null))
				{
					return this;
				}
			}
			flowLevel = level;
			flowLevel2 = FlowLevel.WeavePoint;
			if (((flowLevel.GetValueOrDefault() == flowLevel2) & (flowLevel != null)) || level == null)
			{
				if (this._rootWeave)
				{
					Object @object = (Object)this._rootWeave.WeavePointNamed(name);
					if (@object)
					{
						return @object;
					}
				}
				flowLevel = level;
				flowLevel2 = FlowLevel.WeavePoint;
				if ((flowLevel.GetValueOrDefault() == flowLevel2) & (flowLevel != null))
				{
					if (!deepSearch)
					{
						return null;
					}
					return this.DeepSearchForAnyLevelContent(name);
				}
			}
			if (level != null)
			{
				flowLevel = level;
				flowLevel2 = this.flowLevel;
				if ((flowLevel.GetValueOrDefault() < flowLevel2) & (flowLevel != null))
				{
					return null;
				}
			}
			FlowBase flowBase = null;
			if (this._subFlowsByName.TryGetValue(name, out flowBase))
			{
				if (level != null)
				{
					flowLevel = level;
					flowLevel2 = flowBase.flowLevel;
					if (!((flowLevel.GetValueOrDefault() == flowLevel2) & (flowLevel != null)))
					{
						goto IL_0115;
					}
				}
				return flowBase;
			}
			IL_0115:
			if (!deepSearch)
			{
				return null;
			}
			return this.DeepSearchForAnyLevelContent(name);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001680C File Offset: 0x00014A0C
		private Object DeepSearchForAnyLevelContent(string name)
		{
			Object @object = this.ContentWithNameAtLevel(name, new FlowLevel?(FlowLevel.WeavePoint), false);
			if (@object)
			{
				return @object;
			}
			foreach (KeyValuePair<string, FlowBase> keyValuePair in this._subFlowsByName)
			{
				Object object2 = keyValuePair.Value.ContentWithNameAtLevel(name, null, true);
				if (object2)
				{
					return object2;
				}
			}
			return null;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001689C File Offset: 0x00014A9C
		public override void ResolveReferences(Story context)
		{
			if (this._startingSubFlowDivert)
			{
				this._startingSubFlowDivert.targetPath = this._startingSubFlowRuntime.path;
			}
			base.ResolveReferences(context);
			if (this.arguments != null)
			{
				foreach (FlowBase.Argument argument in this.arguments)
				{
					context.CheckForNamingCollisions(this, argument.identifier, Story.SymbolType.Arg, "argument");
				}
				for (int i = 0; i < this.arguments.Count; i++)
				{
					for (int j = i + 1; j < this.arguments.Count; j++)
					{
						Identifier identifier = this.arguments[i].identifier;
						string text = ((identifier != null) ? identifier.name : null);
						Identifier identifier2 = this.arguments[j].identifier;
						if (text == ((identifier2 != null) ? identifier2.name : null))
						{
							string text2 = "Multiple arguments with the same name: '";
							Identifier identifier3 = this.arguments[i].identifier;
							this.Error(text2 + ((identifier3 != null) ? identifier3.ToString() : null) + "'", null, false);
						}
					}
				}
			}
			if (this.flowLevel != FlowLevel.Story)
			{
				Story.SymbolType symbolType = ((this.flowLevel == FlowLevel.Knot) ? Story.SymbolType.Knot : Story.SymbolType.SubFlowAndWeave);
				context.CheckForNamingCollisions(this, this.identifier, symbolType, null);
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00016A04 File Offset: 0x00014C04
		private void CheckForDisallowedFunctionFlowControl()
		{
			if (!(this is Knot))
			{
				this.Error("Functions cannot be stitches - i.e. they should be defined as '== function myFunc ==' rather than public to another knot.", null, false);
			}
			foreach (KeyValuePair<string, FlowBase> keyValuePair in this._subFlowsByName)
			{
				string key = keyValuePair.Key;
				FlowBase value = keyValuePair.Value;
				string[] array = new string[5];
				array[0] = "Functions may not contain stitches, but saw '";
				array[1] = key;
				array[2] = "' within the function '";
				int num = 3;
				Identifier identifier = this.identifier;
				array[num] = ((identifier != null) ? identifier.ToString() : null);
				array[4] = "'";
				this.Error(string.Concat(array), value, false);
			}
			foreach (Divert divert in this._rootWeave.FindAll<Divert>(null))
			{
				if (!divert.isFunctionCall && !(divert.parent is DivertTarget))
				{
					this.Error("Functions may not contain diverts, but saw '" + divert.ToString() + "'", divert, false);
				}
			}
			foreach (Choice choice in this._rootWeave.FindAll<Choice>(null))
			{
				this.Error("Functions may not contain choices, but saw '" + choice.ToString() + "'", choice, false);
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00016B94 File Offset: 0x00014D94
		private void WarningInTermination(Object terminatingObject)
		{
			string text = "Apparent loose end exists where the flow runs out. Do you need a '-> DONE' statement, choice or divert?";
			if (terminatingObject.parent == this._rootWeave && this._firstChildFlow)
			{
				string text2 = text;
				string text3 = " Note that if you intend to enter '";
				Identifier identifier = this._firstChildFlow.identifier;
				text = text2 + text3 + ((identifier != null) ? identifier.ToString() : null) + "' next, you need to divert to it explicitly.";
			}
			Divert divert = terminatingObject as Divert;
			if (divert && divert.isTunnel)
			{
				string text4 = text;
				string text5 = " When final tunnel to '";
				Path target = divert.target;
				text = text4 + text5 + ((target != null) ? target.ToString() : null) + " ->' returns it won't have anywhere to go.";
			}
			base.Warning(text, terminatingObject);
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00016C31 File Offset: 0x00014E31
		protected Dictionary<string, FlowBase> subFlowsByName
		{
			get
			{
				return this._subFlowsByName;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00016C3C File Offset: 0x00014E3C
		public override string typeName
		{
			get
			{
				if (this.isFunction)
				{
					return "Function";
				}
				return this.flowLevel.ToString();
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00016C6B File Offset: 0x00014E6B
		public override string ToString()
		{
			string typeName = this.typeName;
			string text = " '";
			Identifier identifier = this.identifier;
			return typeName + text + ((identifier != null) ? identifier.ToString() : null) + "'";
		}

		// Token: 0x0400014F RID: 335
		public Dictionary<string, VariableAssignment> variableDeclarations;

		// Token: 0x04000151 RID: 337
		private Weave _rootWeave;

		// Token: 0x04000152 RID: 338
		private Dictionary<string, FlowBase> _subFlowsByName;

		// Token: 0x04000153 RID: 339
		private Divert _startingSubFlowDivert;

		// Token: 0x04000154 RID: 340
		private Object _startingSubFlowRuntime;

		// Token: 0x04000155 RID: 341
		private FlowBase _firstChildFlow;

		// Token: 0x020000A7 RID: 167
		public class Argument
		{
			// Token: 0x0400028C RID: 652
			public Identifier identifier;

			// Token: 0x0400028D RID: 653
			public bool isByReference;

			// Token: 0x0400028E RID: 654
			public bool isDivertTarget;
		}

		// Token: 0x020000A8 RID: 168
		public struct VariableResolveResult
		{
			// Token: 0x0400028F RID: 655
			public bool found;

			// Token: 0x04000290 RID: 656
			public bool isGlobal;

			// Token: 0x04000291 RID: 657
			public bool isArgument;

			// Token: 0x04000292 RID: 658
			public bool isTemporary;

			// Token: 0x04000293 RID: 659
			public FlowBase ownerFlow;
		}
	}
}
