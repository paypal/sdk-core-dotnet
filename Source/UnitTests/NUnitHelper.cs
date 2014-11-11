#if NUnit
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
/* NuGet Install
 * Visual Studio 2005
    * Install NUnit -OutputDirectory .\packages
    * Add reference from NUnit.2.6.2
 */
using NUnit.Framework;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public class TestClass : TestFixtureAttribute
    {
    }

    public class TestMethod : TestAttribute
    {
    }

    public class ExpectedException : ExpectedExceptionAttribute
    {
        public ExpectedException(Type type) : base(type) { }
        public ExpectedException(Type type, string message)
        {
            this.ExpectedException = type;
            this.UserMessage = message;
        }
    }

    public class Ignore : IgnoreAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class DeploymentItem : System.Attribute
    {
        private readonly string _itemPath;
        private readonly string _filePath;
        private readonly string _binFolderPath;
        private readonly string _itemPathInBin;
        private readonly DirectoryInfo _environmentDir;
        private readonly Uri _itemPathUri;
        private readonly Uri _itemPathInBinUri;

        public DeploymentItem(string fileProjectRelativePath)
        {
            _filePath = fileProjectRelativePath.Replace("/", @"\");

            _environmentDir = new DirectoryInfo(Environment.CurrentDirectory);
            _itemPathUri = new Uri(Path.Combine(_environmentDir.Parent.Parent.FullName
                , _filePath));

            _itemPath = _itemPathUri.LocalPath;
            _binFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _itemPathInBinUri = new Uri(Path.Combine(_binFolderPath, _filePath));
            _itemPathInBin = _itemPathInBinUri.LocalPath;

            if (File.Exists(_itemPathInBin))
            {
                File.Delete(_itemPathInBin);
            }

            if (File.Exists(_itemPath))
            {
                File.Copy(_itemPath, _itemPathInBin);
            }
        }
    }

    public class Assert : NUnit.Framework.Assert
    {
    }
}
#endif