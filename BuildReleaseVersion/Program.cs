using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuildReleaseVersion
{
	class Program
	{
		private static Queue<AssemblyFile> filesQueue = new Queue<AssemblyFile>();

		//private static ManualResetEvent single = new ManualResetEvent(false);

		static void Main(string[] args)
		{
			// Command: BuildReleaseVersion.exe 4.0.1001.2 C:\Source\ABSF
			if (!Utility.CheckArguments(args))
			{
				Console.WriteLine("Please input the correct cut version and source location");
				Console.WriteLine("Sample as: BuildReleaseVersion.exe 4.0.1001.2 C:\\Source\\ABSF");
				Console.ReadLine();
				return;
			}

			string verNum = args[0];
			string proLoc = args[1];
			string proName = args.Length > 2 ? args[2] : string.Empty;

			// Use single to create a Producer and Consumer to 
			// handle the assemble files
			ThreadPool.QueueUserWorkItem(delegate
			{
				FindAssemblyFiles(proLoc, true);
			});
			ThreadPool.QueueUserWorkItem(delegate 
			{
				HandleAssemlyFile(verNum, proName);
			});

			Console.ReadLine();
		}
		 
		private static void HandleAssemlyFile(string verNum, string proName = "")
		{
			while (true)
			{
				if(filesQueue.Count > 0) //&& single.WaitOne())
				{
					ChangeVersionVisitor version = new ChangeVersionVisitor(verNum, proName);

					AssemblyFile file = filesQueue.Dequeue();

					// we need to break the while
					if (file == null)
					{
						break;
					}
					ThreadPool.QueueUserWorkItem(delegate
					{
						file.ChangeVersion(version);
					});
				}
			}
		}

		private static void FindAssemblyFiles(string path, bool IsFinished=false)
		{
			if (File.Exists(path))
			{
				ProcessFile(path);
			}
			else if (Directory.Exists(path))
			{
				ProcessDirectory(path);
			}

			if (IsFinished)
			{
				// enqueue null to tell stop the consumer.
				filesQueue.Enqueue(null);
				Console.WriteLine("Finished");
				//single.Set();
			}
		}

		private static void ProcessDirectory(string targetDirectory)
		{
			string[] fileEntries = Directory.GetFiles(targetDirectory);
			foreach (string fileName in fileEntries.Where(name => name.Contains("Assembly")))
				ProcessFile(fileName);

			string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
			foreach (string subdirectory in subdirectoryEntries)
				ProcessDirectory(subdirectory);
		}

		private static void ProcessFile(string file)
		{
			if (file.Contains("Assembly"))
			{
				AssemblyFile assemblyFile = Factory.GetFile(file);
				if (assemblyFile != null)
				{
					filesQueue.Enqueue(assemblyFile);
					//single.Set();
				}
			}
		}
	}

	abstract class AssemblyFile
	{
		public virtual string filepath { get; set; }
		public virtual void ChangeVersion(IVisitor visitor) { }
	}
	
	class CSharpAssemblyFile : AssemblyFile
	{
		public CSharpAssemblyFile(string path)
		{
			this.filepath = path;
		}
		public override void ChangeVersion(IVisitor visitor)
		{
			visitor.ChangeAssemblyVersion(this);
		}
	}
	class CppAssemblyFile : AssemblyFile
	{
		public CppAssemblyFile(string path)
		{
			this.filepath = path;
		}
		public override void ChangeVersion(IVisitor visitor)
		{
			visitor.ChangeAssemblyVersion(this);
		}
	}
	class BetaAssemblyFile : AssemblyFile
	{
		public BetaAssemblyFile(string path)
		{
			this.filepath = path;
		}
		public override void ChangeVersion(IVisitor visitor)
		{
			visitor.ChangeAssemblyVersion(this);
		}
	}
}
