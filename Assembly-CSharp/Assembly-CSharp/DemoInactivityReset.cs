using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class DemoInactivityReset : MonoBehaviour
{
	// Token: 0x06000085 RID: 133 RVA: 0x00008FDC File Offset: 0x000071DC
	private void Update()
	{
		if (!MonoSingleton<BuildSetupManager>.instance.setup.timeoutAfterInactivity)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (Game.loaded)
		{
			this.UpdateDemoInactivityTimer();
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x0000900C File Offset: 0x0000720C
	private void UpdateDemoInactivityTimer()
	{
		this._inactiveTime += Time.unscaledDeltaTime;
		if (Game.instance.inTitleScreenAndIntroState || GameInput.selectChoice || GameInput.rawBackWasPressed || GameInput.moveLeftRight != 0f || GameInput.moveUpDown != 0f || GameInput.jumped || GameInput.showJournal)
		{
			this._inactiveTime = 0f;
		}
		BuildSetup setup = MonoSingleton<BuildSetupManager>.instance.setup;
		bool flag = (Game.instance.inActiveGameplay || Game.instance.inPeakState) && !MonoSingleton<JournalController>.instance.mapConfirmActive && !MonoSingleton<MapsViewController>.instance.isBusy;
		if (this._inactiveTime > setup.inactivityTimeoutSeconds)
		{
			if (!MonoSingleton<Dialogue>.instance.visible && flag)
			{
				MonoSingleton<Dialogue>.instance.Show("Are you still there?", "This demo of A Highland Song will reset imminently unless you cancel!", "Restart game", "Cancel", true, delegate(Dialogue.Result result)
				{
					this._inactiveTime = 0f;
					if (result == Dialogue.Result.Primary)
					{
						this.DemoRestartGame();
					}
				});
			}
			if (MonoSingleton<Dialogue>.instance.visible && flag && this._inactiveTime > setup.inactivityTimeoutSeconds + setup.dialogueSecondsAfterTimeout)
			{
				MonoSingleton<Dialogue>.instance.Hide();
				this.DemoRestartGame();
			}
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00009133 File Offset: 0x00007333
	private void DemoRestartGame()
	{
		this._inactiveTime = 0f;
		MonoSingleton<Launcher>.instance.DemoReturnToTitleScreenAndRestart();
	}

	// Token: 0x040000DA RID: 218
	private float _inactiveTime;
}
