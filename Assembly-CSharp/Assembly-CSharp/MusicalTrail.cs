using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class MusicalTrail : MonoBehaviour
{
	// Token: 0x060005F7 RID: 1527 RVA: 0x0002EB80 File Offset: 0x0002CD80
	public void CreateSuccessNote(bool nailedIt, bool special)
	{
		SuccessNote successNote = this._successNotePrototype.Instantiate<SuccessNote>(null);
		successNote.transform.SetParent(null, false);
		successNote.age = 0f;
		Vector3 vector = base.transform.position + 5f * base.transform.up;
		successNote.transform.position = vector;
		successNote.transform.localScale = this._settings.successNoteScaleOverTime.Evaluate(0f) * Vector3.one;
		int num = ((base.transform.root.localScale.x >= 0f) ? 1 : (-1));
		successNote.transform.rotation = Quaternion.Euler(0f, 0f, (float)num * -this._settings.successNoteAngle.Random());
		if (special)
		{
			successNote.spriteRenderer.color = (nailedIt ? this._settings.successNoteNailedSpecialColor : this._settings.successNoteSpecialColor);
		}
		else
		{
			successNote.spriteRenderer.color = (nailedIt ? this._settings.successNoteNailedColor : this._settings.successNoteColor);
		}
		this._successNotes.Add(successNote);
		if (nailedIt)
		{
			this._successPoofParticles.transform.position = vector;
			this._successPoofParticles.Emit(this._settings.successPoofParticleCount);
		}
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0002ECE4 File Offset: 0x0002CEE4
	private void Start()
	{
		this._musicalTrailBasePositions.Clear();
		foreach (TrailRenderer trailRenderer in this._trails)
		{
			this._musicalTrailBasePositions.Add(trailRenderer.transform.localPosition);
		}
		this._trailColorKeys = this._trails[0].colorGradient.colorKeys;
		this._trailAlphaKeys = this._trails[0].colorGradient.alphaKeys;
		this._trailAlphaKeysScratch = this._trails[0].colorGradient.alphaKeys;
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0002ED74 File Offset: 0x0002CF74
	private void OnEnable()
	{
		ParticlesX.SetActive(this._backgroundNotes, true);
		foreach (TrailRenderer trailRenderer in this._trails)
		{
			trailRenderer.emitting = true;
			trailRenderer.Clear();
		}
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0002EDB4 File Offset: 0x0002CFB4
	private void OnDisable()
	{
		ParticlesX.SetActive(this._backgroundNotes, false);
		foreach (SuccessNote successNote in this._successNotes)
		{
			successNote.prototype.ReturnToPool();
		}
		this._successNotes.Clear();
		TrailRenderer[] trails = this._trails;
		for (int i = 0; i < trails.Length; i++)
		{
			trails[i].emitting = false;
		}
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0002EE40 File Offset: 0x0002D040
	private void Update()
	{
		for (int i = 0; i < this._trails.Length; i++)
		{
			this._trails[i].transform.localPosition = this._musicalTrailBasePositions[i] + this._settings.trailWavesAmplitude * Mathf.Sin(this._settings.trailWavesFrequency * Time.time) * Vector3.up;
			float num = Mathf.PerlinNoise((float)(i + 1) * 1.59832f, this._settings.alphaChangeFrequency * Time.time);
			num = Mathf.Clamp01(this._settings.noiseContrast * (num - this._settings.noiseMid) + this._settings.noiseMid);
			float num2 = this._settings.alphaRange.Lerp(num);
			for (int j = 0; j < this._trailAlphaKeys.Length; j++)
			{
				this._trailAlphaKeysScratch[j] = new GradientAlphaKey(num2 * this._trailAlphaKeys[j].alpha, this._trailAlphaKeys[j].time);
			}
			Gradient colorGradient = this._trails[i].colorGradient;
			colorGradient.SetKeys(this._trailColorKeys, this._trailAlphaKeysScratch);
			this._trails[i].colorGradient = colorGradient;
		}
		this._successNotes.UpdateAndRemoveIf(delegate(SuccessNote note)
		{
			note.age += Time.deltaTime;
			note.transform.position += 1f * Time.deltaTime * Vector3.up;
			note.transform.localScale = this._settings.successNoteScaleOverTime.Evaluate(note.age) * Vector3.one;
			float num3 = 1f - Mathf.InverseLerp(1.05f, 1.5f, note.age);
			note.spriteRenderer.color = note.spriteRenderer.color.WithAlpha(num3);
			if (note.age > 1.5f)
			{
				note.prototype.ReturnToPool();
				note.transform.SetParent(base.transform);
				return true;
			}
			return false;
		});
	}

	// Token: 0x040006F4 RID: 1780
	[SerializeField]
	private Prototype _successNotePrototype;

	// Token: 0x040006F5 RID: 1781
	[SerializeField]
	private TrailRenderer[] _trails;

	// Token: 0x040006F6 RID: 1782
	[SerializeField]
	private ParticleSystem _backgroundNotes;

	// Token: 0x040006F7 RID: 1783
	[SerializeField]
	private MusicalTrailSettings _settings;

	// Token: 0x040006F8 RID: 1784
	[SerializeField]
	private ParticleSystem _successPoofParticles;

	// Token: 0x040006F9 RID: 1785
	private GradientColorKey[] _trailColorKeys;

	// Token: 0x040006FA RID: 1786
	private GradientAlphaKey[] _trailAlphaKeys;

	// Token: 0x040006FB RID: 1787
	private GradientAlphaKey[] _trailAlphaKeysScratch;

	// Token: 0x040006FC RID: 1788
	private List<Vector3> _musicalTrailBasePositions = new List<Vector3>();

	// Token: 0x040006FD RID: 1789
	private List<SuccessNote> _successNotes = new List<SuccessNote>(32);
}
