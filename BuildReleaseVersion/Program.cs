﻿using System;
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

		private static ManualResetEvent single = new ManualResetEvent(true);

		static void Main(string[] args)
		{
			// Command: BuildReleaseVersion.exe 4.0.1001.2 C:\Source\ABSF
			if (args.Length != 2)
			{
				Console.WriteLine("Please input the correct path and version number");
				return;
			}
			//TODO add some checks about the arguments....

			string verNum = args[0];
			string proLoc = args[1];

			ThreadPool.QueueUserWorkItem(delegate
			{
				FindAssemblyFiles(proLoc);
			});

			ThreadPool.QueueUserWorkItem(delegate
			{
				HandleAssemlyFile(verNum);
			});
			Console.ReadLine();
		}

		private static void HandleAssemlyFile(string verNum)
		{
			while (true)
			{
				single.WaitOne();
				{
					ChangeVersionVisitor version = new ChangeVersionVisitor(verNum);

					AssemblyFile file = filesQueue.Dequeue();

					if (file == null)
						break;

					ThreadPool.QueueUserWorkItem(delegate
					{
						file.ChangeVersion(version);
					});
				}
			}
		}

		private static void FindAssemblyFiles(string path)
		{
			if (File.Exists(path))
			{
				ProcessFile(path);
			}
			else if (Directory.Exists(path))
			{
				ProcessDirectory(path);
			}

			// enqueue null to tell stop the consumer.
			filesQueue.Enqueue(null);
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
				AssemblyFile assembly = Factory.GetFile(file);
				if (assembly != null)
				{
					filesQueue.Enqueue(assembly);
					single.Set();
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
