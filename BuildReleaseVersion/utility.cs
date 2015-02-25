using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildReleaseVersion
{
	public enum FileType
	{ 
		CommonAssemblyInfoCS,
		AssemblyInfoCpp,
		AssemblyInfoCS,
	}

	class Utility
	{
		public static string GetCSharpCutVersion(string version)
		{
			string[] cutNum = version.Split('.');
			cutNum[0] = "00" + cutNum[0];
			cutNum[1] = "00" + cutNum[1];
			StringBuilder builder = new StringBuilder();
			foreach (string value in cutNum)
			{
				builder.Append(value);
				builder.Append('.');
			}
			return builder.ToString();
		}

		// Command: BuildReleaseVersion.exe 4.0.1001.2 C:\Source\ABSF
		public static bool CheckArguments(string[] args)
		{
			if (args.Length != 2 && args.Length != 3)
			{
				return false;
			}
			if(args[0].Count(dot => dot == '.') != 3)
			{
				return false;
			}
			if (!Directory.Exists(args[1]))
			{
				return false;
			}
			return true;
		}

		public static void PrintFile(string file)
		{
			Console.WriteLine(file);
		}

		public static string GetBetaInfo(string version)
		{
			return string.Format("Beta Version {0} Beta1", version);
		}

		public static void ReplaceVersion(FileType type, string file, string repValue, string proName = "")
		{
			bool changeFlag = false;
			using (TextReader reader = new StreamReader(file, Encoding.ASCII))
			{
				string stream = reader.ReadToEnd();
				
				string[] lines = stream.Split(new char[] {'\n'});

				for (int i = 0; i < lines.Length; i ++)
				{
					if (type == FileType.CommonAssemblyInfoCS && lines[i].Contains("AssemblyVersion"))
					{
						lines[i] = string.Format("[assembly: AssemblyVersion(\"{0}\")]", repValue);
						changeFlag = true;
					}
					else if (type == FileType.CommonAssemblyInfoCS && lines[i].Contains("AssemblyFileVersion"))
					{
						lines[i] = string.Format("[assembly: AssemblyFileVersion(\"{0}\")]", repValue);
						changeFlag = true;
					}
					else if (type == FileType.AssemblyInfoCpp && lines[i].Contains("AssemblyVersionAttribute"))
					{
						lines[i] = string.Format("[assembly:AssemblyVersionAttribute(\"{0}\")];", repValue);
						changeFlag = true;
					}
					else if (type == FileType.AssemblyInfoCS && 
						proName != string.Empty && 
						file.IndexOf(proName, 0, StringComparison.CurrentCultureIgnoreCase) != -1  && 
						lines[i].Contains("AssemblyTitle"))
					{
						lines[i] = string.Format("[assembly: AssemblyTitle(\"{0}\")]", repValue);
						changeFlag = true;
					}
					else if (type == FileType.AssemblyInfoCS && 
						proName != string.Empty && 
						file.IndexOf(proName, 0, StringComparison.CurrentCultureIgnoreCase) != -1 && 
						lines[i].Contains("AssemblyDescription"))
					{
						lines[i] = string.Format("[assembly: AssemblyDescription(\"{0}\")]", repValue);
						changeFlag = true;
					}
				}
				reader.Close();

				if (changeFlag)
				{
					Utility.PrintFile(file);

					using (TextWriter tw = new StreamWriter(file, false, Encoding.ASCII))
					{
						for (int i = 0; i < lines.Length; i++)
						{
							tw.Write(lines[i] + "\n");
						}
						tw.Close();
					};
				}
			}
		}
	}
}
