using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200017B RID: 379
	internal class TextEditorEngine : TextEditor
	{
		// Token: 0x06000BD2 RID: 3026 RVA: 0x00030A66 File Offset: 0x0002EC66
		public TextEditorEngine(TextEditorEngine.OnDetectFocusChangeFunction detectFocusChange, TextEditorEngine.OnIndexChangeFunction indexChangeFunction)
		{
			this.m_DetectFocusChangeFunction = detectFocusChange;
			this.m_IndexChangeFunction = indexChangeFunction;
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x00030A80 File Offset: 0x0002EC80
		internal override Rect localPosition
		{
			get
			{
				return new Rect(0f, 0f, base.position.width, base.position.height);
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00030ABD File Offset: 0x0002ECBD
		internal override void OnDetectFocusChange()
		{
			this.m_DetectFocusChangeFunction();
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00030ACC File Offset: 0x0002ECCC
		internal override void OnCursorIndexChange()
		{
			this.m_IndexChangeFunction();
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00030ACC File Offset: 0x0002ECCC
		internal override void OnSelectIndexChange()
		{
			this.m_IndexChangeFunction();
		}

		// Token: 0x04000592 RID: 1426
		private TextEditorEngine.OnDetectFocusChangeFunction m_DetectFocusChangeFunction;

		// Token: 0x04000593 RID: 1427
		private TextEditorEngine.OnIndexChangeFunction m_IndexChangeFunction;

		// Token: 0x0200017C RID: 380
		// (Invoke) Token: 0x06000BD8 RID: 3032
		internal delegate void OnDetectFocusChangeFunction();

		// Token: 0x0200017D RID: 381
		// (Invoke) Token: 0x06000BDC RID: 3036
		internal delegate void OnIndexChangeFunction();
	}
}
