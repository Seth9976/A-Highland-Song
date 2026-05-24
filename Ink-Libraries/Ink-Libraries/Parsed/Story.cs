using System;
using System.Collections.Generic;
using System.Text;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x0200005F RID: 95
	public class Story : FlowBase
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00018A61 File Offset: 0x00016C61
		public override FlowLevel flowLevel
		{
			get
			{
				return FlowLevel.Story;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00018A64 File Offset: 0x00016C64
		internal bool hadError
		{
			get
			{
				return this._hadError;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00018A6C File Offset: 0x00016C6C
		internal bool hadWarning
		{
			get
			{
				return this._hadWarning;
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00018A74 File Offset: 0x00016C74
		public Story(List<Object> toplevelObjects, bool isInclude = false)
			: base(null, toplevelObjects, null, false, isInclude)
		{
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00018A8C File Offset: 0x00016C8C
		protected override void PreProcessTopLevelObjects(List<Object> topLevelContent)
		{
			List<FlowBase> list = new List<FlowBase>();
			int i = 0;
			while (i < topLevelContent.Count)
			{
				Object @object = topLevelContent[i];
				if (@object is IncludedFile)
				{
					IncludedFile includedFile = (IncludedFile)@object;
					topLevelContent.RemoveAt(i);
					if (includedFile.includedStory)
					{
						List<Object> list2 = new List<Object>();
						Story includedStory = includedFile.includedStory;
						if (includedStory.content != null)
						{
							foreach (Object object2 in includedStory.content)
							{
								if (object2 is FlowBase)
								{
									list.Add((FlowBase)object2);
								}
								else
								{
									list2.Add(object2);
								}
							}
							list2.Add(new Text("\n"));
							topLevelContent.InsertRange(i, list2);
							i += list2.Count;
						}
					}
				}
				else
				{
					i++;
				}
			}
			topLevelContent.AddRange(list.ToArray());
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00018B94 File Offset: 0x00016D94
		public Story ExportRuntime(ErrorHandler errorHandler = null)
		{
			this._errorHandler = errorHandler;
			this.constants = new Dictionary<string, Expression>();
			foreach (ConstantDeclaration constantDeclaration in base.FindAll<ConstantDeclaration>(null))
			{
				Expression expression = null;
				if (this.constants.TryGetValue(constantDeclaration.constantName, out expression) && !expression.Equals(constantDeclaration.expression))
				{
					string text = string.Format("CONST '{0}' has been redefined with a different value. Multiple definitions of the same CONST are valid so long as they contain the same value. Initial definition was on {1}.", constantDeclaration.constantName, expression.debugMetadata);
					this.Error(text, constantDeclaration, false);
				}
				this.constants[constantDeclaration.constantName] = constantDeclaration.expression;
			}
			this._listDefs = new Dictionary<string, ListDefinition>();
			foreach (ListDefinition listDefinition in base.FindAll<ListDefinition>(null))
			{
				Dictionary<string, ListDefinition> listDefs = this._listDefs;
				Identifier identifier = listDefinition.identifier;
				listDefs[(identifier != null) ? identifier.name : null] = listDefinition;
			}
			this.externals = new Dictionary<string, ExternalDeclaration>();
			base.ResolveWeavePointNaming();
			Container container = base.runtimeObject as Container;
			Container container2 = new Container();
			container2.AddContent(ControlCommand.EvalStart());
			List<ListDefinition> list = new List<ListDefinition>();
			foreach (KeyValuePair<string, VariableAssignment> keyValuePair in this.variableDeclarations)
			{
				string key = keyValuePair.Key;
				VariableAssignment value = keyValuePair.Value;
				if (value.isGlobalDeclaration)
				{
					if (value.listDefinition != null)
					{
						this._listDefs[key] = value.listDefinition;
						container2.AddContent(value.listDefinition.runtimeObject);
						list.Add(value.listDefinition.runtimeListDefinition);
					}
					else
					{
						value.expression.GenerateIntoContainer(container2);
					}
					container2.AddContent(new VariableAssignment(key, true)
					{
						isGlobal = true
					});
				}
			}
			container2.AddContent(ControlCommand.EvalEnd());
			container2.AddContent(ControlCommand.End());
			if (this.variableDeclarations.Count > 0)
			{
				container2.name = "global decl";
				container.AddToNamedContentOnly(container2);
			}
			container.AddContent(ControlCommand.Done());
			Story story = new Story(container, list);
			base.runtimeObject = story;
			if (this._hadError)
			{
				return null;
			}
			this.FlattenContainersIn(container);
			this.ResolveReferences(this);
			if (this._hadError)
			{
				return null;
			}
			story.ResetState();
			return story;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00018E40 File Offset: 0x00017040
		public ListDefinition ResolveList(string listName)
		{
			ListDefinition listDefinition;
			if (!this._listDefs.TryGetValue(listName, out listDefinition))
			{
				return null;
			}
			return listDefinition;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00018E60 File Offset: 0x00017060
		public ListElementDefinition ResolveListItem(string listName, string itemName, Object source = null)
		{
			ListDefinition listDefinition = null;
			if (listName == null)
			{
				ListElementDefinition listElementDefinition = null;
				ListDefinition listDefinition2 = null;
				foreach (KeyValuePair<string, ListDefinition> keyValuePair in this._listDefs)
				{
					ListDefinition value = keyValuePair.Value;
					ListElementDefinition listElementDefinition2 = value.ItemNamed(itemName);
					if (listElementDefinition2)
					{
						if (listElementDefinition != null)
						{
							string[] array = new string[6];
							array[0] = "Ambiguous item name '";
							array[1] = itemName;
							array[2] = "' found in multiple sets, including ";
							int num = 3;
							Identifier identifier = listDefinition2.identifier;
							array[num] = ((identifier != null) ? identifier.ToString() : null);
							array[4] = " and ";
							int num2 = 5;
							Identifier identifier2 = value.identifier;
							array[num2] = ((identifier2 != null) ? identifier2.ToString() : null);
							this.Error(string.Concat(array), source, false);
						}
						else
						{
							listElementDefinition = listElementDefinition2;
							listDefinition2 = value;
						}
					}
				}
				return listElementDefinition;
			}
			if (!this._listDefs.TryGetValue(listName, out listDefinition))
			{
				return null;
			}
			return listDefinition.ItemNamed(itemName);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00018F60 File Offset: 0x00017160
		private void FlattenContainersIn(Container container)
		{
			HashSet<Container> hashSet = new HashSet<Container>();
			foreach (Object @object in container.content)
			{
				Container container2 = @object as Container;
				if (container2)
				{
					hashSet.Add(container2);
				}
			}
			if (container.namedContent != null)
			{
				foreach (KeyValuePair<string, INamedContent> keyValuePair in container.namedContent)
				{
					Container container3 = keyValuePair.Value as Container;
					if (container3)
					{
						hashSet.Add(container3);
					}
				}
			}
			foreach (Container container4 in hashSet)
			{
				this.TryFlattenContainer(container4);
				this.FlattenContainersIn(container4);
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00019074 File Offset: 0x00017274
		private void TryFlattenContainer(Container container)
		{
			if (container.namedContent.Count > 0 || container.hasValidName || this._dontFlattenContainers.Contains(container))
			{
				return;
			}
			Container container2 = container.parent as Container;
			if (container2)
			{
				int num = container2.content.IndexOf(container);
				container2.content.RemoveAt(num);
				DebugMetadata ownDebugMetadata = container.ownDebugMetadata;
				foreach (Object @object in container.content)
				{
					@object.parent = null;
					if (ownDebugMetadata != null && @object.ownDebugMetadata == null)
					{
						@object.debugMetadata = ownDebugMetadata;
					}
					container2.InsertContent(@object, num);
					num++;
				}
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00019144 File Offset: 0x00017344
		public override void Error(string message, Object source, bool isWarning)
		{
			ErrorType errorType = (isWarning ? ErrorType.Warning : ErrorType.Error);
			StringBuilder stringBuilder = new StringBuilder();
			if (source is AuthorWarning)
			{
				stringBuilder.Append("TODO: ");
				errorType = ErrorType.Author;
			}
			else if (isWarning)
			{
				stringBuilder.Append("WARNING: ");
			}
			else
			{
				stringBuilder.Append("ERROR: ");
			}
			if (source && source.debugMetadata != null && source.debugMetadata.startLineNumber >= 1)
			{
				if (source.debugMetadata.fileName != null)
				{
					stringBuilder.AppendFormat("'{0}' ", source.debugMetadata.fileName);
				}
				stringBuilder.AppendFormat("line {0}: ", source.debugMetadata.startLineNumber);
			}
			stringBuilder.Append(message);
			message = stringBuilder.ToString();
			if (this._errorHandler != null)
			{
				this._hadError = errorType == ErrorType.Error;
				this._hadWarning = errorType == ErrorType.Warning;
				this._errorHandler(message, errorType);
				return;
			}
			throw new Exception(message);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00019232 File Offset: 0x00017432
		public void ResetError()
		{
			this._hadError = false;
			this._hadWarning = false;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00019242 File Offset: 0x00017442
		public bool IsExternal(string namedFuncTarget)
		{
			return this.externals.ContainsKey(namedFuncTarget);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00019250 File Offset: 0x00017450
		public void AddExternal(ExternalDeclaration decl)
		{
			if (this.externals.ContainsKey(decl.name))
			{
				this.Error("Duplicate EXTERNAL definition of '" + decl.name + "'", decl, false);
				return;
			}
			this.externals[decl.name] = decl;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000192A0 File Offset: 0x000174A0
		public void DontFlattenContainer(Container container)
		{
			this._dontFlattenContainers.Add(container);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000192B0 File Offset: 0x000174B0
		private void NameConflictError(Object obj, string name, Object existingObj, string typeNameToPrint)
		{
			string[] array = new string[7];
			array[0] = typeNameToPrint;
			array[1] = " '";
			array[2] = name;
			array[3] = "': name has already been used for a ";
			array[4] = existingObj.typeName.ToLower();
			array[5] = " on ";
			int num = 6;
			DebugMetadata debugMetadata = existingObj.debugMetadata;
			array[num] = ((debugMetadata != null) ? debugMetadata.ToString() : null);
			obj.Error(string.Concat(array), null, false);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00019314 File Offset: 0x00017514
		public static bool IsReservedKeyword(string name)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 699505802U)
			{
				if (num <= 184981848U)
				{
					if (num != 172932033U)
					{
						if (num != 184981848U)
						{
							return false;
						}
						if (!(name == "false"))
						{
							return false;
						}
					}
					else if (!(name == "LIST"))
					{
						return false;
					}
				}
				else if (num != 218129310U)
				{
					if (num != 247034676U)
					{
						if (num != 699505802U)
						{
							return false;
						}
						if (!(name == "not"))
						{
							return false;
						}
					}
					else if (!(name == "CONST"))
					{
						return false;
					}
				}
				else if (!(name == "VAR"))
				{
					return false;
				}
			}
			else if (num <= 2246981567U)
			{
				if (num != 1303515621U)
				{
					if (num != 2246981567U)
					{
						return false;
					}
					if (!(name == "return"))
					{
						return false;
					}
				}
				else if (!(name == "true"))
				{
					return false;
				}
			}
			else if (num != 2664841801U)
			{
				if (num != 3183434736U)
				{
					if (num != 3223044039U)
					{
						return false;
					}
					if (!(name == "temp"))
					{
						return false;
					}
				}
				else if (!(name == "else"))
				{
					return false;
				}
			}
			else if (!(name == "function"))
			{
				return false;
			}
			return true;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00019450 File Offset: 0x00017650
		public void CheckForNamingCollisions(Object obj, Identifier identifier, Story.SymbolType symbolType, string typeNameOverride = null)
		{
			string text = typeNameOverride ?? obj.typeName;
			if (Story.IsReservedKeyword((identifier != null) ? identifier.name : null))
			{
				obj.Error(string.Concat(new string[]
				{
					"'",
					base.name,
					"' cannot be used for the name of a ",
					text.ToLower(),
					" because it's a reserved keyword"
				}), null, false);
				return;
			}
			if (FunctionCall.IsBuiltIn((identifier != null) ? identifier.name : null))
			{
				obj.Error(string.Concat(new string[]
				{
					"'",
					base.name,
					"' cannot be used for the name of a ",
					text.ToLower(),
					" because it's a built in function"
				}), null, false);
				return;
			}
			FlowBase flowBase = base.ContentWithNameAtLevel((identifier != null) ? identifier.name : null, new FlowLevel?(FlowLevel.Knot), false) as FlowBase;
			if (flowBase && (flowBase != obj || symbolType == Story.SymbolType.Arg))
			{
				this.NameConflictError(obj, (identifier != null) ? identifier.name : null, flowBase, text);
				return;
			}
			if (symbolType < Story.SymbolType.List)
			{
				return;
			}
			foreach (KeyValuePair<string, ListDefinition> keyValuePair in this._listDefs)
			{
				string key = keyValuePair.Key;
				ListDefinition value = keyValuePair.Value;
				if (((identifier != null) ? identifier.name : null) == key && obj != value && value.variableAssignment != obj)
				{
					this.NameConflictError(obj, (identifier != null) ? identifier.name : null, value, text);
				}
				if (!(obj is ListElementDefinition))
				{
					foreach (ListElementDefinition listElementDefinition in value.itemDefinitions)
					{
						if (((identifier != null) ? identifier.name : null) == listElementDefinition.name)
						{
							this.NameConflictError(obj, (identifier != null) ? identifier.name : null, listElementDefinition, text);
						}
					}
				}
			}
			if (symbolType <= Story.SymbolType.Var)
			{
				return;
			}
			VariableAssignment variableAssignment = null;
			if (this.variableDeclarations.TryGetValue((identifier != null) ? identifier.name : null, out variableAssignment) && variableAssignment != obj && variableAssignment.isGlobalDeclaration && variableAssignment.listDefinition == null)
			{
				this.NameConflictError(obj, (identifier != null) ? identifier.name : null, variableAssignment, text);
			}
			if (symbolType < Story.SymbolType.SubFlowAndWeave)
			{
				return;
			}
			Object @object = new Path(identifier).ResolveFromContext(obj);
			if (@object && @object != obj)
			{
				this.NameConflictError(obj, (identifier != null) ? identifier.name : null, @object, text);
				return;
			}
			if (symbolType < Story.SymbolType.Arg)
			{
				return;
			}
			if (symbolType != Story.SymbolType.Arg)
			{
				FlowBase flowBase2 = obj as FlowBase;
				if (flowBase2 == null)
				{
					flowBase2 = obj.ClosestFlowBase();
				}
				if (flowBase2 && flowBase2.hasParameters)
				{
					foreach (FlowBase.Argument argument in flowBase2.arguments)
					{
						Identifier identifier2 = argument.identifier;
						if (((identifier2 != null) ? identifier2.name : null) == ((identifier != null) ? identifier.name : null))
						{
							string[] array = new string[7];
							array[0] = text;
							array[1] = " '";
							array[2] = base.name;
							array[3] = "': Name has already been used for a argument to ";
							int num = 4;
							Identifier identifier3 = flowBase2.identifier;
							array[num] = ((identifier3 != null) ? identifier3.ToString() : null);
							array[5] = " on ";
							int num2 = 6;
							DebugMetadata debugMetadata = flowBase2.debugMetadata;
							array[num2] = ((debugMetadata != null) ? debugMetadata.ToString() : null);
							obj.Error(string.Concat(array), null, false);
							break;
						}
					}
				}
			}
		}

		// Token: 0x04000181 RID: 385
		public Dictionary<string, Expression> constants;

		// Token: 0x04000182 RID: 386
		public Dictionary<string, ExternalDeclaration> externals;

		// Token: 0x04000183 RID: 387
		public bool countAllVisits;

		// Token: 0x04000184 RID: 388
		private ErrorHandler _errorHandler;

		// Token: 0x04000185 RID: 389
		private bool _hadError;

		// Token: 0x04000186 RID: 390
		private bool _hadWarning;

		// Token: 0x04000187 RID: 391
		private HashSet<Container> _dontFlattenContainers = new HashSet<Container>();

		// Token: 0x04000188 RID: 392
		private Dictionary<string, ListDefinition> _listDefs;

		// Token: 0x020000AC RID: 172
		public enum SymbolType : uint
		{
			// Token: 0x04000299 RID: 665
			Knot,
			// Token: 0x0400029A RID: 666
			List,
			// Token: 0x0400029B RID: 667
			ListItem,
			// Token: 0x0400029C RID: 668
			Var,
			// Token: 0x0400029D RID: 669
			SubFlowAndWeave,
			// Token: 0x0400029E RID: 670
			Arg,
			// Token: 0x0400029F RID: 671
			Temp
		}
	}
}
