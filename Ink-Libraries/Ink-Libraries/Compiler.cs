using System;
using System.Collections.Generic;
using Ink.Parsed;
using Ink.Runtime;

namespace Ink
{
	// Token: 0x02000005 RID: 5
	public class Compiler
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021D5 File Offset: 0x000003D5
		public Ink.Parsed.Story parsedStory
		{
			get
			{
				return this._parsedStory;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021E0 File Offset: 0x000003E0
		public Compiler(string inkSource, Compiler.Options options = null)
		{
			this._inputString = inkSource;
			this._options = options ?? new Compiler.Options();
			if (this._options.pluginNames != null)
			{
				this._pluginManager = new PluginManager(this._options.pluginNames);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002238 File Offset: 0x00000438
		public Ink.Parsed.Story Parse()
		{
			this._parser = new InkParser(this._inputString, this._options.sourceFilename, new ErrorHandler(this.OnParseError), this._options.fileHandler);
			this._parsedStory = this._parser.Parse();
			return this._parsedStory;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002290 File Offset: 0x00000490
		public Ink.Runtime.Story Compile()
		{
			this.Parse();
			if (this._pluginManager != null)
			{
				this._pluginManager.PostParse(this._parsedStory);
			}
			if (this._parsedStory != null && !this._hadParseError)
			{
				this._parsedStory.countAllVisits = this._options.countAllVisits;
				this._runtimeStory = this._parsedStory.ExportRuntime(this._options.errorHandler);
				if (this._pluginManager != null)
				{
					this._pluginManager.PostExport(this._parsedStory, this._runtimeStory);
				}
			}
			else
			{
				this._runtimeStory = null;
			}
			return this._runtimeStory;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002334 File Offset: 0x00000534
		public Compiler.CommandLineInputResult HandleInput(CommandLineInput inputResult)
		{
			Compiler.CommandLineInputResult commandLineInputResult = new Compiler.CommandLineInputResult();
			if (inputResult.debugSource != null)
			{
				int value = inputResult.debugSource.Value;
				DebugMetadata debugMetadata = this.DebugMetadataForContentAtOffset(value);
				if (debugMetadata != null)
				{
					commandLineInputResult.output = "DebugSource: " + debugMetadata.ToString();
				}
				else
				{
					commandLineInputResult.output = "DebugSource: Unknown source";
				}
			}
			else if (inputResult.debugPathLookup != null)
			{
				string debugPathLookup = inputResult.debugPathLookup;
				DebugMetadata debugMetadata2 = this._runtimeStory.ContentAtPath(new Ink.Runtime.Path(debugPathLookup)).obj.debugMetadata;
				if (debugMetadata2 != null)
				{
					commandLineInputResult.output = "DebugSource: " + debugMetadata2.ToString();
				}
				else
				{
					commandLineInputResult.output = "DebugSource: Unknown source";
				}
			}
			else
			{
				if (inputResult.userImmediateModeStatement != null)
				{
					Ink.Parsed.Object @object = inputResult.userImmediateModeStatement as Ink.Parsed.Object;
					return this.ExecuteImmediateStatement(@object);
				}
				return null;
			}
			return commandLineInputResult;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000240C File Offset: 0x0000060C
		private Compiler.CommandLineInputResult ExecuteImmediateStatement(Ink.Parsed.Object parsedObj)
		{
			Compiler.CommandLineInputResult commandLineInputResult = new Compiler.CommandLineInputResult();
			if (parsedObj is Ink.Parsed.VariableAssignment)
			{
				Ink.Parsed.VariableAssignment variableAssignment = (Ink.Parsed.VariableAssignment)parsedObj;
				if (variableAssignment.isNewTemporaryDeclaration)
				{
					this._parsedStory.TryAddNewVariableDeclaration(variableAssignment);
				}
			}
			parsedObj.parent = this._parsedStory;
			Ink.Runtime.Object runtimeObject = parsedObj.runtimeObject;
			parsedObj.ResolveReferences(this._parsedStory);
			if (!this._parsedStory.hadError)
			{
				if (parsedObj is Ink.Parsed.Divert)
				{
					Ink.Parsed.Divert divert = parsedObj as Ink.Parsed.Divert;
					commandLineInputResult.divertedPath = divert.runtimeDivert.targetPath.ToString();
				}
				else if (parsedObj is Expression || parsedObj is Ink.Parsed.VariableAssignment)
				{
					Ink.Runtime.Object @object = this._runtimeStory.EvaluateExpression((Container)runtimeObject);
					if (@object != null)
					{
						commandLineInputResult.output = @object.ToString();
					}
				}
			}
			else
			{
				this._parsedStory.ResetError();
			}
			return commandLineInputResult;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024E0 File Offset: 0x000006E0
		public void RetrieveDebugSourceForLatestContent()
		{
			foreach (Ink.Runtime.Object @object in this._runtimeStory.state.outputStream)
			{
				StringValue stringValue = @object as StringValue;
				if (stringValue != null)
				{
					Compiler.DebugSourceRange debugSourceRange = default(Compiler.DebugSourceRange);
					debugSourceRange.length = stringValue.value.Length;
					debugSourceRange.debugMetadata = stringValue.debugMetadata;
					debugSourceRange.text = stringValue.value;
					this._debugSourceRanges.Add(debugSourceRange);
				}
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002584 File Offset: 0x00000784
		private DebugMetadata DebugMetadataForContentAtOffset(int offset)
		{
			int num = 0;
			DebugMetadata debugMetadata = null;
			foreach (Compiler.DebugSourceRange debugSourceRange in this._debugSourceRanges)
			{
				if (debugSourceRange.debugMetadata != null)
				{
					debugMetadata = debugSourceRange.debugMetadata;
				}
				if (offset >= num && offset < num + debugSourceRange.length)
				{
					return debugMetadata;
				}
				num += debugSourceRange.length;
			}
			return null;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002604 File Offset: 0x00000804
		private void OnParseError(string message, ErrorType errorType)
		{
			if (errorType == ErrorType.Error)
			{
				this._hadParseError = true;
			}
			if (this._options.errorHandler != null)
			{
				this._options.errorHandler(message, errorType);
				return;
			}
			throw new Exception(message);
		}

		// Token: 0x0400000B RID: 11
		private string _inputString;

		// Token: 0x0400000C RID: 12
		private Compiler.Options _options;

		// Token: 0x0400000D RID: 13
		private InkParser _parser;

		// Token: 0x0400000E RID: 14
		private Ink.Parsed.Story _parsedStory;

		// Token: 0x0400000F RID: 15
		private Ink.Runtime.Story _runtimeStory;

		// Token: 0x04000010 RID: 16
		private PluginManager _pluginManager;

		// Token: 0x04000011 RID: 17
		private bool _hadParseError;

		// Token: 0x04000012 RID: 18
		private List<Compiler.DebugSourceRange> _debugSourceRanges = new List<Compiler.DebugSourceRange>();

		// Token: 0x0200006A RID: 106
		public class Options
		{
			// Token: 0x040001A5 RID: 421
			public string sourceFilename;

			// Token: 0x040001A6 RID: 422
			public List<string> pluginNames;

			// Token: 0x040001A7 RID: 423
			public bool countAllVisits;

			// Token: 0x040001A8 RID: 424
			public ErrorHandler errorHandler;

			// Token: 0x040001A9 RID: 425
			public IFileHandler fileHandler;
		}

		// Token: 0x0200006B RID: 107
		public class CommandLineInputResult
		{
			// Token: 0x040001AA RID: 426
			public bool requestsExit;

			// Token: 0x040001AB RID: 427
			public int choiceIdx = -1;

			// Token: 0x040001AC RID: 428
			public string divertedPath;

			// Token: 0x040001AD RID: 429
			public string output;
		}

		// Token: 0x0200006C RID: 108
		public struct DebugSourceRange
		{
			// Token: 0x040001AE RID: 430
			public int length;

			// Token: 0x040001AF RID: 431
			public DebugMetadata debugMetadata;

			// Token: 0x040001B0 RID: 432
			public string text;
		}
	}
}
