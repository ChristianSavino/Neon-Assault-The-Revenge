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
        protected Vector3 _modelOriginalPosition;

        public virtual void SetConfig()
        {
            _ragdollColliders = _model.gameObject.GetComponentsInChildren<Collider>();
            _weaponsModels = _weaponsHolder.GetComponentsInChildren<WeaponThirdPersonModel>(true);
            _model.SetLayerWeight((int)AnimationLayers.WEAPON, 0);

            ToggleCollider(false);
            
            var rigBuilder = _model.GetComponent<RigBuilder>();
            if (rigBuilder != null)
            {
                var aimRigLayer = rigBuilder.layers.FirstOrDefault(x => x.rig.name == "AimRig");
                if (aimRigLayer != null)
                {
                    _rig = aimRigLayer.rig;
                    var multiAim = _rig.transform.Find("MultiAim");
                    if (multiAim != null)
                        _bodyRig = multiAim.GetComponent<MultiAimConstraint>();
                }
            }
            _modelOriginalPosition = _model.transform.localPosition;
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

        protected Dictionary<GameObject, Vector3> GetBones()
        {
            var transforms = _model.transform.GetComponentsInChildren<Transform>().Where(x => x.gameObject.activeSelf);
            if(transforms.Any())
            {
                var dictionary = new Dictionary<GameObject, Vector3>();
                foreach (var transform in transforms)
                {
                    dictionary.Add(transform.gameObject, transform.position);
                }

                return dictionary;
            }

            return null;
        }

        protected void ApplyBones(Dictionary<GameObject, Vector3> transforms)
        {
            foreach (var value in transforms)
            {
                value.Key.transform.position = value.Value;
            }
        }

        protected void ToggleCollider(bool value)
        {
            foreach (var collider in _ragdollColliders)
            {
                collider.enabled = value;
                var rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = !value;
                    if (value)
                    {
                        rb.velocity = Vector3.zero;
                        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        rb.interpolation = RigidbodyInterpolation.Interpolate;
                        rb.drag = 0.5f;
                        rb.angularDrag = 0.5f;
                    }         
                }
            }
        }

        public void ApplyForceToClosestCollider(Vector3 hitpoint, float damageForce)
        {
            if(damageForce <= 0)
            {
                return;
            }
            
            Rigidbody closestCollider = null;
            var minDistance = float.MaxValue;

            foreach (var collider in _ragdollColliders)
            {
                float distance = Vector3.Distance(collider.transform.position, hitpoint);
                var rb = collider.attachedRigidbody;
                if (rb != null)
                {
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

            var weapon = _weaponsModels.FirstOrDefault(x => x.WeaponData.WeaponCode == weaponCode);
            if (weapon != null)
            {
                weapon.gameObject.SetActive(true);
            }     
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

        private string GetAnimationName(WeaponActions weaponAction) => weaponAction switch
        {
            WeaponActions.DEPLOY => AnimationNamesHelper.WeaponAimAnimation,
            WeaponActions.SHOOT => AnimationNamesHelper.WeaponShootAnimation,
            WeaponActions.RELOAD => AnimationNamesHelper.WeaponReloadAnimation,
            WeaponActions.RELOAD_EMPTY => AnimationNamesHelper.WeaponEmptyReloadAnimation,
            WeaponActions.RELOAD_OPEN => AnimationNamesHelper.WeaponReloadOpenAnimation,
            WeaponActions.RELOAD_CLOSE => AnimationNamesHelper.WeaponReloadCloseAnimation,
            WeaponActions.RELOAD_INSERT => AnimationNamesHelper.WeaponReloadInsertAnimation,
            _ => ""
        };

        private IEnumerator SetAnimatorLayer(AnimationLayers layer, int direction, float waitTime = 0f)
        {
            if (waitTime > 0f)
            {
                yield return new WaitForSeconds(waitTime);
            }
                
            float weight = _model.GetLayerWeight((int)layer);
            float target = direction == 1 ? 1f : 0f;
            float sign = direction == 1 ? 1f : -1f;

            while ((direction == 1 && weight < 1f) || (direction == 0 && weight > 0f))
            {
                weight += sign * Time.deltaTime / 0.25f;
                weight = Mathf.Clamp01(weight);
                _model.SetLayerWeight((int)layer, weight);
                if (direction == 0) SetRigWeight(1 - weight);
                yield return new WaitForEndOfFrame();
            }

            if (direction == 0)
            {
                SetRigWeight(1);
            }               
        }

        public GameObject GetModelObject()
        {
            return _model.gameObject;
        }
    }
}