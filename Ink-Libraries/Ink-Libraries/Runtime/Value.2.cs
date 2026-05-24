using System;

namespace Ink.Runtime
{
	// Token: 0x02000032 RID: 50
	public abstract class Value<T> : Value
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000325 RID: 805 RVA: 0x00012EA1 File Offset: 0x000110A1
		// (set) Token: 0x06000326 RID: 806 RVA: 0x00012EA9 File Offset: 0x000110A9
		public T value { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00012EB2 File Offset: 0x000110B2
		public override object valueObject
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00012EBF File Offset: 0x000110BF
		public Value(T val)
		{
			this.value = val;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00012ED0 File Offset: 0x000110D0
		public override string ToString()
		{
			T value = this.value;
			return value.ToString();
		}
	}
}
