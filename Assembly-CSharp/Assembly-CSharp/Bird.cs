using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class Bird : MonoBehaviour
{
	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00060EAD File Offset: 0x0005F0AD
	public FrameAnimator animator
	{
		get
		{
			if (this._animator == null)
			{
				this._animator = base.GetComponent<FrameAnimator>();
			}
			return this._animator;
		}
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00060ECF File Offset: 0x0005F0CF
	public Prototype prototype
	{
		get
		{
			if (this._prototype == null)
			{
				this._prototype = base.GetComponent<Prototype>();
			}
			return this._prototype;
		}
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00060EF4 File Offset: 0x0005F0F4
	public void Perch()
	{
		this.animator.SetAnimation(this._perchAnim, 0, FrameAnimator.PosMatch.None);
		this._hasDestination = false;
		this._destinationIsPerch = false;
		this._velocity = default(Vector3);
		this._acceleration = default(Vector3);
		this.state = Bird.State.Perching;
		this.stateTimer = 0f;
		if (this.perchTimeApprox >= 0f && this.flyTimeApprox > 0f)
		{
			this.scheduledFlyPerchChangeTime = Random.Range(0.5f * this.perchTimeApprox, 1.5f * this.perchTimeApprox);
			return;
		}
		this.scheduledFlyPerchChangeTime = float.MaxValue;
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00060F98 File Offset: 0x0005F198
	public void Perch(Slope slope, float x)
	{
		SlopeSample slopeSample = slope.SampleAt(x, false);
		base.transform.position = slopeSample.point3d;
		this.Perch();
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x00060FC6 File Offset: 0x0005F1C6
	public void Perch(Vector3 perchPoint)
	{
		base.transform.position = perchPoint;
		this.Perch();
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00060FDC File Offset: 0x0005F1DC
	public void TakeOff(bool dueToPlayerAlarm)
	{
		this.state = Bird.State.Flying;
		this.stateTimer = 0f;
		if (this._takeOffAnim != null)
		{
			this.animator.SetAnimationWithTransition(this._takeOffAnim, this._flyAnim, 0, false, false, FrameAnimator.PosMatch.None);
		}
		else
		{
			this.animator.SetAnimation(this._flyAnim, 0, FrameAnimator.PosMatch.None);
		}
		this._destinationIsPerch = false;
		if (dueToPlayerAlarm)
		{
			float num = Mathf.Sign(base.transform.position.x - Runner.instance.physicalPosition3d.x);
			this._destination = base.transform.position + (float)Random.Range(25, 35) * Vector3.up + num * (float)Random.Range(15, 40) * Vector3.right;
			this._hasDestination = true;
			this._tookOffDueToAlarm = true;
		}
		if (this.flyTimeApprox >= 0f && this.perchTimeApprox != 0f)
		{
			this.scheduledFlyPerchChangeTime = Random.Range(0.5f * this.flyTimeApprox, 1.5f * this.flyTimeApprox);
			return;
		}
		this.scheduledFlyPerchChangeTime = float.MaxValue;
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x00061108 File Offset: 0x0005F308
	public void Setup()
	{
		float num = Mathf.Sign(base.transform.localScale.x);
		Vector3 vector = this.size * Vector3.one;
		vector.x *= num;
		base.transform.localScale = vector;
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x00061154 File Offset: 0x0005F354
	public static void Setup(List<Bird> birds, Bounds flyBounds, List<Slope> perchableSlopeList)
	{
		if (birds == null || birds.Count == 0)
		{
			Debug.LogError("No birds to set up!");
			return;
		}
		bool flag = false;
		for (int i = 0; i < birds.Count; i++)
		{
			Bird bird = birds[i];
			bird.Setup();
			if (bird.perchTimeApprox != 0f && perchableSlopeList != null && perchableSlopeList.Count == 0)
			{
				flag = true;
			}
			if (Random.value >= bird.alreadyFlyingProbability || bird.flyTimeApprox == 0f)
			{
				if (perchableSlopeList != null && perchableSlopeList.Count > 0)
				{
					Slope slope = perchableSlopeList.Random<Slope>();
					float num = slope.range.Random();
					bird.Perch(slope.SampleAt(num, false).point3d);
				}
				else
				{
					bird.Perch();
				}
			}
			else
			{
				bird.transform.position = flyBounds.center + Vector3.Scale(Random.insideUnitSphere, flyBounds.extents);
				bird.TakeOff(false);
				bird.stateTimer = 10f;
			}
		}
		if (flag)
		{
			Debug.LogWarning("Bird(s) wanted to be able to perch but no perch slopes were passed in", birds[0]);
		}
		Bird.UpdateAll(birds, flyBounds, null, null, false, 0.5f);
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x00061280 File Offset: 0x0005F480
	public static void UpdateAll(List<Bird> birds, Bounds flyBounds, List<Slope> perchableSlopeList, List<Vector3> originalPerchingPoints, bool allowAlarm, float dt)
	{
		bool flag = false;
		Vector2 vector = default(Vector2);
		Range range = default(Range);
		bool flag2 = false;
		Bird._takeoffAlarmBirds.Clear();
		if (allowAlarm)
		{
			Runner instance = Runner.instance;
			vector = instance.position;
			range = Range.Centered((float)instance.physicalDepthLayerIdx, 10f);
			if (instance.velocity.magnitude > 4f)
			{
				flag = true;
			}
			else if (instance.stoneSkimming && instance.stoneSkimSubState == Runner.SkimStoneSubState.FollowStone)
			{
				flag = true;
				vector = instance.stonePosition;
				flag2 = true;
			}
		}
		bool flag3 = Time.time < Bird.lastTakeOffTime + 0.15f;
		for (int i = 0; i < birds.Count; i++)
		{
			Bird bird = birds[i];
			bird.stateTimer += dt;
			if (bird.state == Bird.State.Flying)
			{
				Vector3 vector2 = bird.transform.position;
				Vector3 vector3 = bird._destination - vector2;
				float num = Vector3.Magnitude(vector3);
				if (!bird._hasDestination || (num < 20f && !bird._destinationIsPerch))
				{
					bird._destination = flyBounds.center + Vector3.Scale(Random.insideUnitSphere, flyBounds.extents);
					Vector3 vector4 = bird._destination - vector2;
					Vector3 vector5 = new Vector3(vector4.x, 0f, vector4.z);
					float magnitude = vector5.magnitude;
					float num2 = Mathf.Abs(vector4.y);
					if (num2 / magnitude > 1f)
					{
						num2 = 1f * magnitude;
						vector4.y = Mathf.Sign(vector4.y) * num2;
						bird._destination = vector2 + vector4;
					}
					bird._hasDestination = true;
				}
				float num3 = Mathf.InverseLerp(20f, 200f, num);
				float num4 = bird.flySpeed.Lerp(num3);
				float num5 = ((bird.stateTimer < 2f) ? 0.3f : 2f);
				if (bird._destinationIsPerch && num < 20f)
				{
					if (num < 1f)
					{
						bird.Perch(bird._destination);
						goto IL_0564;
					}
					num4 = 0.6f * bird.flySpeed.min;
					num5 = 0.5f;
					bird._velocity *= TimeX.Damping(0.98f);
				}
				Vector3 vector6 = num4 * vector3.normalized;
				bird._velocity = Vector3.SmoothDamp(bird._velocity, vector6, ref bird._acceleration, num5, float.MaxValue, dt);
				bool flag4 = false;
				if (bird._takeOffAnim == null || !bird.animator.IsAnimation(bird._takeOffAnim, null, null, null))
				{
					bird.animator.SetAnimation(flag4 ? bird._glideAnim : bird._flyAnim, 0, FrameAnimator.PosMatch.None);
				}
				vector2 += bird._velocity * dt;
				bird.transform.position = vector2;
				bird.transform.localScale = bird.size * new Vector3((float)((bird._velocity.x < 0f) ? (-1) : 1), 1f, 1f);
				if (bird._tookOffDueToAlarm && bird.stateTimer > 0.2f && bird.stateTimer < 1f && Bird._takeoffAlarmBirds.Count < Bird._takeoffAlarmBirds.Capacity)
				{
					Bird._takeoffAlarmBirds.Add(bird);
				}
				if (bird.stateTimer > bird.scheduledFlyPerchChangeTime && !bird._destinationIsPerch)
				{
					if (perchableSlopeList != null && perchableSlopeList.Count > 0)
					{
						Slope slope = perchableSlopeList.Random<Slope>();
						float num6 = slope.range.Random();
						bird._destination = slope.SampleAt(num6, false).point3d;
						bird._hasDestination = true;
						bird._destinationIsPerch = true;
					}
					else if (originalPerchingPoints != null && i < originalPerchingPoints.Count)
					{
						bird._destination = originalPerchingPoints[i];
						bird._hasDestination = true;
						bird._destinationIsPerch = true;
					}
				}
			}
			else if (bird.state == Bird.State.Perching)
			{
				if (flag || bird.fliesAtFirstSign)
				{
					Vector3 position = bird.transform.position;
					if (range.Contains(position.z))
					{
						float num7 = Vector2.Distance(position, vector);
						float num8 = (flag2 ? bird.stoneAlarmRadius : bird.playerAlarmRadius);
						float num9 = Random.Range(0.7f, 1.4f);
						if ((num7 < num8 * num9 && !flag3) || num7 < bird.playerSuperAlarmRadius * num9)
						{
							bird.TakeOff(true);
							Bird.lastTakeOffTime = Time.time;
							flag3 = true;
							if (bird.alarmSound != SoundEffect.None)
							{
								AudioController.instance.PlayWorldSound(bird.alarmSound, bird.transform.position, 2);
							}
							if (!bird.hasInkTrigger)
							{
								goto IL_0564;
							}
							Prop componentInParent = bird.GetComponentInParent<Prop>();
							if (componentInParent == null)
							{
								Debug.LogError("Bird " + bird.name + " was marked as 'hasInkTrigger' but didn't have a Prop parent", bird);
								goto IL_0564;
							}
							Narrative.instance.PlayerAlarmedBird(componentInParent.inkListItemName);
							goto IL_0564;
						}
					}
				}
				if (bird.stateTimer > bird.scheduledFlyPerchChangeTime)
				{
					bird.TakeOff(false);
				}
			}
			IL_0564:;
		}
		if (!flag3)
		{
			foreach (Bird bird2 in birds)
			{
				if (bird2.state == Bird.State.Perching)
				{
					Vector3 position2 = bird2.transform.position;
					foreach (Bird bird3 in Bird._takeoffAlarmBirds)
					{
						if (Vector2.Distance(position2, bird3.transform.position) < bird2.otherBirdAlarmRadius)
						{
							bird2.TakeOff(true);
							Bird.lastTakeOffTime = Time.time;
							return;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x000618DC File Offset: 0x0005FADC
	public static void FindPerchableSlopes(Vector3 position, Vector3 regionSize, List<Slope> slopeListOut)
	{
		WorldManager.instance.GetLevelAtDepth(position.z).slopes.Nearby(position, Range.Centered(position.z, regionSize.z), 0.8f * regionSize.x, slopeListOut);
		Range perchYRange = Range.Centered(position.y, regionSize.y);
		slopeListOut.RemoveAll(delegate(Slope slope)
		{
			Range range = new Range(slope.bounds.min.y, slope.bounds.max.y);
			return !perchYRange.Intersects(range);
		});
	}

	// Token: 0x04000E73 RID: 3699
	public Range flySpeed = new Range(6f, 20f);

	// Token: 0x04000E74 RID: 3700
	public float size = 1f;

	// Token: 0x04000E75 RID: 3701
	public float playerAlarmRadius = 20f;

	// Token: 0x04000E76 RID: 3702
	public float stoneAlarmRadius = 10f;

	// Token: 0x04000E77 RID: 3703
	public float otherBirdAlarmRadius = 10f;

	// Token: 0x04000E78 RID: 3704
	public float playerSuperAlarmRadius = 5f;

	// Token: 0x04000E79 RID: 3705
	[Range(0f, 1f)]
	public float alreadyFlyingProbability = 0.5f;

	// Token: 0x04000E7A RID: 3706
	public bool fliesAtFirstSign;

	// Token: 0x04000E7B RID: 3707
	[Info("Approx ranges between 50% and 150% of this number. -1 means never stop")]
	public float flyTimeApprox = 60f;

	// Token: 0x04000E7C RID: 3708
	public float perchTimeApprox = 60f;

	// Token: 0x04000E7D RID: 3709
	public SoundEffect alarmSound;

	// Token: 0x04000E7E RID: 3710
	public bool hasInkTrigger;

	// Token: 0x04000E7F RID: 3711
	[Disable]
	public Bird.State state;

	// Token: 0x04000E80 RID: 3712
	[Disable]
	public float stateTimer;

	// Token: 0x04000E81 RID: 3713
	[Disable]
	public float scheduledFlyPerchChangeTime = float.MaxValue;

	// Token: 0x04000E82 RID: 3714
	private FrameAnimator _animator;

	// Token: 0x04000E83 RID: 3715
	private Prototype _prototype;

	// Token: 0x04000E84 RID: 3716
	private Vector3 _destination;

	// Token: 0x04000E85 RID: 3717
	private bool _hasDestination;

	// Token: 0x04000E86 RID: 3718
	private bool _destinationIsPerch;

	// Token: 0x04000E87 RID: 3719
	private Vector3 _velocity;

	// Token: 0x04000E88 RID: 3720
	private Vector3 _acceleration;

	// Token: 0x04000E89 RID: 3721
	private bool _tookOffDueToAlarm;

	// Token: 0x04000E8A RID: 3722
	private static List<Bird> _takeoffAlarmBirds = new List<Bird>(4);

	// Token: 0x04000E8B RID: 3723
	[SerializeField]
	private FrameAnimation _perchAnim;

	// Token: 0x04000E8C RID: 3724
	[SerializeField]
	private FrameAnimation _takeOffAnim;

	// Token: 0x04000E8D RID: 3725
	[SerializeField]
	private FrameAnimation _flyAnim;

	// Token: 0x04000E8E RID: 3726
	[SerializeField]
	private FrameAnimation _glideAnim;

	// Token: 0x04000E8F RID: 3727
	public static float lastTakeOffTime;

	// Token: 0x0200039C RID: 924
	public enum State
	{
		// Token: 0x0400196F RID: 6511
		None,
		// Token: 0x04001970 RID: 6512
		Perching,
		// Token: 0x04001971 RID: 6513
		Flying
	}
}
