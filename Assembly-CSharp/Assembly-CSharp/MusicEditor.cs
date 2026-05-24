using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class MusicEditor
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000006 RID: 6 RVA: 0x000020A1 File Offset: 0x000002A1
	// (set) Token: 0x06000007 RID: 7 RVA: 0x000020A9 File Offset: 0x000002A9
	public float playheadTimeSeconds
	{
		get
		{
			return this._playheadTimeSeconds;
		}
		set
		{
			this._playheadTimeSeconds = value;
			this._trackAudioSource.time = value;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000008 RID: 8 RVA: 0x000020BE File Offset: 0x000002BE
	// (set) Token: 0x06000009 RID: 9 RVA: 0x000020C6 File Offset: 0x000002C6
	public BeatTrack track
	{
		get
		{
			return this._track;
		}
		set
		{
			if (this._track != value)
			{
				this._track = value;
				if (this._track != null)
				{
					this.ValidateBarsMatchDownbeats();
				}
			}
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600000A RID: 10 RVA: 0x000020F1 File Offset: 0x000002F1
	private MusicEditorAssets assets
	{
		get
		{
			if (this._assets == null)
			{
				this._assets = Resources.Load<MusicEditorAssets>("musicEditorAssets");
			}
			return this._assets;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600000B RID: 11 RVA: 0x00002118 File Offset: 0x00000318
	private GUIStyle centredTextStyle
	{
		get
		{
			if (this._centredTextStyle == null)
			{
				this._centredTextStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
				this._centredTextStyle.alignment = TextAnchor.MiddleCenter;
				this._centredTextStyle.fontSize = 12;
			}
			return this._centredTextStyle;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600000C RID: 12 RVA: 0x00002166 File Offset: 0x00000366
	private GUIStyle playheadTimeStyle
	{
		get
		{
			if (this._playheadTimeStyle == null)
			{
				this._playheadTimeStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
				this._playheadTimeStyle.normal.textColor = Color.red;
			}
			return this._playheadTimeStyle;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600000D RID: 13 RVA: 0x000021A8 File Offset: 0x000003A8
	private int playheadPos
	{
		get
		{
			if (this._width == 0f || this._audioClip == null)
			{
				return 0;
			}
			float num = this._playheadTimeSeconds / this._audioClip.length;
			return Mathf.RoundToInt(this.TrackNormToRectX(num));
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600000E RID: 14 RVA: 0x000021F4 File Offset: 0x000003F4
	private AudioSource beatTickSource
	{
		get
		{
			if (this._tapAudioSource == null)
			{
				this._tapAudioSource = new GameObject("MusicEditor_TapSource")
				{
					hideFlags = HideFlags.HideAndDontSave
				}.AddComponent<AudioSource>();
				this._tapAudioSource.clip = this.assets.beatTickClip;
			}
			return this._tapAudioSource;
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000224C File Offset: 0x0000044C
	public MusicEditor()
	{
		this._recordLatencyMs = this.GetPref(MusicEditor.Pref.MusicEditor_RecordLatency, 10);
		this._taps = (MusicEditor.Taps)this.GetPref(MusicEditor.Pref.MusicEditor_Taps, 0);
		this._trackAudioSource = new GameObject
		{
			hideFlags = HideFlags.HideAndDontSave
		}.AddComponent<AudioSource>();
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000022F0 File Offset: 0x000004F0
	private int GetPref(MusicEditor.Pref pref, int defaultVal)
	{
		return PlayerPrefsX.GetInt(pref.ToString(), defaultVal);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002305 File Offset: 0x00000505
	private void SetPref(MusicEditor.Pref pref, int val)
	{
		PlayerPrefsX.SetInt(pref.ToString(), val);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000231A File Offset: 0x0000051A
	private bool GetPref(MusicEditor.Pref pref, bool defaultVal)
	{
		return PlayerPrefsX.GetInt(pref.ToString(), 0) != 0;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002332 File Offset: 0x00000532
	private void SetPref(MusicEditor.Pref pref, bool val)
	{
		PlayerPrefsX.SetInt(pref.ToString(), val ? 1 : 0);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002350 File Offset: 0x00000550
	private float RectXToTrackNorm(float x)
	{
		float num = 1f / this.trackViewZoom * (x / this._width - 0.5f);
		return this.trackViewTimeNorm + num;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002384 File Offset: 0x00000584
	private float TrackNormToRectX(float xNorm)
	{
		float num = 1f / this.trackViewZoom;
		return (xNorm - this.trackViewTimeNorm + 0.5f * num) / num * this._width;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000023B7 File Offset: 0x000005B7
	private float TrackWidthNormToRectWidth(float widthNorm)
	{
		return this.trackViewZoom * widthNorm * this._width;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000023C8 File Offset: 0x000005C8
	private void PlayFrom(float time)
	{
		this._trackAudioSource.Stop();
		double num = AudioSettings.dspTime + 0.02;
		this.playheadTimeSeconds = time;
		this._trackAudioSource.volume = 0.8f;
		this._trackAudioSource.PlayScheduled(num);
		this._trackStartDspTime = num - (double)time;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000241D File Offset: 0x0000061D
	private void TogglePlayPause()
	{
		if (this._trackAudioSource.isPlaying)
		{
			this._trackAudioSource.Pause();
			return;
		}
		this.PlayFrom(this.playheadTimeSeconds);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002444 File Offset: 0x00000644
	public bool DrawGUI(Rect outerRect)
	{
		GUI.SetNextControlName("MusicEditor_WaveformArea");
		bool flag = false;
		GUI.BeginGroup(new Rect(outerRect.x, outerRect.y, outerRect.width, outerRect.height - 30f));
		Rect rect = new Rect(0f, 0f, outerRect.width, outerRect.height - 30f);
		int controlID = GUIUtility.GetControlID(FocusType.Passive, rect);
		this._width = outerRect.width;
		if (this.track.clip == null)
		{
			GUI.Label(rect, "Track has no audio assigned", this.centredTextStyle);
			this.Release();
			return flag;
		}
		this.DrawWaveformGUI(rect);
		flag = this.UpdateWaveformGUIInput(controlID, rect) || flag;
		if (this.track.clip != this._audioClip)
		{
			if (this.track.clip != this._audioClip)
			{
				this._audioClip = this.track.clip;
				this._trackAudioSource.Stop();
				this._trackAudioSource.clip = this._audioClip;
				this._waveformTex = null;
				this.playheadTimeSeconds = 0f;
				if (this._mipsLoadState == MusicEditor.MipsLoadState.Loading)
				{
					this._mipsLoadState = MusicEditor.MipsLoadState.LoadingOutOfDate;
				}
				else if (this._mipsLoadState != MusicEditor.MipsLoadState.LoadingOutOfDate)
				{
					this._mipsLoadState = MusicEditor.MipsLoadState.None;
					this._mips = null;
					this._rawSamples = null;
				}
			}
			flag = true;
		}
		if (this._waveformTex == null || rect.width * 2f != (float)this._waveformTex.width || this._waveformPaintedTimeNorm != this.trackViewTimeNorm || this._waveformPaintedZoom != this.trackViewZoom || this._mipsLoadState == MusicEditor.MipsLoadState.JustLoaded || flag)
		{
			if (this._mipsLoadState == MusicEditor.MipsLoadState.JustLoaded)
			{
				this._mipsLoadState = MusicEditor.MipsLoadState.Loaded;
			}
			if (this._mipsLoadState == MusicEditor.MipsLoadState.Loaded || this._mipsLoadState == MusicEditor.MipsLoadState.Loading)
			{
				this.RedrawWaveformTexture();
			}
		}
		GUI.EndGroup();
		GUI.SetNextControlName(null);
		GUILayout.BeginArea(new Rect(outerRect.x + 4f, outerRect.y + outerRect.height - 30f + 4f, outerRect.width - 8f, 26f));
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("Record Latency:", Array.Empty<GUILayoutOption>());
		int num;
		int.TryParse(GUILayout.TextField(this._recordLatencyMs.ToString(), new GUILayoutOption[] { GUILayout.Width(40f) }), out num);
		if (num != this._recordLatencyMs)
		{
			this._recordLatencyMs = num;
			this.SetPref(MusicEditor.Pref.MusicEditor_RecordLatency, this._recordLatencyMs);
		}
		GUILayout.FlexibleSpace();
		GUILayout.Label("Taps:", Array.Empty<GUILayoutOption>());
		if (MusicEditor._tapOptionNames == null)
		{
			MusicEditor._tapOptionNames = Enum.GetNames(typeof(MusicEditor.Taps));
		}
		int num2 = GUILayout.Toolbar((int)this._taps, MusicEditor._tapOptionNames, Array.Empty<GUILayoutOption>());
		if (num2 != (int)this._taps)
		{
			this._taps = (MusicEditor.Taps)num2;
			this.SetPref(MusicEditor.Pref.MusicEditor_Taps, num2);
		}
		GUILayout.FlexibleSpace();
		this._followPlayhead = GUILayout.Toggle(this._followPlayhead, "Follow playhead (F)", Array.Empty<GUILayoutOption>());
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F)
		{
			Event.current.Use();
			this._followPlayhead = !this._followPlayhead;
			flag = true;
		}
		GUILayout.FlexibleSpace();
		bool flag2 = this._selectedBeatStartIdx != -1 && this._selectedBeatStartIdx != -1 && this._selectedBeatEndIdx > this._selectedBeatStartIdx + 1;
		GUI.enabled = flag2;
		bool flag3 = false;
		if (GUILayout.Button("Distribute", Array.Empty<GUILayoutOption>()) && flag2)
		{
			flag3 = true;
		}
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D && flag2)
		{
			Event.current.Use();
			flag3 = true;
		}
		if (flag3)
		{
			this.RecordUndo("Distribute beats");
			float time = this.track.beats[this._selectedBeatStartIdx].time;
			float time2 = this.track.beats[this._selectedBeatEndIdx].time;
			int num3 = this._selectedBeatEndIdx - this._selectedBeatStartIdx;
			float num4 = (time2 - time) / (float)num3;
			for (int i = this._selectedBeatStartIdx + 1; i < this._selectedBeatEndIdx; i++)
			{
				this.track.beats[i].time = time + (float)(i - this._selectedBeatStartIdx) * num4;
			}
		}
		bool flag4 = false;
		if (GUILayout.Button("Subdivide", Array.Empty<GUILayoutOption>()) && flag2)
		{
			flag4 = true;
		}
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S && Event.current.modifiers == EventModifiers.None && flag2)
		{
			Event.current.Use();
			flag4 = true;
		}
		if (flag4)
		{
			this.SubdivideBeats(this._selectedBeatStartIdx, this._selectedBeatEndIdx);
		}
		GUI.enabled = true;
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(this._trackAudioSource.isPlaying ? "Pause" : "Play", new GUILayoutOption[]
		{
			GUILayout.Width(60f),
			GUILayout.ExpandHeight(true)
		}))
		{
			this.TogglePlayPause();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		return flag;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002970 File Offset: 0x00000B70
	private bool UpdateWaveformGUIInput(int controlID, Rect rect)
	{
		bool flag = false;
		if (Event.current != null)
		{
			if (Event.current.isScrollWheel)
			{
				if (Event.current.delta.y != 0f)
				{
					float num = this.RectXToTrackNorm(Event.current.mousePosition.x);
					this.trackViewZoom *= 1f - 0.01f * Event.current.delta.y;
					float num2 = this.RectXToTrackNorm(Event.current.mousePosition.x);
					this.trackViewTimeNorm -= num2 - num;
					Event.current.Use();
					flag = true;
				}
				if (Event.current.delta.x != 0f)
				{
					this.trackViewTimeNorm += 2f * Event.current.delta.x / (this._width * this.trackViewZoom);
					Event.current.Use();
					flag = true;
				}
			}
			bool flag2 = false;
			if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
			{
				this._mouseDownOnWaveform = true;
				this._selectedBeatStartIdx = (this._selectedBeatEndIdx = -1);
				flag = true;
				flag2 = true;
				Event.current.Use();
			}
			if (Event.current.type == EventType.MouseUp)
			{
				this._mouseDownOnWaveform = false;
				this._mouseDownOnBeat = false;
			}
			if (this._mouseDownOnWaveform && Event.current.type == EventType.MouseDrag && Event.current.alt)
			{
				GUI.FocusControl("MusicEditor_WaveformArea");
				this.trackViewTimeNorm -= Event.current.delta.x / (this._width * this.trackViewZoom);
				Event.current.Use();
				flag = true;
			}
			if (this._mouseDownOnWaveform && (flag2 || Event.current.type == EventType.MouseDrag) && !Event.current.alt)
			{
				GUI.FocusControl("MusicEditor_WaveformArea");
				GUIUtility.hotControl = controlID;
				if (this._audioClip != null)
				{
					float num3 = this.RectXToTrackNorm(Event.current.mousePosition.x) * this._audioClip.length;
					this.playheadTimeSeconds = Mathf.Clamp(num3, 0f, this._audioClip.length - 0.01f);
					this._lastUpdatePlayheadPos = this.playheadTimeSeconds;
					if (this._trackAudioSource.isPlaying)
					{
						this.PlayFrom(this.playheadTimeSeconds);
					}
					if (this._tapAudioSource != null && this._tapAudioSource.isPlaying)
					{
						this._tapAudioSource.Stop();
						this._scheduledTapTime = -1f;
					}
				}
				Event.current.Use();
				GUI.changed = true;
				flag = true;
			}
			else if (Event.current.type == EventType.MouseUp)
			{
				if (GUIUtility.hotControl == controlID)
				{
					GUIUtility.hotControl = 0;
				}
				Event.current.Use();
			}
			else if (Event.current.type == EventType.KeyDown)
			{
				if (Event.current.keyCode == KeyCode.Space)
				{
					this.TogglePlayPause();
					Event.current.Use();
				}
				if (GUI.GetNameOfFocusedControl() == "MusicEditor_WaveformArea")
				{
					if (this._selectedBeatStartIdx != -1 && (Event.current.character == '1' || Event.current.character == '2'))
					{
						this.RecordUndo("Modify beat type");
						bool flag3 = Event.current.character == '1';
						BeatTrack.BeatFlags beatFlags = BeatTrack.BeatFlags.On;
						if (flag3)
						{
							beatFlags |= BeatTrack.BeatFlags.Down;
						}
						for (int i = this._selectedBeatStartIdx; i <= this._selectedBeatEndIdx; i++)
						{
							bool flag4 = (this.track.beats[i].flags & BeatTrack.BeatFlags.Down) > BeatTrack.BeatFlags.None;
							BeatTrack.Beat[] beats = this.track.beats;
							if (!flag4 && flag3)
							{
								this._barsScratch.Clear();
								this._barsScratch.AddRange(this.track.bars);
								int num4 = this.track.BarIdxWithBeatIdx(i);
								if (num4 == -1)
								{
									BeatTrack.Bar bar = new BeatTrack.Bar
									{
										firstBeatIdx = i
									};
									this._barsScratch.Insert(0, bar);
								}
								else
								{
									BeatTrack.Bar bar2 = this.track.bars[num4];
									int num5 = num4 + 1;
									BeatTrack.Bar bar3 = new BeatTrack.Bar
									{
										firstBeatIdx = i
									};
									int num6 = i - bar2.firstBeatIdx;
									ValueTuple<Obstacles, Obstacles> valueTuple = bar2.obstacles.Split(2 * num6);
									Obstacles item = valueTuple.Item1;
									Obstacles item2 = valueTuple.Item2;
									bar2.obstacles = item;
									bar3.obstacles = item2;
									this._barsScratch.Insert(num5, bar3);
								}
								this.track.bars = this._barsScratch.ToArray();
							}
							else if (flag4 && !flag3)
							{
								this._barsScratch.Clear();
								this._barsScratch.AddRange(this.track.bars);
								int num7 = this.track.BarIdxWithBeatIdx(i - 1);
								int num8 = this.track.BarIdxWithBeatIdx(i);
								if (num7 == -1)
								{
									this._barsScratch.RemoveAt(0);
								}
								else
								{
									BeatTrack.Bar bar4 = this.track.bars[num7];
									BeatTrack.Bar bar5 = this.track.bars[num8];
									int num9 = bar5.firstBeatIdx - bar4.firstBeatIdx;
									bar4.obstacles = bar4.obstacles.Merge(bar5.obstacles, 2 * num9);
									this._barsScratch[num7] = bar4;
									this._barsScratch.RemoveAt(num8);
								}
								this.track.bars = this._barsScratch.ToArray();
							}
							this.track.beats[i].flags = beatFlags;
						}
						Event.current.Use();
					}
					else if (Event.current.character == '1')
					{
						this.AddBeat(BeatTrack.BeatFlags.Down | BeatTrack.BeatFlags.On);
						Event.current.Use();
					}
					else if (Event.current.character == '2')
					{
						this.AddBeat(BeatTrack.BeatFlags.On);
						Event.current.Use();
					}
					else if ((Event.current.keyCode == KeyCode.Backspace || Event.current.keyCode == KeyCode.Delete) && this._selectedBeatStartIdx != -1)
					{
						this.RemoveBeats(this._selectedBeatStartIdx, this._selectedBeatEndIdx);
						this._selectedBeatStartIdx = (this._selectedBeatEndIdx = -1);
						Event.current.Use();
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002FEA File Offset: 0x000011EA
	private void RecordUndo(string undoName)
	{
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002FEC File Offset: 0x000011EC
	private void AddBeat(BeatTrack.BeatFlags flags)
	{
		float num;
		if (this._trackAudioSource.isPlaying)
		{
			num = (float)(AudioSettings.dspTime - this._trackStartDspTime - (double)((float)this._recordLatencyMs / 1000f));
		}
		else
		{
			num = this.playheadTimeSeconds;
		}
		this.AddBeat(flags, num);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00003034 File Offset: 0x00001234
	private void AddBeat(BeatTrack.BeatFlags flags, float time)
	{
		this.RecordUndo("Add beat");
		this._beatsScratch.Clear();
		this._barsScratch.Clear();
		BeatTrack.Beat beat = new BeatTrack.Beat
		{
			flags = flags,
			time = time
		};
		int num = -1;
		for (int i = 0; i < this.track.beats.Length; i++)
		{
			BeatTrack.Beat beat2 = this.track.beats[i];
			if (num == -1 && beat2.time > beat.time)
			{
				num = this._beatsScratch.Count;
				this._beatsScratch.Add(beat);
			}
			this._beatsScratch.Add(beat2);
		}
		if (num == -1)
		{
			num = this._beatsScratch.Count;
			this._beatsScratch.Add(beat);
		}
		bool flag = false;
		for (int j = 0; j < this.track.bars.Length; j++)
		{
			BeatTrack.Bar bar = this.track.bars[j];
			if (bar.firstBeatIdx >= num)
			{
				if (!flag && beat.isDown)
				{
					this._barsScratch.Add(new BeatTrack.Bar
					{
						firstBeatIdx = num
					});
					flag = true;
				}
				bar.firstBeatIdx++;
			}
			this._barsScratch.Add(bar);
		}
		if (!flag && beat.isDown)
		{
			this._barsScratch.Add(new BeatTrack.Bar
			{
				firstBeatIdx = num
			});
		}
		this.track.beats = this._beatsScratch.ToArray();
		this.track.bars = this._barsScratch.ToArray();
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000031DC File Offset: 0x000013DC
	private void SubdivideBeats(int startBeatIdx, int endBeatIdx)
	{
		this.RecordUndo("Subdivide beats");
		this._beatsScratch.Clear();
		this._barsScratch.Clear();
		int num = this.track.BarIdxWithBeatIdx(startBeatIdx);
		int num2 = this.track.BarIdxWithBeatIdx(endBeatIdx);
		if (num < 0 || num >= this.track.bars.Length || num2 < 0 || num2 >= this.track.bars.Length)
		{
			Debug.LogError("Invalid subdivision range");
			return;
		}
		startBeatIdx = this.track.bars[num].firstBeatIdx;
		if (num2 < this.track.bars.Length - 1)
		{
			endBeatIdx = this.track.bars[num2 + 1].firstBeatIdx - 1;
		}
		else
		{
			endBeatIdx = this.track.beats.Length - 1;
		}
		if (endBeatIdx <= startBeatIdx)
		{
			Debug.LogError("Invalid beat range");
			return;
		}
		for (int i = 0; i < num; i++)
		{
			this._barsScratch.Add(this.track.bars[i]);
		}
		for (int j = 0; j < startBeatIdx; j++)
		{
			this._beatsScratch.Add(this.track.beats[j]);
		}
		float num3 = this.track.beats[startBeatIdx + 1].time - this.track.beats[startBeatIdx].time;
		float num4 = this.track.beats[startBeatIdx].time - num3;
		for (int k = startBeatIdx; k <= endBeatIdx; k++)
		{
			BeatTrack.Beat beat = this.track.beats[k];
			beat.flags |= BeatTrack.BeatFlags.On;
			float num5 = beat.time - num4;
			BeatTrack.Beat beat2 = new BeatTrack.Beat
			{
				time = beat.time + 0.5f * num5,
				flags = BeatTrack.BeatFlags.None
			};
			this._beatsScratch.Add(beat);
			this._beatsScratch.Add(beat2);
			num4 = beat.time;
		}
		int num6 = startBeatIdx;
		int num7 = 0;
		for (int l = num; l <= num2; l++)
		{
			Obstacles obstacles = this.track.bars[l].obstacles;
			int num8;
			if (l < this.track.bars.Length - 1)
			{
				num8 = this.track.bars[l + 1].firstBeatIdx - this.track.bars[l].firstBeatIdx;
			}
			else
			{
				num8 = this.track.beats.Length - this.track.bars[l].firstBeatIdx;
			}
			BeatTrack.Bar bar = new BeatTrack.Bar
			{
				firstBeatIdx = num6
			};
			BeatTrack.Bar bar2 = new BeatTrack.Bar
			{
				firstBeatIdx = num6 + num8
			};
			for (int m = 0; m < num8 * 2; m++)
			{
				ObstacleType obstacleType = obstacles[m];
				if (m < num8)
				{
					bar.obstacles[m * 2] = obstacleType;
				}
				else
				{
					bar2.obstacles[m * 2 - num8 * 2] = obstacleType;
				}
			}
			BeatTrack.Beat beat3 = this._beatsScratch[bar2.firstBeatIdx];
			beat3.flags |= BeatTrack.BeatFlags.Down;
			this._beatsScratch[bar2.firstBeatIdx] = beat3;
			this._barsScratch.Add(bar);
			this._barsScratch.Add(bar2);
			num6 += num8 * 2;
			num7 += num8;
		}
		for (int n = endBeatIdx + 1; n < this.track.beats.Length; n++)
		{
			this._beatsScratch.Add(this.track.beats[n]);
		}
		for (int num9 = num2 + 1; num9 < this.track.bars.Length; num9++)
		{
			BeatTrack.Bar bar3 = this.track.bars[num9];
			bar3.firstBeatIdx += num7;
			this._barsScratch.Add(bar3);
		}
		this.track.beats = this._beatsScratch.ToArray();
		this.track.bars = this._barsScratch.ToArray();
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00003620 File Offset: 0x00001820
	public void ValidateBarsMatchDownbeats()
	{
		this._barsScratch.Clear();
		int num = 0;
		for (int i = 0; i < this.track.beats.Length; i++)
		{
			if (this.track.beats[i].isDown)
			{
				num++;
			}
		}
		if (num != this.track.bars.Length)
		{
			Debug.LogWarning(string.Format("There are {0} downbeats but {1} bars. Attempting to repair...", num, this.track.bars.Length));
			Dictionary<int, BeatTrack.Bar> dictionary = new Dictionary<int, BeatTrack.Bar>();
			foreach (BeatTrack.Bar bar in this.track.bars)
			{
				dictionary[bar.firstBeatIdx] = bar;
			}
			for (int k = 0; k < this.track.beats.Length; k++)
			{
				if (this.track.beats[k].isDown)
				{
					BeatTrack.Bar bar2;
					if (dictionary.TryGetValue(k, out bar2))
					{
						this._barsScratch.Add(bar2);
					}
					else
					{
						this._barsScratch.Add(new BeatTrack.Bar
						{
							firstBeatIdx = k
						});
					}
				}
			}
			this.track.bars = this._barsScratch.ToArray();
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x0000376C File Offset: 0x0000196C
	private void RemoveBeats(int fromBeatIdx, int toBeatIdx)
	{
		this.RecordUndo("Remove beats");
		this._beatsScratch.Clear();
		this._barsScratch.Clear();
		int num = -1;
		int num2 = -1;
		for (int i = fromBeatIdx; i <= toBeatIdx; i++)
		{
			if (this.track.beats[i].isDown)
			{
				if (num == -1)
				{
					num = i;
				}
				num2 = i;
			}
		}
		int num3 = toBeatIdx - fromBeatIdx + 1;
		for (int j = 0; j < this.track.bars.Length; j++)
		{
			BeatTrack.Bar bar = this.track.bars[j];
			if (bar.firstBeatIdx < num)
			{
				this._barsScratch.Add(bar);
			}
			else if (bar.firstBeatIdx > num2)
			{
				if (bar.firstBeatIdx > toBeatIdx)
				{
					bar.firstBeatIdx -= num3;
				}
				this._barsScratch.Add(bar);
			}
		}
		for (int k = 0; k < fromBeatIdx; k++)
		{
			this._beatsScratch.Add(this.track.beats[k]);
		}
		for (int l = toBeatIdx + 1; l < this.track.beats.Length; l++)
		{
			this._beatsScratch.Add(this.track.beats[l]);
		}
		this.track.bars = this._barsScratch.ToArray();
		this.track.beats = this._beatsScratch.ToArray();
		this._beatsScratch.Clear();
		this._barsScratch.Clear();
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000038F0 File Offset: 0x00001AF0
	private void DrawWaveformGUI(Rect rect)
	{
		float num = Mathf.Clamp(this.TrackNormToRectX(0f), 0f, this._width - 1f);
		float num2 = Mathf.Clamp(this.TrackNormToRectX(1f), 0f, this._width - 1f);
		if (num2 > num)
		{
			GUI.color = Color.black.WithAlpha(0.1f);
			GUI.DrawTexture(new Rect(num, 0f, num2 - num, rect.height), Texture2D.whiteTexture);
			GUI.color = Color.white;
		}
		GUI.DrawTexture(new Rect(0f, 0.5f * rect.height, rect.width, 1f), Texture2D.whiteTexture);
		if (this._waveformTex != null)
		{
			GUI.color = Color.white.WithAlpha(0.7f);
			GUI.DrawTexture(rect, this._waveformTex);
			GUI.color = Color.white;
		}
		float num3 = 0.3f * rect.height;
		float num4 = 0.7f * rect.height - 30f;
		float num5 = 0.45f * rect.height;
		MusicEditorAssets assets = this.assets;
		float length = this.track.clip.length;
		float num6 = num3;
		float num7 = num4 - num3;
		for (int i = 0; i < this.track.beats.Length; i++)
		{
			BeatTrack.Beat beat = this.track.beats[i];
			float num8 = this.TrackNormToRectX(beat.time / length);
			if (num8 >= 0f && num8 < this._width)
			{
				if (beat.isDown)
				{
					GUI.color = new Color(0f, 0f, 0f, 0.8f);
					GUI.DrawTexture(new Rect(num8, num6, 1f, num7), Texture2D.whiteTexture);
					GUI.color = Color.white;
				}
				Texture2D texture2D;
				if (i >= this._selectedBeatStartIdx && i <= this._selectedBeatEndIdx)
				{
					texture2D = (beat.isDown ? assets.downBeatIconSelected : assets.onBeatIconSelected);
				}
				else
				{
					texture2D = (beat.isDown ? assets.downBeatIcon : assets.onBeatIcon);
				}
				Rect rect2 = new Rect(num8 - 0.7f * (float)texture2D.width / 2f, num6 - 0.7f * (float)texture2D.height / 2f, 0.7f * (float)texture2D.width, 0.7f * (float)texture2D.height);
				GUI.DrawTexture(rect2, texture2D);
				if (Event.current.type == EventType.MouseDown && rect2.Contains(Event.current.mousePosition) && !Event.current.alt)
				{
					if (Event.current.shift && this._selectedBeatStartIdx != -1)
					{
						if (i < this._selectedBeatStartIdx)
						{
							this._selectedBeatStartIdx = i;
						}
						else if (i > this._selectedBeatEndIdx)
						{
							this._selectedBeatEndIdx = i;
						}
					}
					else if (i < this._selectedBeatStartIdx || i > this._selectedBeatEndIdx)
					{
						this._selectedBeatStartIdx = (this._selectedBeatEndIdx = i);
					}
					this._mouseDownOnBeat = true;
					Event.current.Use();
				}
			}
		}
		float num9 = ((this._audioClip != null) ? this._audioClip.length : 1f);
		if (Event.current.type == EventType.MouseDrag && this._mouseDownOnBeat && this._selectedBeatStartIdx >= 0)
		{
			this.RecordUndo("Move beats");
			float num10 = Event.current.delta.x / (this._width * this.trackViewZoom);
			float num11 = Event.current.delta.x / (this._width * this.trackViewZoom) * num9;
			for (int j = this._selectedBeatStartIdx; j <= this._selectedBeatEndIdx; j++)
			{
				BeatTrack.Beat[] beats = this.track.beats;
				int num12 = j;
				beats[num12].time = beats[num12].time + num11;
			}
			this.RectXToTrackNorm(Event.current.mousePosition.x);
			Event.current.Use();
		}
		int num13 = -1;
		float num14 = 0f;
		Rect rect3 = new Rect(0f, num4, this._width, 120f);
		if (Event.current.type == EventType.MouseDown && rect3.Contains(Event.current.mousePosition))
		{
			float num15 = (Event.current.mousePosition.y - rect3.y) / rect3.height;
			num13 = Mathf.FloorToInt(4f * num15);
			num14 = Event.current.mousePosition.x;
			Event.current.Use();
		}
		GUI.color = Color.black.WithAlpha(0.5f);
		Rect rect4 = rect3;
		rect4.y -= 1f;
		rect4.height += 2f;
		GUI.DrawTexture(rect4, Texture2D.whiteTexture);
		GUI.color = Color.white;
		Color color = new Color(0f, 0.8039216f, 0.654902f);
		Color color2 = new Color(0.50980395f, 0.54509807f, 1f);
		num6 = num4;
		GUI.skin.GetStyle("Button");
		int num16 = 1;
		int playheadPos = this.playheadPos;
		int num17 = 0;
		while (num17 < this.track.bars.Length && this._audioClip != null)
		{
			BeatTrack.Bar bar = this.track.bars[num17];
			int num18 = this.track.BeatCountInBar(num17);
			if (num18 > 8)
			{
				num18 = 8;
			}
			float time = this.track.beats[bar.firstBeatIdx].time;
			float num19 = this.track.BarDuration(num17);
			float num20 = this.TrackNormToRectX(time / this._audioClip.length);
			float num21 = this.TrackNormToRectX((time + num19) / this._audioClip.length) - num20;
			if (num21 > 16f)
			{
				GUI.color = Color.black.WithAlpha(0.8f);
				GUI.Label(new Rect(num20, num5, num21, 20f), num17.ToString(), this.centredTextStyle);
			}
			for (int k = 0; k < num18; k++)
			{
				int num22 = bar.firstBeatIdx + k;
				if (num22 < 0 || num22 >= this.track.beats.Length)
				{
					Debug.LogError(string.Format("Beat idx {0} out of range {1}. ", num22, this.track.beats.Length));
				}
				else
				{
					float time2 = this.track.beats[num22].time;
					float num23 = this.track.BeatDuration(bar.firstBeatIdx + k);
					this.TrackWidthNormToRectWidth(num23 / num9);
					Rect rect5 = new Rect(this.TrackNormToRectX(time2 / num9), num6, this.TrackWidthNormToRectWidth(num23 / num9), 30f);
					if (rect5.x <= this._width && rect5.xMax >= 0f)
					{
						for (int l = 0; l < 3; l++)
						{
							ObstacleType obstacleType = ObstacleType.None;
							switch (l)
							{
							case 0:
								obstacleType = ObstacleType.Hop;
								break;
							case 1:
								obstacleType = ObstacleType.Tricky;
								break;
							case 2:
								obstacleType = ObstacleType.Special;
								break;
							}
							Color color3 = ((obstacleType == ObstacleType.Special) ? color2 : color);
							for (int m = 0; m < 2; m++)
							{
								float num24 = rect5.width / 2f;
								Rect rect6 = new Rect(rect5.x + (float)m * num24, rect5.y + (float)l * 30f, num24, 30f);
								int num25 = 2 * k + m;
								bool flag = bar.obstacles.Has(num25, obstacleType);
								GUI.color = ((m == 1 && !flag) ? Color.white.WithAlpha(0.3f) : Color.white);
								Rect rect7 = rect6;
								if (rect7.width >= 3f && rect7.height >= 3f)
								{
									rect7.x += 1f;
									rect7.y += 1f;
									rect7.width -= 2f;
									rect7.height -= 2f;
								}
								if (flag && (float)playheadPos >= rect6.x && (float)playheadPos <= rect6.xMax)
								{
									GUI.color = Color.Lerp(Color.white, color3, Mathf.InverseLerp(rect6.x, rect6.xMax, (float)playheadPos));
								}
								else
								{
									GUI.color = (flag ? color3 : Color.gray);
								}
								GUI.DrawTexture(rect7, Texture2D.whiteTexture);
								if (num13 == l && num14 >= rect6.x && num14 <= rect6.xMax)
								{
									this.RecordUndo("Toggle obstacle");
									bar.obstacles.Set(num25, obstacleType, !flag);
									this.track.bars[num17] = bar;
								}
								GUI.color = Color.white;
							}
						}
					}
				}
			}
			Rect rect8 = new Rect(num20, num6 + 90f, num21, 30f);
			GUI.color = (bar.hasSwitchback ? color : Color.gray);
			Rect rect9 = rect8;
			if (rect9.width >= 3f && rect9.height >= 3f)
			{
				rect9.x += 1f;
				rect9.y += 1f;
				rect9.width -= 2f;
				rect9.height -= 2f;
			}
			GUI.DrawTexture(rect9, Texture2D.whiteTexture);
			if (num13 == 3 && num14 >= rect8.x && num14 <= rect8.xMax)
			{
				if (!bar.hasSwitchback)
				{
					bar.switchbackLegBeats = 4;
				}
				else if (bar.switchbackLegBeats == 4)
				{
					bar.switchbackLegBeats = 2;
				}
				else
				{
					bar.switchbackLegBeats = 0;
				}
				this.RecordUndo("Switchback toggle");
				this.track.bars[num17] = bar;
			}
			if (bar.hasSwitchback)
			{
				GUI.color = Color.white.WithAlpha(0.8f);
				if (bar.switchbackLegBeats == 4)
				{
					num16 = -num16;
				}
				Rect rect10 = new Rect(rect8.x + 5f, rect8.y + 0.5f * rect8.height - 5f, rect8.width - 10f, 10f);
				if (bar.switchbackLegBeats == 4)
				{
					this.DrawHorizontalArrow(rect10, num16 > 0);
				}
				else if (bar.switchbackLegBeats == 2)
				{
					Rect rect11 = new Rect(rect10.x, rect10.y, rect10.width / 2f - 5f, rect10.height);
					Rect rect12 = new Rect(rect10.x + rect10.width / 2f + 5f, rect11.y, rect11.width, rect11.height);
					this.DrawHorizontalArrow(rect11, num16 <= 0);
					this.DrawHorizontalArrow(rect12, num16 > 0);
					GUI.color = Color.white.WithAlpha(0.3f);
					GUI.DrawTexture(new Rect(rect8.x + rect8.width / 2f, rect8.y + 4f, 1f, rect8.height - 8f), Texture2D.whiteTexture);
				}
				GUI.color = Color.white;
			}
			else
			{
				num16 = 1;
			}
			num17++;
		}
		GUI.color = Color.black.WithAlpha(0.5f);
		GUI.DrawTexture(new Rect(0f, num4, 90f, 120f), Texture2D.whiteTexture);
		GUI.color = Color.white;
		for (int n = 0; n < 4; n++)
		{
			string text = null;
			if (n == 0)
			{
				text = "Jumps";
			}
			else if (n == 2)
			{
				text = "Alt jumps";
			}
			else if (n == 1)
			{
				text = "Tricky jumps";
			}
			else if (n == 3)
			{
				text = "Switchbacks";
			}
			GUI.Label(new Rect(5f, num4 + (float)n * 30f, 90f, 30f), text);
		}
		int num26 = playheadPos;
		if (num26 >= 0 && (float)num26 <= this._width)
		{
			GUI.color = Color.red;
			GUI.DrawTexture(new Rect((float)num26, 0f, 1f, rect.height), Texture2D.whiteTexture);
			GUI.Label(new Rect((float)(num26 + 2), 0f, 200f, 20f), this.playheadTimeSeconds.ToString("F3"), this.playheadTimeStyle);
			GUI.color = Color.white;
		}
		this._lastPlayheadPaintPos = num26;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000045F4 File Offset: 0x000027F4
	private void DrawHorizontalArrow(Rect rect, bool rightwards)
	{
		Texture2D rightArrow = this.assets.rightArrow;
		int num = rightArrow.width / rightArrow.height;
		float num2 = rect.width / rect.height / (float)num;
		Rect rect2;
		if (rightwards)
		{
			rect2 = new Rect(1f - num2, 0f, num2, 1f);
		}
		else
		{
			rect2 = new Rect(1f, 0f, -num2, 1f);
		}
		GUI.DrawTextureWithTexCoords(rect, this.assets.rightArrow, rect2);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00004678 File Offset: 0x00002878
	public bool Update()
	{
		bool flag = false;
		if (this._trackAudioSource.isPlaying)
		{
			this._playheadTimeSeconds = this._trackAudioSource.time;
			int playheadPos = this.playheadPos;
			if (this._lastPlayheadPaintPos != this.playheadPos && playheadPos >= 0 && (float)playheadPos <= this._width)
			{
				flag = true;
			}
		}
		this.UpdateTaps();
		if (this._followPlayhead && this._trackAudioSource.isPlaying && this._lastUpdatePlayheadPos >= 0f)
		{
			float num = this.playheadTimeSeconds - this._lastUpdatePlayheadPos;
			this.trackViewTimeNorm += num / this._audioClip.length;
			flag = true;
		}
		this._lastUpdatePlayheadPos = this.playheadTimeSeconds;
		if (this._audioClip != null && this._mipsLoadState == MusicEditor.MipsLoadState.None)
		{
			this.LoadWaveformMips();
		}
		else if (this._mipsLoadState == MusicEditor.MipsLoadState.None && this._mips != null)
		{
			this._mips = null;
			this._rawSamples = null;
			flag = true;
		}
		else if (this._mipsLoadState == MusicEditor.MipsLoadState.JustLoaded)
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00004774 File Offset: 0x00002974
	private void UpdateTaps()
	{
		if (this._taps == MusicEditor.Taps.None || !this._trackAudioSource.isPlaying || this.track.beats == null || this.track.beats.Length == 0)
		{
			if (this._tapAudioSource != null && this._scheduledTapTime != -1f)
			{
				this._tapAudioSource.Stop();
				this._scheduledTapTime = -1f;
			}
			return;
		}
		float num = -1f;
		if (this._taps == MusicEditor.Taps.Beat && this.track.beats.Length != 0)
		{
			int num2 = BinarySearch.SearchNext<BeatTrack.Beat>(this.track.beats, this.playheadTimeSeconds + 0.02f, (BeatTrack.Beat b) => b.time);
			if (num2 >= 0 && num2 < this.track.beats.Length)
			{
				num = this.track.beats[num2].time;
			}
		}
		else if (this._taps == MusicEditor.Taps.Hop && this.track.beats.Length != 0 && this.track.bars.Length != 0)
		{
			int num3 = BinarySearch.SearchRoundDown<BeatTrack.Beat>(this.track.beats, this.playheadTimeSeconds + 0.02f, (BeatTrack.Beat b) => b.time);
			if (this.playheadTimeSeconds + 0.02f < this.track.beats[0].time)
			{
				if (this.track.bars[0].obstacles.HasAny(0, ObstacleType.Hop | ObstacleType.Special | ObstacleType.Tricky))
				{
					num = this.track.beats[0].time;
				}
			}
			else if (num3 >= 0 && num3 < this.track.beats.Length)
			{
				int num4 = BinarySearch.SearchRoundDown<BeatTrack.Bar>(this.track.bars, (float)num3, (BeatTrack.Bar b) => (float)b.firstBeatIdx);
				int num5 = num3 - this.track.bars[num4].firstBeatIdx;
				float num6 = this.track.BeatDuration(num3);
				float num7 = this.track.beats[num3].time + 0.5f * num6;
				if (this.playheadTimeSeconds <= num7)
				{
					int num8 = 2 * num5 + 1;
					if (num8 < 16 && this.track.bars[num4].obstacles.HasAny(num8, ObstacleType.Hop | ObstacleType.Special | ObstacleType.Tricky))
					{
						num = num7;
					}
				}
				else
				{
					int num9 = num3 + 1;
					if (num9 < this.track.beats.Length)
					{
						int num10 = num5 + 1;
						if (num10 >= this.track.BeatCountInBar(num4))
						{
							num10 = 0;
							num4++;
						}
						int num11 = 2 * num10;
						if (num11 < 16 && this.track.bars[num4].obstacles.HasAny(num11, ObstacleType.Hop | ObstacleType.Special | ObstacleType.Tricky))
						{
							num = this.track.beats[num9].time;
						}
					}
				}
			}
		}
		if (!this.beatTickSource.isPlaying)
		{
			this._scheduledTapTime = -1f;
		}
		if (!Mathf.Approximately(num, this._scheduledTapTime) && !this.beatTickSource.isPlaying)
		{
			double num12 = AudioSettings.dspTime - this._trackStartDspTime;
			if ((double)num > num12 + 0.019999999552965164)
			{
				this.beatTickSource.PlayScheduled(this._trackStartDspTime + (double)num);
				this._scheduledTapTime = num;
			}
		}
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00004B04 File Offset: 0x00002D04
	public void Release()
	{
		if (this._waveformTex != null)
		{
			Object.DestroyImmediate(this._waveformTex);
			this._waveformTex = null;
		}
		if (this._tapAudioSource != null)
		{
			Object.DestroyImmediate(this._tapAudioSource.gameObject);
			this._tapAudioSource = null;
		}
		this.playheadTimeSeconds = 0f;
		this._trackAudioSource.clip = null;
		this._audioClip = null;
		this._pixels = null;
		this._mips = null;
		this._rawSamples = null;
		this._mipsLoadThread = null;
		this.trackViewZoom = 0.9f;
		this.trackViewTimeNorm = 0.5f;
		this._lastUpdatePlayheadPos = -1f;
		this._mouseDownOnWaveform = false;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00004BB8 File Offset: 0x00002DB8
	private void LoadWaveformMips()
	{
		if (this._mipsLoadState != MusicEditor.MipsLoadState.None)
		{
			return;
		}
		this._mipsLoadState = MusicEditor.MipsLoadState.Loading;
		this._rawSamples = new float[this._audioClip.samples * this._audioClip.channels - 2];
		this._audioClip.GetData(this._rawSamples, 0);
		this._mipsLoadThread = new Thread(new ThreadStart(this.MipsLoadingThread));
		this._mipsLoadThread.Priority = ThreadPriority.Normal;
		this._mipsLoadThread.Start();
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00004C3C File Offset: 0x00002E3C
	private void MipsLoadingThread()
	{
		try
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			this._mips = new MusicEditor.Mip[MusicEditor.mipRates.Length];
			for (int i = 0; i < MusicEditor.mipRates.Length; i++)
			{
				this._mips[i].rate = MusicEditor.mipRates[i];
			}
			this._mips[0].samples = new float[this._rawSamples.Length];
			for (int j = 0; j < this._rawSamples.Length; j++)
			{
				this._mips[0].samples[j] = Mathf.Abs(this._rawSamples[j]);
			}
			float[] array = this._mips[0].samples;
			int num = 1;
			for (int k = 1; k < this._mips.Length; k++)
			{
				int num2 = this._mips[k].rate / num;
				float[] array2 = new float[array.Length / num2 + 1];
				MusicEditor.Mip[] mips = this._mips;
				for (int l = 0; l < array.Length; l++)
				{
					int num3 = l / num2;
					array2[num3] = Mathf.Max(array[l], array2[num3]);
				}
				this._mips[k].samples = array2;
				array = array2;
				num = this._mips[k].rate;
			}
			this._maxSample = 0f;
			float[] samples = this._mips[this._mips.Length - 1].samples;
			for (int m = 0; m < samples.Length; m++)
			{
				this._maxSample = Mathf.Max(this._maxSample, samples[m]);
			}
			stopwatch.Stop();
		}
		catch (Exception ex)
		{
			Debug.LogError("EXCEPTION IN MipsLoadingThread: " + ex.Message + " at " + ex.StackTrace.ToString());
		}
		finally
		{
			if (this._mipsLoadState != MusicEditor.MipsLoadState.Loading && this._mipsLoadState != MusicEditor.MipsLoadState.LoadingOutOfDate)
			{
				Debug.LogError("Finished loading but state was unexpected: " + this._mipsLoadState.ToString());
			}
			if (this._mipsLoadState == MusicEditor.MipsLoadState.LoadingOutOfDate)
			{
				this._mipsLoadState = MusicEditor.MipsLoadState.None;
			}
			else if (this._mipsLoadState == MusicEditor.MipsLoadState.Loading)
			{
				this._mipsLoadState = MusicEditor.MipsLoadState.JustLoaded;
			}
			this._mipsLoadThread = null;
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00004EBC File Offset: 0x000030BC
	private void RedrawWaveformTexture()
	{
		int num = (int)(2f * this._width);
		if (this._waveformTex != null && this._waveformTex.width != num)
		{
			Object.DestroyImmediate(this._waveformTex);
			this._waveformTex = null;
			this._pixels = null;
		}
		if (this._waveformTex == null)
		{
			this._waveformTex = new Texture2D(num, 64, TextureFormat.RGBA32, false, false);
		}
		this._pixels = new Color32[num * 64];
		Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		Color32 color2 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		for (int i = 0; i < this._pixels.Length; i++)
		{
			this._pixels[i] = color;
		}
		float num2 = 1f / this.trackViewZoom;
		float num3 = 1f;
		float[] array;
		bool flag;
		if (this._mipsLoadState == MusicEditor.MipsLoadState.Loaded)
		{
			float num4 = num2 * (float)this._audioClip.samples;
			int num5 = 0;
			float num6 = float.MaxValue;
			for (int j = 0; j < this._mips.Length; j++)
			{
				float num7 = num4 / (float)this._mips[j].rate;
				float num8 = Mathf.Abs(MusicEditor.mipSelectionBias * num7 - (float)num);
				if (num8 < num6)
				{
					num6 = num8;
					num5 = j;
				}
			}
			array = this._mips[num5].samples;
			flag = false;
			num3 = this._maxSample;
		}
		else
		{
			array = this._rawSamples;
			flag = true;
		}
		for (int k = 0; k < num; k++)
		{
			float num9 = (float)k / (float)(num - 1);
			int num10 = Mathf.RoundToInt(Mathf.Lerp(this.trackViewTimeNorm - 0.5f * num2, this.trackViewTimeNorm + 0.5f * num2, num9) * (float)(array.Length - 1));
			float num11 = 0f;
			if (num10 >= 0 && num10 < array.Length)
			{
				num11 = array[num10] / num3;
				if (flag)
				{
					num11 = Mathf.Abs(num11);
				}
				num11 = Mathf.Clamp01(num11);
			}
			int num12 = Mathf.FloorToInt(num11 * 64f);
			int num13 = 32 - num12 / 2;
			int num14 = 32 + num12 / 2 - 1;
			for (int l = num13; l <= num14; l++)
			{
				this._pixels[l * num + k] = color2;
			}
		}
		this._waveformTex.SetPixels32(this._pixels);
		this._waveformTex.Apply();
		this._waveformPaintedTimeNorm = this.trackViewTimeNorm;
		this._waveformPaintedZoom = this.trackViewZoom;
	}

	// Token: 0x04000005 RID: 5
	private const float kDefaultZoom = 0.9f;

	// Token: 0x04000006 RID: 6
	private const float iconScalar = 0.7f;

	// Token: 0x04000007 RID: 7
	private const string kWaveformAreaControlName = "MusicEditor_WaveformArea";

	// Token: 0x04000008 RID: 8
	public float trackViewZoom = 0.9f;

	// Token: 0x04000009 RID: 9
	public float trackViewTimeNorm = 0.5f;

	// Token: 0x0400000A RID: 10
	private float _playheadTimeSeconds;

	// Token: 0x0400000B RID: 11
	private BeatTrack _track;

	// Token: 0x0400000C RID: 12
	private MusicEditorAssets _assets;

	// Token: 0x0400000D RID: 13
	private GUIStyle _centredTextStyle;

	// Token: 0x0400000E RID: 14
	private GUIStyle _playheadTimeStyle;

	// Token: 0x0400000F RID: 15
	private AudioSource _tapAudioSource;

	// Token: 0x04000010 RID: 16
	private static int[] mipRates = new int[] { 1, 8, 32, 128, 512, 1024, 2048, 8192, 16384 };

	// Token: 0x04000011 RID: 17
	private static float mipSelectionBias = 0.75f;

	// Token: 0x04000012 RID: 18
	private AudioClip _audioClip;

	// Token: 0x04000013 RID: 19
	private Texture2D _waveformTex;

	// Token: 0x04000014 RID: 20
	private float _waveformPaintedTimeNorm;

	// Token: 0x04000015 RID: 21
	private float _waveformPaintedZoom;

	// Token: 0x04000016 RID: 22
	private Color32[] _pixels;

	// Token: 0x04000017 RID: 23
	private MusicEditor.Mip[] _mips;

	// Token: 0x04000018 RID: 24
	private float[] _rawSamples;

	// Token: 0x04000019 RID: 25
	private MusicEditor.MipsLoadState _mipsLoadState;

	// Token: 0x0400001A RID: 26
	private Thread _mipsLoadThread;

	// Token: 0x0400001B RID: 27
	private float _maxSample;

	// Token: 0x0400001C RID: 28
	private float _lastUpdatePlayheadPos = -1f;

	// Token: 0x0400001D RID: 29
	private float _prevZoom;

	// Token: 0x0400001E RID: 30
	private float _width;

	// Token: 0x0400001F RID: 31
	private int _lastPlayheadPaintPos;

	// Token: 0x04000020 RID: 32
	private double _trackStartDspTime;

	// Token: 0x04000021 RID: 33
	private AudioSource _trackAudioSource;

	// Token: 0x04000022 RID: 34
	private MusicEditor.Taps _taps;

	// Token: 0x04000023 RID: 35
	private static string[] _tapOptionNames;

	// Token: 0x04000024 RID: 36
	private float _scheduledTapTime = -1f;

	// Token: 0x04000025 RID: 37
	private int _recordLatencyMs;

	// Token: 0x04000026 RID: 38
	private bool _mouseDownOnWaveform;

	// Token: 0x04000027 RID: 39
	private bool _mouseDownOnBeat;

	// Token: 0x04000028 RID: 40
	private int _selectedBeatStartIdx = -1;

	// Token: 0x04000029 RID: 41
	private int _selectedBeatEndIdx = -1;

	// Token: 0x0400002A RID: 42
	private bool _followPlayhead;

	// Token: 0x0400002B RID: 43
	private List<BeatTrack.Beat> _beatsScratch = new List<BeatTrack.Beat>(1024);

	// Token: 0x0400002C RID: 44
	private List<BeatTrack.Bar> _barsScratch = new List<BeatTrack.Bar>(512);

	// Token: 0x0200023F RID: 575
	private enum Pref
	{
		// Token: 0x040013AB RID: 5035
		MusicEditor_RecordLatency,
		// Token: 0x040013AC RID: 5036
		MusicEditor_Taps
	}

	// Token: 0x02000240 RID: 576
	private struct Mip
	{
		// Token: 0x040013AD RID: 5037
		public float[] samples;

		// Token: 0x040013AE RID: 5038
		public int rate;
	}

	// Token: 0x02000241 RID: 577
	private enum MipsLoadState
	{
		// Token: 0x040013B0 RID: 5040
		None,
		// Token: 0x040013B1 RID: 5041
		Loading,
		// Token: 0x040013B2 RID: 5042
		LoadingOutOfDate,
		// Token: 0x040013B3 RID: 5043
		JustLoaded,
		// Token: 0x040013B4 RID: 5044
		Loaded
	}

	// Token: 0x02000242 RID: 578
	private enum Taps
	{
		// Token: 0x040013B6 RID: 5046
		None,
		// Token: 0x040013B7 RID: 5047
		Beat,
		// Token: 0x040013B8 RID: 5048
		Hop
	}
}
