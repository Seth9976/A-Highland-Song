using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000BC RID: 188
[Serializable]
public struct SerializedHistory
{
	// Token: 0x17000181 RID: 385
	// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0002E608 File Offset: 0x0002C808
	public History.Frame[] frames
	{
		get
		{
			for (int i = 0; i < this._frames.Length; i++)
			{
				if (this._frames[i].serializedNamedObjects != null)
				{
					History.Frame frame = this._frames[i];
					frame.namedObjects = new List<History.NamedObject>();
					foreach (History.SerializedNamedObject serializedNamedObject in frame.serializedNamedObjects)
					{
						frame.namedObjects.Add(new History.NamedObject
						{
							name = serializedNamedObject.name,
							obj = this.StringToObject(serializedNamedObject.data, serializedNamedObject.typeName),
							priority = serializedNamedObject.priority,
							originalIdx = serializedNamedObject.originalIdx
						});
					}
					this._frames[i] = frame;
				}
			}
			return this._frames;
		}
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0002E704 File Offset: 0x0002C904
	public SerializedHistory(Queue<History.Frame> frames)
	{
		this._frames = frames.ToArray();
		for (int i = 0; i < this._frames.Length; i++)
		{
			if (this._frames[i].namedObjects != null)
			{
				History.Frame frame = this._frames[i];
				frame.serializedNamedObjects = new List<History.SerializedNamedObject>();
				foreach (History.NamedObject namedObject in frame.namedObjects)
				{
					if (namedObject.obj is History.Group)
					{
						History.Group group = (History.Group)namedObject.obj;
						if (group.children.Count == 0)
						{
							History.SerializedNamedObject serializedNamedObject = SerializedHistory.<.ctor>g__SerialiseNamedObj|2_0(namedObject);
							serializedNamedObject.data = "EMPTY";
							frame.serializedNamedObjects.Add(serializedNamedObject);
							continue;
						}
						using (List<History.NamedObject>.Enumerator enumerator2 = group.children.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								History.NamedObject namedObject2 = enumerator2.Current;
								History.SerializedNamedObject serializedNamedObject2 = SerializedHistory.<.ctor>g__SerialiseNamedObj|2_0(namedObject2);
								serializedNamedObject2.name = namedObject.name + ": " + serializedNamedObject2.name;
								serializedNamedObject2.priority = namedObject.priority;
								serializedNamedObject2.originalIdx = namedObject.originalIdx;
								frame.serializedNamedObjects.Add(serializedNamedObject2);
							}
							continue;
						}
					}
					frame.serializedNamedObjects.Add(SerializedHistory.<.ctor>g__SerialiseNamedObj|2_0(namedObject));
				}
				this._frames[i] = frame;
			}
			if (this._frames[i].sprites != null)
			{
				List<History.ActiveSprite> sprites = this._frames[i].sprites;
				for (int j = 0; j < sprites.Count; j++)
				{
					History.ActiveSprite activeSprite = sprites[j];
					activeSprite.serializedAssetPath = activeSprite.sprite.texture.name + ":" + activeSprite.sprite.name;
					this._frames[i].sprites[j] = activeSprite;
				}
			}
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0002E930 File Offset: 0x0002CB30
	private static string ObjectToString(object obj)
	{
		if (obj == null)
		{
			return null;
		}
		if (obj is Object && (Object)obj == null)
		{
			return null;
		}
		GuidComponent guidComponent = null;
		if (obj is MonoBehaviour)
		{
			guidComponent = ((MonoBehaviour)obj).GetComponent<GuidComponent>();
		}
		else if (obj is GameObject)
		{
			guidComponent = ((GameObject)obj).GetComponent<GuidComponent>();
		}
		if (guidComponent != null)
		{
			return "GUID:" + guidComponent.GetGuid().ToString();
		}
		return obj.ToString();
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0002E9B4 File Offset: 0x0002CBB4
	private object StringToObject(string data, string typeName)
	{
		if (typeName == "Int32")
		{
			int num = 0;
			int.TryParse(data, out num);
			return num;
		}
		if (typeName == "Single")
		{
			float num2 = 0f;
			float.TryParse(data, out num2);
			return num2;
		}
		if (typeName == "Vector3" || typeName == "Vector2")
		{
			data = data.Substring(1, data.Length - 2);
			float[] array = data.Split(',', StringSplitOptions.None).Select(delegate(string elStr)
			{
				float num3 = 0f;
				float.TryParse(elStr.Trim(), out num3);
				return num3;
			}).ToArray<float>();
			Vector3 vector = default(Vector3);
			vector.x = array[0];
			vector.y = array[1];
			if (typeName == "Vector2")
			{
				return vector;
			}
			vector.z = array[2];
			return vector;
		}
		else
		{
			if (typeName == "Boolean")
			{
				return data == "True";
			}
			if (typeName == "NoObject")
			{
				return History.NamedObject.NoObject;
			}
			if (data.StartsWith("GUID:"))
			{
				GameObject gameObject = GuidManager.ResolveGuid(new Guid(data.Substring(5)));
				if (gameObject != null)
				{
					return gameObject;
				}
			}
			return data;
		}
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0002EB10 File Offset: 0x0002CD10
	[CompilerGenerated]
	internal static History.SerializedNamedObject <.ctor>g__SerialiseNamedObj|2_0(History.NamedObject namedObj)
	{
		History.SerializedNamedObject serializedNamedObject = default(History.SerializedNamedObject);
		serializedNamedObject.name = namedObj.name;
		object obj = namedObj.obj;
		serializedNamedObject.typeName = ((obj != null) ? obj.GetType().Name : null);
		serializedNamedObject.data = SerializedHistory.ObjectToString(namedObj.obj);
		serializedNamedObject.priority = namedObj.priority;
		serializedNamedObject.originalIdx = namedObj.originalIdx;
		return serializedNamedObject;
	}

	// Token: 0x040006F3 RID: 1779
	[SerializeField]
	private History.Frame[] _frames;
}
