using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TypeScripter.Tests
{
	public static class TypeScriptCompiler
	{
		#region Internal Constructs
		/// <summary>
		/// A class representing the compiler result
		/// </summary>
		public class Result
		{
			/// <summary>
			/// The compiler return code
			/// </summary>
			public int ReturnCode
			{
				get;
				private set;
			}

			/// <summary>
			/// The compiler output
			/// </summary>
			public string Output
			{
				get;
				private set;
			}

			/// <summary>
			/// The compiler output
			/// </summary>
			public string ErrorOutput
			{
				get;
				private set;
			}

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="returnCode"></param>
			/// <param name="output"></param>
			/// <param name="errorOutput"></param>
			public Result(int returnCode, string output, string errorOutput)
			{
				this.ReturnCode = returnCode;
				this.Output = output;
				this.ErrorOutput = errorOutput;
			}

		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Compiles a TypeScript file
		/// </summary>
		/// <param name="file">The typescript file name</param>
		/// <returns>The tsc return code</returns>
		public static Result CompileFiles(params string[] files)
		{
			var options = "";
			var process = new Process();
#if DOTNET
            process.StartInfo.FileName = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"..\..\", @"Tools\Microsoft.TypeScript.Compiler\tsc.exe");
#else
            process.StartInfo.FileName = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"..\..\..\..\..\", @"Tools\Microsoft.TypeScript.Compiler\tsc.exe");
#endif
            process.StartInfo.Arguments = string.Format("{0} {1}", options, string.Join(" ", files));
            process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.Start();
			process.WaitForExit(10 * 1000);
			var output = process.StandardOutput.ReadToEnd();
			var errorOutput = process.StandardError.ReadToEnd();
			return new Result(process.ExitCode, output, errorOutput);
		}

		/// <summary>
		/// Compiles a specific string of TypeScript
		/// </summary>
		/// <param name="typeScript">The TypeScript to compile</param>
		/// <returns>The tsc return code</returns>
		public static Result Compile(string typeScript)
		{
			var tempFile = System.IO.Path.GetTempFileName();
			System.IO.File.WriteAllText(tempFile, typeScript);
			try
			{
				var newTempFile = tempFile.Replace(".tmp", ".ts");
				System.IO.File.Move(tempFile, newTempFile);
				tempFile = newTempFile;
				return CompileFiles(tempFile);
			}
			finally
			{
				System.IO.File.Delete(tempFile);
			}
		}
        #endregion
	}
}
