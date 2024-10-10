using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    public class ParallaxEffect : MonoBehaviour
    {
        #region Variables
        public Camera cam;
        public Transform followTarget;

        private Vector2 startingPosition; //시작 위치(배경, 카메라)
        private float startingZ;   //시작할때 배경의 z축 위치값

        //시작점으로 부터 거리 카메라가 있는 위치까지의 거리
        private Vector2 camMoveSinceStart => startingPosition - (Vector2)cam.transform.position;

        //배경과 플레이어와의 z축 거리
        private float zDistanceFromTarget => transform.position.z - followTarget.position.z;

        private float ClippingPlane => cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane);

        //시차 거리 factor
        private float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / ClippingPlane;
        #endregion
        void Start()
        {
            //초기화
            startingPosition = transform.position;
            startingZ = transform.position.z;
        }
        void Update()
        {
            Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
            transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
        }
    }
}
