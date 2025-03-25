using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
// using UnityEngine.Profiling;

[CreateAssetMenu(menuName = "Rendering/SRP001RPAsset")]
public class SRP001RPAsset : RenderPipelineAsset<SRP001RP> {
    protected override RenderPipeline CreatePipeline() {
        return new SRP001RP(this);
    }
}

public class SRP001RP : RenderPipeline {
    SRP001CameraRenderer renderer = new SRP001CameraRenderer();
    SRP001RPAsset m_asset;

    public SRP001RP(SRP001RPAsset asset) {
        m_asset = asset;
    }

    protected override void Render (ScriptableRenderContext m_context, List<Camera> cameras)
    {
        foreach (var m_camera in cameras) {
			renderer.Render(ref m_context, m_camera, m_asset);
		}
    }

    protected override void Render (ScriptableRenderContext m_context, Camera[] cameras) {
        // do nothing
    }
}

public class SRP001CameraRenderer {
    ScriptableRenderContext m_context;
    Camera                  m_camera;
    SRP001RPAsset           m_asset;
    CullingResults          m_cullingResults;

    const string k_cmdName = "SRP001 CmdBuf";
    CommandBuffer m_cmd = new CommandBuffer { name = k_cmdName };

    public struct ShaderPassTags {
        public static readonly ShaderTagId MyShaderPassName = new ShaderTagId("SRP001_Pass");
    }

    void ExecCmdBuf() {
        m_context.ExecuteCommandBuffer(m_cmd);
        m_cmd.Clear();
	}

    void DrawRendererList(ref DrawingSettings refDrawingSettings, ref FilteringSettings refFilteringSettings) {
        // ensure execute all commands before CoreUtils.DrawRendererList
        ExecCmdBuf();
        
        // and new one
        RendererListParams renderListParams = new RendererListParams(m_cullingResults, refDrawingSettings, refFilteringSettings);
        RendererList renderList = m_context.CreateRendererList(ref renderListParams);
        CoreUtils.DrawRendererList(m_context, m_cmd, renderList);
    }

    void OnRender_Unlit_Opaque() {
        var sortingSettings = new SortingSettings(m_camera) {
            criteria = SortingCriteria.CommonOpaque
        };
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque) {
            layerMask = m_camera.cullingMask
        };
        var drawingSettings = new DrawingSettings(ShaderPassTags.MyShaderPassName, sortingSettings);
        DrawRendererList(ref drawingSettings, ref filteringSettings);
    }

    void OnRender_Transparent() {
        var sortingSettings = new SortingSettings(m_camera) {
            criteria = SortingCriteria.CommonTransparent
        };
        var filteringSettings = new FilteringSettings(RenderQueueRange.transparent) {
            layerMask = m_camera.cullingMask
        };
        var drawingSettings = new DrawingSettings(ShaderPassTags.MyShaderPassName, sortingSettings);
        DrawRendererList(ref drawingSettings, ref filteringSettings);
    }

    void OnRender() {
        //RenderPipelineManager.BeginCameraRendering(m_context, m_camera);
        
        bool clearDepth = m_camera.clearFlags >= CameraClearFlags.Depth;
        bool clearColor = m_camera.clearFlags >= CameraClearFlags.Color;
        m_cmd.ClearRenderTarget(clearDepth, clearColor, m_camera.backgroundColor);


		if (m_camera.TryGetCullingParameters(out var p)) {
            m_cullingResults = m_context.Cull(ref p);
            OnRender_Unlit_Opaque();
            OnRender_Transparent();
		}

        ExecCmdBuf();
        //RenderPipelineManager.EndCameraRendering(m_context, m_camera);
    }

    void OnBeginRender() {
        m_cmd.Clear();
        m_context.SetupCameraProperties(m_camera);
    }

    void OnEndRender() {
        m_context.Submit();
    }

    public void Render (ref ScriptableRenderContext context, Camera camera, SRP001RPAsset asset) {
		m_context = context;
		m_camera  = camera;
        m_asset   = asset;

        OnBeginRender();
        OnRender();
        OnEndRender();
	}
}
