using Keru.Scripts.Game.Entities.Passives;
using Keru.Scripts.Game.Entities.Player.UI;
using Keru.Scripts.Game.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerPassiveHandler : PassiveHandler
    {
        private Player _player;
        private PlayerWeaponHandler _weaponHandler;
        private PlayerMovement _movementHandler;
        private PlayerThirdPersonAnimations _animations;
        private PlayerSpecialHandler _specialHandler;
        private PassiveUIHandler _passiveHandler;

        public override void SetUp(GameObject model)
        {
            base.SetUp(model);
        }

        public void SetUpPlayer(Player player, PlayerWeaponHandler weaponHandler, PlayerMovement playerMovement, PlayerThirdPersonAnimations thirdPersonAnimations, PlayerSpecialHandler specialHandler, PassiveUIHandler passiveHandler)
        {
            _player = player;
            _weaponHandler = weaponHandler;
            _movementHandler = playerMovement;
            _animations = thirdPersonAnimations;
            _specialHandler = specialHandler;
            _passiveHandler = passiveHandler;
        }

        public override Passive AddPassive(PassiveStats stats, int power, Entity owner, Type type = null, PassiveCode? passiveCode = null)
        {
            var result = base.AddPassive(stats, power, owner, type, passiveCode);
            _passiveHandler.AddPassive(result.GetStats());
            return result;
        }

        public override void UpdatePassives()
        {
            base.UpdatePassives();
            _weaponHandler.SetBonus(CalculateDamageBonus(), CalculateFireRateBonus());

        }

        private float CalculateFireRateBonus()
        {
            var fireRatePassives = _passives.Where(x => x.GetStats()?.Code == PassiveCode.FIRE_RATE);
            if(fireRatePassives != null && !fireRatePassives.Any())
            {
                return 0f;
            }

            var fireRateIndex = fireRatePassives.Where(x => x.GetStats().Potivity == PassivePotivity.POSITIVE).Sum(x => x.Power()) - fireRatePassives.Where(x => x.GetStats().Potivity == PassivePotivity.NEGATIVE).Sum(x => x.Power());
            return fireRateIndex / 100f;
        }

        private float CalculateDamageBonus()
        {
            var damagePassives = _passives.Where(x => x.GetStats()?.Code == PassiveCode.DAMAGE_RATE);
            if (damagePassives != null && !damagePassives.Any())
            {
                return 0f;
            }

            var damageIndex = damagePassives.Where(x => x.GetStats().Potivity == PassivePotivity.POSITIVE).Sum(x => x.Power()) - damagePassives.Where(x => x.GetStats().Potivity == PassivePotivity.NEGATIVE).Sum(x => x.Power());
            return damageIndex / 100f;
        }
    }
}
