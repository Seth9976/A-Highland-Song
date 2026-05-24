using System;
using UnityEngine;

// Token: 0x0200016F RID: 367
public class Boat : MonoBehaviour
{
	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06000C2C RID: 3116 RVA: 0x000619F0 File Offset: 0x0005FBF0
	// (set) Token: 0x06000C2D RID: 3117 RVA: 0x000619F8 File Offset: 0x0005FBF8
	public bool arrived { get; private set; }

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00061A01 File Offset: 0x0005FC01
	public float sailDirectionX
	{
		get
		{
			return Mathf.Sign(this.destination.position.x - this._startPos.x);
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00061A24 File Offset: 0x0005FC24
	public Vector3 velocity
	{
		get
		{
			return this._velocity;
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00061A2C File Offset: 0x0005FC2C
	public SlopeSample landingSlopeSample
	{
		get
		{
			return Raycast.FindBestNearbySlopeSample(Level.current, this.destination.position, false, 3f);
		}
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00061A49 File Offset: 0x0005FC49
	private void Awake()
	{
		this._startPos = base.transform.position;
		if (this.chain != null)
		{
			this.chain.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x00061A7B File Offset: 0x0005FC7B
	private void OnDisable()
	{
		this.ResetToStart();
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x00061A83 File Offset: 0x0005FC83
	public void Begin()
	{
		this._velocity = Vector3.zero;
		if (this.chain != null)
		{
			this.chain.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x00061AB0 File Offset: 0x0005FCB0
	private void ResetToStart()
	{
		base.transform.position = this._startPos;
		this._velocity = default(Vector3);
		this.arrived = false;
		this._speed = 0f;
		this._boatLastRippleDist = float.MinValue;
		if (this.chain != null)
		{
			this.chain.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x00061B18 File Offset: 0x0005FD18
	public void ManualUpdate()
	{
		if (this.arrived)
		{
			return;
		}
		Vector3 position = this.destination.position;
		float num = Vector3.Distance(this._startPos, position);
		float num2 = Vector3.Distance(this._startPos, base.transform.position);
		float num3 = 0.05f * this.maxSpeed;
		float num4 = (this.maxSpeed * this.maxSpeed - num3 * num3) / (2f * this.decelleration);
		float num5 = Mathf.InverseLerp(num - num4, num, num2);
		float num6 = Mathf.Lerp(this.maxSpeed, num3, num5);
		float num7 = ((num6 < this._speed) ? this.decelleration : this.acceleration);
		this._speed = Mathf.MoveTowards(this._speed, num6, num7 * Time.deltaTime);
		bool flag = false;
		Vector3 vector = Vector3.MoveTowards(base.transform.position, position, this._speed * Time.deltaTime);
		if (Vector3.Distance(vector, position) < 1f)
		{
			flag = true;
			vector = position;
		}
		base.transform.position = vector;
		Vector3 normalized = (position - this._startPos).normalized;
		this._velocity = this._speed * normalized;
		if (flag)
		{
			this._velocity = default(Vector3);
			this._speed = 0f;
			this.arrived = flag;
			if (this.chain != null)
			{
				this.chain.gameObject.SetActive(false);
			}
			GeneralSplashParticles.Emit(2, base.transform.position);
		}
		if (!Runner.instance.isBoating && this.chain != null)
		{
			this.chain.gameObject.SetActive(false);
		}
		if (num2 > this._boatLastRippleDist + 5f)
		{
			Vector3 vector2 = base.transform.position + 5f * normalized;
			vector2.y = 0f;
			MonoSingleton<WaterRippleManager>.instance.CreateRipple(vector2);
			this._boatLastRippleDist = num2;
		}
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x00061D18 File Offset: 0x0005FF18
	public static void ResetAll()
	{
		Boat[] array = Object.FindObjectsOfType<Boat>(true);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ResetToStart();
		}
	}

	// Token: 0x04000E90 RID: 3728
	public Transform destination;

	// Token: 0x04000E91 RID: 3729
	public Transform sitTransform;

	// Token: 0x04000E92 RID: 3730
	public Transform chain;

	// Token: 0x04000E93 RID: 3731
	public float maxSpeed = 5f;

	// Token: 0x04000E94 RID: 3732
	public float acceleration = 5f;

	// Token: 0x04000E95 RID: 3733
	public float decelleration = 5f;

	// Token: 0x04000E96 RID: 3734
	public FrameAnimation moiraAnim;

	// Token: 0x04000E97 RID: 3735
	public bool jumpOffBeforeArrival;

	// Token: 0x04000E99 RID: 3737
	private Vector3 _startPos;

	// Token: 0x04000E9A RID: 3738
	private Vector3 _velocity;

	// Token: 0x04000E9B RID: 3739
	private float _speed;

	// Token: 0x04000E9C RID: 3740
	private float _boatLastRippleDist = float.MinValue;
}
