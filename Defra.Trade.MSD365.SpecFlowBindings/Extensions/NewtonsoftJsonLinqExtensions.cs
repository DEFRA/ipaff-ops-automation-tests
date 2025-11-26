

namespace Newtonsoft.Json.Linq;

public static class NewtonsoftJsonLinqExtensions
{
    /// <summary>
    /// Removes this token from its parent.
    /// </summary>
    /// <param name="property">Represents a JSON property.</param>
    /// <returns>true if it was removed successfully; otherwise, false.</returns>
    public static bool TryRemove(this JProperty property)
    {
        try
        {
            property.Remove();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
