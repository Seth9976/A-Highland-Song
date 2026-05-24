using System;
using System.Collections.Generic;
using System.Text;

namespace Ink.Runtime
{
	// Token: 0x02000012 RID: 18
	public class CallStack
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00007544 File Offset: 0x00005744
		public List<CallStack.Element> elements
		{
			get
			{
				return this.callStack;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000FD RID: 253 RVA: 0x0000754C File Offset: 0x0000574C
		public int depth
		{
			get
			{
				return this.elements.Count;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00007559 File Offset: 0x00005759
		public CallStack.Element currentElement
		{
			get
			{
				List<CallStack.Element> callstack = this._threads[this._threads.Count - 1].callstack;
				return callstack[callstack.Count - 1];
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00007585 File Offset: 0x00005785
		public int currentElementIndex
		{
			get
			{
				return this.callStack.Count - 1;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00007594 File Offset: 0x00005794
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000075AE File Offset: 0x000057AE
		public CallStack.Thread currentThread
		{
			get
			{
				return this._threads[this._threads.Count - 1];
			}
			set
			{
				this._threads.Clear();
				this._threads.Add(value);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000075C7 File Offset: 0x000057C7
		public bool canPop
		{
			get
			{
				return this.callStack.Count > 1;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000075D7 File Offset: 0x000057D7
		public CallStack(Story storyContext)
		{
			this._startOfRoot = Pointer.StartOf(storyContext.rootContentContainer);
			this.Reset();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000075F8 File Offset: 0x000057F8
		public CallStack(CallStack toCopy)
		{
			this._threads = new List<CallStack.Thread>();
			foreach (CallStack.Thread thread in toCopy._threads)
			{
				this._threads.Add(thread.Copy());
			}
			this._threadCounter = toCopy._threadCounter;
			this._startOfRoot = toCopy._startOfRoot;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00007680 File Offset: 0x00005880
		public void Reset()
		{
			this._threads = new List<CallStack.Thread>();
			this._threads.Add(new CallStack.Thread());
			this._threads[0].callstack.Add(new CallStack.Element(PushPopType.Tunnel, this._startOfRoot, false));
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000076C0 File Offset: 0x000058C0
		public void SetJsonToken(Dictionary<string, object> jObject, Story storyContext)
		{
			this._threads.Clear();
			foreach (object obj in ((List<object>)jObject["threads"]))
			{
				CallStack.Thread thread = new CallStack.Thread((Dictionary<string, object>)obj, storyContext);
				this._threads.Add(thread);
			}
			this._threadCounter = (int)jObject["threadCounter"];
			this._startOfRoot = Pointer.StartOf(storyContext.rootContentContainer);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00007760 File Offset: 0x00005960
		public void WriteJson(SimpleJson.Writer w)
		{
			w.WriteObject(delegate(SimpleJson.Writer writer)
			{
				writer.WritePropertyStart("threads");
				writer.WriteArrayStart();
				foreach (CallStack.Thread thread in this._threads)
				{
					thread.WriteJson(writer);
				}
				writer.WriteArrayEnd();
				writer.WritePropertyEnd();
				writer.WritePropertyStart("threadCounter");
				writer.Write(this._threadCounter);
				writer.WritePropertyEnd();
			});
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00007774 File Offset: 0x00005974
		public void PushThread()
		{
			CallStack.Thread thread = this.currentThread.Copy();
			this._threadCounter++;
			thread.threadIndex = this._threadCounter;
			this._threads.Add(thread);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000077B3 File Offset: 0x000059B3
		public CallStack.Thread ForkThread()
		{
			CallStack.Thread thread = this.currentThread.Copy();
			this._threadCounter++;
			thread.threadIndex = this._threadCounter;
			return thread;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000077DA File Offset: 0x000059DA
		public void PopThread()
		{
			if (this.canPopThread)
			{
				this._threads.Remove(this.currentThread);
				return;
			}
			throw new Exception("Can't pop thread");
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00007801 File Offset: 0x00005A01
		public bool canPopThread
		{
			get
			{
				return this._threads.Count > 1 && !this.elementIsEvaluateFromGame;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600010C RID: 268 RVA: 0x0000781C File Offset: 0x00005A1C
		public bool elementIsEvaluateFromGame
		{
			get
			{
				return this.currentElement.type == PushPopType.FunctionEvaluationFromGame;
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000782C File Offset: 0x00005A2C
		public void Push(PushPopType type, int externalEvaluationStackHeight = 0, int outputStreamLengthWithPushed = 0)
		{
			CallStack.Element element = new CallStack.Element(type, this.currentElement.currentPointer, false);
			element.evaluationStackHeightWhenPushed = externalEvaluationStackHeight;
			element.functionStartInOuputStream = outputStreamLengthWithPushed;
			this.callStack.Add(element);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007868 File Offset: 0x00005A68
		public bool CanPop(PushPopType? type = null)
		{
			if (!this.canPop)
			{
				return false;
			}
			if (type == null)
			{
				return true;
			}
			PushPopType type2 = this.currentElement.type;
			PushPopType? pushPopType = type;
			return (type2 == pushPopType.GetValueOrDefault()) & (pushPopType != null);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000078A8 File Offset: 0x00005AA8
		public void Pop(PushPopType? type = null)
		{
			if (this.CanPop(type))
			{
				this.callStack.RemoveAt(this.callStack.Count - 1);
				return;
			}
			throw new Exception("Mismatched push/pop in Callstack");
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000078D8 File Offset: 0x00005AD8
		public Object GetTemporaryVariableWithName(string name, int contextIndex = -1)
		{
			if (contextIndex == -1)
			{
				contextIndex = this.currentElementIndex + 1;
			}
			Object @object = null;
			if (this.callStack[contextIndex - 1].temporaryVariables.TryGetValue(name, out @object))
			{
				return @object;
			}
			return null;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00007918 File Offset: 0x00005B18
		public void SetTemporaryVariable(string name, Object value, bool declareNew, int contextIndex = -1)
		{
			if (contextIndex == -1)
			{
				contextIndex = this.currentElementIndex + 1;
			}
			CallStack.Element element = this.callStack[contextIndex - 1];
			if (!declareNew && !element.temporaryVariables.ContainsKey(name))
			{
				throw new Exception("Could not find temporary variable to set: " + name);
			}
			Object @object;
			if (element.temporaryVariables.TryGetValue(name, out @object))
			{
				ListValue.RetainListOriginsForAssignment(@object, value);
			}
			element.temporaryVariables[name] = value;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000798A File Offset: 0x00005B8A
		public int ContextForVariableNamed(string name)
		{
			if (this.currentElement.temporaryVariables.ContainsKey(name))
			{
				return this.currentElementIndex + 1;
			}
			return 0;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000079AC File Offset: 0x00005BAC
		public CallStack.Thread ThreadWithIndex(int index)
		{
			return this._threads.Find((CallStack.Thread t) => t.threadIndex == index);
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000079DD File Offset: 0x00005BDD
		private List<CallStack.Element> callStack
		{
			get
			{
				return this.currentThread.callstack;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000079EC File Offset: 0x00005BEC
		public string callStackTrace
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this._threads.Count; i++)
				{
					CallStack.Thread thread = this._threads[i];
					bool flag = i == this._threads.Count - 1;
					stringBuilder.AppendFormat("=== THREAD {0}/{1} {2}===\n", i + 1, this._threads.Count, flag ? "(current) " : "");
					for (int j = 0; j < thread.callstack.Count; j++)
					{
						if (thread.callstack[j].type == PushPopType.Function)
						{
							stringBuilder.Append("  [FUNCTION] ");
						}
						else
						{
							stringBuilder.Append("  [TUNNEL] ");
						}
						Pointer currentPointer = thread.callstack[j].currentPointer;
						if (!currentPointer.isNull)
						{
							stringBuilder.Append("<SOMEWHERE IN ");
							stringBuilder.Append(currentPointer.container.path.ToString());
							stringBuilder.AppendLine(">");
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x04000048 RID: 72
		private List<CallStack.Thread> _threads;

		// Token: 0x04000049 RID: 73
		private int _threadCounter;

		// Token: 0x0400004A RID: 74
		private Pointer _startOfRoot;

		// Token: 0x02000086 RID: 134
		public class Element
		{
			// Token: 0x06000539 RID: 1337 RVA: 0x0001B299 File Offset: 0x00019499
			public Element(PushPopType type, Pointer pointer, bool inExpressionEvaluation = false)
			{
				this.currentPointer = pointer;
				this.inExpressionEvaluation = inExpressionEvaluation;
				this.temporaryVariables = new Dictionary<string, Object>();
				this.type = type;
			}

			// Token: 0x0600053A RID: 1338 RVA: 0x0001B2C4 File Offset: 0x000194C4
			public CallStack.Element Copy()
			{
				return new CallStack.Element(this.type, this.currentPointer, this.inExpressionEvaluation)
				{
					temporaryVariables = new Dictionary<string, Object>(this.temporaryVariables),
					evaluationStackHeightWhenPushed = this.evaluationStackHeightWhenPushed,
					functionStartInOuputStream = this.functionStartInOuputStream
				};
			}

			// Token: 0x040001E7 RID: 487
			public Pointer currentPointer;

			// Token: 0x040001E8 RID: 488
			public bool inExpressionEvaluation;

			// Token: 0x040001E9 RID: 489
			public Dictionary<string, Object> temporaryVariables;

			// Token: 0x040001EA RID: 490
			public PushPopType type;

			// Token: 0x040001EB RID: 491
			public int evaluationStackHeightWhenPushed;

			// Token: 0x040001EC RID: 492
			public int functionStartInOuputStream;
		}

		// Token: 0x02000087 RID: 135
		public class Thread
		{
			// Token: 0x0600053B RID: 1339 RVA: 0x0001B311 File Offset: 0x00019511
			public Thread()
			{
				this.callstack = new List<CallStack.Element>();
			}

			// Token: 0x0600053C RID: 1340 RVA: 0x0001B324 File Offset: 0x00019524
			public Thread(Dictionary<string, object> jThreadObj, Story storyContext)
				: this()
			{
				this.threadIndex = (int)jThreadObj["threadIndex"];
				foreach (object obj in ((List<object>)jThreadObj["callstack"]))
				{
					Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
					PushPopType pushPopType = (PushPopType)((int)dictionary["type"]);
					Pointer @null = Pointer.Null;
					object obj2;
					if (dictionary.TryGetValue("cPath", out obj2))
					{
						string text = obj2.ToString();
						SearchResult searchResult = storyContext.ContentAtPath(new Path(text));
						@null.container = searchResult.container;
						@null.index = (int)dictionary["idx"];
						if (searchResult.obj == null)
						{
							throw new Exception("When loading state, internal story location couldn't be found: " + text + ". Has the story changed since this save data was created?");
						}
						if (searchResult.approximate)
						{
							storyContext.Warning(string.Concat(new string[]
							{
								"When loading state, exact internal story location couldn't be found: '",
								text,
								"', so it was approximated to '",
								@null.container.path.ToString(),
								"' to recover. Has the story changed since this save data was created?"
							}));
						}
					}
					bool flag = (bool)dictionary["exp"];
					CallStack.Element element = new CallStack.Element(pushPopType, @null, flag);
					object obj3;
					if (dictionary.TryGetValue("temp", out obj3))
					{
						element.temporaryVariables = Json.JObjectToDictionaryRuntimeObjs((Dictionary<string, object>)obj3);
					}
					else
					{
						element.temporaryVariables.Clear();
					}
					this.callstack.Add(element);
				}
				object obj4;
				if (jThreadObj.TryGetValue("previousContentObject", out obj4))
				{
					Path path = new Path((string)obj4);
					this.previousPointer = storyContext.PointerAtPath(path);
				}
			}

			// Token: 0x0600053D RID: 1341 RVA: 0x0001B510 File Offset: 0x00019710
			public CallStack.Thread Copy()
			{
				CallStack.Thread thread = new CallStack.Thread();
				thread.threadIndex = this.threadIndex;
				foreach (CallStack.Element element in this.callstack)
				{
					thread.callstack.Add(element.Copy());
				}
				thread.previousPointer = this.previousPointer;
				return thread;
			}

			// Token: 0x0600053E RID: 1342 RVA: 0x0001B58C File Offset: 0x0001978C
			public void WriteJson(SimpleJson.Writer writer)
			{
				writer.WriteObjectStart();
				writer.WritePropertyStart("callstack");
				writer.WriteArrayStart();
				foreach (CallStack.Element element in this.callstack)
				{
					writer.WriteObjectStart();
					if (!element.currentPointer.isNull)
					{
						writer.WriteProperty("cPath", element.currentPointer.container.path.componentsString);
						writer.WriteProperty("idx", element.currentPointer.index);
					}
					writer.WriteProperty("exp", element.inExpressionEvaluation);
					writer.WriteProperty("type", (int)element.type);
					if (element.temporaryVariables.Count > 0)
					{
						writer.WritePropertyStart("temp");
						Json.WriteDictionaryRuntimeObjs(writer, element.temporaryVariables);
						writer.WritePropertyEnd();
					}
					writer.WriteObjectEnd();
				}
				writer.WriteArrayEnd();
				writer.WritePropertyEnd();
				writer.WriteProperty("threadIndex", this.threadIndex);
				if (!this.previousPointer.isNull)
				{
					writer.WriteProperty("previousContentObject", this.previousPointer.Resolve().path.ToString());
				}
				writer.WriteObjectEnd();
			}

			// Token: 0x040001ED RID: 493
			public List<CallStack.Element> callstack;

			// Token: 0x040001EE RID: 494
			public int threadIndex;

			// Token: 0x040001EF RID: 495
			public Pointer previousPointer;
		}
	}
}
