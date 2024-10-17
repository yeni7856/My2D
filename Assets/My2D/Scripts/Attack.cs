using Unity.VisualScripting;
using UnityEngine;

namespace My2D
{
    public class Attack : MonoBehaviour
    {
        //공격력
        #region Variables
        [SerializeField] private float attackDmg = 10f;
        public Vector2 knockback = Vector2.zero;
        #endregion
        //충돌 체크해서 공격력 만큼 데미지
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //데미지 입는 객체 
            Damageable damage = collision.GetComponent<Damageable>();   
            if(damage != null )
            {
                //knockback의 방향 설정
               //내부모의 상환연산자 하고 백터 2 하고 넉백 반대방향 
               // ?. 연산자는[Null이 아니라면 참조하고, Null이라면 Null로 처리]하라
                Vector2 deliveredKnockback = (transform.parent.localScale.x > 0) ? knockback : new Vector2(-knockback.x, knockback.y);

                Debug.Log($"{collision.name} 데미지를 입었다");
                damage.TakeDamge(attackDmg, deliveredKnockback);
            }
        }
    }

}
