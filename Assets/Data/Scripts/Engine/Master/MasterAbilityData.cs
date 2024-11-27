using Keru.Scripts.Game;
using System.Collections.Generic;

namespace Keru.Scripts.Engine.Master
{
    public static class MasterAbilityData
    {
        public static List<AbilityData> SecondaryAbilities { get; } = new List<AbilityData>()
        {
            new AbilityData()
            {
                Code = AbilityCodes.BULLETTIME,
                Name = "Tiempo Bala",
                Description = @"Keru se concentra en el combate que el tiempo casi se detiene, aumentando su letalidad.
Reducción:{Power} | Duracion: {Duration} | Recarga: {Cooldown}",
                AbilitySlot = AbilitySlot.SECONDARY,
                AbilityType = AbilityType.CROWDCONTROL,
                DataPerLevel = new List<AbilityDataPerLevel>()
                {
                    new AbilityDataPerLevel()
                    {
                        Power = 0.5f,
                        CoolDown = 25,
                        Duration = 5,
                        Range = 0
                    },
                    new AbilityDataPerLevel()
                    {
                        Power = 0.5f,
                        CoolDown = 20,
                        Duration = 6,
                        Range = 0
                    },
                    new AbilityDataPerLevel()
                    {
                        Power = 0.5f,
                        CoolDown = 15,
                        Duration = 7,
                        Range = 0
                    },
                }
            }
        };

        public static List<AbilityData> UltimateAbilities { get; } = new List<AbilityData>()
        {
            new AbilityData()
            {
                Code = AbilityCodes.JUDGEMENTCUT,
                Name = "Corte Justiciero",
                Description = @"Keru libera una parte del poder de la espada, dañando todo lo que esté delante de él.
Daño:{Power} | Duracion: {Duration} | Recarga: {Cooldown} | Rango: {Range}",
                AbilitySlot = AbilitySlot.ULTIMATE,
                AbilityType = AbilityType.DAMAGE,
                                DataPerLevel = new List<AbilityDataPerLevel>()
                {
                    new AbilityDataPerLevel()
                    {
                        Power = 500,
                        CoolDown = 90,
                        Duration = 2,
                        Range = 15
                    },
                    new AbilityDataPerLevel()
                    {
                        Power = 550,
                        CoolDown = 80,
                        Duration = 2.5f,
                        Range = 20
                    },
                    new AbilityDataPerLevel()
                    {
                        Power = 600,
                        CoolDown = 70,
                        Duration = 3,
                        Range = 25
                    },
                }
            }
        };
    }
}