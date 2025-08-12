using UnityEngine;

namespace Keru.Debug
{
    public class DrawLineOfSight : MonoBehaviour
    {
        [SerializeField] private Transform _model;
        [SerializeField] private Transform _target;
        [SerializeField] private float _viewDistance = 30f;
        [SerializeField] private float _detectionDistance = 15f;
        [SerializeField] private float _viewAngle = 45f;

        void OnDrawGizmos()
        {
            if (_target == null || _model == null)
            {
                return;
            }

            if (CollidesWithPlayer())
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawLine(transform.position, _target.transform.position);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _viewDistance);
            Gizmos.DrawWireSphere(transform.position, _detectionDistance);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + (_model.transform.forward * _viewDistance));

            var rightLimit = Quaternion.AngleAxis(_viewAngle, transform.up) * _model.transform.forward;
            Gizmos.DrawLine(transform.position, transform.position + (rightLimit * _viewDistance));

            var leftLimit = Quaternion.AngleAxis(-_viewAngle, transform.up) * _model.transform.forward;
            Gizmos.DrawLine(transform.position, transform.position + (leftLimit * _viewDistance));
        }

        private bool CollidesWithPlayer()
        {
            var dirToTarget = (_target.transform.position - transform.position).normalized;
            var angleToTarget = Vector3.Angle(_model.transform.forward, dirToTarget);
            var distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

            if (angleToTarget <= _viewAngle && distanceToTarget <= _detectionDistance)
            {
                RaycastHit rch;
                if (Physics.Raycast(transform.position, dirToTarget, out rch, distanceToTarget))
                {
                    if (rch.collider.gameObject == _target.gameObject)
                    {
                       return true;
                    }
                    else
                    {
                       return false;
                    }
                }
            }

            return false;
        }
    }
}
