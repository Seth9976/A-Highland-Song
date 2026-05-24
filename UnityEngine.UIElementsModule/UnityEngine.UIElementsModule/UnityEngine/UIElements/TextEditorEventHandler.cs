using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200017A RID: 378
	internal class TextEditorEventHandler
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x000309B7 File Offset: 0x0002EBB7
		// (set) Token: 0x06000BCC RID: 3020 RVA: 0x000309BF File Offset: 0x0002EBBF
		private protected TextEditorEngine editorEngine { protected get; private set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x000309C8 File Offset: 0x0002EBC8
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x000309D0 File Offset: 0x0002EBD0
		private protected ITextInputField textInputField { protected get; private set; }

		// Token: 0x06000BCF RID: 3023 RVA: 0x000309D9 File Offset: 0x0002EBD9
		protected TextEditorEventHandler(TextEditorEngine editorEngine, ITextInputField textInputField)
		{
			this.editorEngine = editorEngine;
			this.textInputField = textInputField;
			this.textInputField.SyncTextEngine();
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x000020E6 File Offset: 0x000002E6
		public virtual void ExecuteDefaultActionAtTarget(EventBase evt)
		{
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00030A00 File Offset: 0x0002EC00
		public virtual void ExecuteDefaultAction(EventBase evt)
		{
			bool flag = evt.eventTypeId == EventBase<FocusEvent>.TypeId();
			if (flag)
			{
				this.editorEngine.OnFocus();
				this.editorEngine.SelectAll();
			}
			else
			{
				bool flag2 = evt.eventTypeId == EventBase<BlurEvent>.TypeId();
				if (flag2)
				{
					this.editorEngine.OnLostFocus();
					this.editorEngine.SelectNone();
				}
			}
		}
	}
}
