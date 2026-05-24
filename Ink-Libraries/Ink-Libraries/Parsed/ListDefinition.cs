using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000056 RID: 86
	public class ListDefinition : Object
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00017848 File Offset: 0x00015A48
		public ListDefinition runtimeListDefinition
		{
			get
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (ListElementDefinition listElementDefinition in this.itemDefinitions)
				{
					if (!dictionary.ContainsKey(listElementDefinition.name))
					{
						dictionary.Add(listElementDefinition.name, listElementDefinition.seriesValue);
					}
					else
					{
						string[] array = new string[5];
						array[0] = "List '";
						int num = 1;
						Identifier identifier = this.identifier;
						array[num] = ((identifier != null) ? identifier.ToString() : null);
						array[2] = "' contains dupicate items called '";
						array[3] = listElementDefinition.name;
						array[4] = "'";
						this.Error(string.Concat(array), null, false);
					}
				}
				Identifier identifier2 = this.identifier;
				return new ListDefinition((identifier2 != null) ? identifier2.name : null, dictionary);
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00017920 File Offset: 0x00015B20
		public ListElementDefinition ItemNamed(string itemName)
		{
			if (this._elementsByName == null)
			{
				this._elementsByName = new Dictionary<string, ListElementDefinition>();
				foreach (ListElementDefinition listElementDefinition in this.itemDefinitions)
				{
					this._elementsByName[listElementDefinition.name] = listElementDefinition;
				}
			}
			ListElementDefinition listElementDefinition2;
			if (this._elementsByName.TryGetValue(itemName, out listElementDefinition2))
			{
				return listElementDefinition2;
			}
			return null;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000179A4 File Offset: 0x00015BA4
		public ListDefinition(List<ListElementDefinition> elements)
		{
			this.itemDefinitions = elements;
			int num = 1;
			foreach (ListElementDefinition listElementDefinition in this.itemDefinitions)
			{
				if (listElementDefinition.explicitValue != null)
				{
					num = listElementDefinition.explicitValue.Value;
				}
				listElementDefinition.seriesValue = num;
				num++;
			}
			base.AddContent<ListElementDefinition>(elements);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00017A2C File Offset: 0x00015C2C
		public override Object GenerateRuntimeObject()
		{
			InkList inkList = new InkList();
			foreach (ListElementDefinition listElementDefinition in this.itemDefinitions)
			{
				if (listElementDefinition.inInitialList)
				{
					Identifier identifier = this.identifier;
					InkListItem inkListItem = new InkListItem((identifier != null) ? identifier.name : null, listElementDefinition.name);
					inkList[inkListItem] = listElementDefinition.seriesValue;
				}
			}
			InkList inkList2 = inkList;
			Identifier identifier2 = this.identifier;
			inkList2.SetInitialOriginName((identifier2 != null) ? identifier2.name : null);
			return new ListValue(inkList);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00017AD0 File Offset: 0x00015CD0
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			context.CheckForNamingCollisions(this, this.identifier, Story.SymbolType.List, null);
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00017AE8 File Offset: 0x00015CE8
		public override string typeName
		{
			get
			{
				return "List definition";
			}
		}

		// Token: 0x04000166 RID: 358
		public Identifier identifier;

		// Token: 0x04000167 RID: 359
		public List<ListElementDefinition> itemDefinitions;

		// Token: 0x04000168 RID: 360
		public VariableAssignment variableAssignment;

		// Token: 0x04000169 RID: 361
		private Dictionary<string, ListElementDefinition> _elementsByName;
	}
}
