using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000BA RID: 186
public class HealthSettings : ScriptableObject
{
	// Token: 0x040006C2 RID: 1730
	public float minimumMaxHealth = 0.8f;

	// Token: 0x040006C3 RID: 1731
	[Space]
	public float riskChunkChangeSpeed = 1f;

	// Token: 0x040006C4 RID: 1732
	[Space]
	[Header("OvernightEffects")]
	[FormerlySerializedAs("noShelterAtAll")]
	public HealthEffect noShelterAtAll;

	// Token: 0x040006C5 RID: 1733
	[FormerlySerializedAs("veryUncomfortableBothy")]
	public HealthEffect veryUncomfortableShelter;

	// Token: 0x040006C6 RID: 1734
	[FormerlySerializedAs("UncomfortableBothy")]
	public HealthEffect uncomfortableShelter;

	// Token: 0x040006C7 RID: 1735
	[FormerlySerializedAs("comfortableBothy")]
	public HealthEffect comfortableShelter;

	// Token: 0x040006C8 RID: 1736
	[FormerlySerializedAs("veryComfortableBothy")]
	public HealthEffect veryComfortableShelter;

	// Token: 0x040006C9 RID: 1737
	[Header("Temperature Health Effects")]
	[FormerlySerializedAs("badTemperature")]
	public HealthEffect freezing;

	// Token: 0x040006CA RID: 1738
	[FormerlySerializedAs("fineTemperature")]
	public HealthEffect cold;

	// Token: 0x040006CB RID: 1739
	public HealthEffect chilly;

	// Token: 0x040006CC RID: 1740
	[FormerlySerializedAs("goodTemperature")]
	public HealthEffect okay;

	// Token: 0x040006CD RID: 1741
	[Header("Weather Health Effects")]
	public HealthEffect rain;

	// Token: 0x040006CE RID: 1742
	public HealthEffect storm;

	// Token: 0x040006CF RID: 1743
	public HealthEffect snow;

	// Token: 0x040006D0 RID: 1744
	public HealthEffect strongWind;

	// Token: 0x040006D1 RID: 1745
	public HealthEffect moderateWind;

	// Token: 0x040006D2 RID: 1746
	public float weatherHurtDelayTime = 10f;

	// Token: 0x040006D3 RID: 1747
	public float weatherRecoverTimeDaysNorm = 0.041666668f;

	// Token: 0x040006D4 RID: 1748
	public float badWeatherDamageScalar = 1.2f;

	// Token: 0x040006D5 RID: 1749
	public float harshWeatherDamageScalar = 1.8f;

	// Token: 0x040006D6 RID: 1750
	public float verySlowHealingScalar = 0.4f;

	// Token: 0x040006D7 RID: 1751
	public float slowHealingScalar = 0.6f;

	// Token: 0x040006D8 RID: 1752
	public float mediumHealingScalar = 0.85f;

	// Token: 0x040006D9 RID: 1753
	[Header("Night Health Effects")]
	public HealthEffect night;

	// Token: 0x040006DA RID: 1754
	[Header("Activity Health Effects")]
	public float musicRunningMaxHealthBoost;

	// Token: 0x040006DB RID: 1755
	public float musicRunningHealthBoost;

	// Token: 0x040006DC RID: 1756
	[Header("Resting Health Effects")]
	public HealthEffect restingHealthEffect;

	// Token: 0x040006DD RID: 1757
	public float restingMaxHealthScalar = 0.5f;

	// Token: 0x040006DE RID: 1758
	public float restingHealthBoost = 100f;

	// Token: 0x040006DF RID: 1759
	public float restingMinimumHealthGainSpeed = 20f;

	// Token: 0x040006E0 RID: 1760
	[Header("Water Health Effects")]
	public HealthEffect waterHealthEffect;

	// Token: 0x040006E1 RID: 1761
	[Header("Blessed Peak Effects")]
	public HealthEffect maxHealthRefreshEffectInBlessedPeakWeatherModifierZone;

	// Token: 0x040006E2 RID: 1762
	[Space]
	public float fatigueMaxHealthEffect = 1f;

	// Token: 0x040006E3 RID: 1763
	[Header("Hammer Fall Rate Modifiers")]
	public float defaultHammerFallsPerHour = 1.5f;

	// Token: 0x040006E4 RID: 1764
	public float strongWindHammerRateMod = 1f;

	// Token: 0x040006E5 RID: 1765
	public float freezingTempHammerRateMod = 1f;

	// Token: 0x040006E6 RID: 1766
	public float stormHammerRateMod = 1f;

	// Token: 0x040006E7 RID: 1767
	public float snowHammerRateMod = 1f;

	// Token: 0x040006E8 RID: 1768
	public float windHammerRateMod = 0.5f;

	// Token: 0x040006E9 RID: 1769
	public float rainHammerRateMod = 0.5f;

	// Token: 0x040006EA RID: 1770
	public float nightHammerRateMod = 0.5f;

	// Token: 0x040006EB RID: 1771
	[Header("Difficulty based modifiers")]
	public float mildDifficultyScalar = 0.3f;

	// Token: 0x040006EC RID: 1772
	public float harshDifficultyScalar = 1.5f;
}
