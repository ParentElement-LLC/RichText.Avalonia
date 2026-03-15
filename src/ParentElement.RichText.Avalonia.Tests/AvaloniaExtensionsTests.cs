using Avalonia.Input;
using ParentElement.RichText.Avalonia.Platform;
using ParentElement.RichText.Core.Input;
using Xunit;

namespace ParentElement.RichText.Avalonia.Tests;

public class AvaloniaExtensionsTests
{
    // ── Letters ──────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(Key.A, KeyCode.A)]
    [InlineData(Key.B, KeyCode.B)]
    [InlineData(Key.C, KeyCode.C)]
    [InlineData(Key.D, KeyCode.D)]
    [InlineData(Key.E, KeyCode.E)]
    [InlineData(Key.F, KeyCode.F)]
    [InlineData(Key.G, KeyCode.G)]
    [InlineData(Key.H, KeyCode.H)]
    [InlineData(Key.I, KeyCode.I)]
    [InlineData(Key.J, KeyCode.J)]
    [InlineData(Key.K, KeyCode.K)]
    [InlineData(Key.L, KeyCode.L)]
    [InlineData(Key.M, KeyCode.M)]
    [InlineData(Key.N, KeyCode.N)]
    [InlineData(Key.O, KeyCode.O)]
    [InlineData(Key.P, KeyCode.P)]
    [InlineData(Key.Q, KeyCode.Q)]
    [InlineData(Key.R, KeyCode.R)]
    [InlineData(Key.S, KeyCode.S)]
    [InlineData(Key.T, KeyCode.T)]
    [InlineData(Key.U, KeyCode.U)]
    [InlineData(Key.V, KeyCode.V)]
    [InlineData(Key.W, KeyCode.W)]
    [InlineData(Key.X, KeyCode.X)]
    [InlineData(Key.Y, KeyCode.Y)]
    [InlineData(Key.Z, KeyCode.Z)]
    public void ToKeyCode_Letters_ReturnExpected(Key avaloniaKey, KeyCode expected)
    {
        Assert.Equal(expected, avaloniaKey.ToKeyCode());
    }

    // ── Digit row ─────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(Key.D0, KeyCode.D0)]
    [InlineData(Key.D1, KeyCode.D1)]
    [InlineData(Key.D2, KeyCode.D2)]
    [InlineData(Key.D3, KeyCode.D3)]
    [InlineData(Key.D4, KeyCode.D4)]
    [InlineData(Key.D5, KeyCode.D5)]
    [InlineData(Key.D6, KeyCode.D6)]
    [InlineData(Key.D7, KeyCode.D7)]
    [InlineData(Key.D8, KeyCode.D8)]
    [InlineData(Key.D9, KeyCode.D9)]
    public void ToKeyCode_DigitRow_ReturnExpected(Key avaloniaKey, KeyCode expected)
    {
        Assert.Equal(expected, avaloniaKey.ToKeyCode());
    }

    // ── Numpad ────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(Key.NumPad0, KeyCode.NumPad0)]
    [InlineData(Key.NumPad1, KeyCode.NumPad1)]
    [InlineData(Key.NumPad2, KeyCode.NumPad2)]
    [InlineData(Key.NumPad3, KeyCode.NumPad3)]
    [InlineData(Key.NumPad4, KeyCode.NumPad4)]
    [InlineData(Key.NumPad5, KeyCode.NumPad5)]
    [InlineData(Key.NumPad6, KeyCode.NumPad6)]
    [InlineData(Key.NumPad7, KeyCode.NumPad7)]
    [InlineData(Key.NumPad8, KeyCode.NumPad8)]
    [InlineData(Key.NumPad9, KeyCode.NumPad9)]
    public void ToKeyCode_Numpad_ReturnExpected(Key avaloniaKey, KeyCode expected)
    {
        Assert.Equal(expected, avaloniaKey.ToKeyCode());
    }

    // ── Function keys ─────────────────────────────────────────────────────────

    [Theory]
    [InlineData(Key.F1,  KeyCode.F1)]
    [InlineData(Key.F2,  KeyCode.F2)]
    [InlineData(Key.F3,  KeyCode.F3)]
    [InlineData(Key.F4,  KeyCode.F4)]
    [InlineData(Key.F5,  KeyCode.F5)]
    [InlineData(Key.F6,  KeyCode.F6)]
    [InlineData(Key.F7,  KeyCode.F7)]
    [InlineData(Key.F8,  KeyCode.F8)]
    [InlineData(Key.F9,  KeyCode.F9)]
    [InlineData(Key.F10, KeyCode.F10)]
    [InlineData(Key.F11, KeyCode.F11)]
    [InlineData(Key.F12, KeyCode.F12)]
    [InlineData(Key.F13, KeyCode.F13)]
    [InlineData(Key.F14, KeyCode.F14)]
    [InlineData(Key.F15, KeyCode.F15)]
    [InlineData(Key.F16, KeyCode.F16)]
    [InlineData(Key.F17, KeyCode.F17)]
    [InlineData(Key.F18, KeyCode.F18)]
    [InlineData(Key.F19, KeyCode.F19)]
    [InlineData(Key.F20, KeyCode.F20)]
    [InlineData(Key.F21, KeyCode.F21)]
    [InlineData(Key.F22, KeyCode.F22)]
    [InlineData(Key.F23, KeyCode.F23)]
    [InlineData(Key.F24, KeyCode.F24)]
    public void ToKeyCode_FunctionKeys_ReturnExpected(Key avaloniaKey, KeyCode expected)
    {
        Assert.Equal(expected, avaloniaKey.ToKeyCode());
    }

    // ── OEM / punctuation ─────────────────────────────────────────────────────

    [Theory]
    [InlineData(Key.OemComma,        KeyCode.OemComma)]
    [InlineData(Key.OemPeriod,       KeyCode.OemPeriod)]
    [InlineData(Key.OemQuestion,     KeyCode.OemQuestion)]
    [InlineData(Key.OemPlus,         KeyCode.OemPlus)]
    [InlineData(Key.OemMinus,        KeyCode.OemMinus)]
    [InlineData(Key.OemTilde,        KeyCode.OemTilde)]
    [InlineData(Key.OemOpenBrackets, KeyCode.OemOpenBrackets)]
    [InlineData(Key.OemCloseBrackets,KeyCode.OemCloseBrackets)]
    [InlineData(Key.OemPipe,         KeyCode.OemPipe)]
    [InlineData(Key.OemQuotes,       KeyCode.OemQuotes)]
    [InlineData(Key.OemSemicolon,    KeyCode.OemSemicolon)]
    [InlineData(Key.OemBackslash,    KeyCode.OemBackslash)]
    public void ToKeyCode_OemKeys_ReturnExpected(Key avaloniaKey, KeyCode expected)
    {
        Assert.Equal(expected, avaloniaKey.ToKeyCode());
    }

    // ── Modifier keys ─────────────────────────────────────────────────────────

    [Theory]
    [InlineData(Key.LeftCtrl,  KeyCode.LControlKey)]
    [InlineData(Key.RightCtrl, KeyCode.RControlKey)]
    [InlineData(Key.LeftShift, KeyCode.LShiftKey)]
    [InlineData(Key.RightShift,KeyCode.RShiftKey)]
    public void ToKeyCode_ModifierKeys_ReturnExpected(Key avaloniaKey, KeyCode expected)
    {
        Assert.Equal(expected, avaloniaKey.ToKeyCode());
    }

    // ── Numpad arithmetic operators ───────────────────────────────────────────

    [Theory]
    [InlineData(Key.Divide,   KeyCode.Divide)]
    [InlineData(Key.Multiply, KeyCode.Multiply)]
    [InlineData(Key.Add,      KeyCode.Add)]
    [InlineData(Key.Subtract, KeyCode.Subtract)]
    public void ToKeyCode_NumpadOperators_ReturnExpected(Key avaloniaKey, KeyCode expected)
    {
        Assert.Equal(expected, avaloniaKey.ToKeyCode());
    }

    // ── Navigation and editing keys ───────────────────────────────────────────

    [Theory]
    [InlineData(Key.Return,   KeyCode.Enter)]
    [InlineData(Key.Separator,KeyCode.Separator)]
    [InlineData(Key.Insert,   KeyCode.Insert)]
    [InlineData(Key.Delete,   KeyCode.Delete)]
    [InlineData(Key.Space,    KeyCode.Space)]
    [InlineData(Key.PageUp,   KeyCode.PageUp)]
    [InlineData(Key.PageDown, KeyCode.PageDown)]
    [InlineData(Key.End,      KeyCode.End)]
    [InlineData(Key.Home,     KeyCode.Home)]
    [InlineData(Key.Back,     KeyCode.Back)]
    [InlineData(Key.Tab,      KeyCode.Tab)]
    [InlineData(Key.Left,     KeyCode.Left)]
    [InlineData(Key.Right,    KeyCode.Right)]
    [InlineData(Key.Up,       KeyCode.Up)]
    [InlineData(Key.Down,     KeyCode.Down)]
    public void ToKeyCode_NavigationKeys_ReturnExpected(Key avaloniaKey, KeyCode expected)
    {
        Assert.Equal(expected, avaloniaKey.ToKeyCode());
    }

    // ── Unmapped keys return KeyCode.None ─────────────────────────────────────

    [Theory]
    [InlineData(Key.None)]
    [InlineData(Key.Escape)]
    [InlineData(Key.CapsLock)]
    [InlineData(Key.NumLock)]
    [InlineData(Key.Scroll)]
    [InlineData(Key.PrintScreen)]
    [InlineData(Key.Pause)]
    public void ToKeyCode_UnmappedKey_ReturnsNone(Key avaloniaKey)
    {
        Assert.Equal(KeyCode.None, avaloniaKey.ToKeyCode());
    }
}
