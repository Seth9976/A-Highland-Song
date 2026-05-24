using System;

namespace Ink.Runtime
{
	// Token: 0x02000031 RID: 49
	public abstract class Value : Object
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600031D RID: 797
		public abstract ValueType valueType { get; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600031E RID: 798
		public abstract bool isTruthy { get; }

		// Token: 0x0600031F RID: 799
		public abstract Value Cast(ValueType newType);

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000320 RID: 800
		public abstract object valueObject { get; }

		// Token: 0x06000321 RID: 801 RVA: 0x00012D54 File Offset: 0x00010F54
		public static Value Create(object val)
		{
			if (val is double)
			{
				val = (float)((double)val);
			}
			if (val is bool)
			{
				return new BoolValue((bool)val);
			}
			if (val is int)
			{
				return new IntValue((int)val);
			}
			if (val is long)
			{
				return new IntValue((int)((long)val));
			}
			if (val is float)
			{
				return new FloatValue((float)val);
			}
			if (val is double)
			{
				return new FloatValue((float)((double)val));
			}
			if (val is string)
			{
				return new StringValue((string)val);
			}
			if (val is Path)
			{
				return new DivertTargetValue((Path)val);
			}
			if (val is InkList)
			{
				return new ListValue((InkList)val);
			}
			return null;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00012E1A File Offset: 0x0001101A
		public override Object Copy()
		{
			return Value.Create(this.valueObject);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00012E28 File Offset: 0x00011028
		protected StoryException BadCastException(ValueType targetType)
		{
			string[] array = new string[6];
			array[0] = "Can't cast ";
			int num = 1;
			object valueObject = this.valueObject;
			array[num] = ((valueObject != null) ? valueObject.ToString() : null);
			array[2] = " from ";
			array[3] = this.valueType.ToString();
			array[4] = " to ";
			array[5] = targetType.ToString();
			return new StoryException(string.Concat(array));
		}
	}
}
