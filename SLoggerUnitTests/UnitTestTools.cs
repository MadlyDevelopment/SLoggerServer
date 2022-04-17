using NUnit.Framework;
using SLoggerBusinessLogic;

namespace SLogger_Unit_Tests;

public class UnitTestTools
{

    [Test]
    [TestCase("Test", new byte[]{34,84,101,115,116,34})]
    [TestCase(12, new byte[]{49,50})]
    public void TestConvertObjToByteArray(object obj, byte[] testResultData)
    {
        var data = Tools.ConvertAnObjectToAByteArray(obj);
        Assert.AreEqual(testResultData, data);
    }
}