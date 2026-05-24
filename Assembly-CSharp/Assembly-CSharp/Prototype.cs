using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001C7 RID: 455
[NullableContext(2)]
[Nullable(0)]
[DisallowMultipleComponent]
public class Prototype : MonoBehaviour
{
	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06000F02 RID: 3842 RVA: 0x00073C28 File Offset: 0x00071E28
	// (remove) Token: 0x06000F03 RID: 3843 RVA: 0x00073C60 File Offset: 0x00071E60
	public event Action OnReturnToPool;

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06000F04 RID: 3844 RVA: 0x00073C95 File Offset: 0x00071E95
	public bool isOriginalPrototype
	{
		get
		{
			return this._originalPrototype == null;
		}
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06000F05 RID: 3845 RVA: 0x00073CA3 File Offset: 0x00071EA3
	public Prototype originalPrototype
	{
		get
		{
			return this._originalPrototype;
		}
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x00073CAB File Offset: 0x00071EAB
	private void Start()
	{
		if (this.isOriginalPrototype)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x00073CC4 File Offset: 0x00071EC4
	private void OnDestroy()
	{
		if (this._instancePool != null)
		{
			foreach (Prototype prototype in this._instancePool)
			{
				Object.Destroy(prototype);
			}
		}
		if (!this.isOriginalPrototype && this.originalPrototype != null && this.originalPrototype._instancePool != null)
		{
			this.originalPrototype._instancePool.Remove(this);
		}
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x00073D54 File Offset: 0x00071F54
	[NullableContext(1)]
	public T Instantiate<[Nullable(0)] T>(Action<T> onInstanciated) where T : Component
	{
		T t = this.Instantiate<T>(null);
		onInstanciated(t);
		return t;
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x00073D74 File Offset: 0x00071F74
	[NullableContext(1)]
	public T Instantiate<[Nullable(0)] T>([Nullable(2)] Transform parent = null) where T : Component
	{
		Prototype prototype;
		if (this._instancePool != null && this._instancePool.Count > 0)
		{
			int num = this._instancePool.Count - 1;
			prototype = this._instancePool[num];
			this._instancePool.RemoveAt(num);
			if (prototype == null)
			{
				Debug.LogError("Prototype instance for type " + typeof(T).Name + " in pool is null!");
				return this.Instantiate<T>(parent);
			}
			prototype.transform.SetParent(parent ?? base.transform.parent, false);
		}
		else
		{
			Transform transform = parent ?? base.transform.parent;
			prototype = Object.Instantiate<Prototype>(this, transform, false);
			if (transform == null && base.gameObject.scene.IsValid())
			{
				SceneManager.MoveGameObjectToScene(prototype.gameObject, base.gameObject.scene);
			}
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform != null)
			{
				RectTransform rectTransform2 = (RectTransform)prototype.transform;
				rectTransform2.anchorMin = rectTransform.anchorMin;
				rectTransform2.anchorMax = rectTransform.anchorMax;
				rectTransform2.pivot = rectTransform.pivot;
				rectTransform2.sizeDelta = rectTransform.sizeDelta;
			}
			prototype.transform.localPosition = base.transform.localPosition;
			prototype.transform.localRotation = base.transform.localRotation;
			prototype.transform.localScale = base.transform.localScale;
			prototype._originalPrototype = this;
		}
		prototype.gameObject.SetActive(true);
		return prototype.GetComponent<T>();
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x00073F11 File Offset: 0x00072111
	[NullableContext(1)]
	private IEnumerator DelayedReturnCR(float delay)
	{
		yield return new WaitForSeconds(delay);
		this.ReturnToPool();
		yield break;
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x00073F27 File Offset: 0x00072127
	public void ReturnToPool(float delay)
	{
		base.StartCoroutine(this.DelayedReturnCR(delay));
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x00073F37 File Offset: 0x00072137
	public void ReturnToPool()
	{
		base.StopAllCoroutines();
		if (this.isOriginalPrototype)
		{
			Debug.LogError("Can't return to pool because the original prototype doesn't exist. Is this prototype the original?");
			Object.Destroy(base.gameObject);
			return;
		}
		this._originalPrototype.AddToPool(this);
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x00073F6C File Offset: 0x0007216C
	public void DestroyPool()
	{
		if (this._instancePool == null)
		{
			return;
		}
		foreach (Prototype prototype in this._instancePool)
		{
			if (prototype != null)
			{
				Object.Destroy(prototype.gameObject);
			}
		}
		this._instancePool.Clear();
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x00073FE0 File Offset: 0x000721E0
	[NullableContext(1)]
	public T GetOriginal<[Nullable(2)] T>()
	{
		if (this.isOriginalPrototype)
		{
			return base.GetComponent<T>();
		}
		return this.originalPrototype.GetComponent<T>();
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x00073FFC File Offset: 0x000721FC
	[NullableContext(1)]
	private void AddToPool(Prototype instancePrototype)
	{
		if (!this.isOriginalPrototype)
		{
			Debug.LogError(string.Concat(new string[] { "Adding ", instancePrototype.name, " to prototype pool of ", base.name, " but this appears to be an instance itself?" }), instancePrototype);
		}
		instancePrototype.gameObject.SetActive(false);
		if (this._instancePool == null)
		{
			this._instancePool = new List<Prototype>();
		}
		this._instancePool.Add(instancePrototype);
		if (instancePrototype.OnReturnToPool != null)
		{
			instancePrototype.OnReturnToPool();
		}
	}

	// Token: 0x040011BA RID: 4538
	private Prototype _originalPrototype;

	// Token: 0x040011BB RID: 4539
	[Nullable(new byte[] { 2, 1 })]
	private List<Prototype> _instancePool;
}
