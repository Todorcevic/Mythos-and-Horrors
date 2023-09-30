using NUnit.Framework;
using System.ComponentModel;
using System.IO;

namespace GameView.Tests
{

    public class Checkers
    {
        [Test]
        public void CardInfoJson_File_Exist()
        {
            Assert.IsTrue(File.Exists(FilesPath.JSON_DATA_PATH));
        }
    }
}
