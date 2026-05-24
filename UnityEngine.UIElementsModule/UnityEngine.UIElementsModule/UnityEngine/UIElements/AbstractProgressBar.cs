using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200015B RID: 347
	public abstract class AbstractProgressBar : BindableElement, INotifyValueChanged<float>
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0002C6BC File Offset: 0x0002A8BC
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x0002C6C9 File Offset: 0x0002A8C9
		public string title
		{
			get
			{
				return this.m_Title.text;
			}
			set
			{
				this.m_Title.text = value;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0002C6D8 File Offset: 0x0002A8D8
		// (set) Token: 0x06000B0A RID: 2826 RVA: 0x0002C6E0 File Offset: 0x0002A8E0
		public float lowValue
		{
			get
			{
				return this.m_LowValue;
			}
			set
			{
				this.m_LowValue = value;
				this.SetProgress(this.m_Value);
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0002C6F7 File Offset: 0x0002A8F7
		// (set) Token: 0x06000B0C RID: 2828 RVA: 0x0002C6FF File Offset: 0x0002A8FF
		public float highValue
		{
			get
			{
				return this.m_HighValue;
			}
			set
			{
				this.m_HighValue = value;
				this.SetProgress(this.m_Value);
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0002C718 File Offset: 0x0002A918
		public AbstractProgressBar()
		{
			base.AddToClassList(AbstractProgressBar.ussClassName);
			VisualElement visualElement = new VisualElement
			{
				name = AbstractProgressBar.ussClassName
			};
			this.m_Background = new VisualElement();
			this.m_Background.AddToClassList(AbstractProgressBar.backgroundUssClassName);
			visualElement.Add(this.m_Background);
			this.m_Progress = new VisualElement();
			this.m_Progress.AddToClassList(AbstractProgressBar.progressUssClassName);
			this.m_Background.Add(this.m_Progress);
			VisualElement visualElement2 = new VisualElement();
			visualElement2.AddToClassList(AbstractProgressBar.titleContainerUssClassName);
			this.m_Background.Add(visualElement2);
			this.m_Title = new Label();
			this.m_Title.AddToClassList(AbstractProgressBar.titleUssClassName);
			visualElement2.Add(this.m_Title);
			visualElement.AddToClassList(AbstractProgressBar.containerUssClassName);
			base.hierarchy.Add(visualElement);
			base.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnGeometryChanged), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0002C825 File Offset: 0x0002AA25
		private void OnGeometryChanged(GeometryChangedEvent e)
		{
			this.SetProgress(this.value);
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0002C838 File Offset: 0x0002AA38
		// (set) Token: 0x06000B10 RID: 2832 RVA: 0x0002C850 File Offset: 0x0002AA50
		public virtual float value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				bool flag = !EqualityComparer<float>.Default.Equals(this.m_Value, value);
				if (flag)
				{
					bool flag2 = base.panel != null;
					if (flag2)
					{
						using (ChangeEvent<float> pooled = ChangeEvent<float>.GetPooled(this.m_Value, value))
						{
							pooled.target = this;
							this.SetValueWithoutNotify(value);
							this.SendEvent(pooled);
						}
					}
					else
					{
						this.SetValueWithoutNotify(value);
					}
				}
			}
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0002C8D8 File Offset: 0x0002AAD8
		public void SetValueWithoutNotify(float newValue)
		{
			this.m_Value = newValue;
			this.SetProgress(this.value);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0002C8F0 File Offset: 0x0002AAF0
		private void SetProgress(float p)
		{
			bool flag = p < this.lowValue;
			float num;
			if (flag)
			{
				num = this.lowValue;
			}
			else
			{
				bool flag2 = p > this.highValue;
				if (flag2)
				{
					num = this.highValue;
				}
				else
				{
					num = p;
				}
			}
			num = this.CalculateProgressWidth(num);
			bool flag3 = num >= 0f;
			if (flag3)
			{
				this.m_Progress.style.right = num;
			}
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0002C964 File Offset: 0x0002AB64
		private float CalculateProgressWidth(float width)
		{
			bool flag = this.m_Background == null || this.m_Progress == null;
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				bool flag2 = float.IsNaN(this.m_Background.layout.width);
				if (flag2)
				{
					num = 0f;
				}
				else
				{
					float num2 = this.m_Background.layout.width - 2f;
					num = num2 - Mathf.Max(num2 * width / this.highValue, 1f);
				}
			}
			return num;
		}

		// Token: 0x040004F3 RID: 1267
		public static readonly string ussClassName = "unity-progress-bar";

		// Token: 0x040004F4 RID: 1268
		public static readonly string containerUssClassName = AbstractProgressBar.ussClassName + "__container";

		// Token: 0x040004F5 RID: 1269
		public static readonly string titleUssClassName = AbstractProgressBar.ussClassName + "__title";

		// Token: 0x040004F6 RID: 1270
		public static readonly string titleContainerUssClassName = AbstractProgressBar.ussClassName + "__title-container";

		// Token: 0x040004F7 RID: 1271
		public static readonly string progressUssClassName = AbstractProgressBar.ussClassName + "__progress";

		// Token: 0x040004F8 RID: 1272
		public static readonly string backgroundUssClassName = AbstractProgressBar.ussClassName + "__background";

		// Token: 0x040004F9 RID: 1273
		private readonly VisualElement m_Background;

		// Token: 0x040004FA RID: 1274
		private readonly VisualElement m_Progress;

		// Token: 0x040004FB RID: 1275
		private readonly Label m_Title;

		// Token: 0x040004FC RID: 1276
		private float m_LowValue;

		// Token: 0x040004FD RID: 1277
		private float m_HighValue = 100f;

		// Token: 0x040004FE RID: 1278
		private float m_Value;

		// Token: 0x040004FF RID: 1279
		private const float k_MinVisibleProgress = 1f;

		// Token: 0x0200015C RID: 348
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			// Token: 0x06000B15 RID: 2837 RVA: 0x0002CA6C File Offset: 0x0002AC6C
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				AbstractProgressBar abstractProgressBar = ve as AbstractProgressBar;
				abstractProgressBar.lowValue = this.m_LowValue.GetValueFromBag(bag, cc);
				abstractProgressBar.highValue = this.m_HighValue.GetValueFromBag(bag, cc);
				abstractProgressBar.value = this.m_Value.GetValueFromBag(bag, cc);
				abstractProgressBar.title = this.m_Title.GetValueFromBag(bag, cc);
			}

			// Token: 0x04000500 RID: 1280
			private UxmlFloatAttributeDescription m_LowValue = new UxmlFloatAttributeDescription
			{
				name = "low-value",
				defaultValue = 0f
			};

			// Token: 0x04000501 RID: 1281
			private UxmlFloatAttributeDescription m_HighValue = new UxmlFloatAttributeDescription
			{
				name = "high-value",
				defaultValue = 100f
			};

			// Token: 0x04000502 RID: 1282
			private UxmlFloatAttributeDescription m_Value = new UxmlFloatAttributeDescription
			{
				name = "value",
				defaultValue = 0f
			};

			// Token: 0x04000503 RID: 1283
			private UxmlStringAttributeDescription m_Title = new UxmlStringAttributeDescription
			{
				name = "title",
				defaultValue = string.Empty
			};
		}
	}
}
