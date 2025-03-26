using System;
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
        
        [Header("碰撞点标记设置")]
        public GameObject hitPointMarkerPrefab;  // 碰撞点标记预制体
        private List<GameObject> hitMarkers = new List<GameObject>();  // 用于存储所有生成的标记物体

        [Header("激光停止设置")]
        public Transform laserHitParticlePrefab; // 激光停止时的粒子效果预制体
        private Transform laserHitParticle; // 实例化的激光粒子效果
        private bool isStopMode = false; // 是否处于停止模式

        private LineRenderer _lineRenderer;
        private Camera _myCam;
        private RaycastHit hit;
        private Ray ray;
        private Vector3 direction;
        
        private bool isHittingPoint = false;// 是否正在击中point标签
        private float hitTimer = 0f;        // 计时器
        private float hitDuration = 1f;   // 需要持续击中的时间
        private bool ishited = false;//是否启用过
        
        private LaserStop laserStop; // LaserStop脚本引用

        // Start is called before the first frame update
        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _myCam = Camera.main;
            
            // 初始化激光停止粒子效果
            if (laserHitParticlePrefab != null)
            {
                laserHitParticle = Instantiate(laserHitParticlePrefab, Vector3.zero, Quaternion.identity);
                laserHitParticle.gameObject.SetActive(false);
            }

            // 获取LaserStop组件
            laserStop = GetComponent<LaserStop>();
            if (laserStop == null)
            {
                // 如果没有LaserStop组件，添加一个
                laserStop = gameObject.AddComponent<LaserStop>();
                laserStop.enabled = false; // 默认禁用
                
                // 复制必要的参数
                laserStop.layerMask = this.layerMask;
                laserStop.defaultLength = this.defaultLength;
            }
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
            
            if (laserHitParticle != null)
            {
                laserHitParticle.gameObject.SetActive(false);
            }
        
            ray = new Ray(transform.position, transform.forward);
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, transform.position);

            float remainLength = defaultLength;
            bool currentlyHittingPoint = false; // 当前帧是否击中point标签

            for (int i = 0; i < numOfReflections; i++)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, remainLength, layerMask))
                {
                    _lineRenderer.positionCount += 1;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);
                    
                    // 检查是否碰到带有"point"标签的物体
                    if (hit.collider.CompareTag("point") || hit.collider.CompareTag("Player"))
                    {
                        currentlyHittingPoint = true;
                        CheckPointHit(true); // 更新计时器
                        break;
                    }
                    
                    remainLength -= Vector3.Distance(ray.origin, hit.point);
                    Vector3 reflectDirection = Vector3.Reflect(ray.direction, hit.normal);
                
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

            // 如果这一帧没有击中point，重置计时器
            if (!currentlyHittingPoint)
            {
                CheckPointHit(false);
            }

            // 根据是否击中point标签来切换脚本
            if (currentlyHittingPoint)
            {
                laserStop.enabled = true; // 启用LaserStop脚本
            }
            else
            {
                laserStop.enabled = false; // 禁用LaserStop脚本
            }
        }

        // 检查point标签的持续击中状态
        void CheckPointHit(bool isHitting)
        {
            if (isHitting)
            {
                if (!isHittingPoint)
                {
                    // 首次击中，开始计时
                    isHittingPoint = true;
                    hitTimer = 0f;
                }
                else
                {
                    // 持续击中，累加时间
                    hitTimer += Time.deltaTime;
                    print(hitTimer);
                    // 检查是否达到持续时间
                    if (hitTimer >= hitDuration&&!ishited)
                    {
                        hit.collider.gameObject.GetComponent<MoveUp>().StartMoveUp();
                        print(hit.collider.gameObject);
                        // 重置计时器，防止重复输出
                        hitTimer = 0f;
                        ishited = true;
                    }
                }
            }
            else
            {
                // 未击中，重置状态
                isHittingPoint = false;
                hitTimer = 0f;
            }
        }

        // 显示激光停止时的粒子效果
        void ShowLaserHitParticle(Vector3 hitPoint)
        {
            if (laserHitParticle != null)
            {
                laserHitParticle.gameObject.SetActive(true);
                laserHitParticle.position = hitPoint;
            }
        }
    
        private GameObject CreateHitPointMarker(Vector3 position, Vector3 direction)
        {
            GameObject marker = Instantiate(hitPointMarkerPrefab, position, Quaternion.identity);
            marker.transform.forward = direction;
            return marker;
        }

        private void OnDisable()
        {
            // 清理之前的所有标记物体
            foreach (GameObject marker in hitMarkers)
            {
                if (marker != null)
                    Destroy(marker);
            }
            hitMarkers.Clear();
            
            if (laserHitParticle != null)
            {
                laserHitParticle.gameObject.SetActive(false);
            }
        
            ray = new Ray(transform.position, transform.forward);
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, transform.position);
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