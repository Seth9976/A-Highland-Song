using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000129 RID: 297
	public class Button : TextElement
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x0002698C File Offset: 0x00024B8C
		// (set) Token: 0x060009DD RID: 2525 RVA: 0x000269A4 File Offset: 0x00024BA4
		public Clickable clickable
		{
			get
			{
				return this.m_Clickable;
			}
			set
			{
				bool flag = this.m_Clickable != null && this.m_Clickable.target == this;
				if (flag)
				{
					this.RemoveManipulator(this.m_Clickable);
				}
				this.m_Clickable = value;
				bool flag2 = this.m_Clickable != null;
				if (flag2)
				{
					this.AddManipulator(this.m_Clickable);
				}
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060009DE RID: 2526 RVA: 0x00026A01 File Offset: 0x00024C01
		// (remove) Token: 0x060009DF RID: 2527 RVA: 0x00026A0C File Offset: 0x00024C0C
		[Obsolete("onClick is obsolete. Use clicked instead (UnityUpgradable) -> clicked", true)]
		public event Action onClick
		{
			add
			{
				this.clicked += value;
			}
			remove
			{
				this.clicked -= value;
			}
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060009E0 RID: 2528 RVA: 0x00026A18 File Offset: 0x00024C18
		// (remove) Token: 0x060009E1 RID: 2529 RVA: 0x00026A54 File Offset: 0x00024C54
		public event Action clicked
		{
			add
			{
				bool flag = this.m_Clickable == null;
				if (flag)
				{
					this.clickable = new Clickable(value);
				}
				else
				{
					this.m_Clickable.clicked += value;
				}
			}
			remove
			{
				bool flag = this.m_Clickable != null;
				if (flag)
				{
					this.m_Clickable.clicked -= value;
				}
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00026A7E File Offset: 0x00024C7E
		public Button()
			: this(null)
		{
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00026A8C File Offset: 0x00024C8C
		public Button(Action clickEvent)
		{
			base.AddToClassList(Button.ussClassName);
			this.clickable = new Clickable(clickEvent);
			base.focusable = true;
			base.RegisterCallback<NavigationSubmitEvent>(new EventCallback<NavigationSubmitEvent>(this.OnNavigationSubmit), TrickleDown.NoTrickleDown);
			base.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), TrickleDown.NoTrickleDown);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00026AEA File Offset: 0x00024CEA
		private void OnNavigationSubmit(NavigationSubmitEvent evt)
		{
			Clickable clickable = this.clickable;
			if (clickable != null)
			{
				clickable.SimulateSingleClick(evt, 100);
			}
			evt.StopPropagation();
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00026B0C File Offset: 0x00024D0C
		private void OnKeyDown(KeyDownEvent evt)
		{
			IPanel panel = base.panel;
			bool flag = panel == null || panel.contextType != ContextType.Editor;
			if (!flag)
			{
				bool flag2 = evt.keyCode == KeyCode.KeypadEnter || evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.Space;
				if (flag2)
				{
					Clickable clickable = this.clickable;
					if (clickable != null)
					{
						clickable.SimulateSingleClick(evt, 100);
					}
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00026B80 File Offset: 0x00024D80
		protected internal override Vector2 DoMeasure(float desiredWidth, VisualElement.MeasureMode widthMode, float desiredHeight, VisualElement.MeasureMode heightMode)
		{
			string text = this.text;
			bool flag = string.IsNullOrEmpty(text);
			if (flag)
			{
				text = Button.NonEmptyString;
			}
			return base.MeasureTextSize(text, desiredWidth, widthMode, desiredHeight, heightMode);
		}

		// Token: 0x04000428 RID: 1064
		public new static readonly string ussClassName = "unity-button";

		// Token: 0x04000429 RID: 1065
		private Clickable m_Clickable;

		// Token: 0x0400042A RID: 1066
		private static readonly string NonEmptyString = " ";

		// Token: 0x0200012A RID: 298
		public new class UxmlFactory : UxmlFactory<Button, Button.UxmlTraits>
		{
		}

		// Token: 0x0200012B RID: 299
		public new class UxmlTraits : TextElement.UxmlTraits
		{
			// Token: 0x060009E9 RID: 2537 RVA: 0x00026BD6 File Offset: 0x00024DD6
			public UxmlTraits()
			{
				base.focusable.defaultValue = true;
			}
		}
	}
}
