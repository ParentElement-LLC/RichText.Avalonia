using Avalonia.Input;
using ParentElement.RichText.Core.Input;

namespace ParentElement.RichText.Avalonia.Platform;

/// <summary>
/// Extension methods that bridge Avalonia input types to the platform-agnostic
/// RichText core input abstractions.
/// </summary>
public static class AvaloniaExtensions
{
    /// <summary>
    /// Converts an Avalonia <see cref="Key"/> value to the corresponding
    /// platform-agnostic <see cref="KeyCode"/>.
    /// </summary>
    /// <param name="key">The Avalonia key to convert.</param>
    /// <returns>
    /// The matching <see cref="KeyCode"/>, or <see cref="KeyCode.None"/> if the key
    /// is not present in the conversion map.
    /// </returns>
    public static KeyCode ToKeyCode(this Key key)
    {
        if(Statics.KeyConversionMap.TryGetValue(key, out KeyCode value))
            return value;

        return KeyCode.None;
    }
}
