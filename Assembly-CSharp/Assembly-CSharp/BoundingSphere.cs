using System;
using UnityEngine;

// Token: 0x020001E6 RID: 486
[Serializable]
public sealed class BoundingSphere
{
	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06001126 RID: 4390 RVA: 0x0007F33C File Offset: 0x0007D53C
	// (set) Token: 0x06001127 RID: 4391 RVA: 0x0007F344 File Offset: 0x0007D544
	public Vector3 center
	{
		get
		{
			return this.m_center;
		}
		set
		{
			this.m_center = value;
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06001128 RID: 4392 RVA: 0x0007F34D File Offset: 0x0007D54D
	// (set) Token: 0x06001129 RID: 4393 RVA: 0x0007F355 File Offset: 0x0007D555
	public float radius
	{
		get
		{
			return this.m_radius;
		}
		set
		{
			this.m_radius = value;
		}
	}

	// Token: 0x0600112A RID: 4394 RVA: 0x0007F35E File Offset: 0x0007D55E
	public BoundingSphere()
	{
		this.m_center = Vector3.zero;
		this.m_radius = 0f;
	}

	// Token: 0x0600112B RID: 4395 RVA: 0x0007F37C File Offset: 0x0007D57C
	public BoundingSphere(Vector3 center, float radius)
	{
		this.m_center = center;
		this.m_radius = radius;
	}

	// Token: 0x0600112C RID: 4396 RVA: 0x0007F392 File Offset: 0x0007D592
	public BoundingSphere(global::BoundingSphere source)
	{
		this.m_center = source.center;
		this.m_radius = source.radius;
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x0007F3B4 File Offset: 0x0007D5B4
	public void Set(Bounds bounds)
	{
		this.m_center = bounds.center;
		this.m_radius = Mathf.Max(new float[]
		{
			bounds.extents.x,
			bounds.extents.y,
			bounds.extents.z
		});
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x0007F40C File Offset: 0x0007D60C
	public void Set(global::BoundingSphere bounds)
	{
		this.m_center = bounds.center;
		this.m_radius = bounds.radius;
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x0007F426 File Offset: 0x0007D626
	public void Set(Vector3 center, float radius)
	{
		this.m_center = center;
		this.m_radius = radius;
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x0007F436 File Offset: 0x0007D636
	public void SetCenter(float x, float y, float z)
	{
		this.m_center.Set(x, y, z);
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x0007F448 File Offset: 0x0007D648
	public void CreateFromPoints(Vector3[] points)
	{
		Vector3[] array = new Vector3[points.Length];
		Array.Copy(points, array, points.Length);
		this.CalculateWelzl(array, array.Length, 0, 0);
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x0007F474 File Offset: 0x0007D674
	private void CalculateWelzl(Vector3[] points, int length, int supportCount, int index)
	{
		switch (supportCount)
		{
		case 0:
			this.m_radius = 0f;
			this.m_center = Vector3.zero;
			break;
		case 1:
			this.m_radius = -1.001358E-05f;
			this.m_center = points[index - 1];
			break;
		case 2:
			this.SetSphere(points[index - 1], points[index - 2]);
			break;
		case 3:
			this.SetSphere(points[index - 1], points[index - 2], points[index - 3]);
			break;
		case 4:
			this.SetSphere(points[index - 1], points[index - 2], points[index - 3], points[index - 4]);
			return;
		}
		for (int i = 0; i < length; i++)
		{
			if ((points[i + index] - this.m_center).sqrMagnitude - this.m_radius * this.m_radius > 1.001358E-05f)
			{
				for (int j = i; j > 0; j--)
				{
					Vector3 vector = points[j + index];
					Vector3 vector2 = points[j - 1 + index];
					points[j + index] = vector2;
					points[j - 1 + index] = vector;
				}
				this.CalculateWelzl(points, i, supportCount + 1, index + 1);
			}
		}
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x0007F5DC File Offset: 0x0007D7DC
	private void SetSphere(Vector3 O, Vector3 A)
	{
		this.radius = (float)Math.Sqrt((double)(((A.x - O.x) * (A.x - O.x) + (A.y - O.y) * (A.y - O.y) + (A.z - O.z) * (A.z - O.z)) / 4f)) + 1.00001f - 1f;
		float num = 0.5f * O.x + 0.5f * A.x;
		float num2 = 0.5f * O.y + 0.5f * A.y;
		float num3 = 0.5f * O.z + 0.5f * A.z;
		this.SetCenter(num, num2, num3);
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x0007F6B4 File Offset: 0x0007D8B4
	private void SetSphere(Vector3 O, Vector3 A, Vector3 B)
	{
		Vector3 vector = A - O;
		Vector3 vector2 = B - O;
		Vector3 vector3 = Vector3.Cross(vector, vector2);
		float num = 2f * Vector3.Dot(vector3, vector3);
		if (num == 0f)
		{
			this.m_center = Vector3.zero;
			this.m_radius = 0f;
			return;
		}
		Vector3 vector4 = (Vector3.Cross(vector3, vector) * vector2.sqrMagnitude + Vector3.Cross(vector2, vector3) * vector.sqrMagnitude) / num;
		this.m_radius = vector4.magnitude * 1.00001f;
		this.m_center = O + vector4;
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x0007F75C File Offset: 0x0007D95C
	private void SetSphere(Vector3 O, Vector3 A, Vector3 B, Vector3 C)
	{
		Vector3 vector = A - O;
		Vector3 vector2 = B - O;
		Vector3 vector3 = C - O;
		float num = 2f * (vector.x * (vector2.y * vector3.z - vector3.y * vector2.z) - vector2.x * (vector.y * vector3.z - vector3.y * vector.z) + vector3.x * (vector.y * vector2.z - vector2.y * vector.z));
		if (num == 0f)
		{
			this.m_center = Vector3.zero;
			this.m_radius = 0f;
			return;
		}
		Vector3 vector4 = (Vector3.Cross(vector, vector2) * vector3.sqrMagnitude + Vector3.Cross(vector3, vector) * vector2.sqrMagnitude + Vector3.Cross(vector2, vector3) * vector.sqrMagnitude) / num;
		this.m_radius = vector4.magnitude * 1.00001f;
		this.m_center = O + vector4;
	}

	// Token: 0x04001263 RID: 4707
	private Vector3 m_center;

	// Token: 0x04001264 RID: 4708
	private float m_radius;

	// Token: 0x04001265 RID: 4709
	private const float RADIUS_EPSILON = 1.00001f;
}
