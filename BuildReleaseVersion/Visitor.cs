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
			string cut = Utility.GetCSharpCutVersion(this.v);

			Utility.ReplaceVersion(FileType.CommonAssemblyInfoCS, file.filepath, cut);

			Utility.PrintFile(file.filepath);
		}
		public void ChangeAssemblyVersion(CppAssemblyFile file)
		{
			Utility.ReplaceVersion(FileType.AssemblyInfoCpp, file.filepath, this.v);

			Utility.PrintFile(file.filepath);
		}
		public void ChangeAssemblyVersion(BetaAssemblyFile file)
		{
			string beta = Utility.GetBetaInfo(this.v);

			Utility.ReplaceVersion(FileType.AssemblyInfoCS, file.filepath, beta);
			
			Utility.PrintFile(file.filepath);
		}
	}
}
