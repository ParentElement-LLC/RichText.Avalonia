using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia;
using Avalonia.Media;
using SkiaSharp;
using ParentElement.RichText.Core.Controllers;

namespace ParentElement.RichText.Avalonia.Platform;

/// <summary>
/// Long-lived cache that holds the SKPicture for the document. One instance lives
/// for the lifetime of the RichTextEditor control. It is NOT an ICustomDrawOperation
/// because Avalonia calls Dispose() on the "old" ICustomDrawOperation whenever Equals()
/// returns false — which would null _cache and cause blank frames if the same instance
/// were reused. Instead, a lightweight DocumentRenderOp is created fresh each frame.
/// </summary>
internal sealed class DocumentRenderer : IDisposable
{
    private readonly DocumentController _doc;
    private SKPicture? _cache;
    private volatile bool _documentDirty = true;

    /// <summary>
    /// Initializes a new instance of <see cref="DocumentRenderer"/> bound to the
    /// given <paramref name="document"/> controller.
    /// </summary>
    /// <param name="document">The document controller whose content will be rendered.</param>
    public DocumentRenderer(DocumentController document)
    {
        _doc = document;
    }

    /// <summary>
    /// Marks the cached picture as stale so it will be re-recorded on the next render pass.
    /// Call this whenever the document content changes.
    /// </summary>
    public void InvalidateDocument() => _documentDirty = true;

    /// <summary>
    /// Called by DocumentRenderOp.Render() on the render thread each frame.
    /// Only re-records the SKPicture when the document is marked dirty.
    /// </summary>
    internal void Render(SKCanvas canvas, SKRect bounds)
    {
        if (_documentDirty)
        {
            _cache?.Dispose();
            using var recorder = new SKPictureRecorder();
            var recordCanvas = recorder.BeginRecording(bounds);
            _doc.Draw(recordCanvas);
            _cache = recorder.EndRecording();
            _documentDirty = false;
        }

        if (_cache != null)
            canvas.DrawPicture(_cache);
    }

    /// <summary>
    /// Called when the RichTextEditor control is destroyed (not by Avalonia's
    /// ICustomDrawOperation lifecycle, which only touches DocumentRenderOp).
    /// </summary>
    public void Dispose()
    {
        _cache?.Dispose();
        _cache = null;
    }
}

/// <summary>
/// Lightweight per-frame ICustomDrawOperation created fresh in RichTextEditor.Render().
/// Avalonia calls Dispose() on the previous frame's instance — which is a no-op here
/// because this class owns no resources. The actual cache lives in DocumentRenderer.
/// </summary>
internal sealed class DocumentRenderOp : ICustomDrawOperation
{
    private readonly DocumentRenderer _renderer;

    /// <summary>
    /// Gets the bounding rectangle of the render target in logical (DIU) coordinates,
    /// as required by the <see cref="ICustomDrawOperation"/> contract.
    /// </summary>
    public Rect Bounds { get; }

    /// <summary>
    /// Initializes a new <see cref="DocumentRenderOp"/> for the current frame.
    /// </summary>
    /// <param name="renderer">The long-lived renderer that owns the SKPicture cache.</param>
    /// <param name="bounds">The bounding rectangle passed to Avalonia's rendering pipeline.</param>
    public DocumentRenderOp(DocumentRenderer renderer, Rect bounds)
    {
        _renderer = renderer;
        Bounds = bounds;
    }

    /// <summary>
    /// No-op: this descriptor owns no resources. The SKPicture cache is in DocumentRenderer,
    /// which is managed by the RichTextEditor and is unaffected by Avalonia disposing this op.
    /// </summary>
    public void Dispose() { }

    /// <summary>
    /// Always false so Avalonia always calls Render() and updates the selection display.
    /// Disposing the old DocumentRenderOp (a no-op) does not corrupt the cache.
    /// </summary>
    public bool Equals(ICustomDrawOperation? other) => false;

    /// <summary>
    /// Always returns <see langword="false"/>; hit-testing is handled by the
    /// <c>RichTextEditor</c> control directly.
    /// </summary>
    /// <param name="p">The point to test (unused).</param>
    /// <returns><see langword="false"/>.</returns>
    public bool HitTest(Point p) => false;

    /// <summary>
    /// Executes the Skia draw call for this frame by leasing the underlying
    /// <see cref="SKCanvas"/> from <paramref name="context"/> and delegating to
    /// <see cref="DocumentRenderer.Render"/>.
    /// </summary>
    /// <param name="context">The immediate drawing context provided by Avalonia's render thread.</param>
    public void Render(ImmediateDrawingContext context)
    {
        if (!context.TryGetFeature<ISkiaSharpApiLeaseFeature>(out ISkiaSharpApiLeaseFeature? feature) || feature == null)
            return;

        using var lease = feature.Lease();
        var canvas = lease.SkCanvas;
        if (canvas == null)
            return;

        var skBounds = SKRect.Create(0, 0, (float)Bounds.Width, (float)Bounds.Height);
        _renderer.Render(canvas, skBounds);
    }
}
