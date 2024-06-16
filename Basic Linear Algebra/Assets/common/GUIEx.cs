using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GUIEx {

    public static Vector2 CalcTextSize(string text)
    {
        if (string.IsNullOrEmpty(text)) {
            return Vector2.zero;
        }
        var content = new GUIContent(text);
        return GUI.skin.label.CalcSize(content);
    }

    public static void DrawText(in float screenX,
                                in float screenY,
                                in string text,
                                in Color color,
                                in int fontSize = 30)
    {
        if (string.IsNullOrEmpty(text)) {
            return;
        }

        using (new ScopedHandlesBeginGUI())
        {
            using (new ScopedValueGUI(color, ScopedValueGUI.ToGUILabelFontSize(fontSize)))
            {
                Vector2 size = CalcTextSize(text);
                GUI.Label(new Rect(screenX, screenY, size.x, size.y), text);
            }
        }
    }

    public static void DrawText(in Vector3 worldPos, string text, in Color color, int fontSize = 30)
    {
        var editorSceneView = UnityEditor.SceneView.currentDrawingSceneView;
        var sceneCamera     = editorSceneView.camera;
        Vector3 screenPos   = sceneCamera.WorldToScreenPoint(worldPos);
        float inverseY      = sceneCamera.pixelHeight - screenPos.y;
        DrawText(screenPos.x, inverseY, text, color, fontSize);
    }
}

public class ScopedSetterValue<T> : IDisposable
    where T : struct
{
    T?          _oldValue   = null;
    Action<T>   _setter     = null;

    public void Set(T value, T newValue, Action<T> setter)
    {
        Detach();
        _oldValue = value;
        _setter = setter;
        setter(newValue);
    }

    void Detach()
    {
        if (_setter != null && _oldValue != null)
        {
            _setter((T)_oldValue);
        }
        _setter     = null;
        _oldValue   = null;
    }

    public void Dispose()
    {
        Detach();
    }
}

public enum GUILabelFontSize : int
{
    Zero = 0,
}

public struct ScopedValueGUI : IDisposable
{
    private ScopedColor _color;
    private ScopedLabelFontSize _labelFontSize;

    public ScopedValueGUI(in Color newColor)
    {
        _color = newColor;
        _labelFontSize = null;
    }

    public ScopedValueGUI(in GUILabelFontSize newlabelFontSize)
    {
        _color = null;
        _labelFontSize = newlabelFontSize;
    }

    public ScopedValueGUI(in Color newColor, in GUILabelFontSize newlabelFontSize)
    {
        _color = newColor;
        _labelFontSize = newlabelFontSize;
    }

    public void Dispose()
    {
        DisposeVar(ref _color);
        DisposeVar(ref _labelFontSize);
    }

    static void DisposeVar<T>(ref T v) where T : class, IDisposable
    {
        if (v != null)
        {
            v.Dispose();
            v = null;
        }
    }

    #region

    public static GUILabelFontSize ToGUILabelFontSize(int fs)
    {
        return GUILabelFontSize.Zero + fs;
    }

    sealed class ScopedLabelFontSize : ScopedSetterValue<int>
    {
        static ScopedLabelFontSize Create(GUILabelFontSize newFontSize)
        {
            var newValue = Convert.ToInt32(newFontSize);
            var res = new ScopedLabelFontSize();
            res.Set(GUI.skin.label.fontSize, newValue, (o) => { GUI.skin.label.fontSize = o; });
            return res;
        }

        public static implicit operator ScopedLabelFontSize(GUILabelFontSize fs) => Create(fs);
    }

    sealed class ScopedColor : ScopedSetterValue<Color>
    {
        static ScopedColor Create(Color newColor)
        {
            var res = new ScopedColor();
            res.Set(GUI.color, newColor, (o) => { GUI.color = o; });
            return res;
        }

        public static implicit operator ScopedColor(Color c) => Create(c);
    }
    #endregion
}


public class ScopedHandlesBeginGUI : IDisposable
{
    public ScopedHandlesBeginGUI()
    {
        UnityEditor.Handles.BeginGUI();
    }

    public void Dispose()
    {
        UnityEditor.Handles.EndGUI();
    }
}