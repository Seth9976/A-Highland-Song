using System;
using UnityEngine;

// Token: 0x020001E5 RID: 485
[Serializable]
public class CameraPropertiesModifier
{
	// Token: 0x06001122 RID: 4386 RVA: 0x0007EB53 File Offset: 0x0007CD53
	public CameraPropertiesModifier()
	{
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x0007EB6A File Offset: 0x0007CD6A
	public CameraPropertiesModifier(CameraPropertiesModifier toClone)
	{
		this.CopyFrom(toClone);
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x0007EB88 File Offset: 0x0007CD88
	public void CopyFrom(CameraPropertiesModifier toClone)
	{
		this.modifiers = toClone.modifiers;
		this.mode = toClone.mode;
		this.properties = toClone.properties;
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x0007EBB0 File Offset: 0x0007CDB0
	public void ModifyWithStrength(ref CameraProperties propertiesToModify, float strength)
	{
		if (strength == 0f && this.mode != CameraPropertiesModifier.Mode.Multiply)
		{
			return;
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.TargetPoint))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.targetPoint += this.properties.targetPoint * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.targetPoint = Vector3.Scale(propertiesToModify.targetPoint, this.properties.targetPoint * strength);
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.targetPoint = Vector3.Lerp(propertiesToModify.targetPoint, this.properties.targetPoint, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.Axis))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.axis *= Quaternion.Euler(this.properties.targetPoint * strength);
			}
			else if (this.mode != CameraPropertiesModifier.Mode.Multiply && this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.axis = Quaternion.Slerp(propertiesToModify.axis, this.properties.axis, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.Distance))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.distance += this.properties.distance * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.distance *= this.properties.distance * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.distance = Mathf.Lerp(propertiesToModify.distance, this.properties.distance, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.WorldPitch))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.worldEulerAngles.x = propertiesToModify.worldEulerAngles.x + this.properties.worldEulerAngles.x * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.worldEulerAngles.x = propertiesToModify.worldEulerAngles.x * (this.properties.worldEulerAngles.x * strength);
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.worldEulerAngles.x = Mathf.LerpAngle(propertiesToModify.worldEulerAngles.x, this.properties.worldEulerAngles.x, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.WorldYaw))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.worldEulerAngles.y = propertiesToModify.worldEulerAngles.y + this.properties.worldEulerAngles.y * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.worldEulerAngles.y = propertiesToModify.worldEulerAngles.y * (this.properties.worldEulerAngles.y * strength);
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.worldEulerAngles.y = Mathf.LerpAngle(propertiesToModify.worldEulerAngles.y, this.properties.worldEulerAngles.y, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.LocalPitch))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.localEulerAngles.x = propertiesToModify.localEulerAngles.x + this.properties.localEulerAngles.x * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.localEulerAngles.x = propertiesToModify.localEulerAngles.x * (this.properties.localEulerAngles.x * strength);
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.localEulerAngles.x = Mathf.LerpAngle(propertiesToModify.localEulerAngles.x, this.properties.localEulerAngles.x, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.LocalYaw))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.localEulerAngles.y = propertiesToModify.localEulerAngles.y + this.properties.localEulerAngles.y * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.localEulerAngles.y = propertiesToModify.localEulerAngles.y * (this.properties.localEulerAngles.y * strength);
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.localEulerAngles.y = Mathf.LerpAngle(propertiesToModify.localEulerAngles.y, this.properties.localEulerAngles.y, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.LocalRoll))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.localEulerAngles.z = propertiesToModify.localEulerAngles.z + this.properties.localEulerAngles.z * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.localEulerAngles.z = propertiesToModify.localEulerAngles.z * (this.properties.localEulerAngles.z * strength);
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.localEulerAngles.z = Mathf.Lerp(propertiesToModify.localEulerAngles.z, this.properties.localEulerAngles.z, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.HorizontalViewportOffset))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.viewportOffset.x = propertiesToModify.viewportOffset.x + this.properties.viewportOffset.x * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.viewportOffset.x = propertiesToModify.viewportOffset.x * (this.properties.viewportOffset.x * strength);
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.viewportOffset.x = Mathf.Lerp(propertiesToModify.viewportOffset.x, this.properties.viewportOffset.x, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.VerticalViewportOffset))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.viewportOffset.y = propertiesToModify.viewportOffset.y + this.properties.viewportOffset.y * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.viewportOffset.y = propertiesToModify.viewportOffset.y * (this.properties.viewportOffset.y * strength);
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.viewportOffset.y = Mathf.Lerp(propertiesToModify.viewportOffset.y, this.properties.viewportOffset.y, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.FieldOfView))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.fieldOfView += this.properties.fieldOfView * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.fieldOfView *= this.properties.fieldOfView * strength;
			}
			else if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.fieldOfView = Mathf.Lerp(propertiesToModify.fieldOfView, this.properties.fieldOfView, strength);
			}
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.Orthographic))
		{
			propertiesToModify.orthographic = this.properties.orthographic;
		}
		if (this.modifiers.HasFlag(CameraProperties.CameraPropertiesAxis.OrthographicSize))
		{
			if (this.mode == CameraPropertiesModifier.Mode.Additive)
			{
				propertiesToModify.orthographicSize += this.properties.orthographicSize * strength;
				return;
			}
			if (this.mode == CameraPropertiesModifier.Mode.Multiply)
			{
				propertiesToModify.orthographicSize *= this.properties.orthographicSize * strength;
				return;
			}
			if (this.mode == CameraPropertiesModifier.Mode.Override)
			{
				propertiesToModify.orthographicSize = Mathf.Lerp(propertiesToModify.orthographicSize, this.properties.orthographicSize, strength);
			}
		}
	}

	// Token: 0x04001260 RID: 4704
	[EnumFlag]
	public CameraProperties.CameraPropertiesAxis modifiers = CameraProperties.CameraPropertiesAxis.WorldYaw;

	// Token: 0x04001261 RID: 4705
	public CameraPropertiesModifier.Mode mode = CameraPropertiesModifier.Mode.Override;

	// Token: 0x04001262 RID: 4706
	public CameraProperties properties;

	// Token: 0x020003FA RID: 1018
	public enum Mode
	{
		// Token: 0x04001AB8 RID: 6840
		Additive,
		// Token: 0x04001AB9 RID: 6841
		Multiply,
		// Token: 0x04001ABA RID: 6842
		Override
	}
}
