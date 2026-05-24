using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000189 RID: 393
public class Flock : MonoBehaviour
{
	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x000674E5 File Offset: 0x000656E5
	private Bounds flyBounds
	{
		get
		{
			return new Bounds(base.transform.position + this.flyRegionOffset, this.flyRegionSize);
		}
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x00067508 File Offset: 0x00065708
	private void Awake()
	{
		Bird.lastTakeOffTime = 0f;
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x00067514 File Offset: 0x00065714
	public void Begin()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this._birds == null)
		{
			this._birds = new List<Bird>(this.numBirds);
		}
		bool flag = this._birdProto != null;
		if (!flag)
		{
			base.GetComponentsInChildren<Bird>(false, this._birds);
			if (this._originalPerchingPoints == null)
			{
				this._originalPerchingPoints = new List<Vector3>(this._birds.Count);
			}
			else
			{
				this._originalPerchingPoints.Clear();
			}
			using (List<Bird>.Enumerator enumerator = this._birds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Bird bird = enumerator.Current;
					this._originalPerchingPoints.Add(bird.transform.position);
				}
				goto IL_00DA;
			}
		}
		for (int i = 0; i < this.numBirds; i++)
		{
			Bird bird2 = this._birdProto.Instantiate<Bird>(null);
			this._birds.Add(bird2);
		}
		IL_00DA:
		if (flag)
		{
			if (this._perchableSlopeList == null)
			{
				this._perchableSlopeList = new List<Slope>(128);
			}
		}
		else
		{
			this._perchableSlopeList = null;
		}
		if (flag && this._perchableSlopeList.Count == 0)
		{
			if (!this.hasSeparatePerchRegion)
			{
				Bird.FindPerchableSlopes(this.flyBounds.center, this.flyBounds.size, this._perchableSlopeList);
			}
			else
			{
				Bird.FindPerchableSlopes(base.transform.position + this.perchRegionOffset, this.perchRegionSize, this._perchableSlopeList);
			}
		}
		Bird.Setup(this._birds, this.flyBounds, this._perchableSlopeList);
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x000676B0 File Offset: 0x000658B0
	public void Clear()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this._birds != null)
		{
			if (this._birdProto != null)
			{
				foreach (Bird bird in this._birds)
				{
					if (bird != null)
					{
						bird.prototype.ReturnToPool();
					}
				}
			}
			this._birds.Clear();
		}
		this._perchableSlopeList = null;
		if (this._originalPerchingPoints != null)
		{
			this._originalPerchingPoints.Clear();
		}
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x00067754 File Offset: 0x00065954
	public void ResetGoshawk()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this._birds != null)
		{
			for (int i = 0; i < this._birds.Count; i++)
			{
				this._birds[i].Perch(this._originalPerchingPoints[i]);
			}
		}
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x000677A4 File Offset: 0x000659A4
	public void Update()
	{
		if (this._birds == null || this._birds.Count == 0)
		{
			return;
		}
		Bird.UpdateAll(this._birds, this.flyBounds, this._perchableSlopeList, this._originalPerchingPoints, true, Time.deltaTime);
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x000677E0 File Offset: 0x000659E0
	private void OnValidate()
	{
		this.flyRegionSize.x = Mathf.Abs(this.flyRegionSize.x);
		this.flyRegionSize.y = Mathf.Abs(this.flyRegionSize.y);
		this.flyRegionSize.z = Mathf.Abs(this.flyRegionSize.z);
		this.perchRegionSize.x = Mathf.Abs(this.perchRegionSize.x);
		this.perchRegionSize.y = Mathf.Abs(this.perchRegionSize.y);
		this.perchRegionSize.z = Mathf.Abs(this.perchRegionSize.z);
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x00067890 File Offset: 0x00065A90
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(base.transform.position + this.flyRegionOffset, this.flyRegionSize);
		if (this.hasSeparatePerchRegion)
		{
			Gizmos.color = Color.black;
			Gizmos.DrawWireCube(base.transform.position + this.perchRegionOffset, this.perchRegionSize);
		}
	}

	// Token: 0x04000FB3 RID: 4019
	public int numBirds = 20;

	// Token: 0x04000FB4 RID: 4020
	[FormerlySerializedAs("regionSize")]
	public Vector3 flyRegionOffset;

	// Token: 0x04000FB5 RID: 4021
	public Vector3 flyRegionSize = new Vector3(200f, 100f, 100f);

	// Token: 0x04000FB6 RID: 4022
	public bool hasSeparatePerchRegion;

	// Token: 0x04000FB7 RID: 4023
	public Vector3 perchRegionOffset;

	// Token: 0x04000FB8 RID: 4024
	public Vector3 perchRegionSize = new Vector3(200f, 100f, 100f);

	// Token: 0x04000FB9 RID: 4025
	private List<Bird> _birds;

	// Token: 0x04000FBA RID: 4026
	private List<Slope> _perchableSlopeList;

	// Token: 0x04000FBB RID: 4027
	private List<Vector3> _originalPerchingPoints;

	// Token: 0x04000FBC RID: 4028
	[SerializeField]
	private Prototype _birdProto;
}
