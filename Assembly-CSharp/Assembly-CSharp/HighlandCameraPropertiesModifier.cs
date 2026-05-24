using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
[Serializable]
public class HighlandCameraPropertiesModifier
{
	// Token: 0x06000141 RID: 321 RVA: 0x0000D033 File Offset: 0x0000B233
	public HighlandCameraPropertiesModifier()
	{
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000D049 File Offset: 0x0000B249
	public HighlandCameraPropertiesModifier(HighlandCameraPropertiesModifier toClone)
	{
		this.CopyFrom(toClone);
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000D066 File Offset: 0x0000B266
	public void CopyFrom(HighlandCameraPropertiesModifier toClone)
	{
		this.modifiers = toClone.modifiers;
		this.mode = toClone.mode;
		this.properties = toClone.properties;
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0000D08C File Offset: 0x0000B28C
	public void ModifyWithStrength(ref HighlandCameraProperties propertiesToModify, float strength)
	{
		if (strength == 0f && this.mode != HighlandCameraPropertiesModifier.Mode.Multiply)
		{
			return;
		}
		if (this.modifiers.HasFlag(HighlandCameraProperties.HighlandCameraPropertiesAxis.TargetPoint))
		{
			if (this.mode == HighlandCameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.targetPoint += this.properties.targetPoint * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.targetPoint = Vector3.Scale(propertiesToModify.targetPoint, this.properties.targetPoint * strength);
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.targetPoint = Vector3.Lerp(propertiesToModify.targetPoint, this.properties.targetPoint, strength);
			}
		}
		if (this.modifiers.HasFlag(HighlandCameraProperties.HighlandCameraPropertiesAxis.Distance))
		{
			if (this.mode == HighlandCameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.distance += this.properties.distance * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.distance *= this.properties.distance * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.distance = Mathf.Lerp(propertiesToModify.distance, this.properties.distance, strength);
			}
		}
		if (this.modifiers.HasFlag(HighlandCameraProperties.HighlandCameraPropertiesAxis.FieldOfView))
		{
			if (this.mode == HighlandCameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.fieldOfView += this.properties.fieldOfView * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.fieldOfView *= this.properties.fieldOfView * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.fieldOfView = Mathf.Lerp(propertiesToModify.fieldOfView, this.properties.fieldOfView, strength);
			}
		}
		if (this.modifiers.HasFlag(HighlandCameraProperties.HighlandCameraPropertiesAxis.ClipPlaneSplitPoint))
		{
			if (this.mode == HighlandCameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.clipPlaneSplitPoint += this.properties.clipPlaneSplitPoint * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.clipPlaneSplitPoint *= this.properties.clipPlaneSplitPoint * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.clipPlaneSplitPoint = Mathf.Lerp(propertiesToModify.clipPlaneSplitPoint, this.properties.clipPlaneSplitPoint, strength);
			}
		}
		if (this.modifiers.HasFlag(HighlandCameraProperties.HighlandCameraPropertiesAxis.DepthOfFieldStrength))
		{
			if (this.mode == HighlandCameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.depthOfFieldStrength += this.properties.depthOfFieldStrength * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.depthOfFieldStrength *= this.properties.depthOfFieldStrength * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.depthOfFieldStrength = Mathf.Lerp(propertiesToModify.depthOfFieldStrength, this.properties.depthOfFieldStrength, strength);
			}
		}
		if (this.modifiers.HasFlag(HighlandCameraProperties.HighlandCameraPropertiesAxis.ShearFactor))
		{
			if (this.mode == HighlandCameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.shearFactor += this.properties.shearFactor * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.shearFactor *= this.properties.shearFactor * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.shearFactor = Mathf.Lerp(propertiesToModify.shearFactor, this.properties.shearFactor, strength);
			}
		}
		if (this.modifiers.HasFlag(HighlandCameraProperties.HighlandCameraPropertiesAxis.Viewport))
		{
			if (this.mode == HighlandCameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.viewport += this.properties.viewport * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.viewport *= this.properties.viewport * strength;
			}
			else if (this.mode == HighlandCameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.viewport = Vector2.Lerp(propertiesToModify.viewport, this.properties.viewport, strength);
			}
		}
		if (this.modifiers.HasFlag(HighlandCameraProperties.HighlandCameraPropertiesAxis.ViewportScale))
		{
			if (this.mode == HighlandCameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.viewportScale += this.properties.viewportScale * strength;
				return;
			}
			if (this.mode == HighlandCameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.viewportScale *= this.properties.viewportScale * strength;
				return;
			}
			if (this.mode == HighlandCameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.viewportScale = Mathf.Lerp(propertiesToModify.viewportScale, this.properties.viewportScale, strength);
			}
		}
	}

	// Token: 0x0400020F RID: 527
	[EnumFlag]
	public HighlandCameraProperties.HighlandCameraPropertiesAxis modifiers = HighlandCameraProperties.HighlandCameraPropertiesAxis.Distance;

	// Token: 0x04000210 RID: 528
	public HighlandCameraPropertiesModifier.Mode mode = HighlandCameraPropertiesModifier.Mode.Override;

	// Token: 0x04000211 RID: 529
	public HighlandCameraProperties properties;

	// Token: 0x0200026C RID: 620
	public enum Mode
	{
		// Token: 0x040014A0 RID: 5280
		Ignore,
		// Token: 0x040014A1 RID: 5281
		Additive,
		// Token: 0x040014A2 RID: 5282
		Multiply,
		// Token: 0x040014A3 RID: 5283
		Override
	}
}
