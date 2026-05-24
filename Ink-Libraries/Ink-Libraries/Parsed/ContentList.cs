using System;
using System.Collections.Generic;
using System.Text;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000043 RID: 67
	public class ContentList : Object
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060003CC RID: 972 RVA: 0x000149A3 File Offset: 0x00012BA3
		// (set) Token: 0x060003CD RID: 973 RVA: 0x000149AB File Offset: 0x00012BAB
		public bool dontFlatten { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060003CE RID: 974 RVA: 0x000149B4 File Offset: 0x00012BB4
		public Container runtimeContainer
		{
			get
			{
				return (Container)base.runtimeObject;
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000149C1 File Offset: 0x00012BC1
		public ContentList(List<Object> objects)
		{
			if (objects != null)
			{
				base.AddContent<Object>(objects);
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000149D4 File Offset: 0x00012BD4
		public ContentList(params Object[] objects)
		{
			if (objects != null)
			{
				List<Object> list = new List<Object>(objects);
				base.AddContent<Object>(list);
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000149F8 File Offset: 0x00012BF8
		public ContentList()
		{
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00014A00 File Offset: 0x00012C00
		public void TrimTrailingWhitespace()
		{
			for (int i = base.content.Count - 1; i >= 0; i--)
			{
				Text text = base.content[i] as Text;
				if (text == null)
				{
					break;
				}
				text.text = text.text.TrimEnd(new char[] { ' ', '\t' });
				if (text.text.Length != 0)
				{
					break;
				}
				base.content.RemoveAt(i);
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00014A7C File Offset: 0x00012C7C
		public override Object GenerateRuntimeObject()
		{
			Container container = new Container();
			if (base.content != null)
			{
				foreach (Object @object in base.content)
				{
					Object runtimeObject = @object.runtimeObject;
					if (runtimeObject)
					{
						container.AddContent(runtimeObject);
					}
				}
			}
			if (this.dontFlatten)
			{
				base.story.DontFlattenContainer(container);
			}
			return container;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00014B00 File Offset: 0x00012D00
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ContentList(");
			stringBuilder.Append(string.Join(", ", base.content.ToStringsArray<Object>()));
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}
