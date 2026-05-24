using System;
using UnityEngine;

// Token: 0x0200014E RID: 334
[Serializable]
public class SpringDamper
{
	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0005B6D0 File Offset: 0x000598D0
	// (set) Token: 0x06000B5A RID: 2906 RVA: 0x0005B6D8 File Offset: 0x000598D8
	public float current
	{
		get
		{
			return this._current;
		}
		set
		{
			this._current = value;
			if (this.OnChangeCurrent != null)
			{
				this.OnChangeCurrent(this.current);
			}
		}
	}

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000B5B RID: 2907 RVA: 0x0005B6FC File Offset: 0x000598FC
	// (remove) Token: 0x06000B5C RID: 2908 RVA: 0x0005B734 File Offset: 0x00059934
	public event Action<float> OnChangeCurrent;

	// Token: 0x06000B5D RID: 2909 RVA: 0x0005B769 File Offset: 0x00059969
	public void AddImpulse(float force)
	{
		this.currentVelocity += force;
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x0005B779 File Offset: 0x00059979
	public void AddForce(float force)
	{
		this.AddForce(force, Time.deltaTime);
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x0005B787 File Offset: 0x00059987
	public void AddForce(float force, float deltaTime)
	{
		this.currentVelocity += force * deltaTime;
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0005B799 File Offset: 0x00059999
	public virtual float Update()
	{
		return this.Update(Time.deltaTime);
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x0005B7A8 File Offset: 0x000599A8
	public virtual float Update(float deltaTime)
	{
		return this.current = SpringDamper.DampedSpring(this.current, this.target, ref this.currentVelocity, this.stiffness, this.damping, deltaTime);
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x0005B7E2 File Offset: 0x000599E2
	public virtual void Reset(float newDefaultValue)
	{
		this.current = newDefaultValue;
		this.currentVelocity = 0f;
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x0005B7F6 File Offset: 0x000599F6
	public override string ToString()
	{
		return string.Format("[SpringDamper] Current={0}, Velocity={1}", this.current, this.currentVelocity);
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0005B818 File Offset: 0x00059A18
	public static float DampedSpring(float current, float target, ref float velocity, float springConstant, float damping)
	{
		return SpringDamper.DampedSpring(current, target, ref velocity, springConstant, damping, Time.deltaTime);
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0005B82C File Offset: 0x00059A2C
	public static float DampedSpring(float current, float target, ref float velocity, float springConstant, float damping, float deltaTime)
	{
		deltaTime = 0.016666668f;
		float num = (target - current) * springConstant;
		float num2 = velocity * -damping;
		float num3 = num + num2;
		velocity += num3 * deltaTime;
		float num4 = velocity * deltaTime;
		return current + num4;
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0005B864 File Offset: 0x00059A64
	public static float CriticallyDampedSpring(float current, float target, ref float velocity, float springConstant)
	{
		float num = 0.016666668f;
		float num2 = (target - current) * springConstant;
		float num3 = -velocity * 2f * Mathf.Sqrt(springConstant);
		float num4 = num2 + num3;
		velocity += num4 * num;
		float num5 = velocity * num;
		return current + num5;
	}

	// Token: 0x04000D79 RID: 3449
	[SerializeField]
	[Tooltip("The current value")]
	private float _current;

	// Token: 0x04000D7A RID: 3450
	public float target;

	// Token: 0x04000D7B RID: 3451
	[Tooltip("The current velocity")]
	public float currentVelocity;

	// Token: 0x04000D7C RID: 3452
	[Tooltip("The rigidity of the spring. A high value makes a more powerful spring.")]
	public float stiffness = 1f;

	// Token: 0x04000D7D RID: 3453
	[Tooltip("The damping of the spring. It affects how quickly the spring comes to a stop.")]
	public float damping = 0.1f;
}
