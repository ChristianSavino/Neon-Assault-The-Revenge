using Keru.Scripts.Engine.Master;
using Keru.Scripts.Game.ScriptableObjects;
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

        private void Start()
        {
            DisableAllAttachments();
            var level = LevelBase.CurrentSave.Weapons.First(x => x.Code == WeaponData.WeaponCode);
            var attachments = WeaponData.WeaponDataPerLevel[level.Level - 1].Attachments.Split(",");
            foreach (var attachment in attachments)
            {
                _attachments[int.Parse(attachment) - 1].SetActive(true);
            }
        }

        private void DisableAllAttachments()
        {
            foreach (var attachment in _attachments)
            {
                attachment.SetActive(false);
            }
        }
    }
}