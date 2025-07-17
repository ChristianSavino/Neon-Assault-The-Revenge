using Keru.Scripts.Game.Weapons;
using Keru.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Keru.Scripts.Game.Entities.Humanoid
{
    public class ThirdPersonAnimations : MonoBehaviour
    {
        [SerializeField] protected Animator _model;
        [SerializeField] protected GameObject _weaponsHolder;

        protected IEnumerable<Collider> _ragdollColliders;
        protected IEnumerable<WeaponThirdPersonModel> _weaponsModels;
        protected MultiAimConstraint _bodyRig;
        protected Rig _rig;

        public virtual void SetConfig()
        {
            _ragdollColliders = _model.gameObject.GetComponentsInChildren<Collider>();
            _weaponsModels = _weaponsHolder.GetComponentsInChildren<WeaponThirdPersonModel>(true);
            _model.SetLayerWeight((int)AnimationLayers.WEAPON, 0);

            ToggleCollider(false);
            
            var rigBuilder = _model.GetComponent<RigBuilder>();
            if (rigBuilder != null)
            {
                _rig = rigBuilder.layers.FirstOrDefault(x => x.rig.name == "AimRig").rig;
                _bodyRig = rigBuilder.layers.FirstOrDefault(x => x.rig.name == "AimRig").rig.transform.Find("MultiAim").GetComponent<MultiAimConstraint>();
            }
        }

        public virtual void SetParameter(string parameterName, float value)
        {
            _model.SetFloat(parameterName, value);
        }

        public virtual void SetParameter(string parameterName, bool value)
        {
            _model.SetBool(parameterName, value);
        }

        public virtual void SetParameter(string parameterName, int value)
        {
            _model.SetInteger(parameterName, value);
        }

        public virtual void PlayAnimation(string animationName, int layer = 0, int timeAt = 0)
        {
            _model.Play(animationName, layer, timeAt);
        }

        public virtual void PlayWeaponAnimation(WeaponActions weaponAction, WeaponCodes weaponCode, int timeAt = 0)
        {
            SetMultiAimRigWeight(1);
            var fullAnimationName = $"{WeaponAnimationNamesHelper.ReturnAnimationWeaponName(weaponCode)}{GetAnimationName(weaponAction)}";
            StartCoroutine(SetAnimatorLayer(AnimationLayers.WEAPON, 1));

            PlayAnimation(fullAnimationName, (int)AnimationLayers.WEAPON, timeAt);
        }

        public virtual void PlayMeleeWeaponAnimation(WeaponActions weaponAction, WeaponCodes weaponCode, int timeAt = 0)
        {
            SetMultiAimRigWeight(0);
            var fullAnimationName = $"{WeaponAnimationNamesHelper.ReturnAnimationWeaponName(weaponCode)}{GetAnimationName(weaponAction)}";
            var attack = Random.Range(0, 11);
            SetParameter("meleeAttack", attack);
            StartCoroutine(SetAnimatorLayer(AnimationLayers.WEAPON, 1));
            PlayAnimation(fullAnimationName, (int)AnimationLayers.WEAPON, timeAt);
        }

        public virtual void PlaySpecialAnimation(string name, float time)
        {
            SetRigWeight(0);
            StartCoroutine(SetAnimatorLayer(AnimationLayers.SPECIAL, 1));
            PlayAnimation(name, (int)AnimationLayers.SPECIAL);

            StartCoroutine(SetAnimatorLayer(AnimationLayers.SPECIAL, 0, time));
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

        protected virtual void SetRigWeight(float weight)
        {
            if (_rig != null)
            {
                _rig.weight = weight;
            }
        }

        protected virtual void SetMultiAimRigWeight(float weight)
        {
            if (_bodyRig != null)
            {
                _bodyRig.weight = weight;
            }
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

        private IEnumerator SetAnimatorLayer(AnimationLayers layer, int direction, float waitTime = 0f)
        {
            if(waitTime > 0f)
            {
                yield return new WaitForSeconds(waitTime);
            }

            var weight = _model.GetLayerWeight((int)layer);

            if (direction == 1)
            {
                while(weight < 1)
                {
                    weight += Time.deltaTime / 0.25f;
                    _model.SetLayerWeight((int)layer, Mathf.Clamp01(weight));
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                while(weight > 0)
                {
                    weight -= Time.deltaTime / 0.25f;
                    SetRigWeight(1 - weight);
                    _model.SetLayerWeight((int)layer, Mathf.Clamp01(weight));
                    yield return new WaitForEndOfFrame();
                }
                SetRigWeight(1);
            }
        }

        public GameObject GetModelObject()
        {
            return _model.gameObject;
        }
    }
}