using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001E8 RID: 488
	public abstract class KeyboardEventBase<T> : EventBase<T>, IKeyboardEvent where T : KeyboardEventBase<T>, new()
	{
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x0003C9DD File Offset: 0x0003ABDD
		// (set) Token: 0x06000F19 RID: 3865 RVA: 0x0003C9E5 File Offset: 0x0003ABE5
		public EventModifiers modifiers { get; protected set; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x0003C9EE File Offset: 0x0003ABEE
		// (set) Token: 0x06000F1B RID: 3867 RVA: 0x0003C9F6 File Offset: 0x0003ABF6
		public char character { get; protected set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x0003C9FF File Offset: 0x0003ABFF
		// (set) Token: 0x06000F1D RID: 3869 RVA: 0x0003CA07 File Offset: 0x0003AC07
		public KeyCode keyCode { get; protected set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0003CA10 File Offset: 0x0003AC10
		public bool shiftKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Shift) > EventModifiers.None;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0003CA30 File Offset: 0x0003AC30
		public bool ctrlKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Control) > EventModifiers.None;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x0003CA50 File Offset: 0x0003AC50
		public bool commandKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Command) > EventModifiers.None;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0003CA70 File Offset: 0x0003AC70
		public bool altKey
		{
			get
			{
				return (this.modifiers & EventModifiers.Alt) > EventModifiers.None;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x0003CA90 File Offset: 0x0003AC90
		public bool actionKey
		{
			get
			{
				bool flag = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
				bool flag2;
				if (flag)
				{
					flag2 = this.commandKey;
				}
				else
				{
					flag2 = this.ctrlKey;
				}
				return flag2;
			}
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0003CAC9 File Offset: 0x0003ACC9
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0003CADA File Offset: 0x0003ACDA
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
			this.modifiers = EventModifiers.None;
			this.character = '\0';
			this.keyCode = KeyCode.None;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0003CB00 File Offset: 0x0003AD00
		public static T GetPooled(char c, KeyCode keyCode, EventModifiers modifiers)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.modifiers = modifiers;
			pooled.character = c;
			pooled.keyCode = keyCode;
			return pooled;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0003CB40 File Offset: 0x0003AD40
		public static T GetPooled(Event systemEvent)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.imguiEvent = systemEvent;
			bool flag = systemEvent != null;
			if (flag)
			{
				pooled.modifiers = systemEvent.modifiers;
				pooled.character = systemEvent.character;
				pooled.keyCode = systemEvent.keyCode;
			}
			return pooled;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0003CBA6 File Offset: 0x0003ADA6
		protected KeyboardEventBase()
		{
			this.LocalInit();
		}
	}
}
