using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Ink.Runtime
{
	// Token: 0x02000029 RID: 41
	public static class SimpleJson
	{
		// Token: 0x06000243 RID: 579 RVA: 0x0000D808 File Offset: 0x0000BA08
		public static Dictionary<string, object> TextToDictionary(string text)
		{
			return new SimpleJson.Reader(text).ToDictionary();
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000D815 File Offset: 0x0000BA15
		public static List<object> TextToArray(string text)
		{
			return new SimpleJson.Reader(text).ToArray();
		}

		// Token: 0x02000094 RID: 148
		private class Reader
		{
			// Token: 0x060005AF RID: 1455 RVA: 0x0001BDB3 File Offset: 0x00019FB3
			public Reader(string text)
			{
				this._text = text;
				this._offset = 0;
				this.SkipWhitespace();
				this._rootObject = this.ReadObject();
			}

			// Token: 0x060005B0 RID: 1456 RVA: 0x0001BDDB File Offset: 0x00019FDB
			public Dictionary<string, object> ToDictionary()
			{
				return (Dictionary<string, object>)this._rootObject;
			}

			// Token: 0x060005B1 RID: 1457 RVA: 0x0001BDE8 File Offset: 0x00019FE8
			public List<object> ToArray()
			{
				return (List<object>)this._rootObject;
			}

			// Token: 0x060005B2 RID: 1458 RVA: 0x0001BDF5 File Offset: 0x00019FF5
			private bool IsNumberChar(char c)
			{
				return (c >= '0' && c <= '9') || c == '.' || c == '-' || c == '+' || c == 'E' || c == 'e';
			}

			// Token: 0x060005B3 RID: 1459 RVA: 0x0001BE1C File Offset: 0x0001A01C
			private bool IsFirstNumberChar(char c)
			{
				return (c >= '0' && c <= '9') || c == '-' || c == '+';
			}

			// Token: 0x060005B4 RID: 1460 RVA: 0x0001BE34 File Offset: 0x0001A034
			private object ReadObject()
			{
				char c = this._text[this._offset];
				if (c == '{')
				{
					return this.ReadDictionary();
				}
				if (c == '[')
				{
					return this.ReadArray();
				}
				if (c == '"')
				{
					return this.ReadString();
				}
				if (this.IsFirstNumberChar(c))
				{
					return this.ReadNumber();
				}
				if (this.TryRead("true"))
				{
					return true;
				}
				if (this.TryRead("false"))
				{
					return false;
				}
				if (this.TryRead("null"))
				{
					return null;
				}
				throw new Exception("Unhandled object type in JSON: " + this._text.Substring(this._offset, 30));
			}

			// Token: 0x060005B5 RID: 1461 RVA: 0x0001BEE0 File Offset: 0x0001A0E0
			private Dictionary<string, object> ReadDictionary()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.Expect("{");
				this.SkipWhitespace();
				if (this.TryRead("}"))
				{
					return dictionary;
				}
				do
				{
					this.SkipWhitespace();
					string text = this.ReadString();
					this.Expect(text != null, "dictionary key");
					this.SkipWhitespace();
					this.Expect(":");
					this.SkipWhitespace();
					object obj = this.ReadObject();
					this.Expect(obj != null, "dictionary value");
					dictionary[text] = obj;
					this.SkipWhitespace();
				}
				while (this.TryRead(","));
				this.Expect("}");
				return dictionary;
			}

			// Token: 0x060005B6 RID: 1462 RVA: 0x0001BF84 File Offset: 0x0001A184
			private List<object> ReadArray()
			{
				List<object> list = new List<object>();
				this.Expect("[");
				this.SkipWhitespace();
				if (this.TryRead("]"))
				{
					return list;
				}
				do
				{
					this.SkipWhitespace();
					object obj = this.ReadObject();
					list.Add(obj);
					this.SkipWhitespace();
				}
				while (this.TryRead(","));
				this.Expect("]");
				return list;
			}

			// Token: 0x060005B7 RID: 1463 RVA: 0x0001BFEC File Offset: 0x0001A1EC
			private string ReadString()
			{
				this.Expect("\"");
				StringBuilder stringBuilder = new StringBuilder();
				while (this._offset < this._text.Length)
				{
					char c = this._text[this._offset];
					if (c == '\\')
					{
						this._offset++;
						if (this._offset >= this._text.Length)
						{
							throw new Exception("Unexpected EOF while reading string");
						}
						c = this._text[this._offset];
						if (c <= '\\')
						{
							if (c == '"' || c == '/' || c == '\\')
							{
								stringBuilder.Append(c);
								goto IL_019A;
							}
						}
						else if (c <= 'f')
						{
							if (c == 'b' || c == 'f')
							{
								goto IL_019A;
							}
						}
						else
						{
							if (c == 'n')
							{
								stringBuilder.Append('\n');
								goto IL_019A;
							}
							switch (c)
							{
							case 'r':
								goto IL_019A;
							case 't':
								stringBuilder.Append('\t');
								goto IL_019A;
							case 'u':
							{
								if (this._offset + 4 >= this._text.Length)
								{
									throw new Exception("Unexpected EOF while reading string");
								}
								int num;
								if (int.TryParse(this._text.Substring(this._offset + 1, 4), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out num))
								{
									stringBuilder.Append((char)num);
									this._offset += 4;
									goto IL_019A;
								}
								throw new Exception("Invalid Unicode escape character at offset " + (this._offset - 1).ToString());
							}
							}
						}
						throw new Exception("Invalid Unicode escape character at offset " + (this._offset - 1).ToString());
					}
					else
					{
						if (c == '"')
						{
							break;
						}
						stringBuilder.Append(c);
					}
					IL_019A:
					this._offset++;
				}
				this.Expect("\"");
				return stringBuilder.ToString();
			}

			// Token: 0x060005B8 RID: 1464 RVA: 0x0001C1C8 File Offset: 0x0001A3C8
			private object ReadNumber()
			{
				int offset = this._offset;
				bool flag = false;
				while (this._offset < this._text.Length)
				{
					char c = this._text[this._offset];
					if (c == '.' || c == 'e' || c == 'E')
					{
						flag = true;
					}
					if (!this.IsNumberChar(c))
					{
						break;
					}
					this._offset++;
				}
				string text = this._text.Substring(offset, this._offset - offset);
				int num2;
				if (flag)
				{
					float num;
					if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
					{
						return num;
					}
				}
				else if (int.TryParse(text, out num2))
				{
					return num2;
				}
				throw new Exception("Failed to parse number value: " + text);
			}

			// Token: 0x060005B9 RID: 1465 RVA: 0x0001C284 File Offset: 0x0001A484
			private bool TryRead(string textToRead)
			{
				if (this._offset + textToRead.Length > this._text.Length)
				{
					return false;
				}
				for (int i = 0; i < textToRead.Length; i++)
				{
					if (textToRead[i] != this._text[this._offset + i])
					{
						return false;
					}
				}
				this._offset += textToRead.Length;
				return true;
			}

			// Token: 0x060005BA RID: 1466 RVA: 0x0001C2F0 File Offset: 0x0001A4F0
			private void Expect(string expectedStr)
			{
				if (!this.TryRead(expectedStr))
				{
					this.Expect(false, expectedStr);
				}
			}

			// Token: 0x060005BB RID: 1467 RVA: 0x0001C303 File Offset: 0x0001A503
			private void Expect(bool condition, string message = null)
			{
				if (!condition)
				{
					if (message == null)
					{
						message = "Unexpected token";
					}
					else
					{
						message = "Expected " + message;
					}
					message = message + " at offset " + this._offset.ToString();
					throw new Exception(message);
				}
			}

			// Token: 0x060005BC RID: 1468 RVA: 0x0001C340 File Offset: 0x0001A540
			private void SkipWhitespace()
			{
				while (this._offset < this._text.Length)
				{
					char c = this._text[this._offset];
					if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
					{
						break;
					}
					this._offset++;
				}
			}

			// Token: 0x0400026A RID: 618
			private string _text;

			// Token: 0x0400026B RID: 619
			private int _offset;

			// Token: 0x0400026C RID: 620
			private object _rootObject;
		}

		// Token: 0x02000095 RID: 149
		public class Writer
		{
			// Token: 0x060005BD RID: 1469 RVA: 0x0001C396 File Offset: 0x0001A596
			public Writer()
			{
				this._writer = new StringWriter();
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x0001C3B4 File Offset: 0x0001A5B4
			public Writer(Stream stream)
			{
				this._writer = new StreamWriter(stream, Encoding.UTF8);
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x0001C3D8 File Offset: 0x0001A5D8
			public void Clear()
			{
				StringWriter stringWriter = this._writer as StringWriter;
				if (stringWriter == null)
				{
					throw new NotSupportedException("Writer.Clear() is only supported for the StringWriter variant, not for streams");
				}
				stringWriter.GetStringBuilder().Clear();
			}

			// Token: 0x060005C0 RID: 1472 RVA: 0x0001C3FE File Offset: 0x0001A5FE
			public void WriteObject(Action<SimpleJson.Writer> inner)
			{
				this.WriteObjectStart();
				inner(this);
				this.WriteObjectEnd();
			}

			// Token: 0x060005C1 RID: 1473 RVA: 0x0001C414 File Offset: 0x0001A614
			public void WriteObjectStart()
			{
				this.StartNewObject(true);
				this._stateStack.Push(new SimpleJson.Writer.StateElement
				{
					type = SimpleJson.Writer.State.Object
				});
				this._writer.Write("{");
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x0001C454 File Offset: 0x0001A654
			public void WriteObjectEnd()
			{
				this._writer.Write("}");
				this._stateStack.Pop();
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x0001C472 File Offset: 0x0001A672
			public void WriteProperty(string name, Action<SimpleJson.Writer> inner)
			{
				this.WriteProperty<string>(name, inner);
			}

			// Token: 0x060005C4 RID: 1476 RVA: 0x0001C47C File Offset: 0x0001A67C
			public void WriteProperty(int id, Action<SimpleJson.Writer> inner)
			{
				this.WriteProperty<int>(id, inner);
			}

			// Token: 0x060005C5 RID: 1477 RVA: 0x0001C486 File Offset: 0x0001A686
			public void WriteProperty(string name, string content)
			{
				this.WritePropertyStart(name);
				this.Write(content, true);
				this.WritePropertyEnd();
			}

			// Token: 0x060005C6 RID: 1478 RVA: 0x0001C49D File Offset: 0x0001A69D
			public void WriteProperty(string name, int content)
			{
				this.WritePropertyStart(name);
				this.Write(content);
				this.WritePropertyEnd();
			}

			// Token: 0x060005C7 RID: 1479 RVA: 0x0001C4B3 File Offset: 0x0001A6B3
			public void WriteProperty(string name, bool content)
			{
				this.WritePropertyStart(name);
				this.Write(content);
				this.WritePropertyEnd();
			}

			// Token: 0x060005C8 RID: 1480 RVA: 0x0001C4C9 File Offset: 0x0001A6C9
			public void WritePropertyStart(string name)
			{
				this.WritePropertyStart<string>(name);
			}

			// Token: 0x060005C9 RID: 1481 RVA: 0x0001C4D2 File Offset: 0x0001A6D2
			public void WritePropertyStart(int id)
			{
				this.WritePropertyStart<int>(id);
			}

			// Token: 0x060005CA RID: 1482 RVA: 0x0001C4DB File Offset: 0x0001A6DB
			public void WritePropertyEnd()
			{
				this._stateStack.Pop();
			}

			// Token: 0x060005CB RID: 1483 RVA: 0x0001C4EC File Offset: 0x0001A6EC
			public void WritePropertyNameStart()
			{
				if (this.childCount > 0)
				{
					this._writer.Write(",");
				}
				this._writer.Write("\"");
				this.IncrementChildCount();
				this._stateStack.Push(new SimpleJson.Writer.StateElement
				{
					type = SimpleJson.Writer.State.Property
				});
				this._stateStack.Push(new SimpleJson.Writer.StateElement
				{
					type = SimpleJson.Writer.State.PropertyName
				});
			}

			// Token: 0x060005CC RID: 1484 RVA: 0x0001C560 File Offset: 0x0001A760
			public void WritePropertyNameEnd()
			{
				this._writer.Write("\":");
				this._stateStack.Pop();
			}

			// Token: 0x060005CD RID: 1485 RVA: 0x0001C57E File Offset: 0x0001A77E
			public void WritePropertyNameInner(string str)
			{
				this._writer.Write(str);
			}

			// Token: 0x060005CE RID: 1486 RVA: 0x0001C58C File Offset: 0x0001A78C
			private void WritePropertyStart<T>(T name)
			{
				if (this.childCount > 0)
				{
					this._writer.Write(",");
				}
				this._writer.Write("\"");
				this._writer.Write(name);
				this._writer.Write("\":");
				this.IncrementChildCount();
				this._stateStack.Push(new SimpleJson.Writer.StateElement
				{
					type = SimpleJson.Writer.State.Property
				});
			}

			// Token: 0x060005CF RID: 1487 RVA: 0x0001C605 File Offset: 0x0001A805
			private void WriteProperty<T>(T name, Action<SimpleJson.Writer> inner)
			{
				this.WritePropertyStart<T>(name);
				inner(this);
				this.WritePropertyEnd();
			}

			// Token: 0x060005D0 RID: 1488 RVA: 0x0001C61C File Offset: 0x0001A81C
			public void WriteArrayStart()
			{
				this.StartNewObject(true);
				this._stateStack.Push(new SimpleJson.Writer.StateElement
				{
					type = SimpleJson.Writer.State.Array
				});
				this._writer.Write("[");
			}

			// Token: 0x060005D1 RID: 1489 RVA: 0x0001C65C File Offset: 0x0001A85C
			public void WriteArrayEnd()
			{
				this._writer.Write("]");
				this._stateStack.Pop();
			}

			// Token: 0x060005D2 RID: 1490 RVA: 0x0001C67A File Offset: 0x0001A87A
			public void Write(int i)
			{
				this.StartNewObject(false);
				this._writer.Write(i);
			}

			// Token: 0x060005D3 RID: 1491 RVA: 0x0001C690 File Offset: 0x0001A890
			public void Write(float f)
			{
				this.StartNewObject(false);
				string text = f.ToString(CultureInfo.InvariantCulture);
				if (text == "Infinity")
				{
					this._writer.Write("3.4E+38");
					return;
				}
				if (text == "-Infinity")
				{
					this._writer.Write("-3.4E+38");
					return;
				}
				if (text == "NaN")
				{
					this._writer.Write("0.0");
					return;
				}
				this._writer.Write(text);
				if (!text.Contains(".") && !text.Contains("E"))
				{
					this._writer.Write(".0");
				}
			}

			// Token: 0x060005D4 RID: 1492 RVA: 0x0001C744 File Offset: 0x0001A944
			public void Write(string str, bool escape = true)
			{
				this.StartNewObject(false);
				this._writer.Write("\"");
				if (escape)
				{
					this.WriteEscapedString(str);
				}
				else
				{
					this._writer.Write(str);
				}
				this._writer.Write("\"");
			}

			// Token: 0x060005D5 RID: 1493 RVA: 0x0001C790 File Offset: 0x0001A990
			public void Write(bool b)
			{
				this.StartNewObject(false);
				this._writer.Write(b ? "true" : "false");
			}

			// Token: 0x060005D6 RID: 1494 RVA: 0x0001C7B3 File Offset: 0x0001A9B3
			public void WriteNull()
			{
				this.StartNewObject(false);
				this._writer.Write("null");
			}

			// Token: 0x060005D7 RID: 1495 RVA: 0x0001C7CC File Offset: 0x0001A9CC
			public void WriteStringStart()
			{
				this.StartNewObject(false);
				this._stateStack.Push(new SimpleJson.Writer.StateElement
				{
					type = SimpleJson.Writer.State.String
				});
				this._writer.Write("\"");
			}

			// Token: 0x060005D8 RID: 1496 RVA: 0x0001C80C File Offset: 0x0001AA0C
			public void WriteStringEnd()
			{
				this._writer.Write("\"");
				this._stateStack.Pop();
			}

			// Token: 0x060005D9 RID: 1497 RVA: 0x0001C82A File Offset: 0x0001AA2A
			public void WriteStringInner(string str, bool escape = true)
			{
				if (escape)
				{
					this.WriteEscapedString(str);
					return;
				}
				this._writer.Write(str);
			}

			// Token: 0x060005DA RID: 1498 RVA: 0x0001C844 File Offset: 0x0001AA44
			private void WriteEscapedString(string str)
			{
				foreach (char c in str)
				{
					if (c < ' ')
					{
						if (c != '\t')
						{
							if (c == '\n')
							{
								this._writer.Write("\\n");
							}
						}
						else
						{
							this._writer.Write("\\t");
						}
					}
					else if (c == '"' || c == '\\')
					{
						this._writer.Write("\\");
						this._writer.Write(c);
					}
					else
					{
						this._writer.Write(c);
					}
				}
			}

			// Token: 0x060005DB RID: 1499 RVA: 0x0001C8D4 File Offset: 0x0001AAD4
			private void StartNewObject(bool container)
			{
				if (this.state == SimpleJson.Writer.State.Array && this.childCount > 0)
				{
					this._writer.Write(",");
				}
				SimpleJson.Writer.State state = this.state;
				if (this.state == SimpleJson.Writer.State.Array || this.state == SimpleJson.Writer.State.Property)
				{
					this.IncrementChildCount();
				}
			}

			// Token: 0x17000141 RID: 321
			// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001C926 File Offset: 0x0001AB26
			private SimpleJson.Writer.State state
			{
				get
				{
					if (this._stateStack.Count > 0)
					{
						return this._stateStack.Peek().type;
					}
					return SimpleJson.Writer.State.None;
				}
			}

			// Token: 0x17000142 RID: 322
			// (get) Token: 0x060005DD RID: 1501 RVA: 0x0001C948 File Offset: 0x0001AB48
			private int childCount
			{
				get
				{
					if (this._stateStack.Count > 0)
					{
						return this._stateStack.Peek().childCount;
					}
					return 0;
				}
			}

			// Token: 0x060005DE RID: 1502 RVA: 0x0001C96C File Offset: 0x0001AB6C
			private void IncrementChildCount()
			{
				SimpleJson.Writer.StateElement stateElement = this._stateStack.Pop();
				stateElement.childCount++;
				this._stateStack.Push(stateElement);
			}

			// Token: 0x060005DF RID: 1503 RVA: 0x0001C99D File Offset: 0x0001AB9D
			[Conditional("DEBUG")]
			private void Assert(bool condition)
			{
				if (!condition)
				{
					throw new Exception("Assert failed while writing JSON");
				}
			}

			// Token: 0x060005E0 RID: 1504 RVA: 0x0001C9AD File Offset: 0x0001ABAD
			public override string ToString()
			{
				return this._writer.ToString();
			}

			// Token: 0x0400026D RID: 621
			private Stack<SimpleJson.Writer.StateElement> _stateStack = new Stack<SimpleJson.Writer.StateElement>();

			// Token: 0x0400026E RID: 622
			private TextWriter _writer;

			// Token: 0x020000B2 RID: 178
			private enum State
			{
				// Token: 0x040002B3 RID: 691
				None,
				// Token: 0x040002B4 RID: 692
				Object,
				// Token: 0x040002B5 RID: 693
				Array,
				// Token: 0x040002B6 RID: 694
				Property,
				// Token: 0x040002B7 RID: 695
				PropertyName,
				// Token: 0x040002B8 RID: 696
				String
			}

			// Token: 0x020000B3 RID: 179
			private struct StateElement
			{
				// Token: 0x040002B9 RID: 697
				public SimpleJson.Writer.State type;

				// Token: 0x040002BA RID: 698
				public int childCount;
			}
		}
	}
}
