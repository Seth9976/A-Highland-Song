using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
[ExecuteInEditMode]
public class GSR : MonoBehaviour
{
	// Token: 0x0600012A RID: 298 RVA: 0x0000C750 File Offset: 0x0000A950
	private void OnEnable()
	{
		if (this._game == null)
		{
			Debug.LogError("GSR.OnEnable: game not found!");
		}
		else
		{
			GSR.Game = this._game;
		}
		if (this._worldManager == null)
		{
			Debug.LogError("GSR.OnEnable: WorldManager not found!");
		}
		else
		{
			GSR.WorldManager = this._worldManager;
		}
		if (this._audioController == null)
		{
			Debug.LogError("GSR.OnEnable: AudioController not found!");
		}
		else
		{
			GSR.AudioController = this._audioController;
		}
		if (this._gameCam == null)
		{
			Debug.LogError("GSR.OnEnable: gameCam not found!");
		}
		else
		{
			GSR.GameCam = this._gameCam;
		}
		if (this._propsController == null)
		{
			Debug.LogError("GSR.OnEnable: propsController not found!");
		}
		else
		{
			GSR.PropsController = this._propsController;
		}
		if (this._narrative == null)
		{
			Debug.LogError("GSR.OnEnable: narrative not found!");
		}
		else
		{
			GSR.Narrative = this._narrative;
		}
		if (this._narrativePresenter == null)
		{
			Debug.LogError("GSR.OnEnable: narrativePresenter not found!");
		}
		else
		{
			GSR.NarrativePresenter = this._narrativePresenter;
		}
		if (this._weatherSystem == null)
		{
			Debug.LogError("GSR.OnEnable: weatherSystem not found!");
		}
		else
		{
			GSR.WeatherSystem = this._weatherSystem;
		}
		if (this._timeOfDayEffects == null)
		{
			Debug.LogError("GSR.OnEnable: timeOfDayEffects not found!");
		}
		else
		{
			GSR.timeOfDayEffects = this._timeOfDayEffects;
		}
		if (this._clock == null)
		{
			Debug.LogError("GSR.OnEnable: clock not found!");
		}
		else
		{
			GSR.Clock = this._clock;
		}
		if (this._peakStateController == null)
		{
			Debug.LogError("GSR.OnEnable: peakStateController not found!");
			return;
		}
		GSR.PeakStateController = this._peakStateController;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x0000C8F3 File Offset: 0x0000AAF3
	public static void SetRunner(Runner _runner)
	{
		GSR.Runner = _runner;
	}

	// Token: 0x040001EF RID: 495
	[SerializeField]
	private Game _game;

	// Token: 0x040001F0 RID: 496
	public static Game Game;

	// Token: 0x040001F1 RID: 497
	[SerializeField]
	private WorldManager _worldManager;

	// Token: 0x040001F2 RID: 498
	public static WorldManager WorldManager;

	// Token: 0x040001F3 RID: 499
	[SerializeField]
	private AudioController _audioController;

	// Token: 0x040001F4 RID: 500
	public static AudioController AudioController;

	// Token: 0x040001F5 RID: 501
	public static Runner Runner;

	// Token: 0x040001F6 RID: 502
	[SerializeField]
	private GameCamera _gameCam;

	// Token: 0x040001F7 RID: 503
	public static GameCamera GameCam;

	// Token: 0x040001F8 RID: 504
	[SerializeField]
	private PropsController _propsController;

	// Token: 0x040001F9 RID: 505
	public static PropsController PropsController;

	// Token: 0x040001FA RID: 506
	[SerializeField]
	private Narrative _narrative;

	// Token: 0x040001FB RID: 507
	public static Narrative Narrative;

	// Token: 0x040001FC RID: 508
	[SerializeField]
	private NarrativePresenter _narrativePresenter;

	// Token: 0x040001FD RID: 509
	public static NarrativePresenter NarrativePresenter;

	// Token: 0x040001FE RID: 510
	[SerializeField]
	private WeatherSystem _weatherSystem;

	// Token: 0x040001FF RID: 511
	public static WeatherSystem WeatherSystem;

	// Token: 0x04000200 RID: 512
	[SerializeField]
	private TimeOfDayEffects _timeOfDayEffects;

	// Token: 0x04000201 RID: 513
	public static TimeOfDayEffects timeOfDayEffects;

	// Token: 0x04000202 RID: 514
	[SerializeField]
	private GameClock _clock;

	// Token: 0x04000203 RID: 515
	public static GameClock Clock;

	// Token: 0x04000204 RID: 516
	[SerializeField]
	private PeakStateController _peakStateController;

	// Token: 0x04000205 RID: 517
	public static PeakStateController PeakStateController;
}
