using System;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class StateFuncMachine : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060007E8 RID: 2024 RVA: 0x0004621C File Offset: 0x0004441C
	// (remove) Token: 0x060007E9 RID: 2025 RVA: 0x00046254 File Offset: 0x00044454
	public event Action<StateFuncMachine.StateFunc> onStateChanged;

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x060007EA RID: 2026 RVA: 0x00046289 File Offset: 0x00044489
	// (set) Token: 0x060007EB RID: 2027 RVA: 0x00046291 File Offset: 0x00044491
	public float stateTimer { get; protected set; }

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x060007EC RID: 2028 RVA: 0x0004629A File Offset: 0x0004449A
	// (set) Token: 0x060007ED RID: 2029 RVA: 0x000462A2 File Offset: 0x000444A2
	public float prevStateTimer { get; protected set; }

	// Token: 0x060007EE RID: 2030 RVA: 0x000462AB File Offset: 0x000444AB
	public bool StateTimeJustPassed(float t)
	{
		return this.stateTimer >= t && this.prevStateTimer < t;
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x060007EF RID: 2031 RVA: 0x000462C1 File Offset: 0x000444C1
	// (set) Token: 0x060007F0 RID: 2032 RVA: 0x000462CC File Offset: 0x000444CC
	public StateFuncMachine.StateFunc state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state != value)
			{
				if (this._state != null)
				{
					this._state(false, true);
				}
				this.prevPrevState = this.prevState;
				this.prevState = this._state;
				this._state = value;
				this.prevStateTimer = -0.033333335f;
				this.stateTimer = 0f;
				if (this.onStateChanged != null)
				{
					this.onStateChanged(this._state);
				}
				if (this._state != null)
				{
					this._state(true, false);
				}
			}
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0004635F File Offset: 0x0004455F
	// (set) Token: 0x060007F2 RID: 2034 RVA: 0x00046367 File Offset: 0x00044567
	public StateFuncMachine.StateFunc prevState { get; protected set; }

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x060007F3 RID: 2035 RVA: 0x00046370 File Offset: 0x00044570
	// (set) Token: 0x060007F4 RID: 2036 RVA: 0x00046378 File Offset: 0x00044578
	public StateFuncMachine.StateFunc prevPrevState { get; protected set; }

	// Token: 0x060007F5 RID: 2037 RVA: 0x00046381 File Offset: 0x00044581
	public void UpdateStateMachine(float dt)
	{
		this.stateTimer += dt;
		if (this.state != null)
		{
			this.state(false, false);
			this.prevStateTimer = this.stateTimer;
		}
	}

	// Token: 0x040009D9 RID: 2521
	[Disable]
	public string stateName;

	// Token: 0x040009DA RID: 2522
	private StateFuncMachine.StateFunc _state;

	// Token: 0x02000319 RID: 793
	// (Invoke) Token: 0x06001687 RID: 5767
	public delegate void StateFunc(bool start, bool end);
}
