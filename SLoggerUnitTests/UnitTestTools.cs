using NUnit.Framework;
using SLoggerBusinessLogic;

namespace SLogger_Unit_Tests;

public class UnitTestTools
{
    /// <summary>
    /// Test the method which
    /// converts an object to an byte array
    /// </summary>
    /// <param name="obj">Test object used for conversion</param>
    /// <param name="testResultData">The result which the test should output</param>
    [Test]
    [TestCase("Test", new byte[]{34,84,101,115,116,34})]
    [TestCase(12, new byte[]{49,50})]
    public void TestConvertObjToByteArray(object obj, byte[] testResultData)
    {
        var data = Tools.ConvertAnObjectToAByteArray(obj);
        Assert.AreEqual(testResultData, data);
    }

    /// <summary>
    /// Test the method which
    /// converts a byte array to an object
    /// </summary>
    [Test]
    public void TestConvertByteArrayToObj()
    {
        var dataOne = Tools.ConvertByteArrayToObject(new byte[]{49,50});
        var dataTwo = Tools.ConvertByteArrayToObject(new byte[]{34,84,101,115,116,34});
        if (int.Parse(dataOne.ToString()!)  != 12)
        {
            Assert.Fail("Conversation from byte array to int failed");
        }
        if (dataTwo.ToString() != "Test")
        {
            Assert.Fail("Conversation from byte array to string failed");
        }
    }
}