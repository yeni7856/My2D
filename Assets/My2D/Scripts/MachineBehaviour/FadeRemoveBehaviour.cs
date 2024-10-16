using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    //게임 스프라이트 오브젝트를 페이드 아웃 후 킬
    public class FadeRemoveBehaviour : StateMachineBehaviour
    {
        #region Variables
        public float fadeTimer = 1f;
        private float countdown = 0f;  

        private SpriteRenderer spriteRenderer;
        private GameObject removeObjcet;
        private Color StartColor;

        //딜레이 시간후에 페이드 효과 처리
        public float delayTime = 2f;
        private float delayCount = 0f;
        #endregion

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //참조
            spriteRenderer = animator.GetComponent<SpriteRenderer>();
            StartColor = spriteRenderer.color;
            removeObjcet = animator.gameObject;

            //초기화
            countdown = fadeTimer;
            
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //딜레이타임 만큼 딜레이 
            if(delayCount < delayTime)
            {
                delayCount += Time.deltaTime;
                return;
            }

            //페이드 아웃 효과 spriteRenderer.color.a  : 1 -> 0, 0 -> 1
            countdown -= Time.deltaTime;  

            float newAlpha = StartColor.a * (countdown / fadeTimer);  //1에서 0까지
            //float newAlpha = StartColor.a * (1 - countdown / fadeTimer);  //0에서 1까지
            spriteRenderer.color = new Color(StartColor.r, StartColor.g, StartColor.b, newAlpha);

            //페이드 타임 끝
            if(countdown <= 0)
            {
                Destroy(removeObjcet);
            }
            
        }    
    }

}
