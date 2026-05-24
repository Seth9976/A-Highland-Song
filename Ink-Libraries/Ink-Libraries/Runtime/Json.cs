using System;
using System.Collections.Generic;
using System.Linq;

namespace Ink.Runtime
{
	// Token: 0x0200001E RID: 30
	public static class Json
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x00009E18 File Offset: 0x00008018
		public static List<T> JArrayToRuntimeObjList<T>(List<object> jArray, bool skipLast = false) where T : Object
		{
			int num = jArray.Count;
			if (skipLast)
			{
				num--;
			}
			List<T> list = new List<T>(jArray.Count);
			for (int i = 0; i < num; i++)
			{
				T t = Json.JTokenToRuntimeObject(jArray[i]) as T;
				list.Add(t);
			}
			return list;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009E6A File Offset: 0x0000806A
		public static List<Object> JArrayToRuntimeObjList(List<object> jArray, bool skipLast = false)
		{
			return Json.JArrayToRuntimeObjList<Object>(jArray, skipLast);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009E74 File Offset: 0x00008074
		public static void WriteDictionaryRuntimeObjs(SimpleJson.Writer writer, Dictionary<string, Object> dictionary)
		{
			writer.WriteObjectStart();
			foreach (KeyValuePair<string, Object> keyValuePair in dictionary)
			{
				writer.WritePropertyStart(keyValuePair.Key);
				Json.WriteRuntimeObject(writer, keyValuePair.Value);
				writer.WritePropertyEnd();
			}
			writer.WriteObjectEnd();
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009EE8 File Offset: 0x000080E8
		public static void WriteListRuntimeObjs(SimpleJson.Writer writer, List<Object> list)
		{
			writer.WriteArrayStart();
			foreach (Object @object in list)
			{
				Json.WriteRuntimeObject(writer, @object);
			}
			writer.WriteArrayEnd();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00009F44 File Offset: 0x00008144
		public static void WriteIntDictionary(SimpleJson.Writer writer, Dictionary<string, int> dict)
		{
			writer.WriteObjectStart();
			foreach (KeyValuePair<string, int> keyValuePair in dict)
			{
				writer.WriteProperty(keyValuePair.Key, keyValuePair.Value);
			}
			writer.WriteObjectEnd();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00009FAC File Offset: 0x000081AC
		public static void WriteRuntimeObject(SimpleJson.Writer writer, Object obj)
		{
			Container container = obj as Container;
			if (container)
			{
				Json.WriteRuntimeContainer(writer, container, false);
				return;
			}
			Divert divert = obj as Divert;
			if (divert)
			{
				string text = "->";
				if (divert.isExternal)
				{
					text = "x()";
				}
				else if (divert.pushesToStack)
				{
					if (divert.stackPushType == PushPopType.Function)
					{
						text = "f()";
					}
					else if (divert.stackPushType == PushPopType.Tunnel)
					{
						text = "->t->";
					}
				}
				string text2;
				if (divert.hasVariableTarget)
				{
					text2 = divert.variableDivertName;
				}
				else
				{
					text2 = divert.targetPathString;
				}
				writer.WriteObjectStart();
				writer.WriteProperty(text, text2);
				if (divert.hasVariableTarget)
				{
					writer.WriteProperty("var", true);
				}
				if (divert.isConditional)
				{
					writer.WriteProperty("c", true);
				}
				if (divert.externalArgs > 0)
				{
					writer.WriteProperty("exArgs", divert.externalArgs);
				}
				writer.WriteObjectEnd();
				return;
			}
			ChoicePoint choicePoint = obj as ChoicePoint;
			if (choicePoint)
			{
				writer.WriteObjectStart();
				writer.WriteProperty("*", choicePoint.pathStringOnChoice);
				writer.WriteProperty("flg", choicePoint.flags);
				writer.WriteObjectEnd();
				return;
			}
			BoolValue boolValue = obj as BoolValue;
			if (boolValue)
			{
				writer.Write(boolValue.value);
				return;
			}
			IntValue intValue = obj as IntValue;
			if (intValue)
			{
				writer.Write(intValue.value);
				return;
			}
			FloatValue floatValue = obj as FloatValue;
			if (floatValue)
			{
				writer.Write(floatValue.value);
				return;
			}
			StringValue stringValue = obj as StringValue;
			if (stringValue)
			{
				if (stringValue.isNewline)
				{
					writer.Write("\\n", false);
					return;
				}
				writer.WriteStringStart();
				writer.WriteStringInner("^", true);
				writer.WriteStringInner(stringValue.value, true);
				writer.WriteStringEnd();
				return;
			}
			else
			{
				ListValue listValue = obj as ListValue;
				if (listValue)
				{
					Json.WriteInkList(writer, listValue);
					return;
				}
				DivertTargetValue divertTargetValue = obj as DivertTargetValue;
				if (divertTargetValue)
				{
					writer.WriteObjectStart();
					writer.WriteProperty("^->", divertTargetValue.value.componentsString);
					writer.WriteObjectEnd();
					return;
				}
				VariablePointerValue variablePointerValue = obj as VariablePointerValue;
				if (variablePointerValue)
				{
					writer.WriteObjectStart();
					writer.WriteProperty("^var", variablePointerValue.value);
					writer.WriteProperty("ci", variablePointerValue.contextIndex);
					writer.WriteObjectEnd();
					return;
				}
				if (obj as Glue)
				{
					writer.Write("<>", true);
					return;
				}
				ControlCommand controlCommand = obj as ControlCommand;
				if (controlCommand)
				{
					writer.Write(Json._controlCommandNames[(int)controlCommand.commandType], true);
					return;
				}
				NativeFunctionCall nativeFunctionCall = obj as NativeFunctionCall;
				if (nativeFunctionCall)
				{
					string text3 = nativeFunctionCall.name;
					if (text3 == "^")
					{
						text3 = "L^";
					}
					writer.Write(text3, true);
					return;
				}
				VariableReference variableReference = obj as VariableReference;
				if (variableReference)
				{
					writer.WriteObjectStart();
					string pathStringForCount = variableReference.pathStringForCount;
					if (pathStringForCount != null)
					{
						writer.WriteProperty("CNT?", pathStringForCount);
					}
					else
					{
						writer.WriteProperty("VAR?", variableReference.name);
					}
					writer.WriteObjectEnd();
					return;
				}
				VariableAssignment variableAssignment = obj as VariableAssignment;
				if (variableAssignment)
				{
					writer.WriteObjectStart();
					string text4 = (variableAssignment.isGlobal ? "VAR=" : "temp=");
					writer.WriteProperty(text4, variableAssignment.variableName);
					if (!variableAssignment.isNewDeclaration)
					{
						writer.WriteProperty("re", true);
					}
					writer.WriteObjectEnd();
					return;
				}
				if (obj as Void)
				{
					writer.Write("void", true);
					return;
				}
				Tag tag = obj as Tag;
				if (tag)
				{
					writer.WriteObjectStart();
					writer.WriteProperty("#", tag.text);
					writer.WriteObjectEnd();
					return;
				}
				Choice choice = obj as Choice;
				if (choice)
				{
					Json.WriteChoice(writer, choice);
					return;
				}
				throw new Exception("Failed to write runtime object to JSON: " + ((obj != null) ? obj.ToString() : null));
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000A3AC File Offset: 0x000085AC
		public static Dictionary<string, Object> JObjectToDictionaryRuntimeObjs(Dictionary<string, object> jObject)
		{
			Dictionary<string, Object> dictionary = new Dictionary<string, Object>(jObject.Count);
			foreach (KeyValuePair<string, object> keyValuePair in jObject)
			{
				dictionary[keyValuePair.Key] = Json.JTokenToRuntimeObject(keyValuePair.Value);
			}
			return dictionary;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000A41C File Offset: 0x0000861C
		public static Dictionary<string, int> JObjectToIntDictionary(Dictionary<string, object> jObject)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>(jObject.Count);
			foreach (KeyValuePair<string, object> keyValuePair in jObject)
			{
				dictionary[keyValuePair.Key] = (int)keyValuePair.Value;
			}
			return dictionary;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000A48C File Offset: 0x0000868C
		public static Object JTokenToRuntimeObject(object token)
		{
			if (token is int || token is float || token is bool)
			{
				return Value.Create(token);
			}
			if (token is string)
			{
				string text = (string)token;
				char c = text[0];
				if (c == '^')
				{
					return new StringValue(text.Substring(1));
				}
				if (c == '\n' && text.Length == 1)
				{
					return new StringValue("\n");
				}
				if (text == "<>")
				{
					return new Glue();
				}
				for (int i = 0; i < Json._controlCommandNames.Length; i++)
				{
					string text2 = Json._controlCommandNames[i];
					if (text == text2)
					{
						return new ControlCommand((ControlCommand.CommandType)i);
					}
				}
				if (text == "L^")
				{
					text = "^";
				}
				if (NativeFunctionCall.CallExistsWithName(text))
				{
					return NativeFunctionCall.CallWithName(text);
				}
				if (text == "->->")
				{
					return ControlCommand.PopTunnel();
				}
				if (text == "~ret")
				{
					return ControlCommand.PopFunction();
				}
				if (text == "void")
				{
					return new Void();
				}
			}
			if (token is Dictionary<string, object>)
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)token;
				object obj;
				if (dictionary.TryGetValue("^->", out obj))
				{
					return new DivertTargetValue(new Path((string)obj));
				}
				if (dictionary.TryGetValue("^var", out obj))
				{
					VariablePointerValue variablePointerValue = new VariablePointerValue((string)obj, -1);
					if (dictionary.TryGetValue("ci", out obj))
					{
						variablePointerValue.contextIndex = (int)obj;
					}
					return variablePointerValue;
				}
				bool flag = false;
				bool flag2 = false;
				PushPopType pushPopType = PushPopType.Function;
				bool flag3 = false;
				if (dictionary.TryGetValue("->", out obj))
				{
					flag = true;
				}
				else if (dictionary.TryGetValue("f()", out obj))
				{
					flag = true;
					flag2 = true;
					pushPopType = PushPopType.Function;
				}
				else if (dictionary.TryGetValue("->t->", out obj))
				{
					flag = true;
					flag2 = true;
					pushPopType = PushPopType.Tunnel;
				}
				else if (dictionary.TryGetValue("x()", out obj))
				{
					flag = true;
					flag3 = true;
					flag2 = false;
					pushPopType = PushPopType.Function;
				}
				if (flag)
				{
					Divert divert = new Divert();
					divert.pushesToStack = flag2;
					divert.stackPushType = pushPopType;
					divert.isExternal = flag3;
					string text3 = obj.ToString();
					if (dictionary.TryGetValue("var", out obj))
					{
						divert.variableDivertName = text3;
					}
					else
					{
						divert.targetPathString = text3;
					}
					divert.isConditional = dictionary.TryGetValue("c", out obj);
					if (flag3 && dictionary.TryGetValue("exArgs", out obj))
					{
						divert.externalArgs = (int)obj;
					}
					return divert;
				}
				if (dictionary.TryGetValue("*", out obj))
				{
					ChoicePoint choicePoint = new ChoicePoint();
					choicePoint.pathStringOnChoice = obj.ToString();
					if (dictionary.TryGetValue("flg", out obj))
					{
						choicePoint.flags = (int)obj;
					}
					return choicePoint;
				}
				if (dictionary.TryGetValue("VAR?", out obj))
				{
					return new VariableReference(obj.ToString());
				}
				if (dictionary.TryGetValue("CNT?", out obj))
				{
					return new VariableReference
					{
						pathStringForCount = obj.ToString()
					};
				}
				bool flag4 = false;
				bool flag5 = false;
				if (dictionary.TryGetValue("VAR=", out obj))
				{
					flag4 = true;
					flag5 = true;
				}
				else if (dictionary.TryGetValue("temp=", out obj))
				{
					flag4 = true;
					flag5 = false;
				}
				if (flag4)
				{
					string text4 = obj.ToString();
					bool flag6 = !dictionary.TryGetValue("re", out obj);
					return new VariableAssignment(text4, flag6)
					{
						isGlobal = flag5
					};
				}
				if (dictionary.TryGetValue("#", out obj))
				{
					return new Tag((string)obj);
				}
				if (dictionary.TryGetValue("list", out obj))
				{
					Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
					InkList inkList = new InkList();
					if (dictionary.TryGetValue("origins", out obj))
					{
						List<object> list = (List<object>)obj;
						inkList.SetInitialOriginNames(list.Cast<string>().ToList<string>());
					}
					foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
					{
						InkListItem inkListItem = new InkListItem(keyValuePair.Key);
						int num = (int)keyValuePair.Value;
						inkList.Add(inkListItem, num);
					}
					return new ListValue(inkList);
				}
				if (dictionary["originalChoicePath"] != null)
				{
					return Json.JObjectToChoice(dictionary);
				}
			}
			if (token is List<object>)
			{
				return Json.JArrayToContainer((List<object>)token);
			}
			if (token == null)
			{
				return null;
			}
			throw new Exception("Failed to convert token to runtime object: " + ((token != null) ? token.ToString() : null));
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000A914 File Offset: 0x00008B14
		public static void WriteRuntimeContainer(SimpleJson.Writer writer, Container container, bool withoutName = false)
		{
			writer.WriteArrayStart();
			foreach (Object @object in container.content)
			{
				Json.WriteRuntimeObject(writer, @object);
			}
			Dictionary<string, Object> namedOnlyContent = container.namedOnlyContent;
			int countFlags = container.countFlags;
			bool flag = container.name != null && !withoutName;
			bool flag2 = namedOnlyContent != null || countFlags > 0 || flag;
			if (flag2)
			{
				writer.WriteObjectStart();
			}
			if (namedOnlyContent != null)
			{
				foreach (KeyValuePair<string, Object> keyValuePair in namedOnlyContent)
				{
					string key = keyValuePair.Key;
					Container container2 = keyValuePair.Value as Container;
					writer.WritePropertyStart(key);
					Json.WriteRuntimeContainer(writer, container2, true);
					writer.WritePropertyEnd();
				}
			}
			if (countFlags > 0)
			{
				writer.WriteProperty("#f", countFlags);
			}
			if (flag)
			{
				writer.WriteProperty("#n", container.name);
			}
			if (flag2)
			{
				writer.WriteObjectEnd();
			}
			else
			{
				writer.WriteNull();
			}
			writer.WriteArrayEnd();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000AA48 File Offset: 0x00008C48
		private static Container JArrayToContainer(List<object> jArray)
		{
			Container container = new Container();
			container.content = Json.JArrayToRuntimeObjList(jArray, true);
			Dictionary<string, object> dictionary = jArray[jArray.Count - 1] as Dictionary<string, object>;
			if (dictionary != null)
			{
				Dictionary<string, Object> dictionary2 = new Dictionary<string, Object>(dictionary.Count);
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					if (keyValuePair.Key == "#f")
					{
						container.countFlags = (int)keyValuePair.Value;
					}
					else if (keyValuePair.Key == "#n")
					{
						container.name = keyValuePair.Value.ToString();
					}
					else
					{
						Object @object = Json.JTokenToRuntimeObject(keyValuePair.Value);
						Container container2 = @object as Container;
						if (container2)
						{
							container2.name = keyValuePair.Key;
						}
						dictionary2[keyValuePair.Key] = @object;
					}
				}
				container.namedOnlyContent = dictionary2;
			}
			return container;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000AB64 File Offset: 0x00008D64
		private static Choice JObjectToChoice(Dictionary<string, object> jObj)
		{
			return new Choice
			{
				text = jObj["text"].ToString(),
				index = (int)jObj["index"],
				sourcePath = jObj["originalChoicePath"].ToString(),
				originalThreadIndex = (int)jObj["originalThreadIndex"],
				pathStringOnChoice = jObj["targetPath"].ToString()
			};
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000ABE4 File Offset: 0x00008DE4
		public static void WriteChoice(SimpleJson.Writer writer, Choice choice)
		{
			writer.WriteObjectStart();
			writer.WriteProperty("text", choice.text);
			writer.WriteProperty("index", choice.index);
			writer.WriteProperty("originalChoicePath", choice.sourcePath);
			writer.WriteProperty("originalThreadIndex", choice.originalThreadIndex);
			writer.WriteProperty("targetPath", choice.pathStringOnChoice);
			writer.WriteObjectEnd();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000AC54 File Offset: 0x00008E54
		private static void WriteInkList(SimpleJson.Writer writer, ListValue listVal)
		{
			InkList value = listVal.value;
			writer.WriteObjectStart();
			writer.WritePropertyStart("list");
			writer.WriteObjectStart();
			foreach (KeyValuePair<InkListItem, int> keyValuePair in value)
			{
				InkListItem key = keyValuePair.Key;
				int value2 = keyValuePair.Value;
				writer.WritePropertyNameStart();
				writer.WritePropertyNameInner(key.originName ?? "?");
				writer.WritePropertyNameInner(".");
				writer.WritePropertyNameInner(key.itemName);
				writer.WritePropertyNameEnd();
				writer.Write(value2);
				writer.WritePropertyEnd();
			}
			writer.WriteObjectEnd();
			writer.WritePropertyEnd();
			if (value.Count == 0 && value.originNames != null && value.originNames.Count > 0)
			{
				writer.WritePropertyStart("origins");
				writer.WriteArrayStart();
				foreach (string text in value.originNames)
				{
					writer.Write(text, true);
				}
				writer.WriteArrayEnd();
				writer.WritePropertyEnd();
			}
			writer.WriteObjectEnd();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000ADA4 File Offset: 0x00008FA4
		public static ListDefinitionsOrigin JTokenToListDefinitions(object obj)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			List<ListDefinition> list = new List<ListDefinition>();
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				string key = keyValuePair.Key;
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)keyValuePair.Value;
				Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
				foreach (KeyValuePair<string, object> keyValuePair2 in dictionary2)
				{
					dictionary3.Add(keyValuePair2.Key, (int)keyValuePair2.Value);
				}
				ListDefinition listDefinition = new ListDefinition(key, dictionary3);
				list.Add(listDefinition);
			}
			return new ListDefinitionsOrigin(list);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000AE80 File Offset: 0x00009080
		static Json()
		{
			Json._controlCommandNames[0] = "ev";
			Json._controlCommandNames[1] = "out";
			Json._controlCommandNames[2] = "/ev";
			Json._controlCommandNames[3] = "du";
			Json._controlCommandNames[4] = "pop";
			Json._controlCommandNames[5] = "~ret";
			Json._controlCommandNames[6] = "->->";
			Json._controlCommandNames[7] = "str";
			Json._controlCommandNames[8] = "/str";
			Json._controlCommandNames[9] = "nop";
			Json._controlCommandNames[10] = "choiceCnt";
			Json._controlCommandNames[11] = "turn";
			Json._controlCommandNames[12] = "turns";
			Json._controlCommandNames[13] = "readc";
			Json._controlCommandNames[14] = "rnd";
			Json._controlCommandNames[15] = "srnd";
			Json._controlCommandNames[16] = "visit";
			Json._controlCommandNames[17] = "seq";
			Json._controlCommandNames[18] = "thread";
			Json._controlCommandNames[19] = "done";
			Json._controlCommandNames[20] = "end";
			Json._controlCommandNames[21] = "listInt";
			Json._controlCommandNames[22] = "range";
			Json._controlCommandNames[23] = "lrnd";
			for (int i = 0; i < 24; i++)
			{
				if (Json._controlCommandNames[i] == null)
				{
					throw new Exception("Control command not accounted for in serialisation");
				}
			}
		}

		// Token: 0x04000076 RID: 118
		private static string[] _controlCommandNames = new string[24];
	}
}
