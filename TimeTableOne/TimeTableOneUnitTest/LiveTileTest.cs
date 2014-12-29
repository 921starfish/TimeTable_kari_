using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeTableOneUnitTest
{
    [TestClass]
    public class LiveTileTest
    {
        [TestMethod]
        public void TestGenerateXML()
        {
            XmlDocument template = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare310x310BlockAndText01);

        }
    }
}
