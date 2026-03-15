using Avalonia;
using ParentElement.RichText.Avalonia.Platform;
using Xunit;
using ParentElement.RichText.Core.Abstractions.IO;
using ParentElement.RichText.Core.Content;
using ParentElement.RichText.Core.Controllers;
using ParentElement.RichText.Core.Data;

namespace ParentElement.RichText.Avalonia.Tests;

/// <summary>
/// Tests for <see cref="DocumentRenderOp"/> that cover the pure-logic members
/// not requiring a live Avalonia rendering pipeline.
/// </summary>
public class DocumentRenderOpTests
{
    // ── HitTest ───────────────────────────────────────────────────────────────

    [Fact]
    public void HitTest_AlwaysReturnsFalse()
    {
        var op = CreateOp(new Rect(0, 0, 100, 50));

        Assert.False(op.HitTest(new Point(0, 0)));
        Assert.False(op.HitTest(new Point(50, 25)));
        Assert.False(op.HitTest(new Point(100, 50)));
        Assert.False(op.HitTest(new Point(-1, -1)));
        Assert.False(op.HitTest(new Point(double.MaxValue, double.MaxValue)));
    }

    // ── Equals ────────────────────────────────────────────────────────────────

    [Fact]
    public void Equals_AlwaysReturnsFalse_WhenComparedToItself()
    {
        var op = CreateOp(new Rect(0, 0, 100, 50));
        Assert.False(op.Equals(op));
    }

    [Fact]
    public void Equals_AlwaysReturnsFalse_WhenComparedToNull()
    {
        var op = CreateOp(new Rect(0, 0, 100, 50));
        Assert.False(op.Equals(null));
    }

    [Fact]
    public void Equals_AlwaysReturnsFalse_WhenComparedToAnotherOp()
    {
        var op1 = CreateOp(new Rect(0, 0, 100, 50));
        var op2 = CreateOp(new Rect(0, 0, 100, 50));
        Assert.False(op1.Equals(op2));
    }

    // ── Bounds ────────────────────────────────────────────────────────────────

    [Fact]
    public void Bounds_ReflectsConstructorValue()
    {
        var expected = new Rect(10, 20, 300, 150);
        var op = CreateOp(expected);
        Assert.Equal(expected, op.Bounds);
    }

    [Fact]
    public void Bounds_ZeroRect_IsStoredCorrectly()
    {
        var op = CreateOp(default);
        Assert.Equal(default(Rect), op.Bounds);
    }

    // ── Dispose ───────────────────────────────────────────────────────────────

    [Fact]
    public void Dispose_IsNoOp_DoesNotThrow()
    {
        var op = CreateOp(new Rect(0, 0, 100, 50));
        op.Dispose(); // first call
        op.Dispose(); // second call — also must not throw
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static DocumentRenderOp CreateOp(Rect bounds)
    {
        var controller = new DocumentController(new DocumentSettings(), new NullClipboardHandler());
        var renderer = new DocumentRenderer(controller);
        return new DocumentRenderOp(renderer, bounds);
    }

    private sealed class NullClipboardHandler : IClipboardHandler
    {
        public Task<string?> GetTextAsync() => Task.FromResult<string?>(null);
        public Task SetTextAsync(string text) => Task.CompletedTask;
        public Task<byte[]?> GetImageDataAsync() => Task.FromResult<byte[]?>(null);
        public Task SetImageBytesAsync(byte[] pngData) => Task.CompletedTask;
        public Task SetRichDataAsync(IReadOnlyList<ContentBlock> blocks, string? plainText) => Task.CompletedTask;
    }
}
