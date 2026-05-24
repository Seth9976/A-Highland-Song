using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000025 RID: 37
	public class PlayerAction : OneAxisInputControl
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004FEE File Offset: 0x000031EE
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004FF6 File Offset: 0x000031F6
		public string Name { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004FFF File Offset: 0x000031FF
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00005007 File Offset: 0x00003207
		public PlayerActionSet Owner { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000F5 RID: 245 RVA: 0x00005010 File Offset: 0x00003210
		// (remove) Token: 0x060000F6 RID: 246 RVA: 0x00005048 File Offset: 0x00003248
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000F7 RID: 247 RVA: 0x00005080 File Offset: 0x00003280
		// (remove) Token: 0x060000F8 RID: 248 RVA: 0x000050B8 File Offset: 0x000032B8
		public event Action OnBindingsChanged;

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000050ED File Offset: 0x000032ED
		// (set) Token: 0x060000FA RID: 250 RVA: 0x000050F5 File Offset: 0x000032F5
		public object UserData { get; set; }

		// Token: 0x060000FB RID: 251 RVA: 0x00005100 File Offset: 0x00003300
		public PlayerAction(string name, PlayerActionSet owner)
		{
			this.Raw = true;
			this.Name = name;
			this.Owner = owner;
			this.bindings = new ReadOnlyCollection<BindingSource>(this.visibleBindings);
			this.unfilteredBindings = new ReadOnlyCollection<BindingSource>(this.regularBindings);
			owner.AddPlayerAction(this);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000051A0 File Offset: 0x000033A0
		public void AddDefaultBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			if (binding.BoundTo != null)
			{
				throw new InControlException("Binding source is already bound to action " + binding.BoundTo.Name);
			}
			if (!this.defaultBindings.Contains(binding))
			{
				this.defaultBindings.Add(binding);
				binding.BoundTo = this;
			}
			if (!this.regularBindings.Contains(binding))
			{
				this.regularBindings.Add(binding);
				binding.BoundTo = this;
				if (binding.IsValid)
				{
					this.visibleBindings.Add(binding);
				}
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005230 File Offset: 0x00003430
		public void AddDefaultBinding(params Key[] keys)
		{
			this.AddDefaultBinding(new KeyBindingSource(keys));
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000523E File Offset: 0x0000343E
		public void AddDefaultBinding(KeyCombo keyCombo)
		{
			this.AddDefaultBinding(new KeyBindingSource(keyCombo));
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000524C File Offset: 0x0000344C
		public void AddDefaultBinding(Mouse control)
		{
			this.AddDefaultBinding(new MouseBindingSource(control));
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000525A File Offset: 0x0000345A
		public void AddDefaultBinding(InputControlType control)
		{
			this.AddDefaultBinding(new DeviceBindingSource(control));
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005268 File Offset: 0x00003468
		public bool AddBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			if (binding.BoundTo != null)
			{
				Logger.LogWarning("Binding source is already bound to action " + binding.BoundTo.Name);
				return false;
			}
			if (this.regularBindings.Contains(binding))
			{
				return false;
			}
			this.regularBindings.Add(binding);
			binding.BoundTo = this;
			if (binding.IsValid)
			{
				this.visibleBindings.Add(binding);
			}
			this.triggerBindingChanged = true;
			return true;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000052E4 File Offset: 0x000034E4
		public bool InsertBindingAt(int index, BindingSource binding)
		{
			if (index < 0 || index > this.visibleBindings.Count)
			{
				throw new InControlException("Index is out of range for bindings on this action.");
			}
			if (index == this.visibleBindings.Count)
			{
				return this.AddBinding(binding);
			}
			if (binding == null)
			{
				return false;
			}
			if (binding.BoundTo != null)
			{
				Logger.LogWarning("Binding source is already bound to action " + binding.BoundTo.Name);
				return false;
			}
			if (this.regularBindings.Contains(binding))
			{
				return false;
			}
			int num = ((index == 0) ? 0 : this.regularBindings.IndexOf(this.visibleBindings[index]));
			this.regularBindings.Insert(num, binding);
			binding.BoundTo = this;
			if (binding.IsValid)
			{
				this.visibleBindings.Insert(index, binding);
			}
			this.triggerBindingChanged = true;
			return true;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000053B4 File Offset: 0x000035B4
		public bool ReplaceBinding(BindingSource findBinding, BindingSource withBinding)
		{
			if (findBinding == null || withBinding == null)
			{
				return false;
			}
			if (withBinding.BoundTo != null)
			{
				Logger.LogWarning("Binding source is already bound to action " + withBinding.BoundTo.Name);
				return false;
			}
			int num = this.regularBindings.IndexOf(findBinding);
			if (num < 0)
			{
				Logger.LogWarning("Binding source to replace is not present in this action.");
				return false;
			}
			findBinding.BoundTo = null;
			this.regularBindings[num] = withBinding;
			withBinding.BoundTo = this;
			num = this.visibleBindings.IndexOf(findBinding);
			if (num >= 0)
			{
				this.visibleBindings[num] = withBinding;
			}
			this.triggerBindingChanged = true;
			return true;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005458 File Offset: 0x00003658
		public bool HasBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			BindingSource bindingSource = this.FindBinding(binding);
			return !(bindingSource == null) && bindingSource.BoundTo == this;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000548C File Offset: 0x0000368C
		public BindingSource FindBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return null;
			}
			int num = this.regularBindings.IndexOf(binding);
			if (num >= 0)
			{
				return this.regularBindings[num];
			}
			return null;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000054C4 File Offset: 0x000036C4
		private void HardRemoveBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			int num = this.regularBindings.IndexOf(binding);
			if (num >= 0)
			{
				BindingSource bindingSource = this.regularBindings[num];
				if (bindingSource.BoundTo == this)
				{
					bindingSource.BoundTo = null;
					this.regularBindings.RemoveAt(num);
					this.UpdateVisibleBindings();
					this.triggerBindingChanged = true;
				}
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005524 File Offset: 0x00003724
		public void RemoveBinding(BindingSource binding)
		{
			BindingSource bindingSource = this.FindBinding(binding);
			if (bindingSource != null && bindingSource.BoundTo == this)
			{
				bindingSource.BoundTo = null;
				this.triggerBindingChanged = true;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005559 File Offset: 0x00003759
		public void RemoveBindingAt(int index)
		{
			if (index < 0 || index >= this.regularBindings.Count)
			{
				throw new InControlException("Index is out of range for bindings on this action.");
			}
			this.regularBindings[index].BoundTo = null;
			this.triggerBindingChanged = true;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005594 File Offset: 0x00003794
		private int CountBindingsOfType(BindingSourceType bindingSourceType)
		{
			int num = 0;
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo == this && bindingSource.BindingSourceType == bindingSourceType)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000055E0 File Offset: 0x000037E0
		private void RemoveFirstBindingOfType(BindingSourceType bindingSourceType)
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo == this && bindingSource.BindingSourceType == bindingSourceType)
				{
					bindingSource.BoundTo = null;
					this.regularBindings.RemoveAt(i);
					this.triggerBindingChanged = true;
					return;
				}
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005640 File Offset: 0x00003840
		private int IndexOfFirstInvalidBinding()
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				if (!this.regularBindings[i].IsValid)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000567C File Offset: 0x0000387C
		public void ClearBindings()
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				this.regularBindings[i].BoundTo = null;
			}
			this.regularBindings.Clear();
			this.visibleBindings.Clear();
			this.triggerBindingChanged = true;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000056D0 File Offset: 0x000038D0
		public void ResetBindings()
		{
			this.ClearBindings();
			this.regularBindings.AddRange(this.defaultBindings);
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				bindingSource.BoundTo = this;
				if (bindingSource.IsValid)
				{
					this.visibleBindings.Add(bindingSource);
				}
			}
			this.triggerBindingChanged = true;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000573B File Offset: 0x0000393B
		public void ListenForBinding()
		{
			this.ListenForBindingReplacing(null);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005744 File Offset: 0x00003944
		public void ListenForBindingReplacing(BindingSource binding)
		{
			(this.ListenOptions ?? this.Owner.ListenOptions).ReplaceBinding = binding;
			this.Owner.listenWithAction = this;
			int num = this.bindingSourceListeners.Length;
			for (int i = 0; i < num; i++)
			{
				this.bindingSourceListeners[i].Reset();
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000579A File Offset: 0x0000399A
		public void StopListeningForBinding()
		{
			if (this.IsListeningForBinding)
			{
				this.Owner.listenWithAction = null;
				this.triggerBindingEnded = true;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000057B7 File Offset: 0x000039B7
		public bool IsListeningForBinding
		{
			get
			{
				return this.Owner.listenWithAction == this;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000057C7 File Offset: 0x000039C7
		public ReadOnlyCollection<BindingSource> Bindings
		{
			get
			{
				return this.bindings;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000057CF File Offset: 0x000039CF
		public ReadOnlyCollection<BindingSource> UnfilteredBindings
		{
			get
			{
				return this.unfilteredBindings;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000057D8 File Offset: 0x000039D8
		private void RemoveOrphanedBindings()
		{
			for (int i = this.regularBindings.Count - 1; i >= 0; i--)
			{
				if (this.regularBindings[i].BoundTo != this)
				{
					this.regularBindings.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005820 File Offset: 0x00003A20
		internal void Update(ulong updateTick, float deltaTime, InputDevice device)
		{
			this.Device = device;
			this.UpdateBindings(updateTick, deltaTime);
			if (this.triggerBindingChanged)
			{
				if (this.OnBindingsChanged != null)
				{
					this.OnBindingsChanged();
				}
				this.triggerBindingChanged = false;
			}
			if (this.triggerBindingEnded)
			{
				(this.ListenOptions ?? this.Owner.ListenOptions).CallOnBindingEnded(this);
				this.triggerBindingEnded = false;
			}
			this.DetectBindings();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005890 File Offset: 0x00003A90
		private void UpdateBindings(ulong updateTick, float deltaTime)
		{
			bool flag = this.IsListeningForBinding || (this.Owner.IsListeningForBinding && this.Owner.PreventInputWhileListeningForBinding);
			BindingSourceType bindingSourceType = this.LastInputType;
			ulong num = this.LastInputTypeChangedTick;
			ulong updateTick2 = base.UpdateTick;
			InputDeviceClass inputDeviceClass = this.LastDeviceClass;
			InputDeviceStyle inputDeviceStyle = this.LastDeviceStyle;
			int count = this.regularBindings.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo != this)
				{
					this.regularBindings.RemoveAt(i);
					this.visibleBindings.Remove(bindingSource);
					this.triggerBindingChanged = true;
				}
				else if (!flag)
				{
					float value = bindingSource.GetValue(this.Device);
					if (base.UpdateWithValue(value, updateTick, deltaTime))
					{
						bindingSourceType = bindingSource.BindingSourceType;
						num = updateTick;
						inputDeviceClass = bindingSource.DeviceClass;
						inputDeviceStyle = bindingSource.DeviceStyle;
					}
				}
			}
			if (flag || count == 0)
			{
				base.UpdateWithValue(0f, updateTick, deltaTime);
			}
			base.Commit();
			this.ownerEnabled = this.Owner.Enabled;
			if (num > this.LastInputTypeChangedTick && (bindingSourceType != BindingSourceType.MouseBindingSource || Utility.Abs(base.LastValue - base.Value) >= MouseBindingSource.JitterThreshold))
			{
				bool flag2 = bindingSourceType != this.LastInputType;
				this.LastInputType = bindingSourceType;
				this.LastInputTypeChangedTick = num;
				this.LastDeviceClass = inputDeviceClass;
				this.LastDeviceStyle = inputDeviceStyle;
				if (this.OnLastInputTypeChanged != null && flag2)
				{
					this.OnLastInputTypeChanged(bindingSourceType);
				}
			}
			if (base.UpdateTick > updateTick2)
			{
				this.activeDevice = (this.LastInputTypeIsDevice ? this.Device : null);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005A3C File Offset: 0x00003C3C
		private void DetectBindings()
		{
			if (this.IsListeningForBinding)
			{
				BindingSource bindingSource = null;
				BindingListenOptions bindingListenOptions = this.ListenOptions ?? this.Owner.ListenOptions;
				int num = this.bindingSourceListeners.Length;
				for (int i = 0; i < num; i++)
				{
					bindingSource = this.bindingSourceListeners[i].Listen(bindingListenOptions, this.device);
					if (bindingSource != null)
					{
						break;
					}
				}
				if (bindingSource == null)
				{
					return;
				}
				if (!bindingListenOptions.CallOnBindingFound(this, bindingSource))
				{
					return;
				}
				if (this.HasBinding(bindingSource))
				{
					if (bindingListenOptions.RejectRedundantBindings)
					{
						bindingListenOptions.CallOnBindingRejected(this, bindingSource, BindingSourceRejectionType.DuplicateBindingOnAction);
						return;
					}
					this.StopListeningForBinding();
					bindingListenOptions.CallOnBindingAdded(this, bindingSource);
					return;
				}
				else
				{
					if (bindingListenOptions.UnsetDuplicateBindingsOnSet)
					{
						int count = this.Owner.Actions.Count;
						for (int j = 0; j < count; j++)
						{
							this.Owner.Actions[j].HardRemoveBinding(bindingSource);
						}
					}
					if (!bindingListenOptions.AllowDuplicateBindingsPerSet && this.Owner.HasBinding(bindingSource))
					{
						bindingListenOptions.CallOnBindingRejected(this, bindingSource, BindingSourceRejectionType.DuplicateBindingOnActionSet);
						return;
					}
					this.StopListeningForBinding();
					if (bindingListenOptions.ReplaceBinding == null)
					{
						if (bindingListenOptions.MaxAllowedBindingsPerType > 0U)
						{
							while ((long)this.CountBindingsOfType(bindingSource.BindingSourceType) >= (long)((ulong)bindingListenOptions.MaxAllowedBindingsPerType))
							{
								this.RemoveFirstBindingOfType(bindingSource.BindingSourceType);
							}
						}
						else if (bindingListenOptions.MaxAllowedBindings > 0U)
						{
							while ((long)this.regularBindings.Count >= (long)((ulong)bindingListenOptions.MaxAllowedBindings))
							{
								int num2 = Mathf.Max(0, this.IndexOfFirstInvalidBinding());
								this.regularBindings.RemoveAt(num2);
								this.triggerBindingChanged = true;
							}
						}
						this.AddBinding(bindingSource);
					}
					else
					{
						this.ReplaceBinding(bindingListenOptions.ReplaceBinding, bindingSource);
					}
					this.UpdateVisibleBindings();
					bindingListenOptions.CallOnBindingAdded(this, bindingSource);
				}
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005BEC File Offset: 0x00003DEC
		private void UpdateVisibleBindings()
		{
			this.visibleBindings.Clear();
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.IsValid)
				{
					this.visibleBindings.Add(bindingSource);
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00005C3D File Offset: 0x00003E3D
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00005C64 File Offset: 0x00003E64
		internal InputDevice Device
		{
			get
			{
				if (this.device == null)
				{
					this.device = this.Owner.Device;
					this.UpdateVisibleBindings();
				}
				return this.device;
			}
			set
			{
				if (this.device != value)
				{
					this.device = value;
					this.UpdateVisibleBindings();
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00005C7C File Offset: 0x00003E7C
		public InputDevice ActiveDevice
		{
			get
			{
				return this.activeDevice ?? InputDevice.Null;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00005C8D File Offset: 0x00003E8D
		private bool LastInputTypeIsDevice
		{
			get
			{
				return this.LastInputType == BindingSourceType.DeviceBindingSource || this.LastInputType == BindingSourceType.UnknownDeviceBindingSource;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00005CA3 File Offset: 0x00003EA3
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00005CAA File Offset: 0x00003EAA
		[Obsolete("Please set this property on device controls directly. It does nothing here.")]
		public new float LowerDeadZone
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00005CAC File Offset: 0x00003EAC
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00005CB3 File Offset: 0x00003EB3
		[Obsolete("Please set this property on device controls directly. It does nothing here.")]
		public new float UpperDeadZone
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005CB8 File Offset: 0x00003EB8
		internal void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			this.ClearBindings();
			int num = reader.ReadInt32();
			int i = 0;
			while (i < num)
			{
				BindingSourceType bindingSourceType = (BindingSourceType)reader.ReadInt32();
				BindingSource bindingSource;
				switch (bindingSourceType)
				{
				case BindingSourceType.None:
					IL_0081:
					i++;
					continue;
				case BindingSourceType.DeviceBindingSource:
					bindingSource = new DeviceBindingSource();
					break;
				case BindingSourceType.KeyBindingSource:
					bindingSource = new KeyBindingSource();
					break;
				case BindingSourceType.MouseBindingSource:
					bindingSource = new MouseBindingSource();
					break;
				case BindingSourceType.UnknownDeviceBindingSource:
					bindingSource = new UnknownDeviceBindingSource();
					break;
				default:
					throw new InControlException("Don't know how to load BindingSourceType: " + bindingSourceType.ToString());
				}
				bindingSource.Load(reader, dataFormatVersion);
				this.AddBinding(bindingSource);
				goto IL_0081;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005D50 File Offset: 0x00003F50
		internal void Save(BinaryWriter writer)
		{
			this.RemoveOrphanedBindings();
			writer.Write(this.Name);
			int count = this.regularBindings.Count;
			writer.Write(count);
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				writer.Write((int)bindingSource.BindingSourceType);
				bindingSource.Save(writer);
			}
		}

		// Token: 0x04000156 RID: 342
		public BindingListenOptions ListenOptions;

		// Token: 0x04000157 RID: 343
		public BindingSourceType LastInputType;

		// Token: 0x04000159 RID: 345
		public ulong LastInputTypeChangedTick;

		// Token: 0x0400015A RID: 346
		public InputDeviceClass LastDeviceClass;

		// Token: 0x0400015B RID: 347
		public InputDeviceStyle LastDeviceStyle;

		// Token: 0x0400015E RID: 350
		private readonly List<BindingSource> defaultBindings = new List<BindingSource>();

		// Token: 0x0400015F RID: 351
		private readonly List<BindingSource> regularBindings = new List<BindingSource>();

		// Token: 0x04000160 RID: 352
		private readonly List<BindingSource> visibleBindings = new List<BindingSource>();

		// Token: 0x04000161 RID: 353
		private readonly ReadOnlyCollection<BindingSource> bindings;

		// Token: 0x04000162 RID: 354
		private readonly ReadOnlyCollection<BindingSource> unfilteredBindings;

		// Token: 0x04000163 RID: 355
		private readonly BindingSourceListener[] bindingSourceListeners = new BindingSourceListener[]
		{
			new DeviceBindingSourceListener(),
			new UnknownDeviceBindingSourceListener(),
			new KeyBindingSourceListener(),
			new MouseBindingSourceListener()
		};

		// Token: 0x04000164 RID: 356
		private bool triggerBindingEnded;

		// Token: 0x04000165 RID: 357
		private bool triggerBindingChanged;

		// Token: 0x04000166 RID: 358
		private InputDevice device;

		// Token: 0x04000167 RID: 359
		private InputDevice activeDevice;
	}
}
