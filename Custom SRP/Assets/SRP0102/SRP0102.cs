using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/SRP0102RPAsset")]
public class SRP0102RPAsset : SRP0101RPAsset {
    protected override RenderPipeline CreatePipeline() {
        MyShaderTagIDManager.instance.Register("SRP0102_Pass");
        return new SRP0102RP(new SRP0102CameraRenderer(), this);
    }
}

public class SRP0102RP : MyRenderPipelineBase {
    public SRP0102RP(SRP0102CameraRenderer renderer, SRP0102RPAsset asset) : base(renderer, asset) {}
}

public class SRP0102CameraRenderer : SRP0101CameraRenderer {
    SRP0102RPAsset m_asset;

    protected override ShaderTagId shaderTagId() {
        return MyShaderTagIDManager.instance.GetShaderTagId("SRP0102_Pass");
    }

    protected override void OnRender(MyRenderPipelineAssetBase asset) {
        m_asset    = asset as SRP0102RPAsset;
        m_cmd.name = "SRP0102 CmdBuf";

        OnBeginRender();
        OnRender_Impl();
        OnEndRender();
	}
}