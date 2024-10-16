using UnityEngine;

namespace My2D
{
    public class Attack : MonoBehaviour
    {
        //공격력
        #region Variables
        [SerializeField] private float attackDmg = 10f;
        #endregion
        //충돌 체크해서 공격력 만큼 데미지
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //데미지 입는 객체 
            Damageable damage = collision.GetComponent<Damageable>();   
            if(damage != null )
            {
                Debug.Log($"{collision.name} 데미지를 입었다");
                damage.TakeDamge(attackDmg);
            }
        }
    }

}
