using UnityEngine;
using UnityEngine.Events;

namespace My2D
{
    public class Damageable : MonoBehaviour
    {
        #region Variables
        private Animator animator;

        //유니티 액션 데미지 입을때 등록된 함수 호출
        public UnityAction<float, Vector2> hitAction;

        //체력
        [SerializeField] private float maxHp = 100f;
        public float MaxHp
        {
            get { return maxHp; }
            set { maxHp = value; }
        }
        [SerializeField] private float currentHp;
        public float CurrentHp
        {
            get { return currentHp; }
            set
            {
                currentHp = value;
                //죽음 처리
                if (currentHp <= 0)
                {
                    IsDeath = true;
                }
            }
        }
        private bool isDeath = false;
        public bool IsDeath
        {
            get { return isDeath; }
            set 
            { 
                isDeath = value; 
                //애니메이션
                animator.SetBool(AnimationString.IsDeath, value);
            }
        }
        //무적모드
        private bool isInvincible = false;
        [SerializeField] private float invincibleTimer = 3f;
        private float countdown = 0f;

        //락밸로시티 잠굼
        public bool LockVelocity
        {
            get 
            {
                return animator.GetBool(AnimationString.LockVelocity);
            }
            set
            {
                animator.SetBool (AnimationString.LockVelocity, value);
            }
        }
        #endregion

        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            //초기화
            CurrentHp = MaxHp;
            //초기화
            countdown = invincibleTimer;
        }
        private void Update()
        {
            //무적상태이면 무적 타이머를 돌린다
            if (isInvincible)
            {
                if(countdown <= 0f)
                {
                    isInvincible = false ;
                    //초기화
                    countdown = invincibleTimer;
                }
                countdown -= Time.deltaTime;    
            }
        }
        public void TakeDamge(float dmg, Vector2 knockback)
        {
            if (!IsDeath && !isInvincible)
            {
                isInvincible = true;
                CurrentHp -= dmg;
                Debug.Log($"{transform.name}의 현재 체력은{currentHp}");

                //애니매이션 
                LockVelocity = true;
                animator.SetTrigger(AnimationString.HitTrigger);

                //데미지 효과
                /*if(hitAction != null )
                {
                    hitAction.Invoke(dmg, knocboack);
                }*/
                hitAction?.Invoke(dmg, knockback);
                CharacterEvents.characterDamaged?.Invoke(gameObject, dmg);
            }
        }
        //힐 회복 매서드
        public bool Heal(float amount)
        {
            if(CurrentHp >= MaxHp)
            {
                return false;
            }
            CurrentHp += amount;
            CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);   //맥스 못넘게

            CharacterEvents.characterHealed?.Invoke(gameObject, amount);
            return true;
        }
    }
}
