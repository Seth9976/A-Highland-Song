using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000064 RID: 100
	[Serializable]
	public sealed class Spline
	{
		// Token: 0x06000211 RID: 529 RVA: 0x00010150 File Offset: 0x0000E350
		public Spline(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
			this.cachedData = new float[128];
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0001019C File Offset: 0x0000E39C
		public void Cache(int frame)
		{
			if (frame == this.frameCount)
			{
				return;
			}
			int length = this.curve.length;
			if (this.m_Loop && length > 1)
			{
				if (this.m_InternalLoopingCurve == null)
				{
					this.m_InternalLoopingCurve = new AnimationCurve();
				}
				Keyframe keyframe = this.curve[length - 1];
				keyframe.time -= this.m_Range;
				Keyframe keyframe2 = this.curve[0];
				keyframe2.time += this.m_Range;
				this.m_InternalLoopingCurve.keys = this.curve.keys;
				this.m_InternalLoopingCurve.AddKey(keyframe);
				this.m_InternalLoopingCurve.AddKey(keyframe2);
			}
			for (int i = 0; i < 128; i++)
			{
				this.cachedData[i] = this.Evaluate((float)i * 0.0078125f, length);
			}
			this.frameCount = Time.renderedFrameCount;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0001028A File Offset: 0x0000E48A
		public float Evaluate(float t, int length)
		{
			if (length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000102BB File Offset: 0x0000E4BB
		public float Evaluate(float t)
		{
			return this.Evaluate(t, this.curve.length);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000102CF File Offset: 0x0000E4CF
		public override int GetHashCode()
		{
			return 17 * 23 + this.curve.GetHashCode();
		}

		// Token: 0x04000234 RID: 564
		public const int k_Precision = 128;

		// Token: 0x04000235 RID: 565
		public const float k_Step = 0.0078125f;

		// Token: 0x04000236 RID: 566
		public AnimationCurve curve;

		// Token: 0x04000237 RID: 567
		[SerializeField]
		private bool m_Loop;

		// Token: 0x04000238 RID: 568
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x04000239 RID: 569
		[SerializeField]
		private float m_Range;

		// Token: 0x0400023A RID: 570
		private AnimationCurve m_InternalLoopingCurve;

		// Token: 0x0400023B RID: 571
		private int frameCount = -1;

		// Token: 0x0400023C RID: 572
		public float[] cachedData;
	}
}
