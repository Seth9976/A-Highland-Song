using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class MusicRunFlock : MonoBehaviour
{
	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06000D59 RID: 3417 RVA: 0x0006AADD File Offset: 0x00068CDD
	public bool spawned
	{
		get
		{
			return this._spawnTime > 0f;
		}
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x0006AAEC File Offset: 0x00068CEC
	private void OnEnable()
	{
		this._birds.Clear();
		for (int i = 0; i < this.numBirds; i++)
		{
			Bird bird = this.birdProto.Instantiate<Bird>(null);
			this._birds.Add(bird);
		}
		foreach (Bird bird2 in this._birds)
		{
			bird2.prototype.ReturnToPool();
		}
		this._birds.Clear();
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x0006AB84 File Offset: 0x00068D84
	public void Spawn(Chunk chunk, float flyDirection)
	{
		if (this.spawned)
		{
			return;
		}
		for (int i = 0; i < this.numBirds; i++)
		{
			Bird bird = this.birdProto.Instantiate<Bird>(null);
			this._birds.Add(bird);
		}
		Vector3 vector = 0.5f * (chunk.leftSlope.leftPoint + chunk.rightSlope.rightPoint);
		this._flyBounds = new Bounds(vector + 30f * Vector3.up + 20f * flyDirection * Vector3.right, new Vector3(40f, 20f, 4f));
		Bird.Setup(this._birds, this._flyBounds, chunk.slopes);
		this._spawnTime = Time.time;
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x0006AC58 File Offset: 0x00068E58
	private void Update()
	{
		if (this.spawned)
		{
			Bird.UpdateAll(this._birds, this._flyBounds, null, null, true, Time.deltaTime);
			if (Time.time > this._spawnTime + 10f)
			{
				this._birds.UpdateAndRemoveIf(delegate(Bird bird)
				{
					Vector3 vector = GameCamera.instance.camera.WorldToViewportPoint(bird.transform.position);
					if (vector.x < 0f || vector.x > 1f || vector.y < 0f || vector.y > 1f || vector.z < 0f)
					{
						bird.GetComponent<Prototype>().ReturnToPool();
						return true;
					}
					return false;
				});
				if (this._birds.Count == 0)
				{
					this._perchableSlopeList.Clear();
					this._spawnTime = 0f;
				}
			}
		}
	}

	// Token: 0x0400103A RID: 4154
	public Prototype birdProto;

	// Token: 0x0400103B RID: 4155
	public int numBirds;

	// Token: 0x0400103C RID: 4156
	private Bounds _flyBounds;

	// Token: 0x0400103D RID: 4157
	private List<Bird> _birds = new List<Bird>(32);

	// Token: 0x0400103E RID: 4158
	private List<Slope> _perchableSlopeList = new List<Slope>(32);

	// Token: 0x0400103F RID: 4159
	private float _spawnTime;
}
