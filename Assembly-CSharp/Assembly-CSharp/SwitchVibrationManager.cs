using System;
using System.Collections.Generic;

// Token: 0x020000A9 RID: 169
public class SwitchVibrationManager : MonoSingleton<SwitchVibrationManager>
{
	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000578 RID: 1400 RVA: 0x0002B888 File Offset: 0x00029A88
	private Dictionary<SwitchVibrationManager.VibrationType, SwitchVibrationManager.VibrationFileSampler> vibrations
	{
		get
		{
			if (this._vibrations == null)
			{
				this._vibrations = new Dictionary<SwitchVibrationManager.VibrationType, SwitchVibrationManager.VibrationFileSampler>();
				if (this.vibrationDatabase.beginGame != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.beginGame, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.beginGame.bytes));
				}
				if (this.vibrationDatabase.clearNotification != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.clearNotification, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.clearNotification.bytes));
				}
				if (this.vibrationDatabase.toggleHeader != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.toggleHeader, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.toggleHeader.bytes));
				}
				if (this.vibrationDatabase.splashAndDrown != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.splashAndDrown, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.splashAndDrown.bytes));
				}
				if (this.vibrationDatabase.screenFlash != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.screenFlash, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.screenFlash.bytes));
				}
				if (this.vibrationDatabase.enterMap != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.enterMap, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.enterMap.bytes));
				}
				if (this.vibrationDatabase.titleCardThump != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.titleCardThump, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.titleCardThump.bytes));
				}
				if (this.vibrationDatabase.titleCardHorn != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.titleCardHorn, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.titleCardHorn.bytes));
				}
				if (this.vibrationDatabase.dialogueShow != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.dialogueShow, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.dialogueShow.bytes));
				}
				if (this.vibrationDatabase.enableVibration != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.enableVibration, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.enableVibration.bytes));
				}
				if (this.vibrationDatabase.select != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.select, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.select.bytes));
				}
				if (this.vibrationDatabase.navigateOptions != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.navigateOptions, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.navigateOptions.bytes));
				}
				if (this.vibrationDatabase.navigateOptionsBlocked != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.navigateOptionsBlocked, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.navigateOptionsBlocked.bytes));
				}
				if (this.vibrationDatabase.makeStoryChoice != null)
				{
					this._vibrations.Add(SwitchVibrationManager.VibrationType.makeStoryChoice, new SwitchVibrationManager.VibrationFileSampler(this.vibrationDatabase.makeStoryChoice.bytes));
				}
			}
			return this._vibrations;
		}
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0002BB8E File Offset: 0x00029D8E
	private void OnEnable()
	{
		base.enabled = false;
	}

	// Token: 0x0400063F RID: 1599
	public VibrationDatabase vibrationDatabase;

	// Token: 0x04000640 RID: 1600
	private Dictionary<SwitchVibrationManager.VibrationType, SwitchVibrationManager.VibrationFileSampler> _vibrations;

	// Token: 0x020002D1 RID: 721
	public enum VibrationType
	{
		// Token: 0x0400164A RID: 5706
		beginGame,
		// Token: 0x0400164B RID: 5707
		clearNotification,
		// Token: 0x0400164C RID: 5708
		toggleHeader,
		// Token: 0x0400164D RID: 5709
		splashAndDrown,
		// Token: 0x0400164E RID: 5710
		screenFlash,
		// Token: 0x0400164F RID: 5711
		enterMap,
		// Token: 0x04001650 RID: 5712
		titleCardThump,
		// Token: 0x04001651 RID: 5713
		titleCardHorn,
		// Token: 0x04001652 RID: 5714
		dialogueShow,
		// Token: 0x04001653 RID: 5715
		enableVibration,
		// Token: 0x04001654 RID: 5716
		navigateOptions,
		// Token: 0x04001655 RID: 5717
		navigateOptionsBlocked,
		// Token: 0x04001656 RID: 5718
		select,
		// Token: 0x04001657 RID: 5719
		makeStoryChoice
	}

	// Token: 0x020002D2 RID: 722
	public class VibrationFileSampler
	{
		// Token: 0x0600164D RID: 5709 RVA: 0x00098567 File Offset: 0x00096767
		public VibrationFileSampler(byte[] file)
		{
			this.file = file;
		}

		// Token: 0x04001658 RID: 5720
		public byte[] file;
	}
}
