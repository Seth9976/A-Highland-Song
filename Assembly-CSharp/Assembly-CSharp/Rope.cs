using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class Rope : MonoBehaviour
{
	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x0006CB3C File Offset: 0x0006AD3C
	public Rope.Params currentParams
	{
		get
		{
			Vector3 vector = this.anchor1.position;
			Vector3 vector2 = this.anchor2.position;
			if (vector.x > vector2.x)
			{
				Vector3 vector3 = vector2;
				Vector3 vector4 = vector;
				vector = vector3;
				vector2 = vector4;
			}
			return new Rope.Params
			{
				left = vector,
				right = vector2,
				z = base.transform.position.z,
				ropeTension = this.taughtness * Mathf.Pow(Vector2.Distance(vector, vector2), this.distancePower),
				numPoints = this.numPointsOnRope
			};
		}
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x0006CBDC File Offset: 0x0006ADDC
	private void OnEnable()
	{
		this._params = default(Rope.Params);
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x0006CBEA File Offset: 0x0006ADEA
	private void Update()
	{
		if (this.anchor1 == null || this.anchor2 == null)
		{
			return;
		}
		if (this.RefreshCatenaryIfNecessary() || this._segments == null)
		{
			this.RefreshSegments();
		}
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x0006CC1F File Offset: 0x0006AE1F
	[Button("Force refresh")]
	private void ForceRefresh()
	{
		if (!Application.isPlaying)
		{
			Debug.Log("Rope force refresh is for play mode only");
			return;
		}
		this.RefreshCatenaryIfNecessary();
		this.RefreshSegments();
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x0006CC40 File Offset: 0x0006AE40
	private bool RefreshCatenaryIfNecessary()
	{
		Rope.Params currentParams = this.currentParams;
		if (this._params.Equals(currentParams))
		{
			return false;
		}
		if (this._points == null || this._points.Length != currentParams.numPoints)
		{
			this._points = new Vector3[currentParams.numPoints];
		}
		float num = currentParams.right.x - currentParams.left.x;
		float num2 = currentParams.right.y - currentParams.left.y;
		this._length = 0f;
		Vector3 vector = default(Vector3);
		for (int i = 0; i < currentParams.numPoints; i++)
		{
			float num3 = currentParams.left.x + num / (float)(currentParams.numPoints - 1) * (float)i;
			float num4 = currentParams.ropeTension * (float)Math.Cosh((double)((num3 - (currentParams.left.x + currentParams.right.x) / 2f) / currentParams.ropeTension)) + num2 * ((num3 - currentParams.left.x) / num);
			this._points[i] = new Vector3(num3, num4, currentParams.z);
			if (i == 0)
			{
				Vector3 left = currentParams.left;
				left.z = currentParams.z;
				vector = left - this._points[0];
				this._points[0] = left;
			}
			else
			{
				this._points[i] += vector;
				this._length += Vector2.Distance(this._points[i - 1], this._points[i]);
			}
		}
		this._params = currentParams;
		return true;
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x0006CE09 File Offset: 0x0006B009
	private static Material material
	{
		get
		{
			if (Rope._material == null)
			{
				Rope._material = Resources.Load<Material>("SpriteWithLighting");
			}
			return Rope._material;
		}
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x0006CE2C File Offset: 0x0006B02C
	private Vector3 GetPoint(int idx, bool leftToRight)
	{
		if (leftToRight)
		{
			return this._points[idx];
		}
		return this._points[this._points.Length - idx - 1];
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0006CE58 File Offset: 0x0006B058
	private void RefreshSegments()
	{
		if (this._params.numPoints < 2)
		{
			return;
		}
		if (this._segments == null)
		{
			this._segments = new List<SpriteRenderer>(this._params.numPoints - 1);
		}
		else
		{
			foreach (SpriteRenderer spriteRenderer in this._segments)
			{
				this._unusedSegments.Push(spriteRenderer);
			}
			this._segments.Clear();
		}
		Vector3 vector2;
		Vector3 vector = (vector2 = this.GetPoint(0, this.leftToRight));
		Vector3 vector3 = default(Vector3);
		float num = 0f;
		int num2 = 1;
		Vector3 vector4 = vector;
		Vector3 vector5 = vector;
		float num3 = 0f;
		float num4 = 0f;
		while (num < this._length)
		{
			SpriteRenderer spriteRenderer2;
			if (this._unusedSegments.Count > 0)
			{
				spriteRenderer2 = this._unusedSegments.Pop();
				spriteRenderer2.gameObject.SetActive(true);
			}
			else
			{
				GameObject gameObject = new GameObject("Rope segment", new Type[] { typeof(SpriteRenderer) });
				gameObject.transform.SetParent(base.transform, false);
				spriteRenderer2 = gameObject.GetComponent<SpriteRenderer>();
				spriteRenderer2.material = Rope.material;
			}
			int count = this._segments.Count;
			int num5;
			if (this.leftToRight)
			{
				num5 = count % this._sprites.Count;
			}
			else
			{
				num5 = -count % this._sprites.Count + this._sprites.Count - 1;
			}
			spriteRenderer2.sprite = this._sprites[num5];
			spriteRenderer2.transform.localScale = this.thickness * Vector3.one;
			float num6 = this.thickness * spriteRenderer2.localBounds.size.x;
			num += num6;
			while (num > num4 && num2 < this._points.Length - 1)
			{
				vector4 = vector5;
				num3 = num4;
				num2++;
				vector5 = this.GetPoint(num2, this.leftToRight);
				num4 += Vector2.Distance(vector4, vector5);
			}
			float num7 = Mathf.InverseLerp(num3, num4, num);
			vector3 = Vector3.Lerp(vector4, vector5, num7);
			Vector3 vector6 = 0.5f * (vector2 + vector3);
			spriteRenderer2.transform.position = vector6;
			Vector3 vector7 = vector3 - vector2;
			if (vector7.x < 0f)
			{
				vector7 = -vector7;
			}
			Vector3 normalized = new Vector3(-vector7.y, vector7.x, 0f).normalized;
			spriteRenderer2.transform.rotation = Quaternion.LookRotation(Vector3.forward, normalized);
			this._segments.Add(spriteRenderer2);
			vector2 = vector3;
		}
		foreach (SpriteRenderer spriteRenderer3 in this._unusedSegments)
		{
			spriteRenderer3.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x0006D168 File Offset: 0x0006B368
	private void OnDrawGizmos()
	{
		if (this.anchor1 == null || this.anchor2 == null)
		{
			return;
		}
		this.RefreshCatenaryIfNecessary();
		for (int i = 0; i < this._points.Length - 1; i++)
		{
			Gizmos.DrawLine(this._points[i], this._points[i + 1]);
		}
	}

	// Token: 0x0400108B RID: 4235
	public Transform anchor1;

	// Token: 0x0400108C RID: 4236
	public Transform anchor2;

	// Token: 0x0400108D RID: 4237
	public bool leftToRight;

	// Token: 0x0400108E RID: 4238
	public float taughtness = 1f;

	// Token: 0x0400108F RID: 4239
	public float distancePower = 1f;

	// Token: 0x04001090 RID: 4240
	[Range(2f, 100f)]
	public int numPointsOnRope = 20;

	// Token: 0x04001091 RID: 4241
	public float thickness = 1f;

	// Token: 0x04001092 RID: 4242
	private static Material _material;

	// Token: 0x04001093 RID: 4243
	private Rope.Params _params;

	// Token: 0x04001094 RID: 4244
	private Vector3[] _points;

	// Token: 0x04001095 RID: 4245
	private float _length;

	// Token: 0x04001096 RID: 4246
	private List<SpriteRenderer> _segments;

	// Token: 0x04001097 RID: 4247
	private Stack<SpriteRenderer> _unusedSegments = new Stack<SpriteRenderer>();

	// Token: 0x04001098 RID: 4248
	[SerializeField]
	private List<Sprite> _sprites;

	// Token: 0x020003B9 RID: 953
	public struct Params
	{
		// Token: 0x0600184F RID: 6223 RVA: 0x0009E5BC File Offset: 0x0009C7BC
		public bool Equals(Rope.Params other)
		{
			return this.left == other.left && this.right == other.right && this.z == other.z && this.ropeTension == other.ropeTension && this.numPoints == other.numPoints;
		}

		// Token: 0x040019EE RID: 6638
		public Vector3 left;

		// Token: 0x040019EF RID: 6639
		public Vector3 right;

		// Token: 0x040019F0 RID: 6640
		public float z;

		// Token: 0x040019F1 RID: 6641
		public float ropeTension;

		// Token: 0x040019F2 RID: 6642
		public int numPoints;
	}
}
