using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class Crow : MonoInstancer<Crow>
{
	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00066212 File Offset: 0x00064412
	private Bird bird
	{
		get
		{
			if (this._bird == null)
			{
				this._bird = base.GetComponent<Bird>();
			}
			return this._bird;
		}
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00066234 File Offset: 0x00064434
	private AudioSource audioSource
	{
		get
		{
			if (this._audioSource == null)
			{
				this._audioSource = base.GetComponent<AudioSource>();
			}
			return this._audioSource;
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00066256 File Offset: 0x00064456
	// (set) Token: 0x06000CB9 RID: 3257 RVA: 0x00066260 File Offset: 0x00064460
	public static bool allFlownAway
	{
		get
		{
			return Crow._flownAway;
		}
		set
		{
			Crow._flownAway = value;
			foreach (Crow crow in MonoInstancer<Crow>.all)
			{
				if (Crow._flownAway)
				{
					Vector3 vector = Vector3.Scale(Random.insideUnitSphere, crow._worldFlyBounds.extents);
					crow.transform.position = crow._worldFlyBounds.center + vector;
					crow.bird.TakeOff(false);
				}
				else
				{
					crow.bird.Perch(crow._originalPerchPos);
					crow.transform.localScale = new Vector3(crow._originalFacingDir, 1f, 1f);
				}
			}
		}
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x00066330 File Offset: 0x00064530
	public static void ResetAll()
	{
		Crow.allFlownAway = false;
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00066338 File Offset: 0x00064538
	private void Awake()
	{
		this._originalPerchPos = base.transform.position;
		this._originalFacingDir = Mathf.Sign(base.transform.localScale.x);
		this._singleBirdList.Add(this.bird);
		this._worldFlyBounds = this.localFlyBounds;
		this._worldFlyBounds.center = this._worldFlyBounds.center + base.transform.position;
		this.bird.Perch();
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x000663BA File Offset: 0x000645BA
	protected override void OnEnable()
	{
		base.OnEnable();
		Narrative.onEventDidFire += this.OnEventDidFire;
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x000663D3 File Offset: 0x000645D3
	protected override void OnDisable()
	{
		base.OnDisable();
		Narrative.onEventDidFire -= this.OnEventDidFire;
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x000663EC File Offset: 0x000645EC
	private void Update()
	{
		if (this.bird.state == Bird.State.Perching && Random.value < this.cawChance)
		{
			this.bird.animator.SetAnimationWithTransition("CrowCaw", "CrowIdle", 0, true, false, FrameAnimator.PosMatch.None);
			if (!this.audioSource.isPlaying)
			{
				this.audioSource.pitch = Random.Range(0.85f, 1.1f);
				this.audioSource.Play();
			}
		}
		Bird.UpdateAll(this._singleBirdList, this._worldFlyBounds, null, null, true, Time.deltaTime);
		if (this.bird.state == Bird.State.Perching && this._countDownToTakeOff > 0f)
		{
			this._countDownToTakeOff -= Time.deltaTime;
			if (this._countDownToTakeOff <= 0f)
			{
				this.bird.TakeOff(true);
			}
		}
		if (this.bird.state == Bird.State.Flying && !Crow._flownAway)
		{
			Crow._flownAway = true;
			Narrative.onEventDidFire -= this.OnEventDidFire;
		}
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x000664F0 File Offset: 0x000646F0
	private void OnEventDidFire(string eventName)
	{
		if (eventName != "CrowsAllFlyAway")
		{
			return;
		}
		Crow._flownAway = true;
		Narrative.onEventDidFire -= this.OnEventDidFire;
		if (this.bird.state == Bird.State.Perching)
		{
			this._countDownToTakeOff = Random.Range(0.1f, 0.6f);
		}
	}

	// Token: 0x04000F7C RID: 3964
	public Bounds localFlyBounds;

	// Token: 0x04000F7D RID: 3965
	public float cawChance = 0.002f;

	// Token: 0x04000F7E RID: 3966
	private Bird _bird;

	// Token: 0x04000F7F RID: 3967
	private AudioSource _audioSource;

	// Token: 0x04000F80 RID: 3968
	private static bool _flownAway;

	// Token: 0x04000F81 RID: 3969
	private Vector3 _originalPerchPos;

	// Token: 0x04000F82 RID: 3970
	private float _originalFacingDir;

	// Token: 0x04000F83 RID: 3971
	private Bounds _worldFlyBounds;

	// Token: 0x04000F84 RID: 3972
	private List<Bird> _singleBirdList = new List<Bird>(1);

	// Token: 0x04000F85 RID: 3973
	private float _countDownToTakeOff;
}
