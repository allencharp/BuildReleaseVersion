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
		private string name;
		public ChangeVersionVisitor(string version, string proName)
		{
			this.v = version;
			this.name = proName;
		}
		public void ChangeAssemblyVersion(CSharpAssemblyFile file)
		{
			string cut = Utility.GetCSharpCutVersion(this.v);

			Utility.ReplaceVersion(FileType.CommonAssemblyInfoCS, file.filepath, cut);
		}
		public void ChangeAssemblyVersion(CppAssemblyFile file)
		{
			Utility.ReplaceVersion(FileType.AssemblyInfoCpp, file.filepath, this.v);
		}
		public void ChangeAssemblyVersion(BetaAssemblyFile file)
		{
			string beta = Utility.GetBetaInfo(this.v);

			Utility.ReplaceVersion(FileType.AssemblyInfoCS, file.filepath, beta, this.name);
		}
	}
}
