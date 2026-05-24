using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000208 RID: 520
public class InputTypeEnabler : MonoBehaviour
{
	// Token: 0x0600132E RID: 4910 RVA: 0x00087F44 File Offset: 0x00086144
	private void OnEnable()
	{
		GameInput.onInputTypeChanged = (Action<GameInput.InputType>)Delegate.Combine(GameInput.onInputTypeChanged, new Action<GameInput.InputType>(this.OnChangeInputType));
		this.OnChangeInputType(GameInput.activeInputType);
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x00087F71 File Offset: 0x00086171
	private void OnDisable()
	{
		GameInput.onInputTypeChanged = (Action<GameInput.InputType>)Delegate.Remove(GameInput.onInputTypeChanged, new Action<GameInput.InputType>(this.OnChangeInputType));
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x00087F94 File Offset: 0x00086194
	private void OnChangeInputType(GameInput.InputType newInputType)
	{
		bool flag = newInputType == this.targetInputType;
		foreach (Object @object in this.targets)
		{
			if (@object is GameObject)
			{
				((GameObject)@object).SetActive(flag);
			}
			else if (@object is Behaviour)
			{
				((Behaviour)@object).enabled = flag;
			}
		}
	}

	// Token: 0x040012AA RID: 4778
	public GameInput.InputType targetInputType;

	// Token: 0x040012AB RID: 4779
	public List<Object> targets;
}
