using System;
using System.Collections;
using System.Collections.Generic;

namespace Ink.Runtime
{
	// Token: 0x0200003C RID: 60
	public class VariablesState : IEnumerable<string>, IEnumerable
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000371 RID: 881 RVA: 0x00013578 File Offset: 0x00011778
		// (remove) Token: 0x06000372 RID: 882 RVA: 0x000135B0 File Offset: 0x000117B0
		public event VariablesState.VariableChanged variableChangedEvent;

		// Token: 0x06000373 RID: 883 RVA: 0x000135E5 File Offset: 0x000117E5
		public void StartVariableObservation()
		{
			this._batchObservingVariableChanges = true;
			this._changedVariablesForBatchObs = new HashSet<string>();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000135FC File Offset: 0x000117FC
		public Dictionary<string, Object> CompleteVariableObservation()
		{
			this._batchObservingVariableChanges = false;
			Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
			if (this._changedVariablesForBatchObs != null)
			{
				foreach (string text in this._changedVariablesForBatchObs)
				{
					Object @object = this._globalVariables[text];
					dictionary[text] = @object;
				}
			}
			if (this.patch != null)
			{
				foreach (string text2 in this.patch.changedVariables)
				{
					Object object2;
					if (this.patch.TryGetGlobal(text2, out object2))
					{
						dictionary[text2] = object2;
					}
				}
			}
			this._changedVariablesForBatchObs = null;
			return dictionary;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000136E0 File Offset: 0x000118E0
		public void NotifyObservers(Dictionary<string, Object> changedVars)
		{
			foreach (KeyValuePair<string, Object> keyValuePair in changedVars)
			{
				this.variableChangedEvent(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00013740 File Offset: 0x00011940
		// (set) Token: 0x06000377 RID: 887 RVA: 0x00013748 File Offset: 0x00011948
		public CallStack callStack
		{
			get
			{
				return this._callStack;
			}
			set
			{
				this._callStack = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		public object this[string variableName]
		{
			get
			{
				Object @object;
				if (this.patch != null && this.patch.TryGetGlobal(variableName, out @object))
				{
					return (@object as Value).valueObject;
				}
				if (this._globalVariables.TryGetValue(variableName, out @object) || this._defaultGlobalVariables.TryGetValue(variableName, out @object))
				{
					return (@object as Value).valueObject;
				}
				return null;
			}
			set
			{
				if (!this._defaultGlobalVariables.ContainsKey(variableName))
				{
					throw new StoryException("Cannot assign to a variable (" + variableName + ") that hasn't been declared in the story");
				}
				Value value2 = Value.Create(value);
				if (!(value2 == null))
				{
					this.SetGlobal(variableName, value2);
					return;
				}
				if (value == null)
				{
					throw new Exception("Cannot pass null to VariableState");
				}
				throw new Exception("Invalid value passed to VariableState: " + value.ToString());
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00013821 File Offset: 0x00011A21
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00013829 File Offset: 0x00011A29
		public IEnumerator<string> GetEnumerator()
		{
			return this._globalVariables.Keys.GetEnumerator();
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00013840 File Offset: 0x00011A40
		public VariablesState(CallStack callStack, ListDefinitionsOrigin listDefsOrigin)
		{
			this._globalVariables = new Dictionary<string, Object>();
			this._callStack = callStack;
			this._listDefsOrigin = listDefsOrigin;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00013864 File Offset: 0x00011A64
		public void ApplyPatch()
		{
			foreach (KeyValuePair<string, Object> keyValuePair in this.patch.globals)
			{
				this._globalVariables[keyValuePair.Key] = keyValuePair.Value;
			}
			if (this._changedVariablesForBatchObs != null)
			{
				foreach (string text in this.patch.changedVariables)
				{
					this._changedVariablesForBatchObs.Add(text);
				}
			}
			this.patch = null;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001392C File Offset: 0x00011B2C
		public void SetJsonToken(Dictionary<string, object> jToken)
		{
			this._globalVariables.Clear();
			foreach (KeyValuePair<string, Object> keyValuePair in this._defaultGlobalVariables)
			{
				object obj;
				if (jToken.TryGetValue(keyValuePair.Key, out obj))
				{
					this._globalVariables[keyValuePair.Key] = Json.JTokenToRuntimeObject(obj);
				}
				else
				{
					this._globalVariables[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000139C8 File Offset: 0x00011BC8
		public void WriteJson(SimpleJson.Writer writer)
		{
			writer.WriteObjectStart();
			foreach (KeyValuePair<string, Object> keyValuePair in this._globalVariables)
			{
				string key = keyValuePair.Key;
				Object value = keyValuePair.Value;
				Object @object;
				if (!VariablesState.dontSaveDefaultValues || !this._defaultGlobalVariables.TryGetValue(key, out @object) || !this.RuntimeObjectsEqual(value, @object))
				{
					writer.WritePropertyStart(key);
					Json.WriteRuntimeObject(writer, value);
					writer.WritePropertyEnd();
				}
			}
			writer.WriteObjectEnd();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00013A68 File Offset: 0x00011C68
		public bool RuntimeObjectsEqual(Object obj1, Object obj2)
		{
			if (obj1.GetType() != obj2.GetType())
			{
				return false;
			}
			BoolValue boolValue = obj1 as BoolValue;
			if (boolValue != null)
			{
				return boolValue.value == ((BoolValue)obj2).value;
			}
			IntValue intValue = obj1 as IntValue;
			if (intValue != null)
			{
				return intValue.value == ((IntValue)obj2).value;
			}
			FloatValue floatValue = obj1 as FloatValue;
			if (floatValue != null)
			{
				return floatValue.value == ((FloatValue)obj2).value;
			}
			Value value = obj1 as Value;
			Value value2 = obj2 as Value;
			if (value != null)
			{
				return value.valueObject.Equals(value2.valueObject);
			}
			string text = "FastRoughDefinitelyEquals: Unsupported runtime object type: ";
			Type type = obj1.GetType();
			throw new Exception(text + ((type != null) ? type.ToString() : null));
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00013B42 File Offset: 0x00011D42
		public Object GetVariableWithName(string name)
		{
			return this.GetVariableWithName(name, -1);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00013B4C File Offset: 0x00011D4C
		public Object TryGetDefaultVariableValue(string name)
		{
			Object @object = null;
			this._defaultGlobalVariables.TryGetValue(name, out @object);
			return @object;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00013B6B File Offset: 0x00011D6B
		public bool GlobalVariableExistsWithName(string name)
		{
			return this._globalVariables.ContainsKey(name) || (this._defaultGlobalVariables != null && this._defaultGlobalVariables.ContainsKey(name));
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00013B94 File Offset: 0x00011D94
		private Object GetVariableWithName(string name, int contextIndex)
		{
			Object @object = this.GetRawVariableWithName(name, contextIndex);
			VariablePointerValue variablePointerValue = @object as VariablePointerValue;
			if (variablePointerValue)
			{
				@object = this.ValueAtVariablePointer(variablePointerValue);
			}
			return @object;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00013BC4 File Offset: 0x00011DC4
		private Object GetRawVariableWithName(string name, int contextIndex)
		{
			Object @object = null;
			if (contextIndex == 0 || contextIndex == -1)
			{
				if (this.patch != null && this.patch.TryGetGlobal(name, out @object))
				{
					return @object;
				}
				if (this._globalVariables.TryGetValue(name, out @object))
				{
					return @object;
				}
				if (this._defaultGlobalVariables != null && this._defaultGlobalVariables.TryGetValue(name, out @object))
				{
					return @object;
				}
				ListValue listValue = this._listDefsOrigin.FindSingleItemListWithName(name);
				if (listValue)
				{
					return listValue;
				}
			}
			@object = this._callStack.GetTemporaryVariableWithName(name, contextIndex);
			return @object;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00013C46 File Offset: 0x00011E46
		public Object ValueAtVariablePointer(VariablePointerValue pointer)
		{
			return this.GetVariableWithName(pointer.variableName, pointer.contextIndex);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00013C5C File Offset: 0x00011E5C
		public void Assign(VariableAssignment varAss, Object value)
		{
			string text = varAss.variableName;
			int num = -1;
			bool flag;
			if (varAss.isNewDeclaration)
			{
				flag = varAss.isGlobal;
			}
			else
			{
				flag = this.GlobalVariableExistsWithName(text);
			}
			if (varAss.isNewDeclaration)
			{
				VariablePointerValue variablePointerValue = value as VariablePointerValue;
				if (variablePointerValue)
				{
					value = this.ResolveVariablePointer(variablePointerValue);
				}
			}
			else
			{
				VariablePointerValue variablePointerValue2;
				do
				{
					variablePointerValue2 = this.GetRawVariableWithName(text, num) as VariablePointerValue;
					if (variablePointerValue2)
					{
						text = variablePointerValue2.variableName;
						num = variablePointerValue2.contextIndex;
						flag = num == 0;
					}
				}
				while (variablePointerValue2);
			}
			if (flag)
			{
				this.SetGlobal(text, value);
				return;
			}
			this._callStack.SetTemporaryVariable(text, value, varAss.isNewDeclaration, num);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00013D08 File Offset: 0x00011F08
		public void SnapshotDefaultGlobals()
		{
			this._defaultGlobalVariables = new Dictionary<string, Object>(this._globalVariables);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00013D1C File Offset: 0x00011F1C
		private void RetainListOriginsForAssignment(Object oldValue, Object newValue)
		{
			ListValue listValue = oldValue as ListValue;
			ListValue listValue2 = newValue as ListValue;
			if (listValue && listValue2 && listValue2.value.Count == 0)
			{
				listValue2.value.SetInitialOriginNames(listValue.value.originNames);
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00013D6C File Offset: 0x00011F6C
		public void SetGlobal(string variableName, Object value)
		{
			Object @object = null;
			if (this.patch == null || !this.patch.TryGetGlobal(variableName, out @object))
			{
				this._globalVariables.TryGetValue(variableName, out @object);
			}
			ListValue.RetainListOriginsForAssignment(@object, value);
			if (this.patch != null)
			{
				this.patch.SetGlobal(variableName, value);
			}
			else
			{
				this._globalVariables[variableName] = value;
			}
			if (this.variableChangedEvent != null && !value.Equals(@object))
			{
				if (this._batchObservingVariableChanges)
				{
					if (this.patch != null)
					{
						this.patch.AddChangedVariable(variableName);
						return;
					}
					if (this._changedVariablesForBatchObs != null)
					{
						this._changedVariablesForBatchObs.Add(variableName);
						return;
					}
				}
				else
				{
					this.variableChangedEvent(variableName, value);
				}
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00013E20 File Offset: 0x00012020
		private VariablePointerValue ResolveVariablePointer(VariablePointerValue varPointer)
		{
			int num = varPointer.contextIndex;
			if (num == -1)
			{
				num = this.GetContextIndexOfVariableNamed(varPointer.variableName);
			}
			VariablePointerValue variablePointerValue = this.GetRawVariableWithName(varPointer.variableName, num) as VariablePointerValue;
			if (variablePointerValue)
			{
				return variablePointerValue;
			}
			return new VariablePointerValue(varPointer.variableName, num);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00013E6E File Offset: 0x0001206E
		private int GetContextIndexOfVariableNamed(string varName)
		{
			if (this.GlobalVariableExistsWithName(varName))
			{
				return 0;
			}
			return this._callStack.currentElementIndex;
		}

		// Token: 0x0400010A RID: 266
		public StatePatch patch;

		// Token: 0x0400010B RID: 267
		public static bool dontSaveDefaultValues = true;

		// Token: 0x0400010C RID: 268
		private Dictionary<string, Object> _globalVariables;

		// Token: 0x0400010D RID: 269
		private Dictionary<string, Object> _defaultGlobalVariables;

		// Token: 0x0400010E RID: 270
		private CallStack _callStack;

		// Token: 0x0400010F RID: 271
		private HashSet<string> _changedVariablesForBatchObs;

		// Token: 0x04000110 RID: 272
		private ListDefinitionsOrigin _listDefsOrigin;

		// Token: 0x04000111 RID: 273
		private bool _batchObservingVariableChanges;

		// Token: 0x020000A5 RID: 165
		// (Invoke) Token: 0x06000601 RID: 1537
		public delegate void VariableChanged(string variableName, Object newValue);
	}
}
