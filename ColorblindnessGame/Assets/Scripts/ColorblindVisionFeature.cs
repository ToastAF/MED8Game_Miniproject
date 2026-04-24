using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorblindVisionFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material colorblindFilterMaterial;
        public Material compositeMaterial;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public float circleRadius = 0.15f;
        public float edgeSoftness = 0.02f;
    }

    public Settings settings = new Settings();
    private ColorblindVisionPass _pass;

    public override void Create()
    {
        _pass = new ColorblindVisionPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.colorblindFilterMaterial == null || settings.compositeMaterial == null)
            return;

        renderer.EnqueuePass(_pass);
    }

    class ColorblindVisionPass : ScriptableRenderPass
    {
        private Settings _settings;
        private RTHandle _grayscaleRT;
        private RTHandle _protanopiaRT;
        private RTHandle _tritanopiaRT;

        public ColorblindVisionPass(Settings settings)
        {
            _settings = settings;
            renderPassEvent = settings.renderPassEvent;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            RenderingUtils.ReAllocateIfNeeded(ref _grayscaleRT, descriptor, name: "_GrayscaleRT");
            RenderingUtils.ReAllocateIfNeeded(ref _protanopiaRT, descriptor, name: "_ProtanopiaRT");
            RenderingUtils.ReAllocateIfNeeded(ref _tritanopiaRT, descriptor, name: "_TritanopiaRT");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("ColorblindVision");

            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // Create grayscale version
            _settings.colorblindFilterMaterial.SetFloat("_FilterType", 2);
            Blitter.BlitCameraTexture(cmd, source, _grayscaleRT, _settings.colorblindFilterMaterial, 0);

            // Create protanopia version
            _settings.colorblindFilterMaterial.SetFloat("_FilterType", 0);
            Blitter.BlitCameraTexture(cmd, source, _protanopiaRT, _settings.colorblindFilterMaterial, 0);

            // Create tritanopia version
            _settings.colorblindFilterMaterial.SetFloat("_FilterType", 1);
            Blitter.BlitCameraTexture(cmd, source, _tritanopiaRT, _settings.colorblindFilterMaterial, 0);

            // Get player positions from the manager
            var manager = ColorblindVisionManager.Instance;
            Vector2 p1Screen = Vector2.zero;
            Vector2 p2Screen = Vector2.zero;

            if (manager != null)
            {
                var cam = renderingData.cameraData.camera;
                if (manager.player1 != null)
                {
                    Vector3 vp = cam.WorldToViewportPoint(manager.player1.position);
                    p1Screen = new Vector2(vp.x, vp.y);
                }
                if (manager.player2 != null)
                {
                    Vector3 vp = cam.WorldToViewportPoint(manager.player2.position);
                    p2Screen = new Vector2(vp.x, vp.y);
                }
            }

            // Set composite material properties
            _settings.compositeMaterial.SetTexture("_ProtanopiaTex", _protanopiaRT);
            _settings.compositeMaterial.SetTexture("_TritanopiaTex", _tritanopiaRT);
            _settings.compositeMaterial.SetVector("_Player1Pos", p1Screen);
            _settings.compositeMaterial.SetVector("_Player2Pos", p2Screen);
            _settings.compositeMaterial.SetFloat("_CircleRadius", _settings.circleRadius);
            _settings.compositeMaterial.SetFloat("_EdgeSoftness", _settings.edgeSoftness);
            _settings.compositeMaterial.SetFloat("_ScreenAspect",
                (float)renderingData.cameraData.camera.pixelWidth / renderingData.cameraData.camera.pixelHeight);

            // Final composite
            Blitter.BlitCameraTexture(cmd, _grayscaleRT, source, _settings.compositeMaterial, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // RTHandles are cleaned up automatically
        }
    }
}
