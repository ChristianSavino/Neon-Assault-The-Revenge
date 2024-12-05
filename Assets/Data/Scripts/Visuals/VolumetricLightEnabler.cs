using Keru.Scripts.Engine.Module;
using UnityEngine;
using UnityEngine.Rendering;

namespace Keru.Scripts.Visuals
{
    public class VolumetricLightEnabler : MonoBehaviour
    {
        private void Start()
        {
            var volume = this.GetComponent<Volume>();
            GraphicsManager.graphicsManager.RegisterVolume(volume);
        }
    }
}