using Keru.Scripts.Game.Weapons;
using Keru.Scripts.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Humanoid
{
    public class ThirdPersonAnimations : MonoBehaviour
    {
        [SerializeField] protected Animator _model;
        [SerializeField] protected GameObject _weaponsHolder;

        protected IEnumerable<Collider> _ragdollColliders;
        protected IEnumerable<WeaponThirdPersonModel> _weaponsModels;

        public virtual void SetConfig()
        {
            _ragdollColliders = _model.gameObject.GetComponentsInChildren<Collider>();
            _weaponsModels = _weaponsHolder.GetComponentsInChildren<WeaponThirdPersonModel>(true);
            _model.SetLayerWeight((int)AnimationLayers.WEAPON, 0);

            ToggleCollider(false);
        }

        public virtual void SetParameter(string parameterName, float value)
        {
            _model.SetFloat(parameterName, value);
        }

        public virtual void SetParameter(string parameterName, bool value)
        {
            _model.SetBool(parameterName, value);
        }

        public virtual void PlayAnimation(string animationName, int layer = 0, int timeAt = 0)
        {
            _model.Play(animationName, layer, timeAt);
        }

        public virtual void PlayWeaponAnimation(WeaponActions weaponAction, WeaponCodes weaponCode, int timeAt = 0)
        {
            var fullAnimationName = $"{WeaponAnimationNamesHelper.ReturnAnimationWeaponName(weaponCode)}{GetAnimationName(weaponAction)}";
            _model.SetLayerWeight((int)AnimationLayers.WEAPON, 1);

            PlayAnimation(fullAnimationName, (int)AnimationLayers.WEAPON, timeAt);
        }

        public virtual void Die(Vector3 hitpoint, float damageForce)
        {
            _model.Update(0);
            _model.enabled = false;

            ToggleCollider(true);
            ApplyForceToClosestCollider(hitpoint, damageForce);
        }

        protected void ToggleCollider(bool value)
        {
            foreach (var collider in _ragdollColliders)
            {
                collider.enabled = value;
                var rb = collider.attachedRigidbody;
                if (rb != null)
                {
                    rb.isKinematic = !value;
                }
            }
        }

        protected void ApplyForceToClosestCollider(Vector3 hitpoint, float damageForce)
        {
            Rigidbody closestCollider = null;
            var minDistance = float.MaxValue;

            foreach (var collider in _ragdollColliders)
            {
                float distance = Vector3.Distance(collider.transform.position, hitpoint);
                var rb = collider.attachedRigidbody;
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    rb.interpolation = RigidbodyInterpolation.Interpolate;
                    rb.drag = 1;
                    rb.angularDrag = 0.5f;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestCollider = rb;
                    }
                }
            }

            if (closestCollider != null)
            {
                var direction = (transform.position - hitpoint).normalized;
                closestCollider.AddForce(direction * damageForce, ForceMode.Impulse);
                print(closestCollider.gameObject.name);
            }
        }

        protected virtual void SetWeaponModel(WeaponCodes weaponCode)
        {
            foreach (var model in _weaponsModels)
            {
                model.gameObject.SetActive(false);
            }

            var weapon = _weaponsModels.First(x => x.WeaponData.WeaponCode == weaponCode);
            weapon.gameObject.SetActive(true);
        }

        public virtual GameObject GetWeaponModel(WeaponCodes weaponCode)
        {
            return _weaponsModels.First(x => x.WeaponData.WeaponCode == weaponCode).gameObject;
        }
        
        private string GetAnimationName(WeaponActions weaponAction)
        {
            switch (weaponAction)
            {
                case WeaponActions.DEPLOY:
                   return AnimationNamesHelper.WeaponAimAnimation;
                case WeaponActions.SHOOT:
                   return AnimationNamesHelper.WeaponShootAnimation;
                case WeaponActions.RELOAD:
                   return AnimationNamesHelper.WeaponReloadAnimation;
                case WeaponActions.RELOAD_EMPTY:
                   return AnimationNamesHelper.WeaponEmptyReloadAnimation;
                case WeaponActions.RELOAD_OPEN:
                   return AnimationNamesHelper.WeaponReloadOpenAnimation;
                case WeaponActions.RELOAD_CLOSE:
                   return AnimationNamesHelper.WeaponReloadCloseAnimation;
                case WeaponActions.RELOAD_INSERT:
                    return AnimationNamesHelper.WeaponReloadInsertAnimation;
            }

            return "";
        }
    }
}