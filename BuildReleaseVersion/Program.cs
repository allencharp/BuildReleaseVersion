using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildReleaseVersion
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine("Please input the correct path and version number");
				return;
			}

			IList<AssemblyFile> files = new List<AssemblyFile>();
			ChangeVersion version = new ChangeVersion(args[1]);
			foreach (AssemblyFile file in files)
			{
				System.Threading.ThreadPool.QueueUserWorkItem(delegate {
					file.ChangeVersion(version);
				}); 
			}
		}
	}
	abstract class AssemblyFile
	{
		public virtual void ChangeVersion(IVisitor visitor) { }
	}
	interface IVisitor
	{
		void ChangeAssemblyVersion(CSharpAssemblyFile file);
		void ChangeAssemblyVersion(CppAssemblyFile file);
	}
	class CSharpAssemblyFile : AssemblyFile
	{
		public override void ChangeVersion(IVisitor visitor)
		{
			visitor.ChangeAssemblyVersion(this);
		}
	}
	class CppAssemblyFile : AssemblyFile
	{
		public override void ChangeVersion(IVisitor visitor)
		{
			visitor.ChangeAssemblyVersion(this);
		}
	}

	class ChangeVersion : IVisitor
	{
		private string v;
		public ChangeVersion(string version)
		{
			this.v = version;
		}
		public void ChangeAssemblyVersion(CSharpAssemblyFile file)
		{
			Console.WriteLine("");
		}
		public void ChangeAssemblyVersion(CppAssemblyFile file)
		{
			Console.WriteLine("");
		}
	}
}
