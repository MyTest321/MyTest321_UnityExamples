using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class MySceneRenderPipeline : MonoBehaviour
{
    public RenderPipelineAsset renderPipelineAsset;

    void OnEnable()
    {
        GraphicsSettings.defaultRenderPipeline = renderPipelineAsset;
    }

    void OnValidate()
    {
        GraphicsSettings.defaultRenderPipeline = renderPipelineAsset;
    }
}
