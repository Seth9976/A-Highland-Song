using System;
using System.Collections.Generic;
using UnityEngine.Internal;

namespace UnityEngine.Device
{
	// Token: 0x02000450 RID: 1104
	public static class Screen
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002725 RID: 10021 RVA: 0x0004108E File Offset: 0x0003F28E
		// (set) Token: 0x06002726 RID: 10022 RVA: 0x00041095 File Offset: 0x0003F295
		public static float brightness
		{
			get
			{
				return Screen.brightness;
			}
			set
			{
				Screen.brightness = value;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002727 RID: 10023 RVA: 0x0004109E File Offset: 0x0003F29E
		// (set) Token: 0x06002728 RID: 10024 RVA: 0x000410A5 File Offset: 0x0003F2A5
		public static bool autorotateToLandscapeLeft
		{
			get
			{
				return Screen.autorotateToLandscapeLeft;
			}
			set
			{
				Screen.autorotateToLandscapeLeft = value;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002729 RID: 10025 RVA: 0x000410AE File Offset: 0x0003F2AE
		// (set) Token: 0x0600272A RID: 10026 RVA: 0x000410B5 File Offset: 0x0003F2B5
		public static bool autorotateToLandscapeRight
		{
			get
			{
				return Screen.autorotateToLandscapeRight;
			}
			set
			{
				Screen.autorotateToLandscapeRight = value;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x0600272B RID: 10027 RVA: 0x000410BE File Offset: 0x0003F2BE
		// (set) Token: 0x0600272C RID: 10028 RVA: 0x000410C5 File Offset: 0x0003F2C5
		public static bool autorotateToPortrait
		{
			get
			{
				return Screen.autorotateToPortrait;
			}
			set
			{
				Screen.autorotateToPortrait = value;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x0600272D RID: 10029 RVA: 0x000410CE File Offset: 0x0003F2CE
		// (set) Token: 0x0600272E RID: 10030 RVA: 0x000410D5 File Offset: 0x0003F2D5
		public static bool autorotateToPortraitUpsideDown
		{
			get
			{
				return Screen.autorotateToPortraitUpsideDown;
			}
			set
			{
				Screen.autorotateToPortraitUpsideDown = value;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x0600272F RID: 10031 RVA: 0x000410DE File Offset: 0x0003F2DE
		public static Resolution currentResolution
		{
			get
			{
				return Screen.currentResolution;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x000410E5 File Offset: 0x0003F2E5
		public static Rect[] cutouts
		{
			get
			{
				return Screen.cutouts;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002731 RID: 10033 RVA: 0x000410EC File Offset: 0x0003F2EC
		public static float dpi
		{
			get
			{
				return Screen.dpi;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x000410F3 File Offset: 0x0003F2F3
		// (set) Token: 0x06002733 RID: 10035 RVA: 0x000410FA File Offset: 0x0003F2FA
		public static bool fullScreen
		{
			get
			{
				return Screen.fullScreen;
			}
			set
			{
				Screen.fullScreen = value;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x00041103 File Offset: 0x0003F303
		// (set) Token: 0x06002735 RID: 10037 RVA: 0x0004110A File Offset: 0x0003F30A
		public static FullScreenMode fullScreenMode
		{
			get
			{
				return Screen.fullScreenMode;
			}
			set
			{
				Screen.fullScreenMode = value;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x00041113 File Offset: 0x0003F313
		public static int height
		{
			get
			{
				return Screen.height;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002737 RID: 10039 RVA: 0x0004111A File Offset: 0x0003F31A
		public static int width
		{
			get
			{
				return Screen.width;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x00041121 File Offset: 0x0003F321
		// (set) Token: 0x06002739 RID: 10041 RVA: 0x00041128 File Offset: 0x0003F328
		public static ScreenOrientation orientation
		{
			get
			{
				return Screen.orientation;
			}
			set
			{
				Screen.orientation = value;
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x0600273A RID: 10042 RVA: 0x00041131 File Offset: 0x0003F331
		public static Resolution[] resolutions
		{
			get
			{
				return Screen.resolutions;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600273B RID: 10043 RVA: 0x00041138 File Offset: 0x0003F338
		public static Rect safeArea
		{
			get
			{
				return Screen.safeArea;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x0600273C RID: 10044 RVA: 0x0004113F File Offset: 0x0003F33F
		// (set) Token: 0x0600273D RID: 10045 RVA: 0x00041146 File Offset: 0x0003F346
		public static int sleepTimeout
		{
			get
			{
				return Screen.sleepTimeout;
			}
			set
			{
				Screen.sleepTimeout = value;
			}
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x0004114F File Offset: 0x0003F34F
		public static void SetResolution(int width, int height, FullScreenMode fullscreenMode, [DefaultValue("0")] int preferredRefreshRate)
		{
			Screen.SetResolution(width, height, fullscreenMode, preferredRefreshRate);
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x0004115C File Offset: 0x0003F35C
		public static void SetResolution(int width, int height, FullScreenMode fullscreenMode)
		{
			Screen.SetResolution(width, height, fullscreenMode, 0);
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x00041169 File Offset: 0x0003F369
		public static void SetResolution(int width, int height, bool fullscreen, [DefaultValue("0")] int preferredRefreshRate)
		{
			Screen.SetResolution(width, height, fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, preferredRefreshRate);
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x0004117C File Offset: 0x0003F37C
		public static void SetResolution(int width, int height, bool fullscreen)
		{
			Screen.SetResolution(width, height, fullscreen, 0);
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002742 RID: 10050 RVA: 0x00041189 File Offset: 0x0003F389
		public static Vector2Int mainWindowPosition
		{
			get
			{
				return Screen.mainWindowPosition;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002743 RID: 10051 RVA: 0x00041190 File Offset: 0x0003F390
		public static DisplayInfo mainWindowDisplayInfo
		{
			get
			{
				return Screen.mainWindowDisplayInfo;
			}
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x00041197 File Offset: 0x0003F397
		public static void GetDisplayLayout(List<DisplayInfo> displayLayout)
		{
			Screen.GetDisplayLayout(displayLayout);
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000411A0 File Offset: 0x0003F3A0
		public static AsyncOperation MoveMainWindowTo(in DisplayInfo display, Vector2Int position)
		{
			return Screen.MoveMainWindowTo(in display, position);
		}
	}
}
