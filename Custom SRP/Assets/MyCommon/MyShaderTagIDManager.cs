using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum MyShaderTagId {
    Always,
    ForwardBase,
    PrepassBase,
    Vertex,
    VertexLMRGBM,
    VertexLM,
    _Unsupported_END,
// https://docs.unity3d.com/6000.2/Documentation/Manual/urp/urp-shaders/urp-shaderlab-pass-tags.html
    UniversalForward,
    UniversalGBuffer,
    UniversalForwardOnly,
    DepthNormalsOnly,
    Universal2D,
    ShadowCaster,
    DepthOnly,
    Meta,
    SRPDefaultUnlit,
    MotionVectors,
    _END,
}

public class MyShaderTagIDManager : MySingleton<MyShaderTagIDManager> {
    Dictionary<string, ShaderTagId> m_tags = new Dictionary<string, ShaderTagId>();

    public void Register(string tag) {
        if (m_tags.ContainsKey(tag)) return;
		m_tags.Add(tag, new ShaderTagId(tag));
	}

	public void Unregister(string tag) {
		if (m_tags.ContainsKey(tag)) {
			m_tags.Remove(tag);
		}
	}

    public ShaderTagId GetShaderTagId(string tag) {
        if (m_tags.TryGetValue(tag, out ShaderTagId res)) {
            return res;
        }
        return ShaderTagId.none;
    }

    public ShaderTagId GetShaderTagId(MyShaderTagId @enum) {
        string enumStr = Enum.GetName(typeof(MyShaderTagId), @enum);
        return GetShaderTagId(enumStr);
    }

    public void Register(MyShaderTagId @enum) {
        string enumStr = Enum.GetName(typeof(MyShaderTagId), @enum);
        Register(enumStr);
    }

    public void Unregister(MyShaderTagId @enum) {
        string enumStr = Enum.GetName(typeof(MyShaderTagId), @enum);
        Unregister(enumStr);
    }
}