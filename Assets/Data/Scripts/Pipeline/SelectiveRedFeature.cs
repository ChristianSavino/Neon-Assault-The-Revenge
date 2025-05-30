using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SelectiveRedFeature : ScriptableRendererFeature
{
    class SelectiveRedPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;

        public SelectiveRedPass(Material material)
        {
            this.material = material;
            tempTexture.Init("_TempSelectiveRedTexture");
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
                return;

            // Aquí obtener el source correcto
            RenderTargetIdentifier cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;

            var stack = VolumeManager.instance.stack;
            var selectiveRedVolume = stack.GetComponent<SelectiveRedVolume>();

            if (selectiveRedVolume == null || !selectiveRedVolume.IsActive())
                return;

            CommandBuffer cmd = CommandBufferPool.Get("SelectiveRedPass");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(tempTexture.id, opaqueDesc);

            material.SetFloat("_Intensity", selectiveRedVolume.intensity.value);

            cmd.Blit(cameraColorTarget, tempTexture.Identifier(), material);
            cmd.Blit(tempTexture.Identifier(), cameraColorTarget);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Material material;

    SelectiveRedPass selectiveRedPass;

    public override void Create()
    {
        if (material == null)
        {
            Debug.LogWarning("SelectiveRedFeature: Material is not assigned!");
            return;
        }

        selectiveRedPass = new SelectiveRedPass(material)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (selectiveRedPass == null)
            return;

        // No usar renderer.cameraColorTarget aquí porque puede dar error
        // Solo llamamos Setup sin argumentos (puedes eliminar Setup o hacer overload)
        selectiveRedPass.Setup(new RenderTargetIdentifier()); // Dummy, porque no lo usaremos directamente aquí

        renderer.EnqueuePass(selectiveRedPass);
    }
}