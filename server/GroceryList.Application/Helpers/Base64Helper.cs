using System.Text;

namespace GroceryList.Application.Helpers;
public static class Base64Helper
{
    public static string DecodeFrom64(string encodedData)
    {
        byte[] encodedDataAsBytes
            = Convert.FromBase64String(encodedData);

        string returnValue =
           ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

        return returnValue;
    }

    public static string EncodeTo64(string toEncode)
    {

        byte[] toEncodeAsBytes

              = ASCIIEncoding.ASCII.GetBytes(toEncode);

        string returnValue

              = Convert.ToBase64String(toEncodeAsBytes);

        return returnValue;

    }
}
