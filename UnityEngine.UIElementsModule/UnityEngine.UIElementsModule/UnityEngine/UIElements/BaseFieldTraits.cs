using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200011A RID: 282
	public class BaseFieldTraits<TValueType, TValueUxmlAttributeType> : BaseField<TValueType>.UxmlTraits where TValueUxmlAttributeType : TypedUxmlAttributeDescription<TValueType>, new()
	{
		// Token: 0x06000916 RID: 2326 RVA: 0x000234A5 File Offset: 0x000216A5
		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			((INotifyValueChanged<TValueType>)ve).SetValueWithoutNotify(this.m_Value.GetValueFromBag(bag, cc));
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x000234D0 File Offset: 0x000216D0
		public BaseFieldTraits()
		{
			TValueUxmlAttributeType tvalueUxmlAttributeType = new TValueUxmlAttributeType();
			tvalueUxmlAttributeType.name = "value";
			this.m_Value = tvalueUxmlAttributeType;
			base..ctor();
		}

		// Token: 0x040003CA RID: 970
		private TValueUxmlAttributeType m_Value;
	}
}
