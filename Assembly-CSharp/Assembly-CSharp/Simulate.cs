using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000DE RID: 222
[NullableContext(1)]
[Nullable(0)]
public static class Simulate
{
	// Token: 0x06000777 RID: 1911 RVA: 0x00043308 File Offset: 0x00041508
	public static Simulate.FindResult FindTimeFromTo(Slope fromSlope, float from, float to, Simulate.FindOptions opts, RunnerSettings settings)
	{
		float num = Mathf.Sign(to - from);
		return Simulate.FindTimeFromTo(new TrackPosition
		{
			slope = fromSlope,
			x = from
		}, new TrackPosition
		{
			x = to
		}, num, opts, settings);
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00043354 File Offset: 0x00041554
	public static Simulate.FindResult FindTimeFromTo(TrackPosition from, TrackPosition to, float momentum, Simulate.FindOptions opts, RunnerSettings settings)
	{
		Simulate.FindResult findResult = Simulate.FindOnTrack(from, to, 30f, momentum, opts, settings);
		findResult.distFromRequestedX = Mathf.Abs(to.x - findResult.sample.point.x);
		findResult.foundRequestedX = true;
		if (to.slope != null && to.slope != findResult.sample.slope)
		{
			findResult.foundRequestedX = false;
		}
		if (findResult.distFromRequestedX > 0.1f)
		{
			findResult.foundRequestedX = false;
		}
		return findResult;
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x000433E4 File Offset: 0x000415E4
	public static Simulate.FindResult FindGroundPositionAtDistance(TrackPosition start, float signedDistance, Simulate.FindOptions opts, RunnerSettings settings)
	{
		TrackPosition trackPosition = new TrackPosition
		{
			chunk = null,
			slope = null,
			x = start.x + signedDistance
		};
		Simulate.FindResult findResult = Simulate.FindOnTrack(start, trackPosition, 30f, Mathf.Sign(signedDistance), opts, settings);
		findResult.distFromRequestedX = Mathf.Abs(trackPosition.x - findResult.sample.point.x);
		findResult.foundRequestedX = true;
		if (findResult.distFromRequestedX > 0.1f)
		{
			findResult.foundRequestedX = false;
		}
		return findResult;
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00043470 File Offset: 0x00041670
	public static Simulate.FindResult IterateForwardOverChunk(Chunk chunk, [Nullable(2)] Simulate.TraceCallback traceCallback, RunnerSettings settings)
	{
		Chunk.Connector connector = chunk.GetConnector(true);
		Chunk.Connector connector2 = chunk.GetConnector(false);
		int num = (connector.rightwards ? 1 : (-1));
		Simulate.FindOptions predictAllowDisabled = Simulate.FindOptions.predictAllowDisabled;
		predictAllowDisabled.direction = Simulate.FindOptions.Direction.Forward;
		predictAllowDisabled.traceCallback = traceCallback;
		predictAllowDisabled.dontChangeChunk = true;
		Simulate.FindResult findResult = Simulate.FindTimeFromTo(new TrackPosition
		{
			slope = connector.slope,
			x = connector.point.x
		}, new TrackPosition
		{
			slope = connector2.slope,
			x = connector2.point.x
		}, (float)num, predictAllowDisabled, settings);
		if (!findResult.foundRequestedX)
		{
			Debug.LogError(string.Format("Failed to find time across chunk '{0}'. Remaining dist = {1}", chunk.name, findResult.distFromRequestedX), chunk);
		}
		return findResult;
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00043544 File Offset: 0x00041744
	public static float TimeSlopeEdge(Slope slope, bool eastEdge, float direction, RunSettings runSettings)
	{
		if (slope.numberOfPoints < 2)
		{
			Debug.LogError("Slope was invalid or had fewer than 2 points", slope);
			return 0f;
		}
		int num = (eastEdge ? (slope.numberOfPoints - 2) : 0);
		Vector3 vector = slope.PointIdx(num);
		Vector3 vector2 = slope.PointIdx(num + 1);
		float num2 = Vector2.Distance(vector, vector2);
		float num3 = Slope.AngleWithVector(vector2 - vector, false, slope);
		float num4 = Mathf.Abs(Simulate.SignedSpeedOnGround(direction, num3, slope.isSlide, runSettings));
		return num2 / num4;
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x000435C9 File Offset: 0x000417C9
	public static Simulate.FindResult FindGroundPositionAtTime(TrackPosition start, float timeOffset, float momentum, Simulate.FindOptions opts, RunnerSettings settings)
	{
		return Simulate.FindOnTrack(start, TrackPosition.none, timeOffset, momentum, opts, settings);
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x000435DC File Offset: 0x000417DC
	private static int CompareSlopeChoices(Simulate.SlopeChoice choice1, Simulate.SlopeChoice choice2, Slope currSlope, Simulate.FindOptions opts)
	{
		if (choice1.isNull && choice2.isNull)
		{
			return 0;
		}
		if (choice1.isNull != choice2.isNull)
		{
			if (!choice1.isNull)
			{
				return -1;
			}
			return 1;
		}
		else
		{
			if (opts.predictMode)
			{
				if (opts.direction == Simulate.FindOptions.Direction.Forward)
				{
					if (currSlope.hintNextSlope != null && (choice1.slope == currSlope.hintNextSlope || choice2.slope == currSlope.hintNextSlope))
					{
						if (!(choice2.slope == currSlope.hintNextSlope))
						{
							return -1;
						}
						return 1;
					}
				}
				else if (opts.direction == Simulate.FindOptions.Direction.East)
				{
					if (currSlope.nextEastSlope != null && (choice1.slope == currSlope.nextEastSlope || choice2.slope == currSlope.nextEastSlope))
					{
						if (!(choice2.slope == currSlope.nextEastSlope))
						{
							return -1;
						}
						return 1;
					}
				}
				else if (opts.direction == Simulate.FindOptions.Direction.West && currSlope.nextWestSlope != null && (choice1.slope == currSlope.nextWestSlope || choice2.slope == currSlope.nextWestSlope))
				{
					if (!(choice2.slope == currSlope.nextWestSlope))
					{
						return -1;
					}
					return 1;
				}
			}
			if (choice1.isSwitchBack != choice2.isSwitchBack)
			{
				if (!choice1.isSwitchBack)
				{
					return -1;
				}
				return 1;
			}
			else if (opts.takeUphillForks)
			{
				if (choice1.localAngle >= choice2.localAngle)
				{
					return -1;
				}
				return 1;
			}
			else if (choice1.depthTenthsChange != choice2.depthTenthsChange)
			{
				if (choice1.depthTenthsChange <= choice2.depthTenthsChange)
				{
					return -1;
				}
				return 1;
			}
			else
			{
				if (choice1.localAngle <= choice2.localAngle)
				{
					return -1;
				}
				return 1;
			}
		}
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00043790 File Offset: 0x00041990
	public static bool CanAutoTraverseStep(Vector2 currentPos, Slope stepSlope, out SlopeSample stepSlopeSample, float extraMargin = 0f)
	{
		stepSlopeSample = stepSlope.SampleAt(currentPos.x, true);
		Vector2 vector = new Vector2(Mathf.Abs(stepSlopeSample.clampedPoint.x - currentPos.x), Mathf.Abs(stepSlopeSample.clampedPoint.y - currentPos.y));
		return vector.x < 0.6f + extraMargin && vector.y < 1.2f + extraMargin;
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00043808 File Offset: 0x00041A08
	public static Simulate.NextSlopeResult NextSlope(Slope currSlope, bool findingRightwards, Simulate.FindOptions opts)
	{
		List<Slope> list = (findingRightwards ? currSlope.rightSlopes : currSlope.leftSlopes);
		Vector3 vector = (findingRightwards ? currSlope.PointIdx(currSlope.numberOfPoints - 1) : currSlope.PointIdx(0));
		if (list.Count == 0)
		{
			return new Simulate.NextSlopeResult
			{
				slope = null
			};
		}
		Simulate.SlopeChoice slopeChoice = new Simulate.SlopeChoice
		{
			slope = null
		};
		foreach (Slope slope in list)
		{
			if (slope == null)
			{
				string text = (findingRightwards ? "rightSlopes" : "leftSlopes");
				Debug.LogError(string.Concat(new string[] { "Null slope connection in ", text, " of ", currSlope.name, ". Do you need to rebuild the level?" }), currSlope);
			}
			else if (slope.isActiveAndEnabled || opts.allowDisabled)
			{
				bool flag;
				if (findingRightwards)
				{
					flag = slope.rightSlopes.Contains(currSlope);
				}
				else
				{
					flag = slope.leftSlopes.Contains(currSlope);
				}
				if (!flag || !opts.stopAtSwitchback)
				{
					SlopeSample slopeSample;
					bool flag2 = Simulate.CanAutoTraverseStep(vector, slope, out slopeSample, 0f);
					if (opts.predictMode || flag2)
					{
						bool flag3 = findingRightwards;
						if (flag)
						{
							flag3 = !findingRightwards;
						}
						float num = (flag3 ? slopeSample.angle : (-slopeSample.angle));
						int num2 = Mathf.RoundToInt(Mathf.Abs(slope.transform.position.z - currSlope.transform.position.z) * 0.1f);
						Simulate.SlopeChoice slopeChoice2 = new Simulate.SlopeChoice
						{
							slope = slope,
							isSwitchBack = flag,
							localAngle = num,
							depthTenthsChange = num2
						};
						if (Simulate.CompareSlopeChoices(slopeChoice, slopeChoice2, currSlope, opts) == 1)
						{
							slopeChoice = slopeChoice2;
						}
					}
				}
			}
		}
		return new Simulate.NextSlopeResult
		{
			slope = slopeChoice.slope,
			switchingDirection = slopeChoice.isSwitchBack
		};
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00043A3C File Offset: 0x00041C3C
	private static Simulate.FindResult FindOnTrack(TrackPosition from, TrackPosition stopAt, float timeLimit, float momentum, Simulate.FindOptions opts, RunnerSettings settings)
	{
		Simulate.<>c__DisplayClass15_0 CS$<>8__locals1;
		CS$<>8__locals1.momentum = momentum;
		if (from.slope == null)
		{
			throw new Exception("Expected from.slope to be non-null");
		}
		float num = Mathf.Sign(timeLimit);
		float num2 = num * timeLimit;
		Slope slope = from.slope;
		SlopeSample slopeSample = slope.SampleAt(from.x, false);
		bool flag = slopeSample.outOfRange;
		Vector3 vector = slopeSample.point3d;
		CS$<>8__locals1.findingRightwards = num * CS$<>8__locals1.momentum >= 0f;
		int num3 = (CS$<>8__locals1.findingRightwards ? slopeSample.i0 : slopeSample.i1);
		CS$<>8__locals1.result = new Simulate.FindResult
		{
			remainingTime = num2,
			duration = 0f,
			flipped = false
		};
		Simulate.NextSlopeResult nextSlopeResult = default(Simulate.NextSlopeResult);
		bool flag2 = false;
		bool flag3 = false;
		int num4 = 0;
		Vector3 vector2;
		float num12;
		float num14;
		for (;;)
		{
			num4++;
			if (num4 >= 1000)
			{
				break;
			}
			if (opts.traceCallback != null)
			{
				opts.traceCallback(new Simulate.TraceState
				{
					slope = slope,
					point = vector,
					rightwards = CS$<>8__locals1.findingRightwards,
					time = CS$<>8__locals1.result.duration
				});
			}
			int num5 = (CS$<>8__locals1.findingRightwards ? (num3 + 1) : (num3 - 1));
			vector2 = Vector3.zero;
			if (!flag2 && !opts.dontChangeSlope && !flag3)
			{
				nextSlopeResult = Simulate.NextSlope(slope, CS$<>8__locals1.findingRightwards, opts);
			}
			bool flag4 = num5 < 0 || num5 > slope.numberOfPoints - 1;
			if (!flag4)
			{
				vector2 = slope.PointIdx(num5);
			}
			if (!flag4 && !opts.dontChangeSlope && !flag3)
			{
				Slope slope2 = nextSlopeResult.slope;
				if (opts.predictMode && !(slope2 == null) && !nextSlopeResult.switchingDirection)
				{
					int num6 = (CS$<>8__locals1.findingRightwards ? 0 : (slope2.numberOfPoints - 1));
					Vector3 vector3 = slope2.PointIdx(num6);
					float num7 = (vector3.x - vector.x) / (vector2.x - vector.x);
					if (num7 >= 0f && num7 <= 1f)
					{
						Vector2 vector4 = Vector2.Lerp(vector, vector2, num7);
						if (vector3.y >= vector4.y)
						{
							if (Mathf.Abs(vector3.x - vector.x) < 0.05f)
							{
								flag4 = true;
							}
							else
							{
								vector2 = vector4;
							}
						}
					}
				}
			}
			if (flag4)
			{
				flag2 = false;
				Chunk chunk = null;
				if (opts.dontChangeChunk)
				{
					chunk = slope.chunk;
				}
				bool flag5 = false;
				if (nextSlopeResult.slope == null || opts.dontChangeSlope || flag3)
				{
					flag5 = true;
				}
				else if (opts.dontChangeChunk && chunk != null && nextSlopeResult.slope.chunk != chunk)
				{
					flag5 = true;
				}
				if (flag5)
				{
					goto Block_30;
				}
				if (nextSlopeResult.switchingDirection)
				{
					Simulate.<FindOnTrack>g__Flip|15_0(ref CS$<>8__locals1);
				}
				slope = nextSlopeResult.slope;
				SlopeSample slopeSample2 = slope.SampleAt(vector.x, false);
				flag = true;
				if (!slopeSample2.outOfRange)
				{
					num5 = (CS$<>8__locals1.findingRightwards ? slopeSample2.i0 : slopeSample2.i1);
					vector2 = slopeSample2.point3d;
				}
				else
				{
					num5 = (CS$<>8__locals1.findingRightwards ? 0 : (slope.numberOfPoints - 1));
					vector2 = slope.PointIdx(num5);
				}
			}
			RunPredictHint runPredictHint = null;
			if (opts.allowBounceHint && slope.predictModeHint)
			{
				float x = slope.predictModeHint.transform.position.x;
				bool flag6 = Range.Auto(vector.x, vector2.x).Contains(x) && slope.predictModeHint.bounceBack && slope.predictModeHint.bounceIsRightward != CS$<>8__locals1.findingRightwards;
				bool flag7 = slope.predictModeHint.bounceBack && slope.predictModeHint.bounceIsRightward != CS$<>8__locals1.findingRightwards && ((slope.predictModeHint.bounceIsRightward && vector.x < x) || (!slope.predictModeHint.bounceIsRightward && vector.x > x));
				if (flag6)
				{
					runPredictHint = slope.predictModeHint;
					vector2 = slope.SampleAt(x, false).point3d;
				}
				else if (flag7)
				{
					Simulate.<FindOnTrack>g__Flip|15_0(ref CS$<>8__locals1);
					continue;
				}
			}
			if (flag)
			{
				float num8 = vector2.y - vector.y;
				num8 *= settings.run.betweenSlopesYScalar;
				vector.y = vector2.y - num8;
			}
			float num9 = Vector2.Distance(vector, vector2);
			float num10 = Slope.AngleWithVector(vector2 - vector, true, slope);
			if (flag)
			{
				num10 = 0f;
			}
			float num11 = Mathf.Abs(Simulate.SignedSpeedOnGround(CS$<>8__locals1.momentum, num10, slope.isSlide, settings.run));
			if (num11 < 0.0001f)
			{
				goto Block_47;
			}
			num12 = num9 / num11;
			float num13 = Mathf.Abs(vector2.x - vector.x);
			num14 = float.MaxValue;
			if (CS$<>8__locals1.result.remainingTime < num12)
			{
				num14 = CS$<>8__locals1.result.remainingTime / num12;
			}
			if (!flag3 && stopAt.isValid && stopAt.slope != null && stopAt.slope == slope)
			{
				flag3 = true;
			}
			if (stopAt.isValid && (flag3 || stopAt.slope == null) && Range.Auto(vector.x, vector2.x).Contains(stopAt.x))
			{
				num14 = Mathf.Abs(vector.x - stopAt.x) / num13;
			}
			if (num14 <= 1f)
			{
				goto Block_57;
			}
			CS$<>8__locals1.result.remainingTime = CS$<>8__locals1.result.remainingTime - num12;
			if (runPredictHint != null && runPredictHint.bounceBack && runPredictHint.bounceIsRightward != CS$<>8__locals1.findingRightwards)
			{
				Simulate.<FindOnTrack>g__Flip|15_0(ref CS$<>8__locals1);
				vector2 = Vector3.Lerp(vector, vector2, 0.98f);
			}
			CS$<>8__locals1.result.duration = CS$<>8__locals1.result.duration + num12;
			vector = vector2;
			num3 = num5;
			flag = false;
		}
		Debug.LogError("Simulate.FindOnTrack hit max iterations. Infinite loop?");
		CS$<>8__locals1.result.sample = slope.SampleAt(vector.x, false);
		if (opts.traceCallback != null)
		{
			opts.traceCallback(new Simulate.TraceState
			{
				slope = slope,
				point = CS$<>8__locals1.result.sample.point3d,
				rightwards = CS$<>8__locals1.findingRightwards,
				time = CS$<>8__locals1.result.duration
			});
		}
		return CS$<>8__locals1.result;
		Block_30:
		CS$<>8__locals1.result.sample = slope.SampleAt(vector.x, false);
		return CS$<>8__locals1.result;
		Block_47:
		CS$<>8__locals1.result.sample = slope.SampleAt(vector.x, false);
		CS$<>8__locals1.result.remainingTime = 0f;
		if (opts.traceCallback != null)
		{
			opts.traceCallback(new Simulate.TraceState
			{
				slope = slope,
				point = CS$<>8__locals1.result.sample.point3d,
				rightwards = CS$<>8__locals1.findingRightwards,
				time = CS$<>8__locals1.result.duration
			});
		}
		return CS$<>8__locals1.result;
		Block_57:
		CS$<>8__locals1.result.duration = CS$<>8__locals1.result.duration + num14 * num12;
		if (Mathf.Abs(CS$<>8__locals1.result.duration - num2) < 0.01f)
		{
			CS$<>8__locals1.result.duration = num2;
		}
		CS$<>8__locals1.result.remainingTime = CS$<>8__locals1.result.remainingTime - num14 * num12;
		if (CS$<>8__locals1.result.remainingTime < 0.01f)
		{
			CS$<>8__locals1.result.remainingTime = 0f;
		}
		float num15 = Mathf.Lerp(vector.x, vector2.x, num14);
		CS$<>8__locals1.result.sample = slope.SampleAt(num15, false);
		if (opts.traceCallback != null)
		{
			opts.traceCallback(new Simulate.TraceState
			{
				slope = slope,
				point = CS$<>8__locals1.result.sample.point3d,
				rightwards = CS$<>8__locals1.findingRightwards,
				time = CS$<>8__locals1.result.duration
			});
		}
		return CS$<>8__locals1.result;
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x000442C4 File Offset: 0x000424C4
	public static float SignedSpeedOnGround(float momentum, float groundAngle, bool canSlide, RunSettings runSettings)
	{
		if (Mathf.Abs(momentum) < 0.01f)
		{
			return 0f;
		}
		float num = Mathf.Sign(momentum) * groundAngle;
		bool flag = num > 0f;
		bool flag2 = num > -runSettings.maxRunGroundAngle && num < runSettings.maxRunGroundAngle;
		AnimationCurve animationCurve = runSettings.speedCurve;
		float num2 = 1f;
		if (canSlide && Mathf.Abs(groundAngle) > runSettings.slideMinAngle)
		{
			animationCurve = (flag ? runSettings.slideUpSpeedCurve : runSettings.slideSpeedCurve);
			if (!flag)
			{
				float num3 = Mathf.InverseLerp(num, runSettings.slideMinAngle, -runSettings.maxRunGroundAngle);
				num2 = runSettings.slideDownSpeedRangeByAngle.Lerp(num3);
			}
		}
		else if (flag2)
		{
			float num4 = (flag ? runSettings.uphillSpeedScalar : runSettings.downhillSpeedScalar);
			float num5 = Mathf.Clamp01(Mathf.Abs(groundAngle) / runSettings.maxRunGroundAngle);
			num2 = Mathf.Lerp(1f, num4, num5);
		}
		else if (flag)
		{
			num2 = runSettings.scrambleUphillScalar;
		}
		else
		{
			num2 = runSettings.runSteepDownhillsScalar;
		}
		return Mathf.Sign(momentum) * animationCurve.Evaluate(Mathf.Abs(momentum)) * num2 * runSettings.maxStandardSpeed;
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x000443D5 File Offset: 0x000425D5
	[CompilerGenerated]
	internal static void <FindOnTrack>g__Flip|15_0(ref Simulate.<>c__DisplayClass15_0 A_0)
	{
		A_0.momentum = -A_0.momentum;
		A_0.findingRightwards = !A_0.findingRightwards;
		A_0.result.flipped = !A_0.result.flipped;
	}

	// Token: 0x02000305 RID: 773
	[NullableContext(0)]
	public struct FindOptions
	{
		// Token: 0x06001675 RID: 5749 RVA: 0x00098859 File Offset: 0x00096A59
		public void SetDirectionEastwards(bool east)
		{
			this.direction = (east ? Simulate.FindOptions.Direction.East : Simulate.FindOptions.Direction.West);
		}

		// Token: 0x0400177D RID: 6013
		public bool takeUphillForks;

		// Token: 0x0400177E RID: 6014
		public bool stopAtSwitchback;

		// Token: 0x0400177F RID: 6015
		public bool predictMode;

		// Token: 0x04001780 RID: 6016
		public Simulate.FindOptions.Direction direction;

		// Token: 0x04001781 RID: 6017
		public bool allowBounceHint;

		// Token: 0x04001782 RID: 6018
		public bool allowDisabled;

		// Token: 0x04001783 RID: 6019
		public bool dontChangeSlope;

		// Token: 0x04001784 RID: 6020
		public bool dontChangeChunk;

		// Token: 0x04001785 RID: 6021
		[Nullable(2)]
		public Simulate.TraceCallback traceCallback;

		// Token: 0x04001786 RID: 6022
		public static Simulate.FindOptions standardSimulate = new Simulate.FindOptions
		{
			stopAtSwitchback = true
		};

		// Token: 0x04001787 RID: 6023
		public static Simulate.FindOptions standardPredict = new Simulate.FindOptions
		{
			predictMode = true,
			allowBounceHint = true
		};

		// Token: 0x04001788 RID: 6024
		public static Simulate.FindOptions predictAllowDisabled = new Simulate.FindOptions
		{
			predictMode = true,
			allowBounceHint = true,
			allowDisabled = true
		};

		// Token: 0x02000435 RID: 1077
		public enum Direction
		{
			// Token: 0x04001B95 RID: 7061
			Forward,
			// Token: 0x04001B96 RID: 7062
			East,
			// Token: 0x04001B97 RID: 7063
			West
		}
	}

	// Token: 0x02000306 RID: 774
	[NullableContext(0)]
	public struct TraceState
	{
		// Token: 0x04001789 RID: 6025
		[Nullable(1)]
		public Slope slope;

		// Token: 0x0400178A RID: 6026
		public Vector3 point;

		// Token: 0x0400178B RID: 6027
		public bool rightwards;

		// Token: 0x0400178C RID: 6028
		public float time;
	}

	// Token: 0x02000307 RID: 775
	// (Invoke) Token: 0x06001678 RID: 5752
	[NullableContext(0)]
	public delegate void TraceCallback(Simulate.TraceState state);

	// Token: 0x02000308 RID: 776
	[NullableContext(0)]
	public struct FindResult
	{
		// Token: 0x0400178D RID: 6029
		public SlopeSample sample;

		// Token: 0x0400178E RID: 6030
		public float duration;

		// Token: 0x0400178F RID: 6031
		public bool flipped;

		// Token: 0x04001790 RID: 6032
		public float remainingTime;

		// Token: 0x04001791 RID: 6033
		public float distFromRequestedX;

		// Token: 0x04001792 RID: 6034
		public bool foundRequestedX;
	}

	// Token: 0x02000309 RID: 777
	[NullableContext(0)]
	public struct NextSlopeResult
	{
		// Token: 0x04001793 RID: 6035
		[Nullable(2)]
		public Slope slope;

		// Token: 0x04001794 RID: 6036
		public bool switchingDirection;
	}

	// Token: 0x0200030A RID: 778
	[NullableContext(0)]
	private struct SlopeChoice
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x000988CF File Offset: 0x00096ACF
		public bool isNull
		{
			get
			{
				return this.slope == null;
			}
		}

		// Token: 0x04001795 RID: 6037
		[Nullable(2)]
		public Slope slope;

		// Token: 0x04001796 RID: 6038
		public bool isSwitchBack;

		// Token: 0x04001797 RID: 6039
		public float localAngle;

		// Token: 0x04001798 RID: 6040
		public int depthTenthsChange;
	}
}
