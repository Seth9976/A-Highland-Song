using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class FPSManager : MonoSingleton<FPSManager>
{
	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x00073547 File Offset: 0x00071747
	public float averageFrameTime
	{
		get
		{
			return 1f / this.averageFPS;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00073558 File Offset: 0x00071758
	public float targetFrameTime
	{
		get
		{
			float averageFrameTime = this.averageFrameTime;
			if (Application.targetFrameRate <= 0)
			{
				return averageFrameTime;
			}
			float num = 1f / (float)Application.targetFrameRate;
			if (Mathf.Abs(averageFrameTime - num) > num * 0.25f)
			{
				return averageFrameTime;
			}
			return num;
		}
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x00073597 File Offset: 0x00071797
	private void OnEnable()
	{
		this.RefreshTargetFrameRate();
		this.SetInitialFPS();
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x000735A5 File Offset: 0x000717A5
	private void Update()
	{
		this.RefreshTargetFrameRate();
		this.RefreshAverageFPS();
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x000735B3 File Offset: 0x000717B3
	private void RefreshTargetFrameRate()
	{
		if (Application.targetFrameRate != this.settings.targetFrameRate)
		{
			Application.targetFrameRate = this.settings.targetFrameRate;
		}
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x000735D7 File Offset: 0x000717D7
	private void SetInitialFPS()
	{
		this.averageFPS = (float)this.settings.targetFrameRate;
		this.maxFPS = (float)this.settings.targetFrameRate;
		this.minFPS = (float)this.settings.targetFrameRate;
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x00073610 File Offset: 0x00071810
	private void RefreshAverageFPS()
	{
		this.deltaTimes.Add(Time.unscaledDeltaTime);
		this.RemoveOldDeltaTimes();
		int count = this.deltaTimes.Count;
		this.averageFPS = 0f;
		this.maxFPS = 0f;
		this.minFPS = 0f;
		if (count > 0)
		{
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = 0f;
			foreach (float num4 in this.deltaTimes)
			{
				num = Mathf.Min(num, num4);
				num2 = Mathf.Max(num2, num4);
				num3 += num4;
			}
			num3 /= (float)count;
			this.averageFPS = 1f / num3;
			this.maxFPS = 1f / num;
			this.minFPS = 1f / num2;
		}
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x00073700 File Offset: 0x00071900
	private void RemoveOldDeltaTimes()
	{
		float num = 0f;
		for (int i = this.deltaTimes.Count - 1; i >= 0; i--)
		{
			num += this.deltaTimes[i];
			if (num > this.debugSettings.fpsGraphHistoryTime)
			{
				this.deltaTimes.RemoveRange(0, i);
				return;
			}
		}
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x00073758 File Offset: 0x00071958
	private void OnGUI()
	{
		bool flag = false;
		if (Application.isEditor)
		{
			if (this.debugSettings.showInEditor)
			{
				flag = true;
			}
		}
		else if (Debug.isDebugBuild && this.debugSettings.showInDevBuilds)
		{
			flag = true;
		}
		else if (!Debug.isDebugBuild && this.debugSettings.showInReleaseBuilds)
		{
			flag = true;
		}
		if (flag)
		{
			if (this.showDetail)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("FPS");
				stringBuilder.Append("TAR: ");
				stringBuilder.AppendLine(string.Format("{0:n1}", this.settings.targetFrameRate));
				stringBuilder.Append("AVG: ");
				stringBuilder.AppendLine(string.Format("{0:n1}", this.averageFPS));
				stringBuilder.Append("MAX: ");
				stringBuilder.AppendLine(string.Format("{0:n1}", this.maxFPS));
				stringBuilder.Append("MIN: ");
				stringBuilder.AppendLine(string.Format("{0:n1}", this.minFPS));
				GUI.Label(this.debugSettings.fpsPos, stringBuilder.ToString());
				return;
			}
			GUI.Label(this.debugSettings.fpsPos, string.Format("{0:n1}", this.averageFPS));
		}
	}

	// Token: 0x040011A4 RID: 4516
	public FPSSettings settings;

	// Token: 0x040011A5 RID: 4517
	public FPSDebugSettings debugSettings;

	// Token: 0x040011A6 RID: 4518
	public float averageFPS;

	// Token: 0x040011A7 RID: 4519
	public float maxFPS;

	// Token: 0x040011A8 RID: 4520
	public float minFPS;

	// Token: 0x040011A9 RID: 4521
	public bool showDetail;

	// Token: 0x040011AA RID: 4522
	private List<float> deltaTimes = new List<float>();
}
