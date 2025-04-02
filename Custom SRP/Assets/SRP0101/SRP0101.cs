using UnityEngine;
using UnityEngine.Rendering;

namespace SRP0101 {

[CreateAssetMenu(menuName = "Rendering/SRP0101RPAsset")]
public class SRP0101RPAsset : MyRenderPipelineAssetBase {
    public ShaderTagId shaderTagId;
    protected override RenderPipeline CreatePipeline() {
        shaderTagId = new ShaderTagId("SRP0101_Pass");
        return new SRP0101RP(new SRP0101CameraRenderer(), this);
    }
}

public sealed class SRP0101RP : MyRenderPipelineBase {
    public SRP0101RP(SRP0101CameraRenderer renderer, SRP0101RPAsset asset) : base(renderer, asset) {}
}

public class SRP0101CameraRenderer : MyCameraRendererBase {
    protected virtual ShaderTagId GetShaderTagId() {
        var asset = m_asset as SRP0101RPAsset;
        return asset.shaderTagId;
    }

    protected virtual void OnRenderSetUp() {
        m_cmd.name = "SRP0101 CmdBuf";
    }

	protected void OnRender_SkyBox() {
		var renderList = m_context.CreateSkyboxRendererList(m_camera);
        DrawRendererList(ref renderList);
	}

    protected void OnRender_Opaque() {
        var sortingSettings = new SortingSettings(m_camera) {
            criteria = SortingCriteria.CommonOpaque
        };
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque) {
            layerMask = m_camera.cullingMask
        };
        var drawingSettings = new DrawingSettings(GetShaderTagId(), sortingSettings);
        DrawObjects(ref drawingSettings, ref filteringSettings);
    }

    protected void OnRender_Transparent() {
        var sortingSettings = new SortingSettings(m_camera) {
            criteria = SortingCriteria.CommonTransparent
        };
        var filteringSettings = new FilteringSettings(RenderQueueRange.transparent) {
            layerMask = m_camera.cullingMask
        };
        var drawingSettings = new DrawingSettings(GetShaderTagId(), sortingSettings);
        DrawObjects(ref drawingSettings, ref filteringSettings);
    }

    protected virtual void OnRender_Impl() {        
        bool clearDepth = m_camera.clearFlags >= CameraClearFlags.Depth;
        bool clearColor = m_camera.clearFlags >= CameraClearFlags.Color;
        m_cmd.ClearRenderTarget(clearDepth, clearColor, m_camera.backgroundColor);

		if (m_camera.TryGetCullingParameters(out var p)) {
            m_cullingResults = m_context.Cull(ref p);

            OnRender_Opaque();
			OnRender_SkyBox();
            OnRender_Transparent();
		}
        ExecCmdBuf();
    }

    protected override void OnRender(MyRenderPipelineAssetBase asset) {
        OnRenderSetUp();
        OnBeginRender();
        OnRender_Impl();
        OnEndRender();
	}

    protected void OnBeginRender() {
        m_cmd.Clear();
        m_context.SetupCameraProperties(m_camera);
    }

    protected void OnEndRender() {
        m_context.Submit();
    }
}
} // namespace SRP0101