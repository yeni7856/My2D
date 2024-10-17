using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace My2D
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public GameObject damageTextPrefab;
        public GameObject healTextPrefab;

        private Canvas canvas;
        [SerializeField] private Vector3 healthTextOffset = Vector3.zero;
        #endregion

        private void Awake()
        {
            //참조
            canvas = FindObjectOfType<Canvas>();
        }

        private void OnEnable()
        {
            //캐릭터 관련 이벤트 함수 등록
            CharacterEvents.characterDamaged += CharacterDamaged;
            CharacterEvents.characterHealed += CharacterHealed;
        }
        private void OnDisable()
        {
            //캐릭터 관련 이벤트 함수 제거 //제거될때 함수도 제거 
            CharacterEvents.characterHealed -= CharacterDamaged;
            CharacterEvents.characterHealed -= CharacterHealed;
        }

        public void CharacterDamaged(GameObject character, float dmg)
        {
            //damageTextPrefab 스폰, 
            //위치 설정
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
           
        
            //데미지 프리팹 + 스폰 위치 + 반복 + 부모위치
            GameObject textGo = Instantiate(damageTextPrefab, spawnPosition + healthTextOffset, Quaternion.identity, canvas.transform);
            //텍스트 메시 프로 가져오기 
            TextMeshProUGUI damageText = textGo.GetComponent<TextMeshProUGUI>();
            damageText.text = dmg.ToString();
        }

        public void CharacterHealed(GameObject character, float restore)
        {
            //위치 설정
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);


            //데미지 프리팹 + 스폰 위치 + 반복 + 부모위치
            GameObject textGo = Instantiate(healTextPrefab, spawnPosition + healthTextOffset, Quaternion.identity, canvas.transform);
            //텍스트 메시 프로 가져오기 
            TextMeshProUGUI healText = textGo.GetComponent<TextMeshProUGUI>();
            healText.text = restore.ToString();
        }
    }
}
