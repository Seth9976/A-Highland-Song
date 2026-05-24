using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class LegacyInstruction : BaseContentInstruction
{
	// Token: 0x060003FF RID: 1023 RVA: 0x00020834 File Offset: 0x0001EA34
	public static LegacyInstruction TryParse(string line, List<string> tags)
	{
		LegacyInstruction legacyInstruction = new LegacyInstruction();
		LegacyInstruction._idx = 0;
		LegacyInstruction._str = line;
		LegacyInstruction.Whitespace();
		if (LegacyInstruction.ParseAny(">") == 0)
		{
			string text = LegacyInstruction.ParseWords(true);
			LegacyInstruction.Whitespace();
			if (LegacyInstruction.ParseSingle(':'))
			{
				legacyInstruction.type = LegacyInstruction.Type.CharacterDialogue;
				legacyInstruction.characterName = text;
				LegacyInstruction.Whitespace();
				legacyInstruction.content = LegacyInstruction.ParseRemaining();
			}
			else
			{
				legacyInstruction.type = LegacyInstruction.Type.NarratorDialogue;
				legacyInstruction.content = LegacyInstruction._str;
			}
			legacyInstruction.content = InkStylingUtility.ProcessText(legacyInstruction.content, true, false);
			legacyInstruction.content = InkStylingUtility.ParseStyling(legacyInstruction.content, true, "#FF1800");
			if (tags.Count > 0)
			{
				legacyInstruction.audioFilename = tags.Random<string>();
			}
			return legacyInstruction;
		}
		LegacyInstruction.Whitespace();
		int idx = LegacyInstruction._idx;
		string text2 = LegacyInstruction.ParseWords(false);
		if (text2 == "START")
		{
			legacyInstruction.start = true;
		}
		else if (text2 == "END" || text2 == "STOP")
		{
			legacyInstruction.end = true;
		}
		else
		{
			LegacyInstruction._idx = idx;
		}
		string text3 = LegacyInstruction.ParseUntil(':');
		if (text3 != null)
		{
			text3 = text3.Trim();
		}
		LegacyInstruction._idx++;
		if (text3 == null)
		{
			Debug.LogError("Failed to parse any command name in " + LegacyInstruction._str);
			return null;
		}
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text3);
		if (num <= 2863215306U)
		{
			if (num <= 1599629839U)
			{
				if (num <= 531595006U)
				{
					if (num <= 236776447U)
					{
						if (num != 4697379U)
						{
							if (num != 166681459U)
							{
								if (num != 236776447U)
								{
									goto IL_0C36;
								}
								if (!(text3 == "EVENT"))
								{
									goto IL_0C36;
								}
								legacyInstruction.type = LegacyInstruction.Type.Event;
								goto IL_0C38;
							}
							else
							{
								if (!(text3 == "WEATHER"))
								{
									goto IL_0C36;
								}
								legacyInstruction.type = LegacyInstruction.Type.Weather;
								goto IL_0C38;
							}
						}
						else
						{
							if (!(text3 == "FADE OUT"))
							{
								goto IL_0C36;
							}
							goto IL_0B29;
						}
					}
					else if (num != 270819972U)
					{
						if (num != 388347983U)
						{
							if (num != 531595006U)
							{
								goto IL_0C36;
							}
							if (!(text3 == "TELEPORT"))
							{
								goto IL_0C36;
							}
							legacyInstruction.type = LegacyInstruction.Type.Teleport;
							goto IL_0C38;
						}
						else
						{
							if (!(text3 == "ENTER DOOR"))
							{
								goto IL_0C36;
							}
							legacyInstruction.type = LegacyInstruction.Type.EnterExitDoor;
							legacyInstruction.start = true;
							goto IL_0C38;
						}
					}
					else
					{
						if (!(text3 == "PROP DOTS"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.PropDots;
						goto IL_0C38;
					}
				}
				else if (num <= 1258268345U)
				{
					if (num != 886209982U)
					{
						if (num != 1150800334U)
						{
							if (num != 1258268345U)
							{
								goto IL_0C36;
							}
							if (!(text3 == "AUDIO"))
							{
								goto IL_0C36;
							}
							legacyInstruction.type = LegacyInstruction.Type.Audio;
							goto IL_0C38;
						}
						else
						{
							if (!(text3 == "CAMERA"))
							{
								goto IL_0C36;
							}
							legacyInstruction.type = LegacyInstruction.Type.Camera;
							goto IL_0C38;
						}
					}
					else
					{
						if (!(text3 == "STING"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.Sting;
						goto IL_0C38;
					}
				}
				else if (num <= 1432262520U)
				{
					if (num != 1331499164U)
					{
						if (num != 1432262520U)
						{
							goto IL_0C36;
						}
						if (!(text3 == "ACHIEVEMENT"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.Achievement;
						goto IL_0C38;
					}
					else
					{
						if (!(text3 == "FACE"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.Face;
						goto IL_0C38;
					}
				}
				else if (num != 1445689015U)
				{
					if (num != 1599629839U)
					{
						goto IL_0C36;
					}
					if (!(text3 == "WAIT FOR"))
					{
						goto IL_0C36;
					}
				}
				else
				{
					if (!(text3 == "PLAYER EXPLOSION"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.PlayerExplosion;
					goto IL_0C38;
				}
			}
			else if (num <= 2151378887U)
			{
				if (num <= 1899763692U)
				{
					if (num != 1643474128U)
					{
						if (num != 1644334788U)
						{
							if (num != 1899763692U)
							{
								goto IL_0C36;
							}
							if (!(text3 == "INVINCIBLE"))
							{
								goto IL_0C36;
							}
							legacyInstruction.type = LegacyInstruction.Type.Invincible;
							goto IL_0C38;
						}
						else
						{
							if (!(text3 == "POSE"))
							{
								goto IL_0C36;
							}
							legacyInstruction.type = LegacyInstruction.Type.Pose;
							goto IL_0C38;
						}
					}
					else
					{
						if (!(text3 == "ANIMATE"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.Animate;
						goto IL_0C38;
					}
				}
				else if (num <= 1960489865U)
				{
					if (num != 1927802341U)
					{
						if (num != 1960489865U)
						{
							goto IL_0C36;
						}
						if (!(text3 == "SHOW PLAYER"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.HidePlayer;
						legacyInstruction.end = true;
						goto IL_0C38;
					}
					else
					{
						if (!(text3 == "MAP CONFIRM"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.MapConfirm;
						goto IL_0C38;
					}
				}
				else if (num != 2104120452U)
				{
					if (num != 2151378887U)
					{
						goto IL_0C36;
					}
					if (!(text3 == "GAME"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.EndGame;
					goto IL_0C38;
				}
				else
				{
					if (!(text3 == "CAMERA SHAKE"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.CameraShake;
					goto IL_0C38;
				}
			}
			else if (num <= 2704266512U)
			{
				if (num != 2393865632U)
				{
					if (num != 2487290559U)
					{
						if (num != 2704266512U)
						{
							goto IL_0C36;
						}
						if (!(text3 == "RESURRECT"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.Resurrect;
						goto IL_0C38;
					}
					else
					{
						if (!(text3 == "CHAIR LIFT"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.ChairLift;
						goto IL_0C38;
					}
				}
				else if (!(text3 == "WAIT"))
				{
					goto IL_0C36;
				}
			}
			else if (num <= 2769530482U)
			{
				if (num != 2726014167U)
				{
					if (num != 2769530482U)
					{
						goto IL_0C36;
					}
					if (!(text3 == "ZOOM"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.Zoom;
					goto IL_0C38;
				}
				else
				{
					if (!(text3 == "EXIT DOOR"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.EnterExitDoor;
					legacyInstruction.end = true;
					goto IL_0C38;
				}
			}
			else if (num != 2770536342U)
			{
				if (num != 2863215306U)
				{
					goto IL_0C36;
				}
				if (!(text3 == "FADE IN"))
				{
					goto IL_0C36;
				}
				legacyInstruction.type = LegacyInstruction.Type.Fade;
				legacyInstruction.content = "IN";
				goto IL_0C38;
			}
			else
			{
				if (!(text3 == "BLACKBARS"))
				{
					goto IL_0C36;
				}
				goto IL_09D6;
			}
			legacyInstruction.type = LegacyInstruction.Type.WaitFor;
			goto IL_0C38;
		}
		if (num <= 3332987719U)
		{
			if (num <= 3035721941U)
			{
				if (num <= 2916790551U)
				{
					if (num != 2878667150U)
					{
						if (num != 2913454762U)
						{
							if (num != 2916790551U)
							{
								goto IL_0C36;
							}
							if (!(text3 == "FOLLOW PATH"))
							{
								goto IL_0C36;
							}
							legacyInstruction.type = LegacyInstruction.Type.FollowPath;
							goto IL_0C38;
						}
						else
						{
							if (!(text3 == "RUN"))
							{
								goto IL_0C36;
							}
							goto IL_09A6;
						}
					}
					else if (!(text3 == "VIEWPOINT"))
					{
						goto IL_0C36;
					}
				}
				else if (num <= 2979305278U)
				{
					if (num != 2961944181U)
					{
						if (num != 2979305278U)
						{
							goto IL_0C36;
						}
						if (!(text3 == "VIEW POINT"))
						{
							goto IL_0C36;
						}
					}
					else
					{
						if (!(text3 == "FACE LEFT"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.Face;
						legacyInstruction.content = "LEFT";
						goto IL_0C38;
					}
				}
				else if (num != 3011136198U)
				{
					if (num != 3035721941U)
					{
						goto IL_0C36;
					}
					if (!(text3 == "WEATHER PATTERN"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.WeatherPattern;
					goto IL_0C38;
				}
				else
				{
					if (!(text3 == "PROGRESS"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.Progress;
					goto IL_0C38;
				}
				legacyInstruction.type = LegacyInstruction.Type.Viewpoint;
				goto IL_0C38;
			}
			if (num <= 3222075428U)
			{
				if (num != 3072217790U)
				{
					if (num != 3127924133U)
					{
						if (num != 3222075428U)
						{
							goto IL_0C36;
						}
						if (!(text3 == "FINAL MUSIC"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.FinalMusic;
						goto IL_0C38;
					}
					else
					{
						if (!(text3 == "PLAYER CONTROL"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.PlayerControl;
						goto IL_0C38;
					}
				}
				else
				{
					if (!(text3 == "BLACK BARS"))
					{
						goto IL_0C36;
					}
					goto IL_09D6;
				}
			}
			else if (num <= 3257650481U)
			{
				if (num != 3256566454U)
				{
					if (num != 3257650481U)
					{
						goto IL_0C36;
					}
					if (!(text3 == "LOG"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.Log;
					goto IL_0C38;
				}
				else
				{
					if (!(text3 == "HIDE PLAYER"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.HidePlayer;
					goto IL_0C38;
				}
			}
			else if (num != 3329317371U)
			{
				if (num != 3332987719U)
				{
					goto IL_0C36;
				}
				if (!(text3 == "STONE SKIMMING"))
				{
					goto IL_0C36;
				}
				legacyInstruction.type = LegacyInstruction.Type.StoneSkimming;
				goto IL_0C38;
			}
			else
			{
				if (!(text3 == "BOAT"))
				{
					goto IL_0C36;
				}
				legacyInstruction.type = LegacyInstruction.Type.Boat;
				goto IL_0C38;
			}
		}
		else if (num <= 3792507828U)
		{
			if (num <= 3465769862U)
			{
				if (num != 3412775364U)
				{
					if (num != 3414063406U)
					{
						if (num != 3465769862U)
						{
							goto IL_0C36;
						}
						if (!(text3 == "BELLY WRIGGLE"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.BellyWriggle;
						goto IL_0C38;
					}
					else
					{
						if (!(text3 == "FACE RIGHT"))
						{
							goto IL_0C36;
						}
						legacyInstruction.type = LegacyInstruction.Type.Face;
						legacyInstruction.content = "RIGHT";
						goto IL_0C38;
					}
				}
				else
				{
					if (!(text3 == "PARTICLES"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.Particles;
					goto IL_0C38;
				}
			}
			else if (num <= 3526321475U)
			{
				if (num != 3503868574U)
				{
					if (num != 3526321475U)
					{
						goto IL_0C36;
					}
					if (!(text3 == "RUN TO"))
					{
						goto IL_0C36;
					}
				}
				else
				{
					if (!(text3 == "ZIP LINE"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.ZipLine;
					goto IL_0C38;
				}
			}
			else if (num != 3743593777U)
			{
				if (num != 3792507828U)
				{
					goto IL_0C36;
				}
				if (!(text3 == "PEAK BANNER"))
				{
					goto IL_0C36;
				}
				legacyInstruction.type = LegacyInstruction.Type.PeakBanner;
				goto IL_0C38;
			}
			else
			{
				if (!(text3 == "ERROR"))
				{
					goto IL_0C36;
				}
				legacyInstruction.type = LegacyInstruction.Type.Error;
				goto IL_0C38;
			}
		}
		else if (num <= 3820253647U)
		{
			if (num != 3802528120U)
			{
				if (num != 3808339971U)
				{
					if (num != 3820253647U)
					{
						goto IL_0C36;
					}
					if (!(text3 == "EAGLE"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.Eagle;
					goto IL_0C38;
				}
				else
				{
					if (!(text3 == "DEATH AUDIO"))
					{
						goto IL_0C36;
					}
					legacyInstruction.type = LegacyInstruction.Type.DeathAudio;
					goto IL_0C38;
				}
			}
			else
			{
				if (!(text3 == "WAIT UNTIL"))
				{
					goto IL_0C36;
				}
				legacyInstruction.type = LegacyInstruction.Type.WaitUntil;
				goto IL_0C38;
			}
		}
		else if (num <= 4091553814U)
		{
			if (num != 4015962131U)
			{
				if (num != 4091553814U)
				{
					goto IL_0C36;
				}
				if (!(text3 == "GET MAPS"))
				{
					goto IL_0C36;
				}
				legacyInstruction.type = LegacyInstruction.Type.GetMaps;
				goto IL_0C38;
			}
			else
			{
				if (!(text3 == "FADE"))
				{
					goto IL_0C36;
				}
				goto IL_0B29;
			}
		}
		else if (num != 4143908101U)
		{
			if (num != 4257214842U)
			{
				goto IL_0C36;
			}
			if (!(text3 == "BACKGROUND MUSIC"))
			{
				goto IL_0C36;
			}
			legacyInstruction.type = LegacyInstruction.Type.BackgroundMusic;
			goto IL_0C38;
		}
		else
		{
			if (!(text3 == "SHELTER CAM"))
			{
				goto IL_0C36;
			}
			legacyInstruction.type = LegacyInstruction.Type.ShelterCam;
			goto IL_0C38;
		}
		IL_09A6:
		legacyInstruction.type = LegacyInstruction.Type.Run;
		goto IL_0C38;
		IL_09D6:
		legacyInstruction.type = LegacyInstruction.Type.BlackBars;
		goto IL_0C38;
		IL_0B29:
		legacyInstruction.type = LegacyInstruction.Type.Fade;
		legacyInstruction.content = "OUT";
		goto IL_0C38;
		IL_0C36:
		return null;
		IL_0C38:
		LegacyInstruction.Whitespace();
		if (legacyInstruction.content == null)
		{
			legacyInstruction.content = LegacyInstruction.ParseRemaining();
		}
		if (legacyInstruction.content != null)
		{
			float.TryParse(legacyInstruction.content, out legacyInstruction.floatContent);
		}
		return legacyInstruction;
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x000214AC File Offset: 0x0001F6AC
	private static string ParseRemaining()
	{
		if (LegacyInstruction._idx < LegacyInstruction._str.Length)
		{
			string text = LegacyInstruction._str.Substring(LegacyInstruction._idx).Trim();
			LegacyInstruction._idx = LegacyInstruction._str.Length;
			return text;
		}
		return "";
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x000214E8 File Offset: 0x0001F6E8
	private static string ParseUntil(char untilC)
	{
		int idx = LegacyInstruction._idx;
		while (LegacyInstruction._idx < LegacyInstruction._str.Length && LegacyInstruction._str[LegacyInstruction._idx] != untilC)
		{
			LegacyInstruction._idx++;
		}
		if (LegacyInstruction._idx > idx)
		{
			return LegacyInstruction._str.Substring(idx, LegacyInstruction._idx - idx);
		}
		return null;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00021548 File Offset: 0x0001F748
	private static string ParseWords(bool multiple = false)
	{
		int idx = LegacyInstruction._idx;
		bool flag = false;
		while (LegacyInstruction._idx < LegacyInstruction._str.Length)
		{
			char c = LegacyInstruction._str[LegacyInstruction._idx];
			if (multiple && c == ' ')
			{
				LegacyInstruction._idx++;
			}
			else
			{
				if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9') && c != '_')
				{
					break;
				}
				flag = true;
				LegacyInstruction._idx++;
			}
		}
		if (LegacyInstruction._idx > idx && flag)
		{
			return LegacyInstruction._str.Substring(idx, LegacyInstruction._idx - idx).Trim();
		}
		return null;
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x000215F1 File Offset: 0x0001F7F1
	private static void Whitespace()
	{
		LegacyInstruction.ParseAny(" \t");
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x00021600 File Offset: 0x0001F800
	private static int ParseAny(string characters)
	{
		int num = 0;
		while (LegacyInstruction._idx < LegacyInstruction._str.Length)
		{
			char c = LegacyInstruction._str[LegacyInstruction._idx];
			bool flag = false;
			foreach (char c2 in characters)
			{
				if (c == c2)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
			LegacyInstruction._idx++;
			num++;
		}
		return num;
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x00021672 File Offset: 0x0001F872
	private static bool ParseSingle(char c)
	{
		if (LegacyInstruction._idx >= LegacyInstruction._str.Length)
		{
			return false;
		}
		if (LegacyInstruction._str[LegacyInstruction._idx] == c)
		{
			LegacyInstruction._idx++;
			return true;
		}
		return false;
	}

	// Token: 0x0400054B RID: 1355
	public LegacyInstruction.Type type;

	// Token: 0x0400054C RID: 1356
	public bool start;

	// Token: 0x0400054D RID: 1357
	public bool end;

	// Token: 0x0400054E RID: 1358
	public string characterName;

	// Token: 0x0400054F RID: 1359
	public string content;

	// Token: 0x04000550 RID: 1360
	public string audioFilename;

	// Token: 0x04000551 RID: 1361
	public float floatContent;

	// Token: 0x04000552 RID: 1362
	private static int _idx;

	// Token: 0x04000553 RID: 1363
	private static string _str;

	// Token: 0x020002A7 RID: 679
	public enum Type
	{
		// Token: 0x04001578 RID: 5496
		NarratorDialogue,
		// Token: 0x04001579 RID: 5497
		CharacterDialogue,
		// Token: 0x0400157A RID: 5498
		Run,
		// Token: 0x0400157B RID: 5499
		Camera,
		// Token: 0x0400157C RID: 5500
		CameraShake,
		// Token: 0x0400157D RID: 5501
		Zoom,
		// Token: 0x0400157E RID: 5502
		Audio,
		// Token: 0x0400157F RID: 5503
		Sting,
		// Token: 0x04001580 RID: 5504
		BackgroundMusic,
		// Token: 0x04001581 RID: 5505
		StoneSkimming,
		// Token: 0x04001582 RID: 5506
		BellyWriggle,
		// Token: 0x04001583 RID: 5507
		BlackBars,
		// Token: 0x04001584 RID: 5508
		Viewpoint,
		// Token: 0x04001585 RID: 5509
		Progress,
		// Token: 0x04001586 RID: 5510
		ShelterCam,
		// Token: 0x04001587 RID: 5511
		PlayerControl,
		// Token: 0x04001588 RID: 5512
		HidePlayer,
		// Token: 0x04001589 RID: 5513
		EnterExitDoor,
		// Token: 0x0400158A RID: 5514
		WaitUntil,
		// Token: 0x0400158B RID: 5515
		WaitFor,
		// Token: 0x0400158C RID: 5516
		FollowPath,
		// Token: 0x0400158D RID: 5517
		Animate,
		// Token: 0x0400158E RID: 5518
		Pose,
		// Token: 0x0400158F RID: 5519
		Face,
		// Token: 0x04001590 RID: 5520
		Teleport,
		// Token: 0x04001591 RID: 5521
		Fade,
		// Token: 0x04001592 RID: 5522
		GetMaps,
		// Token: 0x04001593 RID: 5523
		MapConfirm,
		// Token: 0x04001594 RID: 5524
		Invincible,
		// Token: 0x04001595 RID: 5525
		Resurrect,
		// Token: 0x04001596 RID: 5526
		PeakBanner,
		// Token: 0x04001597 RID: 5527
		Weather,
		// Token: 0x04001598 RID: 5528
		WeatherPattern,
		// Token: 0x04001599 RID: 5529
		Eagle,
		// Token: 0x0400159A RID: 5530
		ChairLift,
		// Token: 0x0400159B RID: 5531
		Boat,
		// Token: 0x0400159C RID: 5532
		ZipLine,
		// Token: 0x0400159D RID: 5533
		FinalMusic,
		// Token: 0x0400159E RID: 5534
		EndGame,
		// Token: 0x0400159F RID: 5535
		Achievement,
		// Token: 0x040015A0 RID: 5536
		Particles,
		// Token: 0x040015A1 RID: 5537
		PlayerExplosion,
		// Token: 0x040015A2 RID: 5538
		PropDots,
		// Token: 0x040015A3 RID: 5539
		DeathAudio,
		// Token: 0x040015A4 RID: 5540
		Event,
		// Token: 0x040015A5 RID: 5541
		Error,
		// Token: 0x040015A6 RID: 5542
		Log
	}
}
