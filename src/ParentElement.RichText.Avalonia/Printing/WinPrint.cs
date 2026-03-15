using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using SkiaSharp;

namespace ParentElement.RichText.Avalonia.Printing;

/// <summary>
/// Minimal Windows GDI / Common-Dialog P/Invoke surface for physical printing.
/// All members are Windows-only; callers must guard with OperatingSystem.IsWindows().
/// </summary>
[SupportedOSPlatform("windows")]
internal static class WinPrint
{
    // ── PrintDlg flags ──────────────────────────────────────────────────────
    internal const uint _pD_ALLPAGES   = 0x00000000;
    internal const uint _pD_PAGENUMS   = 0x00000002;
    internal const uint _pD_NOSELECTION = 0x00000004;
    internal const uint _pD_RETURNDC   = 0x00000100;

    // ── PRINTDLG structure (x64 layout: 120 bytes) ──────────────────────────
    [StructLayout(LayoutKind.Sequential)]
    internal struct PRINTDLG
    {
        public uint    lStructSize;
        public IntPtr  hwndOwner;
        public IntPtr  hDevMode;
        public IntPtr  hDevNames;
        public IntPtr  hDC;
        public uint    Flags;
        public ushort  nFromPage;
        public ushort  nToPage;
        public ushort  nMinPage;
        public ushort  nMaxPage;
        public ushort  nCopies;
        public IntPtr  hInstance;
        public IntPtr  lCustData;
        public IntPtr  lpfnPrintHook;
        public IntPtr  lpfnSetupHook;
        public IntPtr  lpPrintTemplateName;
        public IntPtr  lpSetupTemplateName;
        public IntPtr  hPrintTemplate;
        public IntPtr  hSetupTemplate;
    }

    [DllImport("comdlg32.dll", EntryPoint = "PrintDlgW", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool PrintDlg(ref PRINTDLG lppd);

    // ── DOCINFO ─────────────────────────────────────────────────────────────
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DOCINFO
    {
        public int     cbSize;
        [MarshalAs(UnmanagedType.LPWStr)] public string  lpszDocName;
        [MarshalAs(UnmanagedType.LPWStr)] public string? lpszOutput;
        [MarshalAs(UnmanagedType.LPWStr)] public string? lpszDatatype;
        public int     fwType;
    }

    // ── GDI device-capabilities indices ─────────────────────────────────────
    internal const int _lOGPIXELSX = 88;
    internal const int _hORZRES    = 8;
    internal const int _vERTRES    = 10;

    // ── GDI print-job functions ──────────────────────────────────────────────
    [DllImport("gdi32.dll", EntryPoint = "StartDocW", CharSet = CharSet.Unicode)]
    internal static extern int  StartDoc(IntPtr hdc, ref DOCINFO lpdi);
    [DllImport("gdi32.dll")] internal static extern int  StartPage(IntPtr hdc);
    [DllImport("gdi32.dll")] internal static extern int  EndPage(IntPtr hdc);
    [DllImport("gdi32.dll")] internal static extern int  EndDoc(IntPtr hdc);
    [DllImport("gdi32.dll")] internal static extern bool DeleteDC(IntPtr hdc);
    [DllImport("gdi32.dll")] internal static extern int  GetDeviceCaps(IntPtr hdc, int nIndex);

    // ── StretchDIBits ────────────────────────────────────────────────────────
    [StructLayout(LayoutKind.Sequential)]
    internal struct BITMAPINFOHEADER
    {
        public uint   biSize;
        public int    biWidth;
        public int    biHeight;   // negative = top-down
        public ushort biPlanes;
        public ushort biBitCount;
        public uint   biCompression;
        public uint   biSizeImage;
        public int    biXPelsPerMeter;
        public int    biYPelsPerMeter;
        public uint   biClrUsed;
        public uint   biClrImportant;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader;
        // No colour table needed for 32 bpp BI_RGB
    }

    internal const uint _bI_RGB         = 0;
    internal const uint _dIB_RGB_COLORS = 0;
    internal const uint _sRCCOPY        = 0x00CC0020;

    [DllImport("gdi32.dll")]
    internal static extern int StretchDIBits(
        IntPtr hdc,
        int xDest, int yDest, int wDest, int hDest,
        int xSrc,  int ySrc,  int wSrc,  int hSrc,
        IntPtr lpBits, ref BITMAPINFO lpbmi,
        uint iUsage, uint rop);

    // ── Heap handle helpers ──────────────────────────────────────────────────
    [DllImport("kernel32.dll")]
    internal static extern IntPtr GlobalFree(IntPtr hMem);

    // ── Rendering helper ─────────────────────────────────────────────────────
    /// <summary>
    /// Renders <paramref name="bitmap"/> (Bgra8888) to a printer HDC, stretching it to
    /// fill the printable area (<paramref name="destWidth"/> × <paramref name="destHeight"/>).
    /// </summary>
    internal static void RenderBitmapToHdc(IntPtr hdc, SKBitmap bitmap, int destWidth, int destHeight)
    {
        var bmi = new BITMAPINFO
        {
            bmiHeader = new BITMAPINFOHEADER
            {
                biSize        = (uint)Marshal.SizeOf<BITMAPINFOHEADER>(),
                biWidth       = bitmap.Width,
                biHeight      = -bitmap.Height,   // negative → top-down
                biPlanes      = 1,
                biBitCount    = 32,
                biCompression = _bI_RGB
            }
        };
        // SKBitmap.GetPixels() returns a stable native pointer valid for the bitmap's lifetime.
        StretchDIBits(hdc,
            0, 0, destWidth, destHeight,
            0, 0, bitmap.Width, bitmap.Height,
            bitmap.GetPixels(), ref bmi, _dIB_RGB_COLORS, _sRCCOPY);
    }
}
