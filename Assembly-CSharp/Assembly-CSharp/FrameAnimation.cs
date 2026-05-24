using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000B2 RID: 178
[NullableContext(2)]
[Nullable(0)]
[CreateAssetMenu]
public class FrameAnimation : ScriptableObject
{
	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000595 RID: 1429 RVA: 0x0002C090 File Offset: 0x0002A290
	public float duration
	{
		get
		{
			if (this.frames == null || this.frames.Length == 0)
			{
				return 0f;
			}
			float num = 0f;
			if (this.timingAdjustments != null)
			{
				foreach (float num2 in this.timingAdjustments)
				{
					num += num2;
				}
			}
			float num3 = (float)this.fps;
			if (num3 == 0f)
			{
				num3 = 15f;
			}
			return (float)this.frames.Length / num3 + num;
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000596 RID: 1430 RVA: 0x0002C104 File Offset: 0x0002A304
	[Nullable(1)]
	public new string name
	{
		[NullableContext(1)]
		get
		{
			if (this._name == null || this._name == string.Empty)
			{
				if (string.IsNullOrEmpty(base.name))
				{
					this._name = null;
					return string.Empty;
				}
				this._name = base.name;
			}
			return this._name;
		}
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x0002C157 File Offset: 0x0002A357
	private int Idx(int i)
	{
		if (this.mode == FrameAnimation.Mode.Loop)
		{
			return (i + this.frames.Length) % this.frames.Length;
		}
		return Mathf.Clamp(i, 0, this.frames.Length - 1);
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x0002C188 File Offset: 0x0002A388
	public Vector2 RootMotionBetweenFrames(int startIdx, int endIdx, int dir)
	{
		if (startIdx < 0 || startIdx >= this.frames.Length)
		{
			Debug.LogError("startIdx out of range: " + startIdx.ToString() + " on animation " + this.name, this);
			return Vector2.zero;
		}
		if (endIdx < 0 || endIdx >= this.frames.Length)
		{
			Debug.LogError("endIdx out of range: " + endIdx.ToString() + " on animation " + this.name, this);
			return Vector2.zero;
		}
		if (dir != 1 && dir != -1)
		{
			Debug.LogError("dir should be +1 or -1");
			return Vector2.zero;
		}
		if (this.mode == FrameAnimation.Mode.Clamp && ((endIdx < startIdx && dir > 0) || (endIdx > startIdx && dir < 0)))
		{
			return Vector2.zero;
		}
		Vector2 vector = Vector2.zero;
		if (this.rootMotion != null && this.rootMotion.Length != 0)
		{
			for (int num = startIdx; num != endIdx; num = this.Idx(num + dir))
			{
				int num2 = num;
				if (dir < 0)
				{
					num2 = this.Idx(num - 1);
				}
				if (num2 >= 0 && num2 < this.rootMotion.Length)
				{
					vector += (float)dir * this.rootMotion[num2];
				}
			}
		}
		if (this.pivotMovesRoot)
		{
			int num3 = startIdx;
			int num4 = (startIdx + dir + this.frames.Length) % this.frames.Length;
			do
			{
				Vector2 vector2 = FrameAnimation.<RootMotionBetweenFrames>g__PivotPos|19_0(this.frames[num3]);
				Vector2 vector3 = FrameAnimation.<RootMotionBetweenFrames>g__PivotPos|19_0(this.frames[num4]) - vector2;
				vector += (float)dir * vector3;
				num3 = num4;
				num4 = this.Idx(num4 + dir);
			}
			while (num3 != endIdx);
		}
		return vector;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0002C324 File Offset: 0x0002A524
	[NullableContext(1)]
	[CompilerGenerated]
	internal static Vector2 <RootMotionBetweenFrames>g__PivotPos|19_0(Sprite sprite)
	{
		Bounds bounds = sprite.bounds;
		return Vector2.Scale(new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height), bounds.size);
	}

	// Token: 0x04000666 RID: 1638
	public int fps = 15;

	// Token: 0x04000667 RID: 1639
	public bool flipX;

	// Token: 0x04000668 RID: 1640
	private string _name;

	// Token: 0x04000669 RID: 1641
	public FrameAnimation.Mode mode;

	// Token: 0x0400066A RID: 1642
	public bool reversesDirection;

	// Token: 0x0400066B RID: 1643
	[Nullable(1)]
	public Sprite[] frames = new Sprite[0];

	// Token: 0x0400066C RID: 1644
	public bool pivotMovesRoot;

	// Token: 0x0400066D RID: 1645
	public Vector2[] rootMotion;

	// Token: 0x0400066E RID: 1646
	[Nullable(new byte[] { 2, 1 })]
	public FrameAnimation[] variants;

	// Token: 0x0400066F RID: 1647
	public List<Vector2> mouthPositions;

	// Token: 0x04000670 RID: 1648
	public List<FrameAnimation.HeadTorchPos> headTorchPositions;

	// Token: 0x04000671 RID: 1649
	public float[] timingAdjustments;

	// Token: 0x020002D3 RID: 723
	[NullableContext(0)]
	public enum Mode
	{
		// Token: 0x0400165A RID: 5722
		Loop,
		// Token: 0x0400165B RID: 5723
		Clamp
	}

	// Token: 0x020002D4 RID: 724
	[NullableContext(0)]
	[Serializable]
	public struct HeadTorchPos
	{
		// Token: 0x0400165C RID: 5724
		public Vector2 pos;

		// Token: 0x0400165D RID: 5725
		public float angle;
	}
}
