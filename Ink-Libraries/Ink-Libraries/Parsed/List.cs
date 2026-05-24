using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000055 RID: 85
	public class List : Expression
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x000176B0 File Offset: 0x000158B0
		public List(List<Identifier> itemIdentifierList)
		{
			this.itemIdentifierList = itemIdentifierList;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000176C0 File Offset: 0x000158C0
		public override void GenerateIntoContainer(Container container)
		{
			InkList inkList = new InkList();
			if (this.itemIdentifierList != null)
			{
				foreach (Identifier identifier in this.itemIdentifierList)
				{
					string[] array = ((identifier != null) ? identifier.name.Split('.', StringSplitOptions.None) : null);
					string text = null;
					string text2;
					if (array.Length > 1)
					{
						text = array[0];
						text2 = array[1];
					}
					else
					{
						text2 = array[0];
					}
					ListElementDefinition listElementDefinition = base.story.ResolveListItem(text, text2, this);
					if (listElementDefinition == null)
					{
						if (text == null)
						{
							string text3 = "Could not find list definition that contains item '";
							Identifier identifier2 = identifier;
							this.Error(text3 + ((identifier2 != null) ? identifier2.ToString() : null) + "'", null, false);
						}
						else
						{
							string text4 = "Could not find list item ";
							Identifier identifier3 = identifier;
							this.Error(text4 + ((identifier3 != null) ? identifier3.ToString() : null), null, false);
						}
					}
					else
					{
						if (text == null)
						{
							Identifier identifier4 = ((ListDefinition)listElementDefinition.parent).identifier;
							text = ((identifier4 != null) ? identifier4.name : null);
						}
						InkListItem inkListItem = new InkListItem(text, listElementDefinition.name);
						if (inkList.ContainsKey(inkListItem))
						{
							string text5 = "Duplicate of item '";
							Identifier identifier5 = identifier;
							base.Warning(text5 + ((identifier5 != null) ? identifier5.ToString() : null) + "' in list.", null);
						}
						else
						{
							inkList[inkListItem] = listElementDefinition.seriesValue;
						}
					}
				}
			}
			container.AddContent(new ListValue(inkList));
		}

		// Token: 0x04000165 RID: 357
		public List<Identifier> itemIdentifierList;
	}
}
