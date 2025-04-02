using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class MyRenderPipelineAssetBase : RenderPipelineAsset/*, ISerializationCallbackReceiver*/ {
}

public abstract class MyCameraRendererBase {
    protected ScriptableRenderContext   m_context;
    protected Camera                    m_camera;
    protected CullingResults            m_cullingResults;
    protected MyRenderPipelineAssetBase m_asset;
    protected CommandBuffer             m_cmd = new CommandBuffer();

    protected abstract void OnRender(MyRenderPipelineAssetBase asset);

    public void Render(ref ScriptableRenderContext context, Camera camera, MyRenderPipelineAssetBase asset) {
		m_context = context;
		m_camera  = camera;
        m_asset   = asset;
        OnRender(asset);
	}

    public void ExecCmdBuf() {
        m_context.ExecuteCommandBuffer(m_cmd);
        m_cmd.Clear();
	}

    public void DrawObjects(ref DrawingSettings refDrawingSettings, ref FilteringSettings refFilteringSettings) {
        RendererListParams renderListParams = new RendererListParams(m_cullingResults, refDrawingSettings, refFilteringSettings);
        RendererList renderList = m_context.CreateRendererList(ref renderListParams);
        DrawRendererList(ref renderList);
    }

    public void DrawRendererList(ref RendererList refRenderList) {
		// ensure execute all commands before CoreUtils.DrawRendererList
        ExecCmdBuf();
		CoreUtils.DrawRendererList(m_context, m_cmd, refRenderList);
	}
}

public abstract class MyRenderPipelineBase : RenderPipeline {
    protected readonly MyCameraRendererBase m_renderer;
    protected readonly MyRenderPipelineAssetBase m_asset;

    protected MyRenderPipelineBase(MyCameraRendererBase renderer, MyRenderPipelineAssetBase asset) {
        m_renderer = renderer;
        m_asset = asset;
    }

    protected override void Render (ScriptableRenderContext m_context, List<Camera> cameras)
    {
        foreach (var camera in cameras) {
			m_renderer.Render(ref m_context, camera, m_asset);
		}
    }

    protected override void Render (ScriptableRenderContext m_context, Camera[] cameras) {
        // do nothing
    }
}
