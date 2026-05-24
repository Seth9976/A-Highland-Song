using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class InputVibration : MonoSingleton<InputVibration>
{
	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000252 RID: 594 RVA: 0x00014A87 File Offset: 0x00012C87
	// (set) Token: 0x06000253 RID: 595 RVA: 0x00014AA8 File Offset: 0x00012CA8
	public static bool vibrationEnabled
	{
		get
		{
			if (!InputVibration._vibrationEnabledCached)
			{
				InputVibration._vibrationEnabled = PlayerPrefsX.GetInt(InputVibration.vibrationPrefName, 1) != 0;
			}
			return InputVibration._vibrationEnabled;
		}
		set
		{
			InputVibration._vibrationEnabled = value;
			PlayerPrefsX.SetInt(InputVibration.vibrationPrefName, value ? 1 : 0);
			InputVibration._vibrationEnabledCached = true;
		}
	}

	// Token: 0x06000254 RID: 596 RVA: 0x00014AC7 File Offset: 0x00012CC7
	public static void Small()
	{
		MonoSingleton<InputVibration>.instance._currentSimpleVibrationMoment = VibrationMoment.Create(0.2f);
		MonoSingleton<InputVibration>.instance._simpleVibrationMomentExpiry = Time.unscaledTime + 0.2f;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x00014AF2 File Offset: 0x00012CF2
	public static void Medium()
	{
		MonoSingleton<InputVibration>.instance._currentSimpleVibrationMoment = VibrationMoment.Create(0.5f);
		MonoSingleton<InputVibration>.instance._simpleVibrationMomentExpiry = Time.unscaledTime + 0.3f;
	}

	// Token: 0x06000256 RID: 598 RVA: 0x00014B1D File Offset: 0x00012D1D
	public static void Large()
	{
		MonoSingleton<InputVibration>.instance._currentSimpleVibrationMoment = VibrationMoment.Create(1f);
		MonoSingleton<InputVibration>.instance._simpleVibrationMomentExpiry = Time.unscaledTime + 0.5f;
	}

	// Token: 0x06000257 RID: 599 RVA: 0x00014B48 File Offset: 0x00012D48
	public static void LongSoft()
	{
		MonoSingleton<InputVibration>.instance._currentSimpleVibrationMoment = VibrationMoment.Create(0.2f);
		MonoSingleton<InputVibration>.instance._simpleVibrationMomentExpiry = Time.unscaledTime + 0.5f;
	}

	// Token: 0x06000258 RID: 600 RVA: 0x00014B73 File Offset: 0x00012D73
	public static void LongVerySoft()
	{
		MonoSingleton<InputVibration>.instance._currentSimpleVibrationMoment = VibrationMoment.Create(0.1f);
		MonoSingleton<InputVibration>.instance._simpleVibrationMomentExpiry = Time.unscaledTime + 0.5f;
	}

	// Token: 0x06000259 RID: 601 RVA: 0x00014B9E File Offset: 0x00012D9E
	public static void AddDelegate(InputVibration.GetVibrationDelegate d)
	{
		InputVibration.modifiers.Add(d);
	}

	// Token: 0x0600025A RID: 602 RVA: 0x00014BAB File Offset: 0x00012DAB
	public static void RemoveDelegate(InputVibration.GetVibrationDelegate d)
	{
		InputVibration.modifiers.Remove(d);
	}

	// Token: 0x0600025B RID: 603 RVA: 0x00014BBC File Offset: 0x00012DBC
	public static void DoTimedVibration(BaseTimedVibration vibration)
	{
		InputVibration.TimedVibrationTracker timedVibrationTracker = new InputVibration.TimedVibrationTracker(vibration);
		InputVibration.timedVibrations.Add(timedVibrationTracker);
		timedVibrationTracker.Start();
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00014BE4 File Offset: 0x00012DE4
	public VibrationMoment GetVibrationMoment()
	{
		VibrationMoment vibrationMoment = VibrationMoment.zero;
		if (Game.gameplayPaused || !InputVibration.vibrationEnabled)
		{
			return vibrationMoment;
		}
		foreach (InputVibration.GetVibrationDelegate getVibrationDelegate in InputVibration.modifiers)
		{
			vibrationMoment = VibrationMoment.Max(vibrationMoment, getVibrationDelegate());
		}
		vibrationMoment = VibrationMoment.Max(vibrationMoment, this._currentSimpleVibrationMoment);
		if (vibrationMoment.leftStrength < 0.01f)
		{
			vibrationMoment.leftStrength = 0f;
		}
		if (vibrationMoment.rightStrength < 0.01f)
		{
			vibrationMoment.rightStrength = 0f;
		}
		if (vibrationMoment.smallMotorStrength < 0.01f)
		{
			vibrationMoment.smallMotorStrength = 0f;
		}
		if (vibrationMoment.largeMotorStrength < 0.01f)
		{
			vibrationMoment.largeMotorStrength = 0f;
		}
		return vibrationMoment;
	}

	// Token: 0x0600025D RID: 605 RVA: 0x00014CC4 File Offset: 0x00012EC4
	private void OnDisable()
	{
		this.ClearTimedVibrations();
	}

	// Token: 0x0600025E RID: 606 RVA: 0x00014CCC File Offset: 0x00012ECC
	private void Update()
	{
		this.DoUpdate();
	}

	// Token: 0x0600025F RID: 607 RVA: 0x00014CD4 File Offset: 0x00012ED4
	private void DoUpdate()
	{
		InputDevice activeDevice = InputManager.ActiveDevice;
		for (int i = InputVibration.timedVibrations.Count - 1; i >= 0; i--)
		{
			if (InputVibration.timedVibrations[i].isComplete)
			{
				InputVibration.timedVibrations[i].End();
				InputVibration.timedVibrations.RemoveAt(i);
			}
		}
		if (Time.unscaledTime > this._simpleVibrationMomentExpiry)
		{
			this._currentSimpleVibrationMoment = default(VibrationMoment);
		}
		else
		{
			float deltaTime = Time.deltaTime;
			this._currentSimpleVibrationMoment.rightStrength = Mathf.Clamp01(this._currentSimpleVibrationMoment.rightStrength - deltaTime);
			this._currentSimpleVibrationMoment.leftStrength = Mathf.Clamp01(this._currentSimpleVibrationMoment.leftStrength - deltaTime);
		}
		VibrationMoment vibrationMoment = this.GetVibrationMoment();
		activeDevice.Vibrate(vibrationMoment.leftStrength, vibrationMoment.rightStrength);
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00014DA0 File Offset: 0x00012FA0
	public void ClearTimedVibrations()
	{
		foreach (InputVibration.TimedVibrationTracker timedVibrationTracker in InputVibration.timedVibrations)
		{
			timedVibrationTracker.End();
		}
		InputVibration.timedVibrations.Clear();
	}

	// Token: 0x0400038B RID: 907
	public static bool VibrationEnabled = true;

	// Token: 0x0400038C RID: 908
	public static string vibrationPrefName = "vibrationEnabled";

	// Token: 0x0400038D RID: 909
	private static bool _vibrationEnabled = true;

	// Token: 0x0400038E RID: 910
	private static bool _vibrationEnabledCached;

	// Token: 0x0400038F RID: 911
	private static List<InputVibration.GetVibrationDelegate> modifiers = new List<InputVibration.GetVibrationDelegate>();

	// Token: 0x04000390 RID: 912
	private static List<InputVibration.TimedVibrationTracker> timedVibrations = new List<InputVibration.TimedVibrationTracker>();

	// Token: 0x04000391 RID: 913
	private VibrationMoment _currentSimpleVibrationMoment;

	// Token: 0x04000392 RID: 914
	private float _simpleVibrationMomentExpiry;

	// Token: 0x02000282 RID: 642
	// (Invoke) Token: 0x0600157E RID: 5502
	public delegate VibrationMoment GetVibrationDelegate();

	// Token: 0x02000283 RID: 643
	[Serializable]
	private class TimedVibrationTracker
	{
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001581 RID: 5505 RVA: 0x00093D4C File Offset: 0x00091F4C
		public bool isComplete
		{
			get
			{
				return Time.realtimeSinceStartup - this.startTime > this.vibration.maxTime;
			}
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00093D67 File Offset: 0x00091F67
		public TimedVibrationTracker(BaseTimedVibration vibration)
		{
			this.vibration = vibration;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00093D76 File Offset: 0x00091F76
		public void Start()
		{
			this.startTime = Time.realtimeSinceStartup;
			InputVibration.AddDelegate(new InputVibration.GetVibrationDelegate(this.Evaluate));
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00093D94 File Offset: 0x00091F94
		public void End()
		{
			InputVibration.RemoveDelegate(new InputVibration.GetVibrationDelegate(this.Evaluate));
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x00093DA8 File Offset: 0x00091FA8
		private VibrationMoment Evaluate()
		{
			float num = Time.realtimeSinceStartup - this.startTime;
			return this.vibration.Evaluate(num);
		}

		// Token: 0x040014ED RID: 5357
		public BaseTimedVibration vibration;

		// Token: 0x040014EE RID: 5358
		public float startTime;
	}
}
