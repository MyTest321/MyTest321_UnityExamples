using UnityEngine;
using UnityEngine.Rendering;
using SRP0102;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
#endif

namespace SRP0103 {

//[CreateAssetMenu(menuName = "Rendering/SRP0103RPAsset")]
public class SRP0103RPAsset : MyRenderPipelineAssetBase {
    public bool drawOpaqueObjects = true;
    public bool drawSkyBox = true;
    public bool drawTransparentObjects = true;
    
    protected override RenderPipeline CreatePipeline() {
        MyShaderTagIDManager.instance.Register("SRP0102_Pass");
        return new SRP0103RP(new SRP0103CameraRenderer(), this);
    }

#if UNITY_EDITOR // another work around for [CreateAssetMenu], and it could specific the .asset filename on created
    [MenuItem("Assets/Create/Rendering/SRP0103RPAsset")]
    static void CreateSRP0103()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateSRP0103_Asset>(), "SRP0103.asset", null, null);
    }

    class CreateSRP0103_Asset : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var instance = CreateInstance<SRP0103RPAsset>();
            AssetDatabase.CreateAsset(instance, pathName);
        }
    }
#endif // UNITY_EDITOR
}

public sealed class SRP0103RP : MyRenderPipelineBase {
    public SRP0103RP(SRP0103CameraRenderer renderer, SRP0103RPAsset asset) : base(renderer, asset) {}
}

public class SRP0103CameraRenderer : SRP0102CameraRenderer {
    protected override void OnRender_Impl() {
        var asset = m_asset as SRP0103RPAsset;
        bool clearDepth = m_camera.clearFlags >= CameraClearFlags.Depth;
        bool clearColor = m_camera.clearFlags >= CameraClearFlags.Color;
        m_cmd.ClearRenderTarget(clearDepth, clearColor, m_camera.backgroundColor);

		if (m_camera.TryGetCullingParameters(out var p)) {
            m_cullingResults = m_context.Cull(ref p);
            if (asset.drawOpaqueObjects)        OnRender_Opaque();
            if (asset.drawSkyBox)               OnRender_SkyBox();
            if (asset.drawTransparentObjects)   OnRender_Transparent();
		}
        ExecCmdBuf();
    }
}

} // namespace SRP0103