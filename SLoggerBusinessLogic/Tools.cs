using System.Text;
using Newtonsoft.Json;

namespace SLoggerBusinessLogic;

public class Tools
{
    /// <summary>
    /// This method is used to convert an object
    /// to a byte array
    /// </summary>
    /// <param name="obj"> The object you want to convert </param>
    /// <returns>The object as an byte array</returns>
    public static byte[] ConvertAnObjectToAByteArray(object obj)
    {
        var jsonData = JsonConvert.SerializeObject(obj);
        var byteData = Encoding.UTF8.GetBytes(jsonData);
        return byteData;
    }

    /// <summary>
    /// This method is used to convert an byte
    /// array to an Object
    /// </summary>
    /// <param name="data">The byte array you want to convert</param>
    /// <returns>The Deserialized Object</returns>
    public static object ConvertByteArrayToObject(byte[] data)
    {
        var stringJson = Encoding.UTF8.GetString(data);
        var obj = JsonConvert.DeserializeObject(stringJson)!;
        return obj;
    }
    
}