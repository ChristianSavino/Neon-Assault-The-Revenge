using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Level
{
    public class LevelPickUpSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _pickUps;
        [SerializeField] private List<Vector3> _spawnPosition; //Same as the pickUps list, but for spawn positions

        [SerializeField] private bool _doRespawn;
        [SerializeField] private float _spawnInterval = 5f;

        private List<GameObject> _activePickUps = new List<GameObject>();

        private void Start()
        {
            for (int i = 0; i < _pickUps.Count; i++)
            {
                var pickUp = Instantiate(_pickUps[i], _spawnPosition[i], Quaternion.identity);
                _activePickUps.Add(pickUp);
            }

            if(_doRespawn)
            {
                StartCoroutine(RespawnPickUps());
            }
        }

        private IEnumerator RespawnPickUps()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                for (int i = 0; i < _activePickUps.Count; i++)
                {
                    if (_activePickUps[i] == null)
                    {
                        var pickUp = Instantiate(_pickUps[i], _spawnPosition[i], Quaternion.identity);
                        _activePickUps[i] = pickUp;
                    }
                }
            }
        }
    }
}