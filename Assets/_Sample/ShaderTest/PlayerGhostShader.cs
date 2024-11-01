using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    public class PlayerGhostShader : MonoBehaviour
    {
        #region Variables
        //플레이어 잔상 효과 : 2초동안 잔상효과 발생
        private bool isTrailActive = false; //트루면 잔상효과
        [SerializeField] private float trailActiveTimer = 2f;       //잔상 효과 유효 시간
        [SerializeField] private float trailRefreshRate = 0.1f;     //잔상들의 발생 간격
        [SerializeField] private float fadeOutTime = 1f;            //트레일 디스토리 딜레이 1초후에 킬

        private SpriteRenderer playerRenderer;

        public Material ghostMaterial;  //그림자 메터리얼 (잔상)
        [SerializeField] private string shaderValueRef = "_Alpha";
        [SerializeField] private float shaderValueRate = 0.1f;      //알파값 감소 비율 
        [SerializeField] private float shaderValueRefresh = 0.0f;   //알파값 감소되는 시간 간격
        #endregion

        private void Awake()
        {
            playerRenderer = GetComponent<SpriteRenderer>();
        }
        public void StartGhostActive()
        {
            if(isTrailActive) return;

            isTrailActive = true;
            StartCoroutine(ActiveTrail(trailActiveTimer));
        }
        //activeTime동안 잔상 효과 발생 
        IEnumerator ActiveTrail(float activeTime)
        {
            while(activeTime > 0f)
            {
                activeTime -= trailRefreshRate;

                //잔상효과 발생 - 현재 위치에 
                GameObject ghostObj = new GameObject(); //하이라키창에 빈 오브젝트 만들기
                //트랜스폼 
                ghostObj.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);
                ghostObj.transform.localScale = transform.localScale; 
                //스프라이트랜더러 셋팅
                SpriteRenderer ghostRenderer = ghostObj.AddComponent<SpriteRenderer>();
                ghostRenderer.sprite = playerRenderer.sprite;
                ghostRenderer.sortingLayerName = playerRenderer.sortingLayerName; //먼저그릴수있게
                ghostRenderer.sortingOrder = playerRenderer.sortingOrder - 1; //먼저그리기
                //랜더러에 메트리얼
                ghostRenderer.material = ghostMaterial;

                //메터리얼속성(알파값) 감소
                StartCoroutine(AnimateMaterialFloat(ghostRenderer.material, shaderValueRef, 0f, shaderValueRate, shaderValueRefresh));


                Destroy(ghostObj, fadeOutTime);

                yield return new WaitForSeconds(trailRefreshRate);
            }
            isTrailActive=false;
        }
        IEnumerator AnimateMaterialFloat(Material mat, string valueRef, float goal, float rate, float refreshRate)
        {
            float valueToAnimate = mat.GetFloat(valueRef);

            while(valueToAnimate > goal)
            {
                valueToAnimate -= rate; //빼준값
                mat.SetFloat(valueRef, valueToAnimate); //세팅
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
