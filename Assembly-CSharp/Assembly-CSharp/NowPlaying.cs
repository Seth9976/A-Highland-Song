using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class NowPlaying : MonoSingleton<NowPlaying>
{
	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x0600034F RID: 847 RVA: 0x0001A681 File Offset: 0x00018881
	// (set) Token: 0x06000350 RID: 848 RVA: 0x0001A689 File Offset: 0x00018889
	public bool visible { get; private set; }

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000351 RID: 849 RVA: 0x0001A692 File Offset: 0x00018892
	private SLayout layout
	{
		get
		{
			if (this._layout == null)
			{
				this._layout = base.GetComponent<SLayout>();
			}
			return this._layout;
		}
	}

	// Token: 0x06000352 RID: 850 RVA: 0x0001A6B4 File Offset: 0x000188B4
	public void Clear()
	{
		this.layout.CancelAnimations();
		this.layout.groupAlpha = 0f;
		this.layout.x = this.layout.parentRect.width + 10f;
		this.visible = false;
		this._lastTrackNotified = null;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x0001A70E File Offset: 0x0001890E
	private void OnEnable()
	{
		this.layout.groupAlpha = 0f;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x0001A720 File Offset: 0x00018920
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		if (!this.visible && MonoSingleton<RunTrack>.instance.playing && this._lastTrackNotified != MonoSingleton<RunTrack>.instance.track && !this.layout.isAnimating)
		{
			this.Show();
			return;
		}
		if (this.visible && Time.time > this._showStartedTime + this.duration && !this.layout.isAnimating)
		{
			this.Hide();
			return;
		}
		if (!this.visible && !Runner.instance.inMusicRunningArea && MonoSingleton<RunTrack>.instance.stopped)
		{
			this._lastTrackNotified = null;
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x0001A7CC File Offset: 0x000189CC
	private void Show()
	{
		this._showStartedTime = Time.time;
		this._lastTrackNotified = MonoSingleton<RunTrack>.instance.track;
		this.visible = true;
		BeatTrack track = MonoSingleton<RunTrack>.instance.track;
		this._trackName.textMeshPro.text = track.displayName;
		this._bandLogo.image.sprite = track.bandLogo;
		this.layout.originX = this.layout.parentRect.width + this.offscreenOffset;
		this.layout.Animate(0.2f * this.animDuration, delegate
		{
			this.layout.groupAlpha = 1f;
		});
		this.layout.Animate(this.animDuration, 0f, SLayout.popCurve, delegate
		{
			this.layout.originX = this.layout.parentRect.width;
		});
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0001A8A3 File Offset: 0x00018AA3
	private void Hide()
	{
		this.layout.Animate(this.animDuration, 0f, SLayout.reversePopCurve, delegate
		{
			this.layout.groupAlpha = 0f;
			this.layout.originX = this.layout.parentRect.width + this.offscreenOffset;
		}).Then(delegate
		{
			this.visible = false;
		});
	}

	// Token: 0x0400049A RID: 1178
	public float duration = 5f;

	// Token: 0x0400049B RID: 1179
	public float animDuration = 0.7f;

	// Token: 0x0400049D RID: 1181
	public float offscreenOffset = 600f;

	// Token: 0x0400049E RID: 1182
	private SLayout _layout;

	// Token: 0x0400049F RID: 1183
	private BeatTrack _lastTrackNotified;

	// Token: 0x040004A0 RID: 1184
	private float _showStartedTime;

	// Token: 0x040004A1 RID: 1185
	[SerializeField]
	private SLayout _trackName;

	// Token: 0x040004A2 RID: 1186
	[SerializeField]
	private SLayout _bandLogo;

	// Token: 0x040004A3 RID: 1187
	[SerializeField]
	private NowPlayingSettings _settings;
}
