using System;

namespace UnityEngine.Assertions
{
	// Token: 0x02000486 RID: 1158
	internal class AssertionMessageUtil
	{
		// Token: 0x06002903 RID: 10499 RVA: 0x00043BF8 File Offset: 0x00041DF8
		public static string GetMessage(string failureMessage)
		{
			return UnityString.Format("{0} {1}", new object[] { "Assertion failure.", failureMessage });
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x00043C28 File Offset: 0x00041E28
		public static string GetMessage(string failureMessage, string expected)
		{
			return AssertionMessageUtil.GetMessage(UnityString.Format("{0}{1}{2} {3}", new object[]
			{
				failureMessage,
				Environment.NewLine,
				"Expected:",
				expected
			}));
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x00043C68 File Offset: 0x00041E68
		public static string GetEqualityMessage(object actual, object expected, bool expectEqual)
		{
			return AssertionMessageUtil.GetMessage(UnityString.Format("Values are {0}equal.", new object[] { expectEqual ? "not " : "" }), UnityString.Format("{0} {2} {1}", new object[]
			{
				actual,
				expected,
				expectEqual ? "==" : "!="
			}));
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x00043CCC File Offset: 0x00041ECC
		public static string NullFailureMessage(object value, bool expectNull)
		{
			return AssertionMessageUtil.GetMessage(UnityString.Format("Value was {0}Null", new object[] { expectNull ? "not " : "" }), UnityString.Format("Value was {0}Null", new object[] { expectNull ? "" : "not " }));
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x00043D28 File Offset: 0x00041F28
		public static string BooleanFailureMessage(bool expected)
		{
			return AssertionMessageUtil.GetMessage("Value was " + (!expected).ToString(), expected.ToString());
		}

		// Token: 0x04000F98 RID: 3992
		private const string k_Expected = "Expected:";

		// Token: 0x04000F99 RID: 3993
		private const string k_AssertionFailed = "Assertion failure.";
	}
}
