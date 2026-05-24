using System;
using InControl;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class InputVibrationTest : MonoSingleton<InputVibrationTest>
{
	// Token: 0x06000265 RID: 613 RVA: 0x00014E4E File Offset: 0x0001304E
	private void OnEnable()
	{
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00014E50 File Offset: 0x00013050
	private void OnDisable()
	{
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00014E52 File Offset: 0x00013052
	private VibrationMoment OnRequestVibration()
	{
		if (InputManager.ActiveDevice.Action2.IsPressed)
		{
			return VibrationMoment.Create(this.largeMotor, this.largeMotor);
		}
		return VibrationMoment.zero;
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00014E7C File Offset: 0x0001307C
	private void Update()
	{
		this.smallMotor += InputManager.ActiveDevice.LeftStick.X * Time.deltaTime * 0.5f;
		this.largeMotor += InputManager.ActiveDevice.RightStick.X * Time.deltaTime * 0.5f;
		this.triggerTime += InputManager.ActiveDevice.DPad.X * Time.deltaTime * 0.5f;
		this.smallMotor = Mathf.Clamp01(this.smallMotor);
		this.largeMotor = Mathf.Clamp01(this.largeMotor);
		this.triggerTime = Mathf.Clamp01(this.triggerTime);
		if (InputManager.ActiveDevice.Action1.WasPressed)
		{
			InputVibration.DoTimedVibration(TimedVibration.VibrateForSeconds(this.largeMotor, this.triggerTime));
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00014F5C File Offset: 0x0001315C
	private void OnGUI()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(new GUIContent("Large Motor"), new GUILayoutOption[] { GUILayout.Width(100f) });
		this.largeMotor = GUILayout.HorizontalSlider(this.largeMotor, 0f, 1f, new GUILayoutOption[] { GUILayout.Width(200f) });
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(new GUIContent("Small Motor"), new GUILayoutOption[] { GUILayout.Width(100f) });
		this.smallMotor = GUILayout.HorizontalSlider(this.smallMotor, 0f, 1f, new GUILayoutOption[] { GUILayout.Width(200f) });
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(new GUIContent("Trigger Time"), new GUILayoutOption[] { GUILayout.Width(100f) });
		this.triggerTime = GUILayout.HorizontalSlider(this.triggerTime, 0f, 1f, new GUILayoutOption[] { GUILayout.Width(200f) });
		GUILayout.EndHorizontal();
	}

	// Token: 0x04000393 RID: 915
	[Range(0f, 1f)]
	public float largeMotor;

	// Token: 0x04000394 RID: 916
	[Range(0f, 1f)]
	public float smallMotor;

	// Token: 0x04000395 RID: 917
	public float triggerTime;
}
