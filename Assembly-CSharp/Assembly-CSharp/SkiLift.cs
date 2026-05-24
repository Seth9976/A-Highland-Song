using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class SkiLift : MonoSingleton<SkiLift>
{
	// Token: 0x06000DE6 RID: 3558 RVA: 0x0006DA72 File Offset: 0x0006BC72
	private void OnEnable()
	{
		this.SetupLine(this.upwardLine, true);
		this.SetupLine(this.downwardLine, false);
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x0006DA90 File Offset: 0x0006BC90
	private void SetupLine(SkiLift.Line line, bool upward)
	{
		foreach (object obj in line.pointsParent)
		{
			Transform transform = (Transform)obj;
			line.points.Add(transform.position);
		}
		if (upward)
		{
			line.points.Sort((Vector3 p1, Vector3 p2) => p1.y.CompareTo(p2.y));
		}
		else
		{
			line.points.Sort((Vector3 p1, Vector3 p2) => p2.y.CompareTo(p1.y));
		}
		line.distancesBetweenPoints = new float[line.points.Count - 1];
		line.distancesFromStart = new float[line.points.Count - 1];
		line.length = 0f;
		for (int i = 0; i < line.points.Count - 1; i++)
		{
			float num = Vector2.Distance(line.points[i], line.points[i + 1]);
			line.distancesBetweenPoints[i] = num;
			line.distancesFromStart[i] = line.length;
			line.length += num;
		}
		line.chairs = new List<SkiLiftChair>();
		float num2 = line.length / (float)this._settings.chairsPerLine;
		for (int j = 0; j < this._settings.chairsPerLine; j++)
		{
			SkiLiftChair skiLiftChair = line.chairProto.Instantiate<SkiLiftChair>(null);
			skiLiftChair.swingVariation = Random.Range(0.9f, 1.1f);
			this.SetChairLinePosition(skiLiftChair, line, ((float)j + this._settings.startOffset) * num2);
			line.chairs.Add(skiLiftChair);
		}
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x0006DC80 File Offset: 0x0006BE80
	public Vector3 PredictChairMovement(SkiLiftChair chair, float inSeconds)
	{
		float num = this._speed * inSeconds;
		return this.GetChairLinePosition(chair, this.upwardLine, chair.positionOnLine + num).pos - chair.transform.position;
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x0006DCC0 File Offset: 0x0006BEC0
	public void ResetAfterLoad()
	{
		this._speed = (this.running ? this._settings.linearSpeed : 0f);
		foreach (SkiLiftChair skiLiftChair in this.upwardLine.chairs)
		{
			skiLiftChair.swingMaxAngle = 0f;
		}
		foreach (SkiLiftChair skiLiftChair2 in this.downwardLine.chairs)
		{
			skiLiftChair2.swingMaxAngle = 0f;
		}
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x0006DD84 File Offset: 0x0006BF84
	private void Update()
	{
		if (!this.running && this._speed == 0f && !this._swinging)
		{
			return;
		}
		if (this.running)
		{
			this._speed = Mathf.MoveTowards(this._speed, this._settings.linearSpeed, this._settings.acceleration * Time.deltaTime);
		}
		else if (!this.running && this._speed > 0f)
		{
			this.StartSwing();
			this._speed = 0f;
		}
		float num = this._speed * Time.deltaTime;
		this.UpdateLine(this.upwardLine, num);
		this.UpdateLine(this.downwardLine, num);
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x0006DE34 File Offset: 0x0006C034
	private void UpdateLine(SkiLift.Line line, float lineDelta)
	{
		this._swinging = false;
		foreach (SkiLiftChair skiLiftChair in line.chairs)
		{
			if (this.SetChairLinePosition(skiLiftChair, line, skiLiftChair.positionOnLine + lineDelta))
			{
				skiLiftChair.positionOnLine = 0f;
				skiLiftChair.currentBasePosIdx = 0;
				skiLiftChair.transform.position = line.points[0];
			}
			if (skiLiftChair.positionOnLine < this._settings.fadeStartDist)
			{
				skiLiftChair.alpha = Mathf.InverseLerp(0f, this._settings.fadeStartDist, skiLiftChair.positionOnLine);
			}
			else if (skiLiftChair.positionOnLine > line.length - this._settings.fadeStartDist)
			{
				skiLiftChair.alpha = 1f - Mathf.InverseLerp(line.length - this._settings.fadeStartDist, line.length, skiLiftChair.positionOnLine);
			}
			float num = skiLiftChair.swingMaxAngle * Mathf.Sin(this._settings.swingSpeed * (Time.time - this._swingStartTime) * skiLiftChair.swingVariation);
			skiLiftChair.transform.rotation = Quaternion.Euler(0f, 0f, num);
			skiLiftChair.swingMaxAngle *= TimeX.Damping(this._settings.swingDamping);
			if (Mathf.Abs(skiLiftChair.swingMaxAngle) > 1f)
			{
				this._swinging = true;
			}
			else
			{
				skiLiftChair.swingMaxAngle = 0f;
			}
		}
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0006DFE0 File Offset: 0x0006C1E0
	[Button("Start swing")]
	private void StartSwing()
	{
		this._swinging = true;
		this._swingStartTime = Time.time;
		foreach (SkiLiftChair skiLiftChair in this.upwardLine.chairs)
		{
			skiLiftChair.swingMaxAngle = -this._settings.chairsMaxSwingAngle.Random();
		}
		foreach (SkiLiftChair skiLiftChair2 in this.downwardLine.chairs)
		{
			skiLiftChair2.swingMaxAngle = 0.5f * this._settings.chairsMaxSwingAngle.Random();
		}
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x0006E0B4 File Offset: 0x0006C2B4
	private SkiLift.ChairLinePosition GetChairLinePosition(SkiLiftChair chair, SkiLift.Line line, float posOnLine)
	{
		float num = posOnLine - line.distancesFromStart[chair.currentBasePosIdx];
		for (int i = chair.currentBasePosIdx; i < line.points.Count - 1; i++)
		{
			float num2 = line.distancesBetweenPoints[i];
			if (num < num2)
			{
				float num3 = Mathf.Clamp01(num / num2);
				Vector3 vector = Vector3.Lerp(line.points[i], line.points[i + 1], num3);
				return new SkiLift.ChairLinePosition
				{
					atEnd = false,
					pos = vector,
					basePointIdx = i,
					positionOnLine = posOnLine
				};
			}
			num -= num2;
		}
		int num4 = line.points.Count - 1;
		return new SkiLift.ChairLinePosition
		{
			atEnd = true,
			pos = line.points[num4],
			basePointIdx = num4,
			positionOnLine = Mathf.Clamp(posOnLine, 0f, line.length)
		};
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x0006E1AC File Offset: 0x0006C3AC
	private bool SetChairLinePosition(SkiLiftChair chair, SkiLift.Line line, float posOnLine)
	{
		SkiLift.ChairLinePosition chairLinePosition = this.GetChairLinePosition(chair, line, posOnLine);
		chair.transform.position = chairLinePosition.pos;
		chair.currentBasePosIdx = chairLinePosition.basePointIdx;
		chair.positionOnLine = chairLinePosition.positionOnLine;
		return chairLinePosition.atEnd;
	}

	// Token: 0x040010C0 RID: 4288
	public bool running;

	// Token: 0x040010C1 RID: 4289
	public SkiLift.Line upwardLine;

	// Token: 0x040010C2 RID: 4290
	public SkiLift.Line downwardLine;

	// Token: 0x040010C3 RID: 4291
	public Transform breakDownPosition;

	// Token: 0x040010C4 RID: 4292
	private float _speed;

	// Token: 0x040010C5 RID: 4293
	private float _swingStartTime;

	// Token: 0x040010C6 RID: 4294
	private bool _swinging;

	// Token: 0x040010C7 RID: 4295
	[SerializeField]
	private SkiLiftSettings _settings;

	// Token: 0x020003BB RID: 955
	[Serializable]
	public class Line
	{
		// Token: 0x040019FA RID: 6650
		public Transform pointsParent;

		// Token: 0x040019FB RID: 6651
		public Prototype chairProto;

		// Token: 0x040019FC RID: 6652
		[NonSerialized]
		public List<Vector3> points = new List<Vector3>(64);

		// Token: 0x040019FD RID: 6653
		[NonSerialized]
		public List<SkiLiftChair> chairs;

		// Token: 0x040019FE RID: 6654
		[NonSerialized]
		public float length;

		// Token: 0x040019FF RID: 6655
		[NonSerialized]
		public float[] distancesBetweenPoints;

		// Token: 0x04001A00 RID: 6656
		[NonSerialized]
		public float[] distancesFromStart;
	}

	// Token: 0x020003BC RID: 956
	private struct ChairLinePosition
	{
		// Token: 0x04001A01 RID: 6657
		public bool atEnd;

		// Token: 0x04001A02 RID: 6658
		public Vector3 pos;

		// Token: 0x04001A03 RID: 6659
		public int basePointIdx;

		// Token: 0x04001A04 RID: 6660
		public float positionOnLine;
	}
}
