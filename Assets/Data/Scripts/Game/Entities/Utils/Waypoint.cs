using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Utils
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private List<Vector3> _waypoints = new List<Vector3>();
        private NPC _patroller;
        private int _currentWaypointIndex = 0;

        public void SetConfig(NPC patroller)
        {
            _patroller = patroller;
            transform.position = _waypoints[0];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _patroller.gameObject)
            {
                print("Changed");
                ChangeWaypoint();
            }

            print("Nope");
        }

        private void ChangeWaypoint()
        {
            var next = _currentWaypointIndex + 1;
            if (next >= _waypoints.Count)
            {
                next = 0;
            }

            _currentWaypointIndex = next;
            transform.position = _waypoints[_currentWaypointIndex];
            _patroller.SetPatrolDestination(transform.position);
        }

        public Vector3 GetCurrentWaypoint()
        {
            return _waypoints[_currentWaypointIndex];
        }
    }
}
