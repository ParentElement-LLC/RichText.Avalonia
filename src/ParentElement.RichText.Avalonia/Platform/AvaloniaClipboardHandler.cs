using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Media.Imaging;
using ParentElement.RichText.Core.Abstractions.IO;
using ParentElement.RichText.Core.Content;
using ParentElement.RichText.Core.IO;

namespace ParentElement.RichText.Avalonia.Platform;

/// <summary>
/// Avalonia implementation of <see cref="IClipboardHandler"/> that delegates clipboard
/// operations to the Avalonia <see cref="IClipboard"/> service obtained from the top-level window.
/// </summary>
public class AvaloniaClipboardHandler : IClipboardHandler
{
    private IClipboard? _clipboard;

    /// <summary>
    /// Initializes a new instance of <see cref="AvaloniaClipboardHandler"/> and resolves
    /// the Avalonia clipboard service from the top-level ancestor of <paramref name="control"/>.
    /// </summary>
    /// <param name="control">The control whose top-level window provides the clipboard service.</param>
    public AvaloniaClipboardHandler(Control control)
    {
        _clipboard = TopLevel.GetTopLevel(control)!.Clipboard;
    }

    /// <summary>
    /// Retrieves plain text from the clipboard, or <see langword="null"/> if the clipboard
    /// is unavailable or contains no text.
    /// </summary>
    /// <returns>A task that resolves to the clipboard text, or <see langword="null"/>.</returns>
    public Task<string?> GetTextAsync()
    {
        if (_clipboard == null)
            return Task.FromResult<string?>(null);

        return _clipboard!.TryGetTextAsync();
    }

    /// <summary>
    /// Places <paramref name="text"/> on the clipboard as plain text.
    /// </summary>
    /// <param name="text">The text to write to the clipboard.</param>
    /// <returns>A task that completes when the clipboard has been updated.</returns>
    public Task SetTextAsync(string text)
    {
        return _clipboard!.SetTextAsync(text);
    }

    /// <summary>
    /// Retrieves image data from the clipboard as a PNG-encoded byte array, or
    /// <see langword="null"/> if the clipboard is unavailable or contains no bitmap.
    /// </summary>
    /// <returns>A task that resolves to PNG image bytes, or <see langword="null"/>.</returns>
    public async Task<byte[]?> GetImageDataAsync()
    {
        if (_clipboard == null)
            return null;

        var bitmap = await _clipboard.TryGetBitmapAsync();
        if (bitmap == null)
            return null;

        using var ms = new MemoryStream();
        bitmap.Save(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Places the supplied content <paramref name="blocks"/> on the clipboard as RTF together
    /// with an optional plain-text fallback.
    /// </summary>
    /// <param name="blocks">The document content blocks to serialize and copy.</param>
    /// <param name="plainText">Optional plain-text representation; pass <see langword="null"/> to omit the text format.</param>
    /// <returns>A task that completes when the clipboard has been updated.</returns>
    public async Task SetRichDataAsync(IReadOnlyList<ContentBlock> blocks, string? plainText)
    {
        if (_clipboard == null) return;

        //TODO:  This is temporary - need to abstract from IExporter to allow additional clipboard outputs
        var rtf = RtfExporter.ToRtfString(blocks);

        var item = new DataTransferItem();
        if (plainText != null)
            item.SetText(plainText);

        item.Set(DataFormat.CreateStringPlatformFormat("Rich Text Format"), rtf);

        var dataTransfer = new DataTransfer();
        dataTransfer.Add(item);

        await _clipboard.SetDataAsync(dataTransfer);
    }

    /// <summary>
    /// Places a PNG image onto the clipboard from the supplied raw PNG byte array.
    /// An empty text entry is also added to satisfy applications that require a text format.
    /// </summary>
    /// <param name="pngData">The PNG-encoded image bytes to write to the clipboard.</param>
    /// <returns>A task that completes when the clipboard has been updated.</returns>
    public async Task SetImageBytesAsync(byte[] pngData)
    {
        if (_clipboard == null) return;

        using var ms = new MemoryStream(pngData);
        var bitmap = new Bitmap(ms);

        var item = new DataTransferItem();
        item.SetText(string.Empty); // satisfy apps that require text
        item.SetBitmap(bitmap);

        var dataTransfer = new DataTransfer();
        dataTransfer.Add(item);

        await _clipboard.SetDataAsync(dataTransfer);
    }

    
}
