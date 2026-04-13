using System;
using Avalonia;
using Avalonia.Data;
using AvaloniaEdit;

namespace VmMachineHwVersionUpdater.Avalonia.Views;

/// <summary>
///     Provides a bindable Text property for AvaloniaEdit's TextEditor.
/// </summary>
public static class TextEditorBindingHelper
{
    /// <summary>
    ///     Bindable Text property for AvaloniaEdit's TextEditor.
    /// </summary>
    public static readonly AttachedProperty<string> TextProperty =
        AvaloniaProperty.RegisterAttached<TextEditor, string>(
            "Text",
            typeof(TextEditorBindingHelper),
            string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

    static TextEditorBindingHelper()
    {
        TextProperty.Changed.AddClassHandler<TextEditor>(HandleTextChanged);
    }

    private static void HandleTextChanged(TextEditor editor, AvaloniaPropertyChangedEventArgs e)
    {
        var text = e.NewValue as string ?? string.Empty;
        if (editor.Text != text)
        {
            editor.Text = text;
        }

        editor.TextChanged -= EditorOnTextChanged;
        editor.TextChanged += EditorOnTextChanged;
    }

    private static void EditorOnTextChanged(object sender, EventArgs e)
    {
        if (sender is TextEditor editor)
        {
            SetText(editor, editor.Text);
        }
    }

    /// <summary>
    ///     Gets the value of the Text property.
    /// </summary>
    /// <param name="element">The TextEditor element.</param>
    /// <returns>The value of the Text property.</returns>
    public static string GetText(TextEditor element)
    {
        ArgumentNullException.ThrowIfNull(element);
        return element.GetValue(TextProperty);
    }

    /// <summary>
    ///     Sets the value of the Text property.
    /// </summary>
    /// <param name="element">The TextEditor element.</param>
    /// <param name="value">The value to set.</param>
    public static void SetText(TextEditor element, string value)
    {
        ArgumentNullException.ThrowIfNull(element);
        element.SetValue(TextProperty, value);
    }
}
