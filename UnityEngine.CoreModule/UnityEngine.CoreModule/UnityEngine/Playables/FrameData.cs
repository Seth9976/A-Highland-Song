using System;

namespace UnityEngine.Playables
{
	// Token: 0x0200042E RID: 1070
	public struct FrameData
	{
		// Token: 0x0600254B RID: 9547 RVA: 0x0003EEE0 File Offset: 0x0003D0E0
		private bool HasFlags(FrameData.Flags flag)
		{
			return (this.m_Flags & flag) == flag;
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x0003EF00 File Offset: 0x0003D100
		public ulong frameId
		{
			get
			{
				return this.m_FrameID;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x0600254D RID: 9549 RVA: 0x0003EF18 File Offset: 0x0003D118
		public float deltaTime
		{
			get
			{
				return (float)this.m_DeltaTime;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x0600254E RID: 9550 RVA: 0x0003EF34 File Offset: 0x0003D134
		public float weight
		{
			get
			{
				return this.m_Weight;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x0600254F RID: 9551 RVA: 0x0003EF4C File Offset: 0x0003D14C
		public float effectiveWeight
		{
			get
			{
				return this.m_EffectiveWeight;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002550 RID: 9552 RVA: 0x0003EF64 File Offset: 0x0003D164
		[Obsolete("effectiveParentDelay is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		public double effectiveParentDelay
		{
			get
			{
				return this.m_EffectiveParentDelay;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06002551 RID: 9553 RVA: 0x0003EF7C File Offset: 0x0003D17C
		public float effectiveParentSpeed
		{
			get
			{
				return this.m_EffectiveParentSpeed;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06002552 RID: 9554 RVA: 0x0003EF94 File Offset: 0x0003D194
		public float effectiveSpeed
		{
			get
			{
				return this.m_EffectiveSpeed;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06002553 RID: 9555 RVA: 0x0003EFAC File Offset: 0x0003D1AC
		public FrameData.EvaluationType evaluationType
		{
			get
			{
				return this.HasFlags(FrameData.Flags.Evaluate) ? FrameData.EvaluationType.Evaluate : FrameData.EvaluationType.Playback;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06002554 RID: 9556 RVA: 0x0003EFCC File Offset: 0x0003D1CC
		public bool seekOccurred
		{
			get
			{
				return this.HasFlags(FrameData.Flags.SeekOccured);
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002555 RID: 9557 RVA: 0x0003EFE8 File Offset: 0x0003D1E8
		public bool timeLooped
		{
			get
			{
				return this.HasFlags(FrameData.Flags.Loop);
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002556 RID: 9558 RVA: 0x0003F004 File Offset: 0x0003D204
		public bool timeHeld
		{
			get
			{
				return this.HasFlags(FrameData.Flags.Hold);
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002557 RID: 9559 RVA: 0x0003F020 File Offset: 0x0003D220
		public PlayableOutput output
		{
			get
			{
				return this.m_Output;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002558 RID: 9560 RVA: 0x0003F038 File Offset: 0x0003D238
		public PlayState effectivePlayState
		{
			get
			{
				bool flag = this.HasFlags(FrameData.Flags.EffectivePlayStateDelayed);
				PlayState playState;
				if (flag)
				{
					playState = PlayState.Delayed;
				}
				else
				{
					bool flag2 = this.HasFlags(FrameData.Flags.EffectivePlayStatePlaying);
					if (flag2)
					{
						playState = PlayState.Playing;
					}
					else
					{
						playState = PlayState.Paused;
					}
				}
				return playState;
			}
		}

		// Token: 0x04000DE9 RID: 3561
		internal ulong m_FrameID;

		// Token: 0x04000DEA RID: 3562
		internal double m_DeltaTime;

		// Token: 0x04000DEB RID: 3563
		internal float m_Weight;

		// Token: 0x04000DEC RID: 3564
		internal float m_EffectiveWeight;

		// Token: 0x04000DED RID: 3565
		internal double m_EffectiveParentDelay;

		// Token: 0x04000DEE RID: 3566
		internal float m_EffectiveParentSpeed;

		// Token: 0x04000DEF RID: 3567
		internal float m_EffectiveSpeed;

		// Token: 0x04000DF0 RID: 3568
		internal FrameData.Flags m_Flags;

		// Token: 0x04000DF1 RID: 3569
		internal PlayableOutput m_Output;

		// Token: 0x0200042F RID: 1071
		[Flags]
		internal enum Flags
		{
			// Token: 0x04000DF3 RID: 3571
			Evaluate = 1,
			// Token: 0x04000DF4 RID: 3572
			SeekOccured = 2,
			// Token: 0x04000DF5 RID: 3573
			Loop = 4,
			// Token: 0x04000DF6 RID: 3574
			Hold = 8,
			// Token: 0x04000DF7 RID: 3575
			EffectivePlayStateDelayed = 16,
			// Token: 0x04000DF8 RID: 3576
			EffectivePlayStatePlaying = 32
		}

		// Token: 0x02000430 RID: 1072
		public enum EvaluationType
		{
			// Token: 0x04000DFA RID: 3578
			Evaluate,
			// Token: 0x04000DFB RID: 3579
			Playback
		}
	}
}
