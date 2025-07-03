using UnityEngine;

namespace Keru.Scripts.Game.Effects.AfterImages
{
    public static class AfterImageCreator
    {
        public static void CreateAfterImage(GameObject baseModel, float duration = 0.5f, float fadeSpeed = 2f)
        {
            var afterImageRoot = new GameObject("AfterImage");
            afterImageRoot.transform.position = baseModel.transform.position;
            afterImageRoot.transform.rotation = baseModel.transform.rotation;
            afterImageRoot.transform.localScale = baseModel.transform.lossyScale;

            var skinnedRenderers = baseModel.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in skinnedRenderers)
            {
                if (!renderer.gameObject.activeSelf || !renderer.enabled)
                {
                    continue;
                }

                var bakedMesh = new Mesh();
                renderer.BakeMesh(bakedMesh);

                var meshObj = new GameObject(renderer.name + "_AfterImageMesh");
                meshObj.transform.SetParent(afterImageRoot.transform, false);
                meshObj.transform.position = renderer.transform.position;
                meshObj.transform.rotation = renderer.transform.rotation;
                meshObj.transform.localScale = renderer.transform.lossyScale;

                var meshFilter = meshObj.AddComponent<MeshFilter>();
                meshFilter.mesh = bakedMesh;

                var meshRenderer = meshObj.AddComponent<MeshRenderer>();
                meshRenderer.materials = renderer.materials;
            }

            var meshRenderers = baseModel.GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in meshRenderers)
            {
                if (!renderer.gameObject.activeSelf || !renderer.enabled)
                {
                    continue;
                }

                var meshFilterSource = renderer.GetComponent<MeshFilter>();
                if (meshFilterSource == null || meshFilterSource.sharedMesh == null)
                {
                    continue;
                }

                var meshObj = new GameObject(renderer.name + "_AfterImageMesh");
                meshObj.transform.SetParent(afterImageRoot.transform, false);
                meshObj.transform.position = renderer.transform.position;
                meshObj.transform.rotation = renderer.transform.rotation;
                meshObj.transform.localScale = renderer.transform.lossyScale;

                var meshFilter = meshObj.AddComponent<MeshFilter>();
                meshFilter.mesh = meshFilterSource.sharedMesh;

                var meshRenderer = meshObj.AddComponent<MeshRenderer>();
                meshRenderer.materials = renderer.materials;
            }

            afterImageRoot.AddComponent<AfterImage>().SetConfig(duration, fadeSpeed);
        }
    }
}