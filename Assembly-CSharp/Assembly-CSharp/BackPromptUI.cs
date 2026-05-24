using System;

// Token: 0x020000FD RID: 253
public class BackPromptUI : PromptWithVisibility
{
	// Token: 0x06000856 RID: 2134 RVA: 0x00048230 File Offset: 0x00046430
	protected override bool ShouldShow()
	{
		if (MonoSingleton<SettingsScreen>.instance.visible)
		{
			return true;
		}
		if (MonoSingleton<TitleScreen>.instance.visible)
		{
			return false;
		}
		if (MonoSingleton<JournalController>.instance.canExit || MonoSingleton<JournalController>.instance.mapZoomingActive)
		{
			return true;
		}
		bool isBusy = MonoSingleton<MapsViewController>.instance.isBusy;
		return false;
	}
}
