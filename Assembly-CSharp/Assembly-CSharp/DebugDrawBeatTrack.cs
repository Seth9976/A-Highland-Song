using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class DebugDrawBeatTrack : MonoBehaviour
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000342 RID: 834 RVA: 0x0001A215 File Offset: 0x00018415
	private Runner runner
	{
		get
		{
			return Runner.instance;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000343 RID: 835 RVA: 0x0001A21C File Offset: 0x0001841C
	private RunTrack runTrack
	{
		get
		{
			return MonoSingleton<RunTrack>.instance;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000344 RID: 836 RVA: 0x0001A223 File Offset: 0x00018423
	private Camera camera
	{
		get
		{
			if (this._camera == null)
			{
				this._camera = Camera.main;
			}
			return this._camera;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000345 RID: 837 RVA: 0x0001A244 File Offset: 0x00018444
	private float screenLeft
	{
		get
		{
			return this.camera.transform.position.x - 0.5f * GameCamera.ViewWidth(0f);
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000346 RID: 838 RVA: 0x0001A26C File Offset: 0x0001846C
	private float screenRight
	{
		get
		{
			return this.camera.transform.position.x + 0.5f * GameCamera.ViewWidth(0f);
		}
	}

	// Token: 0x06000347 RID: 839 RVA: 0x0001A294 File Offset: 0x00018494
	private void OnGUI()
	{
		this.DrawGUIBeatMarkers();
		this.DrawGUIChunkStartMarkers();
	}

	// Token: 0x06000348 RID: 840 RVA: 0x0001A2A4 File Offset: 0x000184A4
	private void DrawMarker(Vector3 worldPos, float markerHeight, Color color)
	{
		Vector3 vector = this.camera.WorldToScreenPoint(worldPos);
		vector.y = (float)Screen.height - vector.y;
		GUI.color = color;
		GUI.DrawTexture(new Rect(vector.x - 1f, vector.y - markerHeight, 2f, markerHeight), Texture2D.whiteTexture);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x0001A304 File Offset: 0x00018504
	private void DrawGUIBeatMarkers()
	{
		if (!DebugOptions.opts.beatMarkers)
		{
			return;
		}
		if (Mathf.Abs(this.runner.momentum) < 1f || !this.runTrack.playing)
		{
			return;
		}
		Simulate.FindOptions standardPredict = Simulate.FindOptions.standardPredict;
		standardPredict.SetDirectionEastwards(this.runner.runningEastwards);
		int num = this.runTrack.track.BeatIndexAtTime(this.runner.lastRunSlopeTime);
		int num2 = -1;
		while (num2 == -1 || num2 == 1)
		{
			int num3 = num;
			while (num3 >= 0 && num3 < this.runTrack.track.beats.Length)
			{
				float num4 = this.runTrack.track.beats[num3].time - this.runner.lastRunSlopeTime;
				Simulate.FindResult findResult = Simulate.FindGroundPositionAtTime(this.runner.lastRunPos, num4, this.runner.momentum, standardPredict, this.runner.settings);
				Vector2 point = findResult.sample.point;
				if (point.x < this.screenLeft || point.x > this.screenRight || findResult.remainingTime > 0f)
				{
					break;
				}
				this.DrawMarker(findResult.sample.point3d, 50f, Color.white);
				num3 += num2;
			}
			num2 += 2;
		}
	}

	// Token: 0x0600034A RID: 842 RVA: 0x0001A464 File Offset: 0x00018664
	private void DrawGUIChunkStartMarkers()
	{
		if (!DebugOptions.opts.chunkStartMarkers)
		{
			return;
		}
		if (Mathf.Abs(this.runner.momentum) < 1f || !this.runTrack.playing)
		{
			return;
		}
		MusicRun currentMusicRun = this.runner.currentMusicRun;
		if (currentMusicRun == null)
		{
			return;
		}
		foreach (Chunk chunk in currentMusicRun.chunks)
		{
			Vector3 point = chunk.GetConnector(true).point;
			this.DrawMarker(point, 75f, Color.magenta);
		}
	}

	// Token: 0x04000482 RID: 1154
	private Camera _camera;
}
