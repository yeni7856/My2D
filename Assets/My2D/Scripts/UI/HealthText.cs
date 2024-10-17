using TMPro;
using UnityEngine;

namespace My2D
{
    public class HealthText : MonoBehaviour
    {
        #region Variables
        private TextMeshProUGUI healthTextUI;
        private RectTransform textTransform;

        //이동
        [SerializeField] private float moveSpeed = 5f;

        //페이드효과
        private Color startColor;
        public float fadeTimer = 1f;
        private float countdown = 0f;

        //딜레이 시간후에 페이드 효과 처리
        public float delayTime = 2f;
        private float delayCount = 0f;
        #endregion

        private void Awake()
        {
            healthTextUI = GetComponent<TextMeshProUGUI>();
            textTransform = GetComponent<RectTransform>();
        }
        private void Start()
        {
            //초기화
            startColor = healthTextUI.color;
            countdown = fadeTimer;
        }
        private void Update()
        {
            //이동 포지션
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            //딜레이타임 만큼 딜레이 
            if (delayCount < delayTime)
            {
                delayCount += Time.deltaTime;
                return;
            }
            //페이드효과
            countdown -= Time.deltaTime;

            float newAlpha = startColor.a * (countdown / fadeTimer);  //1에서 0까지
            //float newAlpha = StartColor.a * (1 - countdown / fadeTimer);  //0에서 1까지
            healthTextUI.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            //페이드 타임 끝
            if (countdown <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
