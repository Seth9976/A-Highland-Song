using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000186 RID: 390
	public class Toggle : BaseBoolField
	{
		// Token: 0x06000C82 RID: 3202 RVA: 0x00033114 File Offset: 0x00031314
		public Toggle()
			: this(null)
		{
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00033120 File Offset: 0x00031320
		public Toggle(string label)
			: base(label)
		{
			base.AddToClassList(Toggle.ussClassName);
			base.visualInput.AddToClassList(Toggle.inputUssClassName);
			base.labelElement.AddToClassList(Toggle.labelUssClassName);
			this.m_CheckMark.AddToClassList(Toggle.checkmarkUssClassName);
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00033175 File Offset: 0x00031375
		protected override void InitLabel()
		{
			base.InitLabel();
			this.m_Label.AddToClassList(Toggle.textUssClassName);
		}

		// Token: 0x040005BE RID: 1470
		public new static readonly string ussClassName = "unity-toggle";

		// Token: 0x040005BF RID: 1471
		public new static readonly string labelUssClassName = Toggle.ussClassName + "__label";

		// Token: 0x040005C0 RID: 1472
		public new static readonly string inputUssClassName = Toggle.ussClassName + "__input";

		// Token: 0x040005C1 RID: 1473
		[Obsolete]
		public static readonly string noTextVariantUssClassName = Toggle.ussClassName + "--no-text";

		// Token: 0x040005C2 RID: 1474
		public static readonly string checkmarkUssClassName = Toggle.ussClassName + "__checkmark";

		// Token: 0x040005C3 RID: 1475
		public static readonly string textUssClassName = Toggle.ussClassName + "__text";

		// Token: 0x02000187 RID: 391
		public new class UxmlFactory : UxmlFactory<Toggle, Toggle.UxmlTraits>
		{
		}

		// Token: 0x02000188 RID: 392
		public new class UxmlTraits : BaseFieldTraits<bool, UxmlBoolAttributeDescription>
		{
			// Token: 0x06000C87 RID: 3207 RVA: 0x00033214 File Offset: 0x00031414
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				((Toggle)ve).text = this.m_Text.GetValueFromBag(bag, cc);
			}

			// Token: 0x040005C4 RID: 1476
			private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
			{
				name = "text"
			};
		}
	}
}
