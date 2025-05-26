using Keru.Scripts.Game.Actions.Camera;
using Keru.Scripts.Game.Entities.Humanoid;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerThirdPersonAnimations : ThirdPersonAnimations
    {
        private Collider _hips;

        public override void SetConfig()
        {
            base.SetConfig();
            _hips = _ragdollColliders.First(x => x.gameObject.name == "Hips");
        }

        public override void Die(Vector3 hitpoint, float damageForce)
        {
            base.Die(hitpoint, damageForce);
            if (_hips != null)
            {
                var camera = Camera.main;
                var lookAt = camera.AddComponent<CameraFollowTarget>();
                lookAt.SetConfig(_hips.transform);
            }
        }
    }
}