using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000171 RID: 369
[ExecuteInEditMode]
public class CameraVolume : MonoInstancer<CameraVolume>
{
	// Token: 0x06000C45 RID: 3141 RVA: 0x000623B0 File Offset: 0x000605B0
	public static CameraVolume WithName(string name)
	{
		CameraVolume cameraVolume = null;
		CameraVolume._byName.TryGetValue(name, out cameraVolume);
		return cameraVolume;
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x000623D0 File Offset: 0x000605D0
	public void UpdateActiveStrength(Vector3 focus, bool immediate, bool disableThisCamera = false)
	{
		Vector3 position = base.transform.position;
		float num;
		if ((this.inkDrivenOnly && !this.inkHasEnabled) || Mathf.Abs(position.z - focus.z) > this.depth || !base.enabled || !base.gameObject.activeInHierarchy || disableThisCamera)
		{
			num = 0f;
		}
		else
		{
			float num2 = Vector2.Distance(focus, position);
			num = 1f - this.radius.InverseLerp(num2);
		}
		if (immediate)
		{
			this.activeStrength = num;
			this.activeStrengthSpeed = 0f;
			return;
		}
		float num3 = ((num > this.activeStrength) ? this.activeStrengthBlendInTime : this.activeStrengthBlendOutTime);
		if (Mathf.Abs(num - this.activeStrength) < 0.01f)
		{
			this.activeStrength = num;
			return;
		}
		this.activeStrength = Mathf.SmoothDamp(this.activeStrength, num, ref this.activeStrengthSpeed, num3, float.PositiveInfinity, Time.deltaTime);
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x000624CC File Offset: 0x000606CC
	public void ApplyTo(Vector3 originalTargetPos, ref HighlandCameraProperties propertiesToModify, float inputStrength)
	{
		float num = Mathf.SmoothStep(0f, 1f, this.activeStrength * this.strength * inputStrength);
		if (num == 0f)
		{
			return;
		}
		Vector3 vector = originalTargetPos;
		if (this.target != null)
		{
			if (this.target == Runner.instance.transform)
			{
				vector = Runner.instance.focus;
			}
			else
			{
				vector = this.target.position;
			}
		}
		propertiesToModify.targetPoint = Vector3.Lerp(propertiesToModify.targetPoint, vector + this.targetOffset, num);
		if (this.distanceMode == HighlandCameraPropertiesModifier.Mode.Additive)
		{
			propertiesToModify.distance += this.distance * num;
		}
		else if (this.distanceMode == HighlandCameraPropertiesModifier.Mode.Multiply)
		{
			propertiesToModify.distance = Mathf.Lerp(propertiesToModify.distance, propertiesToModify.distance * this.distance, num);
		}
		else if (this.distanceMode == HighlandCameraPropertiesModifier.Mode.Override)
		{
			propertiesToModify.distance = Mathf.Lerp(propertiesToModify.distance, this.distance, num);
		}
		if (this.shearMode == HighlandCameraPropertiesModifier.Mode.Additive)
		{
			propertiesToModify.shearFactor += this.shear * num;
			return;
		}
		if (this.shearMode == HighlandCameraPropertiesModifier.Mode.Multiply)
		{
			propertiesToModify.shearFactor *= this.shear * num;
			return;
		}
		if (this.shearMode == HighlandCameraPropertiesModifier.Mode.Override)
		{
			propertiesToModify.shearFactor = Mathf.Lerp(propertiesToModify.shearFactor, this.shear, num);
		}
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x00062625 File Offset: 0x00060825
	protected override void OnEnable()
	{
		base.OnEnable();
		CameraVolume._byName[base.name] = this;
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x0006263E File Offset: 0x0006083E
	protected override void OnDisable()
	{
		base.OnDisable();
		CameraVolume._byName.Remove(base.name);
	}

	// Token: 0x04000EA9 RID: 3753
	public Range radius = new Range(3f, 10f);

	// Token: 0x04000EAA RID: 3754
	public float depth = 5f;

	// Token: 0x04000EAB RID: 3755
	public int priority;

	// Token: 0x04000EAC RID: 3756
	public bool inkDrivenOnly;

	// Token: 0x04000EAD RID: 3757
	public bool persistAfterInkCompletes;

	// Token: 0x04000EAE RID: 3758
	[NonSerialized]
	public bool inkHasEnabled;

	// Token: 0x04000EAF RID: 3759
	public float strength = 1f;

	// Token: 0x04000EB0 RID: 3760
	[Space]
	public Transform target;

	// Token: 0x04000EB1 RID: 3761
	public Vector2 targetOffset;

	// Token: 0x04000EB2 RID: 3762
	[Space]
	public HighlandCameraPropertiesModifier.Mode distanceMode;

	// Token: 0x04000EB3 RID: 3763
	[FormerlySerializedAs("zoom")]
	public float distance = 1f;

	// Token: 0x04000EB4 RID: 3764
	[Space]
	public HighlandCameraPropertiesModifier.Mode shearMode;

	// Token: 0x04000EB5 RID: 3765
	public float shear;

	// Token: 0x04000EB6 RID: 3766
	public float activeStrengthBlendInTime = 2f;

	// Token: 0x04000EB7 RID: 3767
	public float activeStrengthBlendOutTime = 2f;

	// Token: 0x04000EB8 RID: 3768
	[NonSerialized]
	public float activeStrength;

	// Token: 0x04000EB9 RID: 3769
	[NonSerialized]
	public float activeStrengthSpeed;

	// Token: 0x04000EBA RID: 3770
	private static Dictionary<string, CameraVolume> _byName = new Dictionary<string, CameraVolume>(StringComparer.OrdinalIgnoreCase);
}
