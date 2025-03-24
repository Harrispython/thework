using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBika
{
    [RequireComponent(typeof(LineRenderer))]
    public class LASER : MonoBehaviour
    {
        [Header("Settings")]
        public LayerMask layerMask;
        public float defaultLength = 50;
        public int numOfReflections = 2;
        
        // 添加新的变量
        [Header("碰撞点标记设置")]
        public GameObject hitPointMarkerPrefab;  // 碰撞点标记预制体
        private List<GameObject> hitMarkers = new List<GameObject>();  // 用于存储所有生成的标记物体

        private LineRenderer _lineRenderer;
        private Camera _myCam;
        private RaycastHit hit;
        private Ray ray;
        private Vector3 direction;

        // Start is called before the first frame update
        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _myCam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            ReflectLaser();
        }

        void ReflectLaser()
        {
            // 清理之前的所有标记物体
            foreach (GameObject marker in hitMarkers)
            {
                if (marker != null)
                    Destroy(marker);
            }
            hitMarkers.Clear();
        
            ray = new Ray(transform.position, transform.forward);
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, transform.position);

            float remainLength = defaultLength;

            for (int i = 0; i < numOfReflections; i++)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, remainLength, layerMask))
                {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);
                    remainLength -= Vector3.Distance(ray.origin, hit.point);
                
                    Vector3 reflectDirection = Vector3.Reflect(ray.direction, hit.normal);
                
                    // 在碰撞点创建标记物体
                    GameObject hitMarker = CreateHitPointMarker(hit.point, reflectDirection);
                    hitMarkers.Add(hitMarker);
                    
                
                    ray = new Ray(hit.point, reflectDirection);
                }
                else
                {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, ray.origin + (ray.direction * remainLength));
                }
            }
        }
    
        // 添加新的方法用于创建碰撞点标记
        private GameObject CreateHitPointMarker(Vector3 position, Vector3 direction)
        {
            GameObject marker = Instantiate(hitPointMarkerPrefab, position, Quaternion.identity);
            marker.transform.forward = direction;  // 设置朝向
            return marker;
        }

        void NormalLaser()
        {
            _lineRenderer.SetPosition(0, transform.position);

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.forward, out hit, defaultLength, layerMask))
            {
                _lineRenderer.SetPosition(1, hit.point);
            }
        }
    }
}