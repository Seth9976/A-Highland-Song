using System;
using System.Collections.Generic;
using ActionIcon;
using InControl;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class SettingKeyAssign : SettingControl<BindingSource>
{
	// Token: 0x06000AFF RID: 2815 RVA: 0x0005928C File Offset: 0x0005748C
	public void Setup(PlayerAction action, string labelOverride = null)
	{
		this._action = action;
		this._actionIcon.bindingSourceTypesOverride = SettingKeyAssign._allowBindingSourceTypes;
		this._actionIcon.SetAction(this._action, BackgroundType.None);
		base.Setup((labelOverride == null) ? action.Name : labelOverride, null, null, null);
		base.prototype.OnReturnToPool += this.OnReturnToPool;
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x000592EE File Offset: 0x000574EE
	private void OnReturnToPool()
	{
		base.prototype.OnReturnToPool -= this.OnReturnToPool;
		this.StopListening();
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x00059310 File Offset: 0x00057510
	public override void Trigger()
	{
		base.Trigger();
		if (!this._setupForListening)
		{
			BindingListenOptions listenOptions = MonoSingleton<GameInput>.instance.mapping.ListenOptions;
			listenOptions.OnBindingAdded = (Action<PlayerAction, BindingSource>)Delegate.Combine(listenOptions.OnBindingAdded, new Action<PlayerAction, BindingSource>(this.OnBindingAdded));
			listenOptions.OnBindingFound = (Func<PlayerAction, BindingSource, bool>)Delegate.Combine(listenOptions.OnBindingFound, new Func<PlayerAction, BindingSource, bool>(this.OnBindingFound));
			listenOptions.AllowDuplicateBindingsPerSet = true;
			listenOptions.MaxAllowedBindings = 1U;
			listenOptions.IncludeKeys = true;
			listenOptions.IncludeModifiersAsFirstClassKeys = true;
			listenOptions.IncludeMouseButtons = true;
			listenOptions.IncludeMouseScrollWheel = false;
			listenOptions.IncludeNonStandardControls = false;
			listenOptions.IncludeControllers = false;
			listenOptions.IncludeUnknownControllers = false;
			this._action.ListenForBinding();
			this._setupForListening = true;
			this.RefreshLayout();
			GameInput.PushControlStack(this);
		}
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x000593DD File Offset: 0x000575DD
	private void Update()
	{
		if (GameInput.Back(this))
		{
			this.StopListening();
		}
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x000593F0 File Offset: 0x000575F0
	private void StopListening()
	{
		if (!this._setupForListening)
		{
			return;
		}
		GameInput.PopControlStack(this, true);
		ActionIconView.ForceRefreshAll();
		BindingListenOptions listenOptions = MonoSingleton<GameInput>.instance.mapping.ListenOptions;
		listenOptions.OnBindingFound = (Func<PlayerAction, BindingSource, bool>)Delegate.Remove(listenOptions.OnBindingFound, new Func<PlayerAction, BindingSource, bool>(this.OnBindingFound));
		BindingListenOptions listenOptions2 = MonoSingleton<GameInput>.instance.mapping.ListenOptions;
		listenOptions2.OnBindingAdded = (Action<PlayerAction, BindingSource>)Delegate.Remove(listenOptions2.OnBindingAdded, new Action<PlayerAction, BindingSource>(this.OnBindingAdded));
		this._action.StopListeningForBinding();
		this._setupForListening = false;
		this.RefreshLayout();
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0005948A File Offset: 0x0005768A
	private void OnBindingAdded(PlayerAction action, BindingSource binding)
	{
		Debug.Log("BindingAdded: " + binding.Name + " for " + action.Name);
		GameInput.SaveCustomMapping();
		this.StopListening();
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x000594B8 File Offset: 0x000576B8
	private bool OnBindingFound(PlayerAction action, BindingSource binding)
	{
		if (binding == new KeyBindingSource(new Key[] { Key.Escape }))
		{
			this.StopListening();
			return false;
		}
		Debug.Log("BindingFound: " + binding.Name + " for " + action.Name);
		return true;
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x00059508 File Offset: 0x00057708
	public override void RefreshLayout()
	{
		base.RefreshLayout();
		this._bg.alpha = (float)(this._highlighted ? 1 : 0);
		if (this._setupForListening)
		{
			this._listenInstruction.groupAlpha = 1f;
			this._actionIconLayout.groupAlpha = 0f;
		}
		else
		{
			this._listenInstruction.groupAlpha = 0f;
			this._actionIconLayout.groupAlpha = 1f;
		}
		this._triggerIcon.groupAlpha = (float)((this._highlighted && !this._setupForListening) ? 1 : 0);
	}

	// Token: 0x04000D24 RID: 3364
	private bool _setupForListening;

	// Token: 0x04000D25 RID: 3365
	private PlayerAction _action;

	// Token: 0x04000D26 RID: 3366
	[SerializeField]
	private SLayout _bg;

	// Token: 0x04000D27 RID: 3367
	[SerializeField]
	private ActionIconView _actionIcon;

	// Token: 0x04000D28 RID: 3368
	[SerializeField]
	private SLayout _actionIconLayout;

	// Token: 0x04000D29 RID: 3369
	[SerializeField]
	private SLayout _listenInstruction;

	// Token: 0x04000D2A RID: 3370
	[SerializeField]
	private SLayout _triggerIcon;

	// Token: 0x04000D2B RID: 3371
	private static List<BindingSourceType> _allowBindingSourceTypes = new List<BindingSourceType>
	{
		BindingSourceType.MouseBindingSource,
		BindingSourceType.KeyBindingSource
	};
}
