using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace My2D
{
    //지정된 충돌체 감지
    public class DetectionZone : MonoBehaviour
    {
        #region Variables
        //감지된 콜라이더 리스트
        public List<Collider2D> detectedColliders = new List<Collider2D>();

        //충돌체 리스트에 더이상 충돌체가 없을대 호출 되는 함수
        public UnityAction noColliderRemain;
        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //충돌체가 감지되면 리스트에 추가하기
            detectedColliders.Add(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            //충돌체가 나가면 리스트에서 삭제하기
            detectedColliders.Remove(collision);

            //충돌체가 하나도 남아있지 않으면 
            if(detectedColliders.Count <= 0)
            {
                noColliderRemain?.Invoke();
            }
        }
    }
}
