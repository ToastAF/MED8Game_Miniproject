using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;

public class ColorBlindRenderFeature : ScriptableRendererFeature
{
    class PassData
    {
        public TextureHandle src;
        public Material mat;
        public int modeIndex;
    }

    class ColorBlindPass : ScriptableRenderPass
    {
        static readonly int ModeID = Shader.PropertyToID("_ColorBlindMode");
        Material mat;
        int modeIndex;

        public ColorBlindPass(Material m)
        {
            mat = m;
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
            requiresIntermediateTexture = true;
        }

        public void Setup(int mode)
        {
            modeIndex = mode;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourceData = frameData.Get<UniversalResourceData>();
            if (resourceData.isActiveTargetBackBuffer) return;

            var src = resourceData.cameraColor;

            var desc = renderGraph.GetTextureDesc(src);
            desc.name = "ColorBlindTemp";
            desc.clearBuffer = false;
            var tmp = renderGraph.CreateTexture(desc);

            int mode = modeIndex;
            Material material = mat;

            // Pass 1: src → tmp with color blind filter
            using (var builder = renderGraph.AddRasterRenderPass<PassData>("ColorBlind_Apply", out var passData))
            {
                passData.src = src;
                passData.mat = material;
                passData.modeIndex = mode;

                builder.UseTexture(src, AccessFlags.Read);
                builder.SetRenderAttachment(tmp, 0, AccessFlags.Write);

                builder.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
                {
                    data.mat.SetInt(ModeID, data.modeIndex);
                    Blitter.BlitTexture(ctx.cmd, data.src, new Vector4(1, 1, 0, 0), data.mat, 0);
                });
            }

            // Pass 2: tmp → cameraColor plain copy
            using (var builder = renderGraph.AddRasterRenderPass<PassData>("ColorBlind_CopyBack", out var passData))
            {
                passData.src = tmp;

                builder.UseTexture(tmp, AccessFlags.Read);
                builder.SetRenderAttachment(src, 0, AccessFlags.Write);

                builder.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
                {
                    Blitter.BlitTexture(ctx.cmd, data.src, new Vector4(1, 1, 0, 0), 0, false);
                });
            }
        }
    }

    Material mat;
    ColorBlindPass pass;

    public override void Create()
    {
        var shader = Shader.Find("Custom/ColorBlindFilter");
        if (shader == null)
        {
            Debug.LogError("ColorBlindRenderFeature: Shader not found!");
            return;
        }
        mat = new Material(shader);
        pass = new ColorBlindPass(mat);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (mat == null) return;

        // Read the ColorBlindCamera component from the current camera being rendered
        var cam = renderingData.cameraData.camera;
        var colorBlindCam = cam.GetComponent<ColorBlindCamera>();

        // Skip this camera if it has no component or is disabled
        if (colorBlindCam == null || !colorBlindCam.enabled) return;

        pass.Setup((int)colorBlindCam.mode);
        renderer.EnqueuePass(pass);
    }
}