using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UnityEngine.Serialization
{
	// Token: 0x020002CE RID: 718
	public class UnitySurrogateSelector : ISurrogateSelector
	{
		// Token: 0x06001DC6 RID: 7622 RVA: 0x000306B8 File Offset: 0x0002E8B8
		public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
		{
			bool isGenericType = type.IsGenericType;
			if (isGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				bool flag = genericTypeDefinition == typeof(List);
				if (flag)
				{
					selector = this;
					return ListSerializationSurrogate.Default;
				}
				bool flag2 = genericTypeDefinition == typeof(Dictionary);
				if (flag2)
				{
					selector = this;
					Type type2 = typeof(DictionarySerializationSurrogate<, >).MakeGenericType(type.GetGenericArguments());
					return (ISerializationSurrogate)Activator.CreateInstance(type2);
				}
			}
			selector = null;
			return null;
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x00016168 File Offset: 0x00014368
		public void ChainSelector(ISurrogateSelector selector)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00016168 File Offset: 0x00014368
		public ISurrogateSelector GetNextSelector()
		{
			throw new NotImplementedException();
		}
	}
}
