using System;
using System.Collections.Generic;

namespace Ink.Runtime
{
	// Token: 0x02000021 RID: 33
	public class NativeFunctionCall : Object
	{
		// Token: 0x060001DC RID: 476 RVA: 0x0000B31B File Offset: 0x0000951B
		public static NativeFunctionCall CallWithName(string functionName)
		{
			return new NativeFunctionCall(functionName);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000B323 File Offset: 0x00009523
		public static bool CallExistsWithName(string functionName)
		{
			NativeFunctionCall.GenerateNativeFunctionsIfNecessary();
			return NativeFunctionCall._nativeFunctions.ContainsKey(functionName);
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000B335 File Offset: 0x00009535
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000B33D File Offset: 0x0000953D
		public string name
		{
			get
			{
				return this._name;
			}
			protected set
			{
				this._name = value;
				if (!this._isPrototype)
				{
					this._prototype = NativeFunctionCall._nativeFunctions[this._name];
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000B364 File Offset: 0x00009564
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x0000B385 File Offset: 0x00009585
		public int numberOfParameters
		{
			get
			{
				if (this._prototype)
				{
					return this._prototype.numberOfParameters;
				}
				return this._numberOfParameters;
			}
			protected set
			{
				this._numberOfParameters = value;
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000B390 File Offset: 0x00009590
		public Object Call(List<Object> parameters)
		{
			if (this._prototype)
			{
				return this._prototype.Call(parameters);
			}
			if (this.numberOfParameters != parameters.Count)
			{
				throw new Exception("Unexpected number of parameters");
			}
			bool flag = false;
			foreach (Object @object in parameters)
			{
				if (@object is Void)
				{
					throw new StoryException("Attempting to perform " + this.name + " operation on a void value. Did you forget to 'return' a value from a function you called here?");
				}
				if (@object is ListValue)
				{
					flag = true;
				}
			}
			if (parameters.Count == 2 && flag)
			{
				return this.CallBinaryListOperation(parameters);
			}
			List<Value> list = this.CoerceValuesToSingleType(parameters);
			ValueType valueType = list[0].valueType;
			if (valueType == ValueType.Int)
			{
				return this.Call<int>(list);
			}
			if (valueType == ValueType.Float)
			{
				return this.Call<float>(list);
			}
			if (valueType == ValueType.String)
			{
				return this.Call<string>(list);
			}
			if (valueType == ValueType.DivertTarget)
			{
				return this.Call<Path>(list);
			}
			if (valueType == ValueType.List)
			{
				return this.Call<InkList>(list);
			}
			return null;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000B4A0 File Offset: 0x000096A0
		private Value Call<T>(List<Value> parametersOfSingleType)
		{
			Value value = parametersOfSingleType[0];
			ValueType valueType = value.valueType;
			Value<T> value2 = (Value<T>)value;
			int count = parametersOfSingleType.Count;
			if (count != 2 && count != 1)
			{
				throw new Exception("Unexpected number of parameters to NativeFunctionCall: " + parametersOfSingleType.Count.ToString());
			}
			object obj = null;
			if (!this._operationFuncs.TryGetValue(valueType, out obj))
			{
				throw new StoryException("Cannot perform operation '" + this.name + "' on " + valueType.ToString());
			}
			if (count == 2)
			{
				Value<T> value3 = (Value<T>)parametersOfSingleType[1];
				return Value.Create(((NativeFunctionCall.BinaryOp<T>)obj)(value2.value, value3.value));
			}
			return Value.Create(((NativeFunctionCall.UnaryOp<T>)obj)(value2.value));
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000B574 File Offset: 0x00009774
		private Value CallBinaryListOperation(List<Object> parameters)
		{
			if ((this.name == "+" || this.name == "-") && parameters[0] is ListValue && parameters[1] is IntValue)
			{
				return this.CallListIncrementOperation(parameters);
			}
			Value value = parameters[0] as Value;
			Value value2 = parameters[1] as Value;
			if ((this.name == "&&" || this.name == "||") && (value.valueType != ValueType.List || value2.valueType != ValueType.List))
			{
				return new BoolValue((bool)(this._operationFuncs[ValueType.Int] as NativeFunctionCall.BinaryOp<int>)(value.isTruthy ? 1 : 0, value2.isTruthy ? 1 : 0));
			}
			if (value.valueType == ValueType.List && value2.valueType == ValueType.List)
			{
				return this.Call<InkList>(new List<Value> { value, value2 });
			}
			throw new StoryException(string.Concat(new string[]
			{
				"Can not call use '",
				this.name,
				"' operation on ",
				value.valueType.ToString(),
				" and ",
				value2.valueType.ToString()
			}));
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000B6E0 File Offset: 0x000098E0
		private Value CallListIncrementOperation(List<Object> listIntParams)
		{
			ListValue listValue = (ListValue)listIntParams[0];
			IntValue intValue = (IntValue)listIntParams[1];
			InkList inkList = new InkList();
			foreach (KeyValuePair<InkListItem, int> keyValuePair in listValue.value)
			{
				InkListItem key = keyValuePair.Key;
				int value = keyValuePair.Value;
				int num = (int)((NativeFunctionCall.BinaryOp<int>)this._operationFuncs[ValueType.Int])(value, intValue.value);
				ListDefinition listDefinition = null;
				foreach (ListDefinition listDefinition2 in listValue.value.origins)
				{
					if (listDefinition2.name == key.originName)
					{
						listDefinition = listDefinition2;
						break;
					}
				}
				InkListItem inkListItem;
				if (listDefinition != null && listDefinition.TryGetItemWithValue(num, out inkListItem))
				{
					inkList.Add(inkListItem, num);
				}
			}
			return new ListValue(inkList);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000B80C File Offset: 0x00009A0C
		private List<Value> CoerceValuesToSingleType(List<Object> parametersIn)
		{
			ValueType valueType = ValueType.Int;
			ListValue listValue = null;
			foreach (Object @object in parametersIn)
			{
				Value value = (Value)@object;
				if (value.valueType > valueType)
				{
					valueType = value.valueType;
				}
				if (value.valueType == ValueType.List)
				{
					listValue = value as ListValue;
				}
			}
			List<Value> list = new List<Value>();
			if (valueType == ValueType.List)
			{
				using (List<Object>.Enumerator enumerator = parametersIn.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Object object2 = enumerator.Current;
						Value value2 = (Value)object2;
						if (value2.valueType == ValueType.List)
						{
							list.Add(value2);
						}
						else
						{
							if (value2.valueType != ValueType.Int)
							{
								throw new StoryException("Cannot mix Lists and " + value2.valueType.ToString() + " values in this operation");
							}
							int num = (int)value2.valueObject;
							ListDefinition originOfMaxItem = listValue.value.originOfMaxItem;
							InkListItem inkListItem;
							if (!originOfMaxItem.TryGetItemWithValue(num, out inkListItem))
							{
								throw new StoryException("Could not find List item with the value " + num.ToString() + " in " + originOfMaxItem.name);
							}
							ListValue listValue2 = new ListValue(inkListItem, num);
							list.Add(listValue2);
						}
					}
					return list;
				}
			}
			foreach (Object object3 in parametersIn)
			{
				Value value3 = ((Value)object3).Cast(valueType);
				list.Add(value3);
			}
			return list;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000B9C8 File Offset: 0x00009BC8
		public NativeFunctionCall(string name)
		{
			NativeFunctionCall.GenerateNativeFunctionsIfNecessary();
			this.name = name;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000B9DC File Offset: 0x00009BDC
		public NativeFunctionCall()
		{
			NativeFunctionCall.GenerateNativeFunctionsIfNecessary();
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000B9E9 File Offset: 0x00009BE9
		private NativeFunctionCall(string name, int numberOfParameters)
		{
			this._isPrototype = true;
			this.name = name;
			this.numberOfParameters = numberOfParameters;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000BA06 File Offset: 0x00009C06
		private static object Identity<T>(T t)
		{
			return t;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000BA10 File Offset: 0x00009C10
		private static void GenerateNativeFunctionsIfNecessary()
		{
			if (NativeFunctionCall._nativeFunctions == null)
			{
				NativeFunctionCall._nativeFunctions = new Dictionary<string, NativeFunctionCall>();
				NativeFunctionCall.AddIntBinaryOp("+", (int x, int y) => x + y);
				NativeFunctionCall.AddIntBinaryOp("-", (int x, int y) => x - y);
				NativeFunctionCall.AddIntBinaryOp("*", (int x, int y) => x * y);
				NativeFunctionCall.AddIntBinaryOp("/", (int x, int y) => x / y);
				NativeFunctionCall.AddIntBinaryOp("%", (int x, int y) => x % y);
				NativeFunctionCall.AddIntUnaryOp("_", (int x) => -x);
				NativeFunctionCall.AddIntBinaryOp("==", (int x, int y) => x == y);
				NativeFunctionCall.AddIntBinaryOp(">", (int x, int y) => x > y);
				NativeFunctionCall.AddIntBinaryOp("<", (int x, int y) => x < y);
				NativeFunctionCall.AddIntBinaryOp(">=", (int x, int y) => x >= y);
				NativeFunctionCall.AddIntBinaryOp("<=", (int x, int y) => x <= y);
				NativeFunctionCall.AddIntBinaryOp("!=", (int x, int y) => x != y);
				NativeFunctionCall.AddIntUnaryOp("!", (int x) => x == 0);
				NativeFunctionCall.AddIntBinaryOp("&&", (int x, int y) => x != 0 && y != 0);
				NativeFunctionCall.AddIntBinaryOp("||", (int x, int y) => x != 0 || y != 0);
				NativeFunctionCall.AddIntBinaryOp("MAX", (int x, int y) => Math.Max(x, y));
				NativeFunctionCall.AddIntBinaryOp("MIN", (int x, int y) => Math.Min(x, y));
				NativeFunctionCall.AddIntBinaryOp("POW", (int x, int y) => (float)Math.Pow((double)x, (double)y));
				NativeFunctionCall.AddIntUnaryOp("FLOOR", new NativeFunctionCall.UnaryOp<int>(NativeFunctionCall.Identity<int>));
				NativeFunctionCall.AddIntUnaryOp("CEILING", new NativeFunctionCall.UnaryOp<int>(NativeFunctionCall.Identity<int>));
				NativeFunctionCall.AddIntUnaryOp("INT", new NativeFunctionCall.UnaryOp<int>(NativeFunctionCall.Identity<int>));
				NativeFunctionCall.AddIntUnaryOp("FLOAT", (int x) => (float)x);
				NativeFunctionCall.AddFloatBinaryOp("+", (float x, float y) => x + y);
				NativeFunctionCall.AddFloatBinaryOp("-", (float x, float y) => x - y);
				NativeFunctionCall.AddFloatBinaryOp("*", (float x, float y) => x * y);
				NativeFunctionCall.AddFloatBinaryOp("/", (float x, float y) => x / y);
				NativeFunctionCall.AddFloatBinaryOp("%", (float x, float y) => x % y);
				NativeFunctionCall.AddFloatUnaryOp("_", (float x) => -x);
				NativeFunctionCall.AddFloatBinaryOp("==", (float x, float y) => x == y);
				NativeFunctionCall.AddFloatBinaryOp(">", (float x, float y) => x > y);
				NativeFunctionCall.AddFloatBinaryOp("<", (float x, float y) => x < y);
				NativeFunctionCall.AddFloatBinaryOp(">=", (float x, float y) => x >= y);
				NativeFunctionCall.AddFloatBinaryOp("<=", (float x, float y) => x <= y);
				NativeFunctionCall.AddFloatBinaryOp("!=", (float x, float y) => x != y);
				NativeFunctionCall.AddFloatUnaryOp("!", (float x) => x == 0f);
				NativeFunctionCall.AddFloatBinaryOp("&&", (float x, float y) => x != 0f && y != 0f);
				NativeFunctionCall.AddFloatBinaryOp("||", (float x, float y) => x != 0f || y != 0f);
				NativeFunctionCall.AddFloatBinaryOp("MAX", (float x, float y) => Math.Max(x, y));
				NativeFunctionCall.AddFloatBinaryOp("MIN", (float x, float y) => Math.Min(x, y));
				NativeFunctionCall.AddFloatBinaryOp("POW", (float x, float y) => (float)Math.Pow((double)x, (double)y));
				NativeFunctionCall.AddFloatUnaryOp("FLOOR", (float x) => (float)Math.Floor((double)x));
				NativeFunctionCall.AddFloatUnaryOp("CEILING", (float x) => (float)Math.Ceiling((double)x));
				NativeFunctionCall.AddFloatUnaryOp("INT", (float x) => (int)x);
				NativeFunctionCall.AddFloatUnaryOp("FLOAT", new NativeFunctionCall.UnaryOp<float>(NativeFunctionCall.Identity<float>));
				NativeFunctionCall.AddStringBinaryOp("+", (string x, string y) => x + y);
				NativeFunctionCall.AddStringBinaryOp("==", (string x, string y) => x.Equals(y));
				NativeFunctionCall.AddStringBinaryOp("!=", (string x, string y) => !x.Equals(y));
				NativeFunctionCall.AddStringBinaryOp("?", (string x, string y) => x.Contains(y));
				NativeFunctionCall.AddStringBinaryOp("!?", (string x, string y) => !x.Contains(y));
				NativeFunctionCall.AddListBinaryOp("+", (InkList x, InkList y) => x.Union(y));
				NativeFunctionCall.AddListBinaryOp("-", (InkList x, InkList y) => x.Without(y));
				NativeFunctionCall.AddListBinaryOp("?", (InkList x, InkList y) => x.Contains(y));
				NativeFunctionCall.AddListBinaryOp("!?", (InkList x, InkList y) => !x.Contains(y));
				NativeFunctionCall.AddListBinaryOp("^", (InkList x, InkList y) => x.Intersect(y));
				NativeFunctionCall.AddListBinaryOp("==", (InkList x, InkList y) => x.Equals(y));
				NativeFunctionCall.AddListBinaryOp(">", (InkList x, InkList y) => x.GreaterThan(y));
				NativeFunctionCall.AddListBinaryOp("<", (InkList x, InkList y) => x.LessThan(y));
				NativeFunctionCall.AddListBinaryOp(">=", (InkList x, InkList y) => x.GreaterThanOrEquals(y));
				NativeFunctionCall.AddListBinaryOp("<=", (InkList x, InkList y) => x.LessThanOrEquals(y));
				NativeFunctionCall.AddListBinaryOp("!=", (InkList x, InkList y) => !x.Equals(y));
				NativeFunctionCall.AddListBinaryOp("&&", (InkList x, InkList y) => x.Count > 0 && y.Count > 0);
				NativeFunctionCall.AddListBinaryOp("||", (InkList x, InkList y) => x.Count > 0 || y.Count > 0);
				NativeFunctionCall.AddListUnaryOp("!", (InkList x) => (x.Count == 0) ? 1 : 0);
				NativeFunctionCall.AddListUnaryOp("LIST_INVERT", (InkList x) => x.inverse);
				NativeFunctionCall.AddListUnaryOp("LIST_ALL", (InkList x) => x.all);
				NativeFunctionCall.AddListUnaryOp("LIST_MIN", (InkList x) => x.MinAsList());
				NativeFunctionCall.AddListUnaryOp("LIST_MAX", (InkList x) => x.MaxAsList());
				NativeFunctionCall.AddListUnaryOp("LIST_COUNT", (InkList x) => x.Count);
				NativeFunctionCall.AddListUnaryOp("LIST_VALUE", (InkList x) => x.maxItem.Value);
				NativeFunctionCall.BinaryOp<Path> binaryOp = (Path d1, Path d2) => d1.Equals(d2);
				NativeFunctionCall.BinaryOp<Path> binaryOp2 = (Path d1, Path d2) => !d1.Equals(d2);
				NativeFunctionCall.AddOpToNativeFunc("==", 2, ValueType.DivertTarget, binaryOp);
				NativeFunctionCall.AddOpToNativeFunc("!=", 2, ValueType.DivertTarget, binaryOp2);
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000C54C File Offset: 0x0000A74C
		private void AddOpFuncForType(ValueType valType, object op)
		{
			if (this._operationFuncs == null)
			{
				this._operationFuncs = new Dictionary<ValueType, object>();
			}
			this._operationFuncs[valType] = op;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000C570 File Offset: 0x0000A770
		private static void AddOpToNativeFunc(string name, int args, ValueType valType, object op)
		{
			NativeFunctionCall nativeFunctionCall = null;
			if (!NativeFunctionCall._nativeFunctions.TryGetValue(name, out nativeFunctionCall))
			{
				nativeFunctionCall = new NativeFunctionCall(name, args);
				NativeFunctionCall._nativeFunctions[name] = nativeFunctionCall;
			}
			nativeFunctionCall.AddOpFuncForType(valType, op);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000C5AA File Offset: 0x0000A7AA
		private static void AddIntBinaryOp(string name, NativeFunctionCall.BinaryOp<int> op)
		{
			NativeFunctionCall.AddOpToNativeFunc(name, 2, ValueType.Int, op);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000C5B5 File Offset: 0x0000A7B5
		private static void AddIntUnaryOp(string name, NativeFunctionCall.UnaryOp<int> op)
		{
			NativeFunctionCall.AddOpToNativeFunc(name, 1, ValueType.Int, op);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000C5C0 File Offset: 0x0000A7C0
		private static void AddFloatBinaryOp(string name, NativeFunctionCall.BinaryOp<float> op)
		{
			NativeFunctionCall.AddOpToNativeFunc(name, 2, ValueType.Float, op);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000C5CB File Offset: 0x0000A7CB
		private static void AddStringBinaryOp(string name, NativeFunctionCall.BinaryOp<string> op)
		{
			NativeFunctionCall.AddOpToNativeFunc(name, 2, ValueType.String, op);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000C5D6 File Offset: 0x0000A7D6
		private static void AddListBinaryOp(string name, NativeFunctionCall.BinaryOp<InkList> op)
		{
			NativeFunctionCall.AddOpToNativeFunc(name, 2, ValueType.List, op);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000C5E1 File Offset: 0x0000A7E1
		private static void AddListUnaryOp(string name, NativeFunctionCall.UnaryOp<InkList> op)
		{
			NativeFunctionCall.AddOpToNativeFunc(name, 1, ValueType.List, op);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000C5EC File Offset: 0x0000A7EC
		private static void AddFloatUnaryOp(string name, NativeFunctionCall.UnaryOp<float> op)
		{
			NativeFunctionCall.AddOpToNativeFunc(name, 1, ValueType.Float, op);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000C5F7 File Offset: 0x0000A7F7
		public override string ToString()
		{
			return "Native '" + this.name + "'";
		}

		// Token: 0x0400007C RID: 124
		public const string Add = "+";

		// Token: 0x0400007D RID: 125
		public const string Subtract = "-";

		// Token: 0x0400007E RID: 126
		public const string Divide = "/";

		// Token: 0x0400007F RID: 127
		public const string Multiply = "*";

		// Token: 0x04000080 RID: 128
		public const string Mod = "%";

		// Token: 0x04000081 RID: 129
		public const string Negate = "_";

		// Token: 0x04000082 RID: 130
		public const string Equal = "==";

		// Token: 0x04000083 RID: 131
		public const string Greater = ">";

		// Token: 0x04000084 RID: 132
		public const string Less = "<";

		// Token: 0x04000085 RID: 133
		public const string GreaterThanOrEquals = ">=";

		// Token: 0x04000086 RID: 134
		public const string LessThanOrEquals = "<=";

		// Token: 0x04000087 RID: 135
		public const string NotEquals = "!=";

		// Token: 0x04000088 RID: 136
		public const string Not = "!";

		// Token: 0x04000089 RID: 137
		public const string And = "&&";

		// Token: 0x0400008A RID: 138
		public const string Or = "||";

		// Token: 0x0400008B RID: 139
		public const string Min = "MIN";

		// Token: 0x0400008C RID: 140
		public const string Max = "MAX";

		// Token: 0x0400008D RID: 141
		public const string Pow = "POW";

		// Token: 0x0400008E RID: 142
		public const string Floor = "FLOOR";

		// Token: 0x0400008F RID: 143
		public const string Ceiling = "CEILING";

		// Token: 0x04000090 RID: 144
		public const string Int = "INT";

		// Token: 0x04000091 RID: 145
		public const string Float = "FLOAT";

		// Token: 0x04000092 RID: 146
		public const string Has = "?";

		// Token: 0x04000093 RID: 147
		public const string Hasnt = "!?";

		// Token: 0x04000094 RID: 148
		public const string Intersect = "^";

		// Token: 0x04000095 RID: 149
		public const string ListMin = "LIST_MIN";

		// Token: 0x04000096 RID: 150
		public const string ListMax = "LIST_MAX";

		// Token: 0x04000097 RID: 151
		public const string All = "LIST_ALL";

		// Token: 0x04000098 RID: 152
		public const string Count = "LIST_COUNT";

		// Token: 0x04000099 RID: 153
		public const string ValueOfList = "LIST_VALUE";

		// Token: 0x0400009A RID: 154
		public const string Invert = "LIST_INVERT";

		// Token: 0x0400009B RID: 155
		private string _name;

		// Token: 0x0400009C RID: 156
		private int _numberOfParameters;

		// Token: 0x0400009D RID: 157
		private NativeFunctionCall _prototype;

		// Token: 0x0400009E RID: 158
		private bool _isPrototype;

		// Token: 0x0400009F RID: 159
		private Dictionary<ValueType, object> _operationFuncs;

		// Token: 0x040000A0 RID: 160
		private static Dictionary<string, NativeFunctionCall> _nativeFunctions;

		// Token: 0x0200008D RID: 141
		// (Invoke) Token: 0x06000547 RID: 1351
		private delegate object BinaryOp<T>(T left, T right);

		// Token: 0x0200008E RID: 142
		// (Invoke) Token: 0x0600054B RID: 1355
		private delegate object UnaryOp<T>(T val);
	}
}
