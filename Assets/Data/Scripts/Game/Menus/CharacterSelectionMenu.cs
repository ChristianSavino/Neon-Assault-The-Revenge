using Keru.Scripts.Engine.Master;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus
{
    public class CharacterSelectionMenu : MonoBehaviour
    {
        [SerializeField] MapMainMenu _map;
        [SerializeField] WeaponSelectionMenu _weaponSelectionMenu;
        [SerializeField] Text _levelNameText;
        [SerializeField] List<GameObject> _characterDescription;
        [SerializeField] List<GameObject> _characterModel;      

        private string _levelName;
        private int _index;
        private CurrentCharacterData _currentCharacter;

        private void OnEnable()
        {
            _levelNameText.text = $"{_levelName} -> selecciona personaje".ToUpper();
            _currentCharacter = LevelBase.CurrentSave.SelectedCharacter;

            ToggleOffAllCharacters();
            _index = (int)_currentCharacter.Character;
            ChangeCharacter(0);
        }

        public void SetLevelName(string levelName)
        {
            _levelName = levelName;
        }

        public void Cancel()
        {
            ToggleOffAllCharacters();
            _map.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void Continue()
        {
            var characterData = LevelBase.CurrentSave.Characters.First(x => x.Character == (Character)_index);
            var health = characterData.BaseHealth + (characterData.HealthPerLevel * (characterData.Level - 1));

            _currentCharacter.Character = characterData.Character;
            _currentCharacter.CurrentHealth = health;
            _currentCharacter.MaxHealth = health;

            _weaponSelectionMenu.SetLevelName(_levelNameText.text);
            _weaponSelectionMenu.gameObject.SetActive(true);
            ToggleOffAllCharacters();
            gameObject.SetActive(false);
        }

        public void ChangeCharacter(int direction)
        {
            ToggleCharacterDescription(_index, false);

            _index += direction;
            if (_index < 0)
            {
                _index = _characterDescription.Count - 1;
            }
            if (_index == _characterDescription.Count)
            {
                _index = 0;
            }

            ToggleCharacterDescription(_index, true);
        }

        private void ToggleOffAllCharacters()
        {
            for (int i = 0; i < _characterDescription.Count; i++)
            {
                ToggleCharacterDescription(i, false);
            }
        }

        private void ToggleCharacterDescription(int index, bool toggle)
        {
            _characterDescription[index].SetActive(toggle);
            _characterModel[index].SetActive(toggle);
        }
    }
}