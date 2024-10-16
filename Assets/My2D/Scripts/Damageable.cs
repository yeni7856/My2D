using UnityEngine;

namespace My2D
{
    public class Damageable : MonoBehaviour
    {
        private Animator animator;
        //체력
        [SerializeField] private float maxHp = 100f;
        public float MaxHp
        {
            get { return maxHp; }
            set { maxHp = value; }
        }
        private float currentHp;
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
        //무적모드ㅜ
        private bool isInvincible = false;
        [SerializeField] private float invincibleTimer = 3f;
        private float countdown = 0f;


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
        public void TakeDamge(float dmg)
        {
            if (!IsDeath && !isInvincible)
            {
                isInvincible = true;
                CurrentHp -= dmg;
                Debug.Log($"{transform.name}의 현재 체력은{currentHp}");

                //애니매이션 
                animator.SetTrigger(AnimationString.HitTrigger);
            }
        }
    }

}
