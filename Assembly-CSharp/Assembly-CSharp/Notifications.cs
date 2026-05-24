using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class Notifications : MonoSingleton<Notifications>
{
	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000A28 RID: 2600 RVA: 0x0005556E File Offset: 0x0005376E
	public Notification activeNotification
	{
		get
		{
			return this._activeNotification;
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000A29 RID: 2601 RVA: 0x00055576 File Offset: 0x00053776
	private bool allowedToShowNotification
	{
		get
		{
			return !MonoSingleton<JournalController>.instance.visible && Blackout.isFullyHidden && !MonoSingleton<PeakStateController>.instance.active && !MonoSingleton<MapsViewController>.instance.maximised && !MonoSingleton<MapsViewController>.instance.isBusy;
		}
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x000555B4 File Offset: 0x000537B4
	public void Add(string title, string body, string body2, NotificationType type, string inkName, Sprite sprite = null)
	{
		if (string.IsNullOrWhiteSpace(body))
		{
			Debug.LogWarning(string.Format("Body of Notification '{0}' is empty! Skipping. Ink name = {1}, type = {2}", title, inkName, type));
			return;
		}
		if (sprite == null)
		{
			sprite = this._settings.defaultIcon;
		}
		Notification notification = ((type == NotificationType.Peak) ? this._peakNotificationPrototype : this._notificationPrototype).Instantiate<Notification>(null);
		notification.Setup(title, body, body2, type, inkName, sprite);
		notification.layout.groupAlpha = 0f;
		notification.layout.x = notification.layout.canvasWidth - this._settings.xFromRight + this._settings.offscreenOffset;
		if (this._activeNotification != null || !this.allowedToShowNotification)
		{
			this._queue.Enqueue(notification);
			return;
		}
		this.Show(notification, 0f);
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00055690 File Offset: 0x00053890
	public void CompleteCurrent()
	{
		if (this._activeNotification == null)
		{
			return;
		}
		Notification hidingNotification = this._activeNotification;
		this._activeNotification = null;
		hidingNotification.StopIfNecessary();
		hidingNotification.layout.Animate(this._settings.popOutDuration, 0f, this._settings.popOutCurve, delegate
		{
			hidingNotification.layout.x = hidingNotification.layout.canvasWidth - this._settings.xFromRight + this._settings.offscreenOffset;
			hidingNotification.layout.groupAlpha = 0f;
		}).Then(delegate
		{
			hidingNotification.ReturnToPool();
		});
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00055720 File Offset: 0x00053920
	private void Show(Notification notification, float delay = 0f)
	{
		if (this._activeNotification != null)
		{
			Debug.LogError("Expected _activeNotification to be null");
			this.CompleteCurrent();
		}
		this._activeNotification = notification;
		this._activeNotification.layout.Animate(this._settings.popInDuration, delay, this._settings.popInCurve, delegate
		{
			this._activeNotification.layout.x = this._activeNotification.layout.canvasWidth - this._settings.xFromRight;
			this._activeNotification.layout.groupAlpha = 1f;
		});
		this._activeNotification.layout.After(delay + 0.5f, delegate
		{
			if (this._activeNotification != null)
			{
				this._activeNotification.Begin();
			}
		});
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x000557AC File Offset: 0x000539AC
	public void Notify(NotificationType type, string inkName)
	{
		if (type == NotificationType.Discovery)
		{
			string text = Narrative.instance.DiscoveryDescription(inkName);
			this.Add("New Discovery!", text, null, type, inkName, null);
			return;
		}
		if (type == NotificationType.Item)
		{
			string text2 = Narrative.instance.ItemDescription(inkName);
			this.Add("New Item!", text2, null, type, inkName, MonoSingleton<JournalController>.instance.IconForInventoryItemNamed(inkName));
			return;
		}
		if (type == NotificationType.Peak)
		{
			Narrative.PeakKnowledge peakKnowledge = Narrative.instance.ProducePeakKnowledge(inkName);
			if (!string.IsNullOrEmpty(peakKnowledge.englishName))
			{
				Sprite sprite = this._settings.peakIconDatabase.SpriteWithName(inkName);
				this.Add("New Peak!", peakKnowledge.englishName, peakKnowledge.gaelicName, type, inkName, sprite);
				return;
			}
		}
		else
		{
			Debug.LogError("Unhandled notification type: " + type.ToString());
		}
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x0005586A File Offset: 0x00053A6A
	public void NotifyStaminaIncrease(int staminaIncreaseForFlawlessRun)
	{
		this.Add("Getting stronger!", "The stronger I get, the further I can run and climb!", null, NotificationType.Stamina, "RUNNING_MAKES_ME_STRONGER", null);
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00055884 File Offset: 0x00053A84
	private void Update()
	{
		if (this._activeNotification != null && (this._activeNotification.expired || !this.allowedToShowNotification))
		{
			this.CompleteCurrent();
		}
		if (this._queue.Count > 0 && this._activeNotification == null && this.allowedToShowNotification)
		{
			Notification notification = this._queue.Dequeue();
			this.Show(notification, this._settings.delayBetweenQueuedNotifications);
		}
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x000558FC File Offset: 0x00053AFC
	[Button("Get random discovery", Mode = ButtonMode.EnabledInPlayMode)]
	private void GetRandomDiscovery()
	{
		Narrative.instance.inkStory.EvaluateFunction("DebugDiscoverSomethingRandom", Array.Empty<object>());
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x00055918 File Offset: 0x00053B18
	[Button("Get random item", Mode = ButtonMode.EnabledInPlayMode)]
	private void GetRandomItem()
	{
		Narrative.instance.inkStory.EvaluateFunction("DebugGetItem", Array.Empty<object>());
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x00055934 File Offset: 0x00053B34
	[Button("Get random peak", Mode = ButtonMode.EnabledInPlayMode)]
	private void GetRandomPeak()
	{
		Narrative.instance.inkStory.EvaluateFunction("DebugGetPeak", Array.Empty<object>());
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x00055950 File Offset: 0x00053B50
	[Button("Dismiss", Mode = ButtonMode.EnabledInPlayMode)]
	private void Dismiss()
	{
		if (this._activeNotification != null)
		{
			this.CompleteCurrent();
		}
	}

	// Token: 0x04000C54 RID: 3156
	private Queue<Notification> _queue = new Queue<Notification>();

	// Token: 0x04000C55 RID: 3157
	private Notification _activeNotification;

	// Token: 0x04000C56 RID: 3158
	[SerializeField]
	private Prototype _notificationPrototype;

	// Token: 0x04000C57 RID: 3159
	[SerializeField]
	private Prototype _peakNotificationPrototype;

	// Token: 0x04000C58 RID: 3160
	[SerializeField]
	private NotificationsSettings _settings;
}
