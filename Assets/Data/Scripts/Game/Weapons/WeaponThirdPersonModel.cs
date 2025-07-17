using Keru.Scripts.Engine.Master;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.Weapons.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Weapons
{
    public class WeaponThirdPersonModel : MonoBehaviour
    {
        public GunStats WeaponData;
        [SerializeField] private List<GameObject> _attachments;
        [SerializeField] private GameObject _bulletModel;
        [SerializeField] private GameObject _magazine;
        [SerializeField] private List<WeaponAnimationStamp> _reloadAnimationStamps;
        [SerializeField] private Vector3 _leftHandMagRotation = Vector3.zero;
        [SerializeField] private List<WeaponAnimationStamp> _overrideAnimationEmptyReloadStamps;
        [SerializeField] private float _overrideReloadtime;

        private bool _changeOnStart = true;
        private int _level;
        private Transform _leftHand;

        private void Start()
        {
            if (_changeOnStart && WeaponData.WeaponSlot != WeaponSlot.MELEE)
            {
                _level = LevelBase.CurrentSave.Weapons.First(x => x.Code == WeaponData.WeaponCode).Level;
                SetUpAttachments(_level);
            }
        }

        public void ConfigWeapon(int level, Transform leftHand)
        {
            _changeOnStart = false;
            _level = level;
            _leftHand = leftHand;
            SetUpAttachments(level);
        }

        private void DisableAllAttachments()
        {
            foreach (var attachment in _attachments)
            {
                attachment.SetActive(false);
            }
        }

        private void SetUpAttachments(int level)
        {
            DisableAllAttachments();
            var attachments = WeaponData.WeaponDataPerLevel[level - 1].Attachments.Split(",");
            foreach (var attachment in attachments)
            {
                _attachments[int.Parse(attachment) - 1].SetActive(true);
            }
        }

        public void PlayReloadAnimation(bool isEmpty = false)
        {
            if(isEmpty && _overrideAnimationEmptyReloadStamps.Count != 0)
            {
                StartCoroutine(AnimationReload(_overrideAnimationEmptyReloadStamps));
            }
            else if (_reloadAnimationStamps.Count != 0)
            {
                StartCoroutine(AnimationReload(_reloadAnimationStamps));
            }
        }

        public void TurnBulletModel(bool toggle)
        {
            if (_bulletModel != null)
            {
                _bulletModel.SetActive(toggle);
            }
        }

        private IEnumerator AnimationReload(List<WeaponAnimationStamp> animationStamps)
        {
            var leftHandMag = SetLeftHandMag(_magazine);
            leftHandMag.SetActive(false);

            for (int i = 0; i < animationStamps.Count; i++)
            {
                var stamp = animationStamps[i];
                var previousStamp = i - 1 >= 0 ? animationStamps[i - 1] : null;

                var time = stamp.Time - (previousStamp != null ? previousStamp.Time : 0f);
                yield return new WaitForSeconds(time);

                switch (stamp.AnimType)
                {
                    case WeaponAnimationStampType.NONE:
                        leftHandMag.SetActive(false);
                        _magazine.SetActive(false);
                        break;
                    case WeaponAnimationStampType.WEAPON:
                        leftHandMag.SetActive(false);
                        _magazine.SetActive(true);
                        break;
                    case WeaponAnimationStampType.LEFT_HAND:
                        leftHandMag.SetActive(true);
                        _magazine.SetActive(false);
                        break;
                    default:
                        break;
                }
            }

            TurnBulletModel(true);
        }

        private GameObject SetLeftHandMag(GameObject mag)
        {
            var obj = Instantiate(mag, _leftHand);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(_leftHandMagRotation);

            return obj;
        }
    }
}