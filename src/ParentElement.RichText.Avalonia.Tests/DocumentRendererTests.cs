using ParentElement.RichText.Avalonia.Platform;
using Xunit;
using ParentElement.RichText.Core.Abstractions.IO;
using ParentElement.RichText.Core.Content;
using ParentElement.RichText.Core.Controllers;
using ParentElement.RichText.Core.Data;

namespace ParentElement.RichText.Avalonia.Tests;

/// <summary>
/// Tests for <see cref="DocumentRenderer"/> that cover lifecycle and
/// invalidation behaviour that does not require a live Skia canvas.
/// </summary>
public class DocumentRendererTests
{
    // ── Construction ──────────────────────────────────────────────────────────

    [Fact]
    public void Constructor_DoesNotThrow()
    {
        var controller = CreateController();
        var ex = Record.Exception(() => new DocumentRenderer(controller));
        Assert.Null(ex);
    }

    // ── InvalidateDocument ────────────────────────────────────────────────────

    [Fact]
    public void InvalidateDocument_DoesNotThrow()
    {
        var renderer = new DocumentRenderer(CreateController());
        var ex = Record.Exception(() => renderer.InvalidateDocument());
        Assert.Null(ex);
    }

    [Fact]
    public void InvalidateDocument_CanBeCalledMultipleTimes()
    {
        var renderer = new DocumentRenderer(CreateController());

        // Calling repeatedly must not throw or cause any error state.
        renderer.InvalidateDocument();
        renderer.InvalidateDocument();
        renderer.InvalidateDocument();
    }

    // ── Dispose ───────────────────────────────────────────────────────────────

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        var renderer = new DocumentRenderer(CreateController());
        var ex = Record.Exception(() => renderer.Dispose());
        Assert.Null(ex);
    }

    [Fact]
    public void Dispose_CalledTwice_DoesNotThrow()
    {
        var renderer = new DocumentRenderer(CreateController());
        renderer.Dispose();
        var ex = Record.Exception(() => renderer.Dispose());
        Assert.Null(ex);
    }

    [Fact]
    public void Dispose_AfterInvalidate_DoesNotThrow()
    {
        var renderer = new DocumentRenderer(CreateController());
        renderer.InvalidateDocument();
        var ex = Record.Exception(() => renderer.Dispose());
        Assert.Null(ex);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static DocumentController CreateController() =>
        new DocumentController(new DocumentSettings(), new NullClipboardHandler());

    private sealed class NullClipboardHandler : IClipboardHandler
    {
        public Task<string?> GetTextAsync() => Task.FromResult<string?>(null);
        public Task SetTextAsync(string text) => Task.CompletedTask;
        public Task<byte[]?> GetImageDataAsync() => Task.FromResult<byte[]?>(null);
        public Task SetImageBytesAsync(byte[] pngData) => Task.CompletedTask;
        public Task SetRichDataAsync(IReadOnlyList<ContentBlock> blocks, string? plainText) => Task.CompletedTask;
    }
}
