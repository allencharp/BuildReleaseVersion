using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildReleaseVersion
{
	class Factory
	{
		public static AssemblyFile GetFile(string file)
		{
			if (IsCppAssemblyFile(file))
				return new CppAssemblyFile(file);
			else if (IsCharpAssemblyFile(file))
				return new CSharpAssemblyFile(file);
			else if (IsCommonAssemblyFile(file))
				return new BetaAssemblyFile(file);

			return null;
		}

		public static bool IsCppAssemblyFile(string file)
		{
			return false;
		}
		public static bool IsCharpAssemblyFile(string file)
		{
			return false;
		}
		public static bool IsCommonAssemblyFile(string file)
		{
			return false;
		}
	}
}
