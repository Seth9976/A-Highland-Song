using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000065 RID: 101
	public class Weave : Object
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0001A1E1 File Offset: 0x000183E1
		public Container rootContainer
		{
			get
			{
				if (this._rootContainer == null)
				{
					this.GenerateRuntimeObject();
				}
				return this._rootContainer;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0001A1FE File Offset: 0x000183FE
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0001A206 File Offset: 0x00018406
		private Container currentContainer { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0001A20F File Offset: 0x0001840F
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0001A217 File Offset: 0x00018417
		public int baseIndentIndex { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0001A220 File Offset: 0x00018420
		public Object lastParsedSignificantObject
		{
			get
			{
				if (base.content.Count == 0)
				{
					return null;
				}
				Object @object = null;
				for (int i = base.content.Count - 1; i >= 0; i--)
				{
					@object = base.content[i];
					Text text = @object as Text;
					if ((!text || !(text.text == "\n")) && !this.IsGlobalDeclaration(@object))
					{
						break;
					}
				}
				Weave weave = @object as Weave;
				if (weave)
				{
					@object = weave.lastParsedSignificantObject;
				}
				return @object;
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001A2A4 File Offset: 0x000184A4
		public Weave(List<Object> cont, int indentIndex = -1)
		{
			if (indentIndex == -1)
			{
				this.baseIndentIndex = this.DetermineBaseIndentationFromContent(cont);
			}
			else
			{
				this.baseIndentIndex = indentIndex;
			}
			base.AddContent<Object>(cont);
			this.ConstructWeaveHierarchyFromIndentation();
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001A2D4 File Offset: 0x000184D4
		public void ResolveWeavePointNaming()
		{
			List<IWeavePoint> list = base.FindAll<IWeavePoint>((IWeavePoint w) => !string.IsNullOrEmpty(w.name));
			this._namedWeavePoints = new Dictionary<string, IWeavePoint>();
			foreach (IWeavePoint weavePoint in list)
			{
				IWeavePoint weavePoint2;
				if (this._namedWeavePoints.TryGetValue(weavePoint.name, out weavePoint2))
				{
					string text = ((weavePoint2 is Gather) ? "gather" : "choice");
					Object @object = (Object)weavePoint2;
					this.Error(string.Concat(new string[]
					{
						"A ",
						text,
						" with the same label name '",
						weavePoint.name,
						"' already exists in this context on line ",
						@object.debugMetadata.startLineNumber.ToString()
					}), (Object)weavePoint, false);
				}
				this._namedWeavePoints[weavePoint.name] = weavePoint;
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001A3E8 File Offset: 0x000185E8
		private void ConstructWeaveHierarchyFromIndentation()
		{
			for (int i = 0; i < base.content.Count; i++)
			{
				Object @object = base.content[i];
				if (@object is IWeavePoint)
				{
					int num = ((IWeavePoint)@object).indentationDepth - 1;
					if (num > this.baseIndentIndex)
					{
						int num2 = i;
						while (i < base.content.Count)
						{
							IWeavePoint weavePoint = base.content[i] as IWeavePoint;
							if (weavePoint != null && weavePoint.indentationDepth - 1 <= this.baseIndentIndex)
							{
								break;
							}
							i++;
						}
						int num3 = i - num2;
						List<Object> range = base.content.GetRange(num2, num3);
						base.content.RemoveRange(num2, num3);
						Weave weave = new Weave(range, num);
						base.InsertContent<Weave>(num2, weave);
						i = num2;
					}
				}
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001A4B4 File Offset: 0x000186B4
		public int DetermineBaseIndentationFromContent(List<Object> contentList)
		{
			foreach (Object @object in contentList)
			{
				if (@object is IWeavePoint)
				{
					return ((IWeavePoint)@object).indentationDepth - 1;
				}
			}
			return 0;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001A518 File Offset: 0x00018718
		public override Object GenerateRuntimeObject()
		{
			this._rootContainer = (this.currentContainer = new Container());
			this.looseEnds = new List<IWeavePoint>();
			this.gatherPointsToResolve = new List<Weave.GatherPointToResolve>();
			foreach (Object @object in base.content)
			{
				if (@object is IWeavePoint)
				{
					this.AddRuntimeForWeavePoint((IWeavePoint)@object);
				}
				else if (@object is Weave)
				{
					Weave weave = (Weave)@object;
					this.AddRuntimeForNestedWeave(weave);
					this.gatherPointsToResolve.AddRange(weave.gatherPointsToResolve);
				}
				else
				{
					this.AddGeneralRuntimeContent(@object.runtimeObject);
				}
			}
			this.PassLooseEndsToAncestors();
			return this._rootContainer;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001A5E8 File Offset: 0x000187E8
		private void AddRuntimeForGather(Gather gather)
		{
			bool flag = !this.hasSeenChoiceInSection;
			this.hasSeenChoiceInSection = false;
			Container runtimeContainer = gather.runtimeContainer;
			if (gather.name == null)
			{
				runtimeContainer.name = "g-" + this._unnamedGatherCount.ToString();
				this._unnamedGatherCount++;
			}
			if (flag)
			{
				this.currentContainer.AddContent(runtimeContainer);
			}
			else
			{
				this._rootContainer.AddToNamedContentOnly(runtimeContainer);
			}
			foreach (IWeavePoint weavePoint in this.looseEnds)
			{
				Object @object = (Object)weavePoint;
				if (!(@object is Gather) || ((Gather)@object).indentationDepth != gather.indentationDepth)
				{
					Divert divert;
					if (@object is Divert)
					{
						divert = (Divert)@object.runtimeObject;
					}
					else
					{
						divert = new Divert();
						(@object as IWeavePoint).runtimeContainer.AddContent(divert);
					}
					this.gatherPointsToResolve.Add(new Weave.GatherPointToResolve
					{
						divert = divert,
						targetRuntimeObj = runtimeContainer
					});
				}
			}
			this.looseEnds.Clear();
			this.currentContainer = runtimeContainer;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001A718 File Offset: 0x00018918
		private void AddRuntimeForWeavePoint(IWeavePoint weavePoint)
		{
			if (weavePoint is Gather)
			{
				this.AddRuntimeForGather((Gather)weavePoint);
			}
			else if (weavePoint is Choice)
			{
				if (this.previousWeavePoint is Gather)
				{
					this.looseEnds.Remove(this.previousWeavePoint);
				}
				Choice choice = (Choice)weavePoint;
				this.currentContainer.AddContent(choice.runtimeObject);
				choice.innerContentContainer.name = "c-" + this._choiceCount.ToString();
				this.currentContainer.AddToNamedContentOnly(choice.innerContentContainer);
				this._choiceCount++;
				this.hasSeenChoiceInSection = true;
			}
			this.addContentToPreviousWeavePoint = false;
			if (this.WeavePointHasLooseEnd(weavePoint))
			{
				this.looseEnds.Add(weavePoint);
				if (weavePoint as Choice)
				{
					this.addContentToPreviousWeavePoint = true;
				}
			}
			this.previousWeavePoint = weavePoint;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001A7FA File Offset: 0x000189FA
		public void AddRuntimeForNestedWeave(Weave nestedResult)
		{
			this.AddGeneralRuntimeContent(nestedResult.rootContainer);
			if (this.previousWeavePoint != null)
			{
				this.looseEnds.Remove(this.previousWeavePoint);
				this.addContentToPreviousWeavePoint = false;
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001A829 File Offset: 0x00018A29
		private void AddGeneralRuntimeContent(Object content)
		{
			if (content == null)
			{
				return;
			}
			if (this.addContentToPreviousWeavePoint)
			{
				this.previousWeavePoint.runtimeContainer.AddContent(content);
				return;
			}
			this.currentContainer.AddContent(content);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001A85C File Offset: 0x00018A5C
		private void PassLooseEndsToAncestors()
		{
			if (this.looseEnds.Count == 0)
			{
				return;
			}
			Weave weave = null;
			Weave weave2 = null;
			bool flag = false;
			Object @object = base.parent;
			while (@object != null)
			{
				Weave weave3 = @object as Weave;
				if (weave3 != null)
				{
					if (!flag && weave == null)
					{
						weave = weave3;
					}
					if (flag && weave2 == null)
					{
						weave2 = weave3;
					}
				}
				if (@object is Sequence || @object is Conditional)
				{
					flag = true;
				}
				@object = @object.parent;
			}
			if (weave == null && weave2 == null)
			{
				return;
			}
			for (int i = this.looseEnds.Count - 1; i >= 0; i--)
			{
				IWeavePoint weavePoint = this.looseEnds[i];
				bool flag2 = false;
				if (flag)
				{
					if (weavePoint is Choice && weave != null)
					{
						weave.ReceiveLooseEnd(weavePoint);
						flag2 = true;
					}
					else if (!(weavePoint is Choice))
					{
						Weave weave4 = weave ?? weave2;
						if (weave4 != null)
						{
							weave4.ReceiveLooseEnd(weavePoint);
							flag2 = true;
						}
					}
				}
				else
				{
					weave.ReceiveLooseEnd(weavePoint);
					flag2 = true;
				}
				if (flag2)
				{
					this.looseEnds.RemoveAt(i);
				}
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001A983 File Offset: 0x00018B83
		private void ReceiveLooseEnd(IWeavePoint childWeaveLooseEnd)
		{
			this.looseEnds.Add(childWeaveLooseEnd);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001A994 File Offset: 0x00018B94
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			if (this.looseEnds != null && this.looseEnds.Count > 0)
			{
				bool flag = false;
				Object @object = base.parent;
				while (@object != null)
				{
					if (@object is Sequence || @object is Conditional)
					{
						flag = true;
						break;
					}
					@object = @object.parent;
				}
				if (flag)
				{
					this.ValidateTermination(new Weave.BadTerminationHandler(this.BadNestedTerminationHandler));
				}
			}
			foreach (Weave.GatherPointToResolve gatherPointToResolve in this.gatherPointsToResolve)
			{
				gatherPointToResolve.divert.targetPath = gatherPointToResolve.targetRuntimeObj.path;
			}
			this.CheckForWeavePointNamingCollisions();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001AA60 File Offset: 0x00018C60
		public IWeavePoint WeavePointNamed(string name)
		{
			if (this._namedWeavePoints == null)
			{
				return null;
			}
			IWeavePoint weavePoint = null;
			if (this._namedWeavePoints.TryGetValue(name, out weavePoint))
			{
				return weavePoint;
			}
			return null;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001AA8C File Offset: 0x00018C8C
		private bool IsGlobalDeclaration(Object obj)
		{
			VariableAssignment variableAssignment = obj as VariableAssignment;
			return (variableAssignment && variableAssignment.isGlobalDeclaration && variableAssignment.isDeclaration) || obj as ConstantDeclaration;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001AACA File Offset: 0x00018CCA
		private IEnumerable<Object> ContentThatFollowsWeavePoint(IWeavePoint weavePoint)
		{
			Object obj = (Object)weavePoint;
			if (obj.content != null)
			{
				foreach (Object @object in obj.content)
				{
					if (!this.IsGlobalDeclaration(@object))
					{
						yield return @object;
					}
				}
				List<Object>.Enumerator enumerator = default(List<Object>.Enumerator);
			}
			Weave parentWeave = obj.parent as Weave;
			if (parentWeave == null)
			{
				throw new Exception("Expected weave point parent to be weave?");
			}
			int num = parentWeave.content.IndexOf(obj);
			int num2;
			for (int i = num + 1; i < parentWeave.content.Count; i = num2 + 1)
			{
				Object object2 = parentWeave.content[i];
				if (!this.IsGlobalDeclaration(object2))
				{
					if (object2 is IWeavePoint || object2 is Weave)
					{
						break;
					}
					yield return object2;
				}
				num2 = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001AAE4 File Offset: 0x00018CE4
		public void ValidateTermination(Weave.BadTerminationHandler badTerminationHandler)
		{
			if (this.lastParsedSignificantObject is AuthorWarning)
			{
				return;
			}
			if (this.looseEnds != null && this.looseEnds.Count > 0)
			{
				using (List<IWeavePoint>.Enumerator enumerator = this.looseEnds.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IWeavePoint weavePoint = enumerator.Current;
						IEnumerable<Object> enumerable = this.ContentThatFollowsWeavePoint(weavePoint);
						this.ValidateFlowOfObjectsTerminates(enumerable, (Object)weavePoint, badTerminationHandler);
					}
					return;
				}
			}
			using (List<Object>.Enumerator enumerator2 = base.content.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current is IWeavePoint)
					{
						return;
					}
				}
			}
			this.ValidateFlowOfObjectsTerminates(base.content, this, badTerminationHandler);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001ABC0 File Offset: 0x00018DC0
		private void BadNestedTerminationHandler(Object terminatingObj)
		{
			Conditional conditional = null;
			Object @object = terminatingObj.parent;
			while (@object != null)
			{
				if (@object is Sequence || @object is Conditional)
				{
					conditional = @object as Conditional;
					break;
				}
				@object = @object.parent;
			}
			string text = "Choices nested in conditionals or sequences need to explicitly divert afterwards.";
			if (conditional != null && conditional.FindAll<Choice>(null).Count == 1)
			{
				text = "Choices with conditions should be written: '* {condition} choice'. Otherwise, " + text.ToLower();
			}
			this.Error(text, terminatingObj, false);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001AC3C File Offset: 0x00018E3C
		private void ValidateFlowOfObjectsTerminates(IEnumerable<Object> objFlow, Object defaultObj, Weave.BadTerminationHandler badTerminationHandler)
		{
			bool flag = false;
			Object @object = defaultObj;
			foreach (Object object2 in objFlow)
			{
				if (object2.Find<Divert>((Divert d) => !d.isThread && !d.isTunnel && !d.isFunctionCall && !(d.parent is DivertTarget)) != null)
				{
					flag = true;
				}
				if (object2.Find<TunnelOnwards>(null) != null)
				{
					flag = true;
					break;
				}
				@object = object2;
			}
			if (!flag)
			{
				if (@object is AuthorWarning)
				{
					return;
				}
				badTerminationHandler(@object);
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001ACD8 File Offset: 0x00018ED8
		private bool WeavePointHasLooseEnd(IWeavePoint weavePoint)
		{
			if (weavePoint.content == null)
			{
				return true;
			}
			for (int i = weavePoint.content.Count - 1; i >= 0; i--)
			{
				Divert divert = weavePoint.content[i] as Divert;
				if (divert && !divert.isThread && !divert.isTunnel && !divert.isFunctionCall)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001AD40 File Offset: 0x00018F40
		private void CheckForWeavePointNamingCollisions()
		{
			if (this._namedWeavePoints == null)
			{
				return;
			}
			List<FlowBase> list = new List<FlowBase>();
			foreach (Object @object in base.ancestry)
			{
				FlowBase flowBase = @object as FlowBase;
				if (!flowBase)
				{
					break;
				}
				list.Add(flowBase);
			}
			foreach (KeyValuePair<string, IWeavePoint> keyValuePair in this._namedWeavePoints)
			{
				string key = keyValuePair.Key;
				Object object2 = (Object)keyValuePair.Value;
				foreach (FlowBase flowBase2 in list)
				{
					Object object3 = flowBase2.ContentWithNameAtLevel(key, null, false);
					if (object3 && object3 != object2)
					{
						string text = string.Format("{0} '{1}' has the same label name as a {2} (on {3})", new object[]
						{
							object2.GetType().Name,
							key,
							object3.GetType().Name,
							object3.debugMetadata
						});
						this.Error(text, object2, false);
					}
				}
			}
		}

		// Token: 0x0400019B RID: 411
		public List<IWeavePoint> looseEnds;

		// Token: 0x0400019C RID: 412
		public List<Weave.GatherPointToResolve> gatherPointsToResolve;

		// Token: 0x0400019D RID: 413
		private IWeavePoint previousWeavePoint;

		// Token: 0x0400019E RID: 414
		private bool addContentToPreviousWeavePoint;

		// Token: 0x0400019F RID: 415
		private bool hasSeenChoiceInSection;

		// Token: 0x040001A0 RID: 416
		private int _unnamedGatherCount;

		// Token: 0x040001A1 RID: 417
		private int _choiceCount;

		// Token: 0x040001A2 RID: 418
		private Container _rootContainer;

		// Token: 0x040001A3 RID: 419
		private Dictionary<string, IWeavePoint> _namedWeavePoints;

		// Token: 0x020000AE RID: 174
		public class GatherPointToResolve
		{
			// Token: 0x040002A3 RID: 675
			public Divert divert;

			// Token: 0x040002A4 RID: 676
			public Object targetRuntimeObj;
		}

		// Token: 0x020000AF RID: 175
		// (Invoke) Token: 0x06000615 RID: 1557
		public delegate void BadTerminationHandler(Object terminatingObj);
	}
}
