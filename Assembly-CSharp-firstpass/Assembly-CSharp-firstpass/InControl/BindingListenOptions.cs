using System;

namespace InControl
{
	// Token: 0x02000017 RID: 23
	public class BindingListenOptions
	{
		// Token: 0x06000077 RID: 119 RVA: 0x000040D6 File Offset: 0x000022D6
		public bool CallOnBindingFound(PlayerAction playerAction, BindingSource bindingSource)
		{
			return this.OnBindingFound == null || this.OnBindingFound(playerAction, bindingSource);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000040EF File Offset: 0x000022EF
		public void CallOnBindingAdded(PlayerAction playerAction, BindingSource bindingSource)
		{
			if (this.OnBindingAdded != null)
			{
				this.OnBindingAdded(playerAction, bindingSource);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004106 File Offset: 0x00002306
		public void CallOnBindingRejected(PlayerAction playerAction, BindingSource bindingSource, BindingSourceRejectionType bindingSourceRejectionType)
		{
			if (this.OnBindingRejected != null)
			{
				this.OnBindingRejected(playerAction, bindingSource, bindingSourceRejectionType);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000411E File Offset: 0x0000231E
		public void CallOnBindingEnded(PlayerAction playerAction)
		{
			if (this.OnBindingEnded != null)
			{
				this.OnBindingEnded(playerAction);
			}
		}

		// Token: 0x0400008D RID: 141
		public bool IncludeControllers = true;

		// Token: 0x0400008E RID: 142
		public bool IncludeUnknownControllers;

		// Token: 0x0400008F RID: 143
		public bool IncludeNonStandardControls = true;

		// Token: 0x04000090 RID: 144
		public bool IncludeMouseButtons;

		// Token: 0x04000091 RID: 145
		public bool IncludeMouseScrollWheel;

		// Token: 0x04000092 RID: 146
		public bool IncludeKeys = true;

		// Token: 0x04000093 RID: 147
		public bool IncludeModifiersAsFirstClassKeys;

		// Token: 0x04000094 RID: 148
		public uint MaxAllowedBindings;

		// Token: 0x04000095 RID: 149
		public uint MaxAllowedBindingsPerType;

		// Token: 0x04000096 RID: 150
		public bool AllowDuplicateBindingsPerSet;

		// Token: 0x04000097 RID: 151
		public bool UnsetDuplicateBindingsOnSet;

		// Token: 0x04000098 RID: 152
		public bool RejectRedundantBindings;

		// Token: 0x04000099 RID: 153
		public BindingSource ReplaceBinding;

		// Token: 0x0400009A RID: 154
		public Func<PlayerAction, BindingSource, bool> OnBindingFound;

		// Token: 0x0400009B RID: 155
		public Action<PlayerAction, BindingSource> OnBindingAdded;

		// Token: 0x0400009C RID: 156
		public Action<PlayerAction, BindingSource, BindingSourceRejectionType> OnBindingRejected;

		// Token: 0x0400009D RID: 157
		public Action<PlayerAction> OnBindingEnded;
	}
}
