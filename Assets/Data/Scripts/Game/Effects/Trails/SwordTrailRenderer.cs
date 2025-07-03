using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Keru.Scripts.Game.Effects.Trails
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SwordTrailRenderer : MonoBehaviour
    {
        [Header("Transforms")]
        [SerializeField] private Transform _tip;
        [SerializeField] private Transform _basePoint;

        [Header("Trail Settings")]
        [SerializeField] private float _trailTime = 0.3f;
        [SerializeField] private float _minVertexDistance = 0.005f;

        private List<TrailSection> _sections = new List<TrailSection>();
        private Mesh _trailMesh;
        private bool _emitting = false;

        private Vector3 _lastTip;
        private Vector3 _lastBase;
        private float _lastCaptureTime;
        private bool _hasLast = false;

        private float _accumulatedTipDistance = 0f;
        private float _accumulatedBaseDistance = 0f;

        private void Awake()
        {
            _trailMesh = new Mesh { name = "SwordTrailMesh" };
            GetComponent<MeshFilter>().mesh = _trailMesh;

            var renderer = GetComponent<MeshRenderer>();
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        private void Update()
        {
            var currentTime = Time.time;

            if (_emitting)
            {
                var currentTip = _tip.position;
                var currentBase = _basePoint.position;

                if (_hasLast)
                {
                    var tipDist = Vector3.Distance(currentTip, _lastTip);
                    var baseDist = Vector3.Distance(currentBase, _lastBase);
                    var maxDist = Mathf.Max(tipDist, baseDist);

                    _accumulatedTipDistance += tipDist;
                    _accumulatedBaseDistance += baseDist;

                    if (_accumulatedTipDistance >= _minVertexDistance || _accumulatedBaseDistance >= _minVertexDistance)
                    {
                        var steps = Mathf.CeilToInt(maxDist / _minVertexDistance);
                        for (var i = 1; i <= steps; i++)
                        {
                            var t = (float)i / steps;
                            var interpTip = Vector3.Lerp(_lastTip, currentTip, t);
                            var interpBase = Vector3.Lerp(_lastBase, currentBase, t);
                            var interpTime = Mathf.Lerp(_lastCaptureTime, currentTime, t);

                            _sections.Add(new TrailSection(interpBase, interpTip, interpTime));
                        }

                        _accumulatedTipDistance = 0f;
                        _accumulatedBaseDistance = 0f;
                        _lastTip = currentTip;
                        _lastBase = currentBase;
                        _lastCaptureTime = currentTime;
                    }
                }
                else
                {
                    _sections.Add(new TrailSection(currentBase, currentTip, currentTime));
                    _lastTip = currentTip;
                    _lastBase = currentBase;
                    _lastCaptureTime = currentTime;
                    _hasLast = true;
                }
            }

            var cutoff = Time.time - _trailTime;
            _sections.RemoveAll(s => s.time < cutoff);

            UpdateTrailMesh();
        }


        private void UpdateTrailMesh()
        {
            _trailMesh.Clear();

            if (_sections.Count < 2)
                return;

            var vertexCount = _sections.Count * 2;
            var vertices = new Vector3[vertexCount];
            var triangles = new int[(_sections.Count - 1) * 6];
            var uv = new Vector2[vertexCount];

            for (int i = 0; i < _sections.Count; i++)
            {
                var t = (float)i / (_sections.Count - 1);

                vertices[i * 2] = transform.InverseTransformPoint(_sections[i].pointStart);
                vertices[i * 2 + 1] = transform.InverseTransformPoint(_sections[i].pointEnd);

                uv[i * 2] = new Vector2(0, t);
                uv[i * 2 + 1] = new Vector2(1, t);

                if (i < _sections.Count - 1)
                {
                    int vi = i * 2;
                    int ti = i * 6;

                    triangles[ti + 0] = vi;
                    triangles[ti + 1] = vi + 1;
                    triangles[ti + 2] = vi + 2;
                    triangles[ti + 3] = vi + 2;
                    triangles[ti + 4] = vi + 1;
                    triangles[ti + 5] = vi + 3;
                }
            }

            _trailMesh.vertices = vertices;
            _trailMesh.triangles = triangles;
            _trailMesh.uv = uv;
            _trailMesh.RecalculateBounds();
            _trailMesh.RecalculateNormals();
        }

        public void Toggle(bool state)
        {
            _emitting = state;

            if (state)
            {
                _hasLast = false;
                _accumulatedTipDistance = 0f;
                _accumulatedBaseDistance = 0f;
            }
        }

        private struct TrailSection
        {
            public Vector3 pointStart;
            public Vector3 pointEnd;
            public float time;

            public TrailSection(Vector3 start, Vector3 end, float t)
            {
                pointStart = start;
                pointEnd = end;
                time = t;
            }
        }
    }
}