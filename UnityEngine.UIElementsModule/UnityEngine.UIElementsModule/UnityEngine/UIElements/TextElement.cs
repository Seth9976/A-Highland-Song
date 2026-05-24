using System;
using System.Collections.Generic;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x020000B7 RID: 183
	public class TextElement : BindableElement, ITextElement, INotifyValueChanged<string>
	{
		// Token: 0x06000609 RID: 1545 RVA: 0x0001669C File Offset: 0x0001489C
		public TextElement()
		{
			base.requireMeasureFunction = true;
			base.AddToClassList(TextElement.ussClassName);
			base.generateVisualContent = (Action<MeshGenerationContext>)Delegate.Combine(base.generateVisualContent, new Action<MeshGenerationContext>(this.OnGenerateVisualContent));
			base.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnGeometryChanged), TrickleDown.NoTrickleDown);
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x00016728 File Offset: 0x00014928
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x00016740 File Offset: 0x00014940
		internal ITextHandle textHandle
		{
			get
			{
				return this.m_TextHandle;
			}
			set
			{
				this.m_TextHandle = value;
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001674C File Offset: 0x0001494C
		public override void HandleEvent(EventBase evt)
		{
			bool flag;
			if (evt.eventTypeId == EventBase<AttachToPanelEvent>.TypeId())
			{
				AttachToPanelEvent attachToPanelEvent = evt as AttachToPanelEvent;
				flag = attachToPanelEvent != null;
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.textHandle = TextCoreHandle.New();
			}
			else
			{
				bool flag3;
				if (evt.eventTypeId == EventBase<DetachFromPanelEvent>.TypeId())
				{
					DetachFromPanelEvent detachFromPanelEvent = evt as DetachFromPanelEvent;
					flag3 = detachFromPanelEvent != null;
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				if (flag4)
				{
				}
			}
			base.HandleEvent(evt);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x000167B2 File Offset: 0x000149B2
		private void OnGeometryChanged(GeometryChangedEvent e)
		{
			this.UpdateVisibleText();
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x000167BC File Offset: 0x000149BC
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x000167D4 File Offset: 0x000149D4
		public virtual string text
		{
			get
			{
				return ((INotifyValueChanged<string>)this).value;
			}
			set
			{
				((INotifyValueChanged<string>)this).value = value;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x000167E0 File Offset: 0x000149E0
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x000167F8 File Offset: 0x000149F8
		public bool enableRichText
		{
			get
			{
				return this.m_EnableRichText;
			}
			set
			{
				bool flag = this.m_EnableRichText == value;
				if (!flag)
				{
					this.m_EnableRichText = value;
					base.MarkDirtyRepaint();
				}
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x00016824 File Offset: 0x00014A24
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x0001683C File Offset: 0x00014A3C
		public bool displayTooltipWhenElided
		{
			get
			{
				return this.m_DisplayTooltipWhenElided;
			}
			set
			{
				bool flag = this.m_DisplayTooltipWhenElided != value;
				if (flag)
				{
					this.m_DisplayTooltipWhenElided = value;
					this.UpdateVisibleText();
					base.MarkDirtyRepaint();
				}
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x00016871 File Offset: 0x00014A71
		// (set) Token: 0x06000615 RID: 1557 RVA: 0x00016879 File Offset: 0x00014A79
		public bool isElided { get; private set; }

		// Token: 0x06000616 RID: 1558 RVA: 0x00016884 File Offset: 0x00014A84
		private void OnGenerateVisualContent(MeshGenerationContext mgc)
		{
			this.UpdateVisibleText();
			mgc.Text(this.m_TextParams, this.m_TextHandle, base.scaledPixelsPerPoint);
			bool flag = this.ShouldElide() && this.TextLibraryCanElide();
			if (flag)
			{
				this.isElided = this.textHandle.IsElided();
			}
			this.UpdateTooltip();
			this.m_UpdateTextParams = true;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x000168E8 File Offset: 0x00014AE8
		internal string ElideText(string drawText, string ellipsisText, float width, TextOverflowPosition textOverflowPosition)
		{
			float num = base.resolvedStyle.paddingRight;
			bool flag = float.IsNaN(num);
			if (flag)
			{
				num = 0f;
			}
			float num2 = Mathf.Clamp(num, 1f / base.scaledPixelsPerPoint, 1f);
			Vector2 vector = this.MeasureTextSize(drawText, 0f, VisualElement.MeasureMode.Undefined, 0f, VisualElement.MeasureMode.Undefined);
			bool flag2 = vector.x <= width + num2 || string.IsNullOrEmpty(ellipsisText);
			string text;
			if (flag2)
			{
				text = drawText;
			}
			else
			{
				string text2 = ((drawText.Length > 1) ? ellipsisText : drawText);
				Vector2 vector2 = this.MeasureTextSize(text2, 0f, VisualElement.MeasureMode.Undefined, 0f, VisualElement.MeasureMode.Undefined);
				bool flag3 = vector2.x >= width;
				if (flag3)
				{
					text = text2;
				}
				else
				{
					int num3 = drawText.Length - 1;
					int num4 = -1;
					string text3 = drawText;
					int i = ((textOverflowPosition == TextOverflowPosition.Start) ? 1 : 0);
					int num5 = ((textOverflowPosition == TextOverflowPosition.Start || textOverflowPosition == TextOverflowPosition.Middle) ? num3 : (num3 - 1));
					int num6 = (i + num5) / 2;
					while (i <= num5)
					{
						bool flag4 = textOverflowPosition == TextOverflowPosition.Start;
						if (flag4)
						{
							text3 = ellipsisText + drawText.Substring(num6, num3 - (num6 - 1));
						}
						else
						{
							bool flag5 = textOverflowPosition == TextOverflowPosition.End;
							if (flag5)
							{
								text3 = drawText.Substring(0, num6) + ellipsisText;
							}
							else
							{
								bool flag6 = textOverflowPosition == TextOverflowPosition.Middle;
								if (flag6)
								{
									text3 = ((num6 - 1 <= 0) ? "" : drawText.Substring(0, num6 - 1)) + ellipsisText + ((num3 - (num6 - 1) <= 0) ? "" : drawText.Substring(num3 - (num6 - 1)));
								}
							}
						}
						vector = this.MeasureTextSize(text3, 0f, VisualElement.MeasureMode.Undefined, 0f, VisualElement.MeasureMode.Undefined);
						bool flag7 = Math.Abs(vector.x - width) < 1E-30f;
						if (flag7)
						{
							return text3;
						}
						bool flag8 = textOverflowPosition == TextOverflowPosition.Start;
						if (flag8)
						{
							bool flag9 = vector.x > width;
							if (flag9)
							{
								bool flag10 = num4 == num6 - 1;
								if (flag10)
								{
									return ellipsisText + drawText.Substring(num4, num3 - (num4 - 1));
								}
								i = num6 + 1;
							}
							else
							{
								num5 = num6 - 1;
								num4 = num6;
							}
						}
						else
						{
							bool flag11 = textOverflowPosition == TextOverflowPosition.End || textOverflowPosition == TextOverflowPosition.Middle;
							if (flag11)
							{
								bool flag12 = vector.x > width;
								if (flag12)
								{
									bool flag13 = num4 == num6 - 1;
									if (flag13)
									{
										bool flag14 = textOverflowPosition == TextOverflowPosition.End;
										if (flag14)
										{
											return drawText.Substring(0, num4) + ellipsisText;
										}
										return drawText.Substring(0, Mathf.Max(num4 - 1, 0)) + ellipsisText + drawText.Substring(num3 - Mathf.Max(num4 - 1, 0));
									}
									else
									{
										num5 = num6 - 1;
									}
								}
								else
								{
									i = num6 + 1;
									num4 = num6;
								}
							}
						}
						num6 = (i + num5) / 2;
					}
					text = text3;
				}
			}
			return text;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00016BC0 File Offset: 0x00014DC0
		private void UpdateTooltip()
		{
			bool flag = this.displayTooltipWhenElided && this.isElided;
			bool flag2 = flag;
			if (flag2)
			{
				base.tooltip = this.text;
				this.m_WasElided = true;
			}
			else
			{
				bool wasElided = this.m_WasElided;
				if (wasElided)
				{
					base.tooltip = null;
					this.m_WasElided = false;
				}
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00016C18 File Offset: 0x00014E18
		private void UpdateVisibleText()
		{
			MeshGenerationContextUtils.TextParams textParams = MeshGenerationContextUtils.TextParams.MakeStyleBased(this, this.text);
			int hashCode = textParams.GetHashCode();
			bool flag = this.m_UpdateTextParams || hashCode != this.m_PreviousTextParamsHashCode;
			if (flag)
			{
				this.m_TextParams = textParams;
				bool flag2 = this.ShouldElide();
				bool flag3 = flag2 && this.TextLibraryCanElide();
				if (!flag3)
				{
					bool flag4 = flag2;
					if (flag4)
					{
						this.m_TextParams.text = this.ElideText(this.m_TextParams.text, TextElement.k_EllipsisText, this.m_TextParams.rect.width, this.m_TextParams.textOverflowPosition);
						this.isElided = flag2 && this.m_TextParams.text != this.text;
						this.m_TextParams.textOverflow = TextOverflow.Clip;
					}
					else
					{
						this.m_TextParams.textOverflow = TextOverflow.Clip;
						this.isElided = false;
					}
				}
				this.m_PreviousTextParamsHashCode = hashCode;
				this.m_UpdateTextParams = false;
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00016D24 File Offset: 0x00014F24
		private bool ShouldElide()
		{
			return base.computedStyle.textOverflow == TextOverflow.Ellipsis && base.computedStyle.overflow == OverflowInternal.Hidden && base.computedStyle.whiteSpace == WhiteSpace.NoWrap;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00016D64 File Offset: 0x00014F64
		private bool TextLibraryCanElide()
		{
			bool flag = this.textHandle.IsLegacy();
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this.m_TextParams.textOverflowPosition == TextOverflowPosition.End;
				flag2 = flag3;
			}
			return flag2;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00016DA0 File Offset: 0x00014FA0
		public Vector2 MeasureTextSize(string textToMeasure, float width, VisualElement.MeasureMode widthMode, float height, VisualElement.MeasureMode heightMode)
		{
			return TextUtilities.MeasureVisualElementTextSize(this, textToMeasure, width, widthMode, height, heightMode, this.m_TextHandle);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00016DC8 File Offset: 0x00014FC8
		internal static Vector2 MeasureVisualElementTextSize(VisualElement ve, string textToMeasure, float width, VisualElement.MeasureMode widthMode, float height, VisualElement.MeasureMode heightMode, TextHandle textHandle)
		{
			return TextUtilities.MeasureVisualElementTextSize(ve, textToMeasure, width, widthMode, height, heightMode, textHandle.textHandle);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00016DF0 File Offset: 0x00014FF0
		protected internal override Vector2 DoMeasure(float desiredWidth, VisualElement.MeasureMode widthMode, float desiredHeight, VisualElement.MeasureMode heightMode)
		{
			return this.MeasureTextSize(this.text, desiredWidth, widthMode, desiredHeight, heightMode);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00016E14 File Offset: 0x00015014
		internal int VerticesCount(string text)
		{
			MeshGenerationContextUtils.TextParams textParams = this.m_TextParams;
			textParams.text = text;
			return this.textHandle.VerticesCount(textParams, base.scaledPixelsPerPoint);
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00016E48 File Offset: 0x00015048
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x00016E6C File Offset: 0x0001506C
		string INotifyValueChanged<string>.value
		{
			get
			{
				return this.m_Text ?? string.Empty;
			}
			set
			{
				bool flag = this.m_Text != value;
				if (flag)
				{
					bool flag2 = base.panel != null;
					if (flag2)
					{
						using (ChangeEvent<string> pooled = ChangeEvent<string>.GetPooled(this.text, value))
						{
							pooled.target = this;
							((INotifyValueChanged<string>)this).SetValueWithoutNotify(value);
							this.SendEvent(pooled);
						}
					}
					else
					{
						((INotifyValueChanged<string>)this).SetValueWithoutNotify(value);
					}
				}
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00016EEC File Offset: 0x000150EC
		void INotifyValueChanged<string>.SetValueWithoutNotify(string newValue)
		{
			bool flag = this.m_Text != newValue;
			if (flag)
			{
				this.m_Text = newValue;
				base.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Repaint);
				bool flag2 = !string.IsNullOrEmpty(base.viewDataKey);
				if (flag2)
				{
					base.SaveViewData();
				}
			}
		}

		// Token: 0x04000265 RID: 613
		public static readonly string ussClassName = "unity-text-element";

		// Token: 0x04000266 RID: 614
		private ITextHandle m_TextHandle;

		// Token: 0x04000267 RID: 615
		internal static int maxTextVertices = MeshBuilder.s_MaxTextMeshVertices;

		// Token: 0x04000268 RID: 616
		[SerializeField]
		private string m_Text = string.Empty;

		// Token: 0x04000269 RID: 617
		private bool m_EnableRichText = true;

		// Token: 0x0400026A RID: 618
		private bool m_DisplayTooltipWhenElided = true;

		// Token: 0x0400026C RID: 620
		internal static readonly string k_EllipsisText = "...";

		// Token: 0x0400026D RID: 621
		private bool m_WasElided;

		// Token: 0x0400026E RID: 622
		private bool m_UpdateTextParams = true;

		// Token: 0x0400026F RID: 623
		private MeshGenerationContextUtils.TextParams m_TextParams;

		// Token: 0x04000270 RID: 624
		private int m_PreviousTextParamsHashCode = int.MaxValue;

		// Token: 0x020000B8 RID: 184
		public new class UxmlFactory : UxmlFactory<TextElement, TextElement.UxmlTraits>
		{
		}

		// Token: 0x020000B9 RID: 185
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			// Token: 0x1700016D RID: 365
			// (get) Token: 0x06000625 RID: 1573 RVA: 0x00016F64 File Offset: 0x00015164
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}

			// Token: 0x06000626 RID: 1574 RVA: 0x00016F84 File Offset: 0x00015184
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				TextElement textElement = (TextElement)ve;
				textElement.text = this.m_Text.GetValueFromBag(bag, cc);
				textElement.enableRichText = this.m_EnableRichText.GetValueFromBag(bag, cc);
				textElement.displayTooltipWhenElided = this.m_DisplayTooltipWhenElided.GetValueFromBag(bag, cc);
			}

			// Token: 0x04000271 RID: 625
			private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
			{
				name = "text"
			};

			// Token: 0x04000272 RID: 626
			private UxmlBoolAttributeDescription m_EnableRichText = new UxmlBoolAttributeDescription
			{
				name = "enable-rich-text",
				defaultValue = true
			};

			// Token: 0x04000273 RID: 627
			private UxmlBoolAttributeDescription m_DisplayTooltipWhenElided = new UxmlBoolAttributeDescription
			{
				name = "display-tooltip-when-elided"
			};
		}
	}
}
