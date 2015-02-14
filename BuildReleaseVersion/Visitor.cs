using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildReleaseVersion
{
	interface IVisitor
	{
		void ChangeAssemblyVersion(CSharpAssemblyFile file);
		void ChangeAssemblyVersion(CppAssemblyFile file);
		void ChangeAssemblyVersion(BetaAssemblyFile file);
	}

	class ChangeVersionVisitor : IVisitor
	{
		private string v;
		public ChangeVersionVisitor(string version)
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
		public void ChangeAssemblyVersion(BetaAssemblyFile file)
		{
			Console.WriteLine("");
		}
	}
}
