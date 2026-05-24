using System;
using UnityEngine;

// Token: 0x0200018F RID: 399
[ExecuteInEditMode]
public class InvisibleCollision : MonoBehaviour
{
	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00068388 File Offset: 0x00066588
	public Range worldDepthRange
	{
		get
		{
			return Range.Centered(base.transform.position.z, this.depth);
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06000D09 RID: 3337 RVA: 0x000683A8 File Offset: 0x000665A8
	public Range worldXRange
	{
		get
		{
			Range range = new Range(float.MaxValue, float.MinValue);
			Vector3 vector = base.transform.TransformPoint(new Vector2(-0.5f * this.size.x, 0.5f * this.size.y));
			Vector3 vector2 = base.transform.TransformPoint(new Vector2(0.5f * this.size.x, 0.5f * this.size.y));
			Vector3 vector3 = base.transform.TransformPoint(new Vector2(-0.5f * this.size.x, -0.5f * this.size.y));
			Vector3 vector4 = base.transform.TransformPoint(new Vector2(0.5f * this.size.x, -0.5f * this.size.y));
			range.min = Mathf.Min(vector.x, vector2.x);
			range.min = Mathf.Min(vector3.x, range.min);
			range.min = Mathf.Min(vector4.x, range.min);
			range.max = Mathf.Max(vector.x, vector2.x);
			range.max = Mathf.Max(vector3.x, range.max);
			range.max = Mathf.Max(vector4.x, range.max);
			return range;
		}
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x0006853A File Offset: 0x0006673A
	private void OnEnable()
	{
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x0006853C File Offset: 0x0006673C
	public bool Contains(Vector3 point, float margin = 0f)
	{
		if (!Range.Centered(base.transform.position.z, this.depth).Contains(point.z))
		{
			return false;
		}
		Vector3 vector = base.transform.InverseTransformPoint(point);
		Vector2 vector2 = Vector2.zero;
		if (margin != 0f)
		{
			vector2 = base.transform.InverseTransformVector(margin * Vector2.one);
			vector2.x = Mathf.Abs(vector2.x);
			vector2.y = Mathf.Abs(vector2.y);
			vector2 *= Mathf.Sign(margin);
		}
		Vector2 vector3 = 0.5f * this.size + vector2;
		return Mathf.Abs(vector.x) < vector3.x && Mathf.Abs(vector.y) < vector3.y;
	}

	// Token: 0x06000D0C RID: 3340 RVA: 0x00068624 File Offset: 0x00066824
	public bool Intersect(Vector2 start, Vector2 end, Range depthRange, bool checkInside, out Vector2 collisionPoint)
	{
		collisionPoint = end;
		Range range = Range.Centered(base.transform.position.z, this.depth);
		if (!depthRange.Intersects(range))
		{
			return false;
		}
		Vector2 vector = base.transform.InverseTransformPoint(new Vector3(start.x, start.y, base.transform.position.z));
		Vector2 vector2 = base.transform.InverseTransformPoint(new Vector3(end.x, end.y, base.transform.position.z));
		Vector2 vector3 = 0.5f * this.size;
		if ((vector.y > vector3.y && vector2.y > vector3.y) || (vector.y < -vector3.y && vector2.y < -vector3.y))
		{
			return false;
		}
		if ((vector.x > vector3.x && vector2.x > vector3.x) || (vector.x < -vector3.x && vector2.x < -vector3.x))
		{
			return false;
		}
		int num = ((vector.x > 0f) ? 1 : (-1));
		if (((vector2.x - vector.x > 0f) ? 1 : (-1)) == num && !checkInside)
		{
			return false;
		}
		if (Mathf.Abs(vector.x) < vector3.x)
		{
			Vector2 vector4 = vector;
			vector4.x = (float)num * vector3.x;
			collisionPoint = base.transform.TransformPoint(vector4);
			return true;
		}
		int num2 = ((vector2.x > 0f) ? 1 : (-1));
		if (num == num2 && Mathf.Abs(vector2.x) >= vector3.x)
		{
			return false;
		}
		float num3 = (float)num * vector3.x;
		float num4 = Mathf.InverseLerp(vector.x, vector2.x, num3);
		Vector2 vector5 = Vector2.Lerp(vector, vector2, num4);
		if (Mathf.Abs(vector5.y) > vector3.y)
		{
			return false;
		}
		collisionPoint = base.transform.TransformPoint(vector5);
		return true;
	}

	// Token: 0x06000D0D RID: 3341 RVA: 0x0006885C File Offset: 0x00066A5C
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.size.x, this.size.y, 0.01f));
		Gizmos.matrix = Matrix4x4.identity;
	}

	// Token: 0x04000FDC RID: 4060
	public Vector2 size = new Vector2(5f, 20f);

	// Token: 0x04000FDD RID: 4061
	[Info("min = z - half depth\nmax = z + half depth")]
	public float depth = 1f;
}
