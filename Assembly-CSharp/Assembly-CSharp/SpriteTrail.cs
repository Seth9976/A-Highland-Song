using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class SpriteTrail : MonoBehaviour
{
	// Token: 0x06000783 RID: 1923 RVA: 0x0004440C File Offset: 0x0004260C
	private void Update()
	{
		if (this._timeSinceLastSnapshot > this.timeBetweenSnapshots && this.visibility > 0f)
		{
			this._timeSinceLastSnapshot = 0f;
			SpriteRenderer spriteRenderer = this.trailSpritePrototype.Instantiate<SpriteRenderer>(null);
			spriteRenderer.transform.SetParent(null);
			spriteRenderer.sprite = this.spriteRendererToFollow.sprite;
			spriteRenderer.transform.position = this.spriteRendererToFollow.transform.position + this.zOffset * Vector3.forward;
			spriteRenderer.transform.rotation = this.spriteRendererToFollow.transform.rotation;
			spriteRenderer.transform.localScale = this.spriteRendererToFollow.transform.lossyScale;
			this._trailSprites.Add(new SpriteTrail.TrailSprite
			{
				sprite = spriteRenderer,
				timeCreated = Time.time
			});
		}
		else
		{
			this._timeSinceLastSnapshot += Time.deltaTime;
		}
		this._trailSprites.UpdateAndRemoveIf(delegate(SpriteTrail.TrailSprite trailSprite)
		{
			float num = (Time.time - trailSprite.timeCreated) / this.fadeTime;
			trailSprite.sprite.color = trailSprite.sprite.color.WithAlpha(this.visibility * this.opacityScalar * (1f - num));
			if (num >= 1f)
			{
				trailSprite.sprite.GetComponent<Prototype>().ReturnToPool();
				return true;
			}
			return false;
		});
	}

	// Token: 0x04000946 RID: 2374
	public float visibility = 1f;

	// Token: 0x04000947 RID: 2375
	public float opacityScalar = 1f;

	// Token: 0x04000948 RID: 2376
	public float zOffset = 0.2f;

	// Token: 0x04000949 RID: 2377
	public SpriteRenderer spriteRendererToFollow;

	// Token: 0x0400094A RID: 2378
	public Prototype trailSpritePrototype;

	// Token: 0x0400094B RID: 2379
	public float timeBetweenSnapshots;

	// Token: 0x0400094C RID: 2380
	public float fadeTime = 1f;

	// Token: 0x0400094D RID: 2381
	private List<SpriteTrail.TrailSprite> _trailSprites = new List<SpriteTrail.TrailSprite>();

	// Token: 0x0400094E RID: 2382
	private float _timeSinceLastSnapshot;

	// Token: 0x0200030C RID: 780
	private struct TrailSprite
	{
		// Token: 0x0400179C RID: 6044
		public SpriteRenderer sprite;

		// Token: 0x0400179D RID: 6045
		public float timeCreated;
	}
}
