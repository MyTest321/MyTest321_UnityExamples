using UnityEngine;
using UnityEngine.Rendering;
using SRP0101;

namespace SRP0102 {

[CreateAssetMenu(menuName = "Rendering/SRP0102RPAsset")]
public class SRP0102RPAsset : MyRenderPipelineAssetBase {
    protected override RenderPipeline CreatePipeline() {
        MyShaderTagIDManager.instance.Register("SRP0102_Pass");
        return new SRP0102RP(new SRP0102CameraRenderer(), this);
    }
}

public sealed class SRP0102RP : MyRenderPipelineBase {
    public SRP0102RP(SRP0102CameraRenderer renderer, SRP0102RPAsset asset) : base(renderer, asset) {}
}

public class SRP0102CameraRenderer : SRP0101CameraRenderer {
    protected override ShaderTagId GetShaderTagId() {
        return MyShaderTagIDManager.instance.GetShaderTagId("SRP0102_Pass");
    }

    protected override void OnRenderSetUp() {
        m_cmd.name = "SRP0102 CmdBuf";
    }
}

} // namespace SRP0102