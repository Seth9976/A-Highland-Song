using System;
using System.Collections;
using System.Runtime.Serialization;

namespace UnityEngine.Serialization
{
	// Token: 0x020002CF RID: 719
	internal class ListSerializationSurrogate : ISerializationSurrogate
	{
		// Token: 0x06001DCA RID: 7626 RVA: 0x0003073C File Offset: 0x0002E93C
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			IList list = (IList)obj;
			info.AddValue("_size", list.Count);
			info.AddValue("_items", ListSerializationSurrogate.ArrayFromGenericList(list));
			info.AddValue("_version", 0);
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x00030784 File Offset: 0x0002E984
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			IList list = (IList)Activator.CreateInstance(obj.GetType());
			int @int = info.GetInt32("_size");
			bool flag = @int == 0;
			object obj2;
			if (flag)
			{
				obj2 = list;
			}
			else
			{
				IEnumerator enumerator = ((IEnumerable)info.GetValue("_items", typeof(IEnumerable))).GetEnumerator();
				for (int i = 0; i < @int; i++)
				{
					bool flag2 = !enumerator.MoveNext();
					if (flag2)
					{
						throw new InvalidOperationException();
					}
					list.Add(enumerator.Current);
				}
				obj2 = list;
			}
			return obj2;
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x00030820 File Offset: 0x0002EA20
		private static Array ArrayFromGenericList(IList list)
		{
			Array array = Array.CreateInstance(list.GetType().GetGenericArguments()[0], list.Count);
			list.CopyTo(array, 0);
			return array;
		}

		// Token: 0x040009B2 RID: 2482
		public static readonly ISerializationSurrogate Default = new ListSerializationSurrogate();
	}
}
