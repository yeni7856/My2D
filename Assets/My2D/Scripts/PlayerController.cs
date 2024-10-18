using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

namespace My2D
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        private Rigidbody2D rb2D;
        private Animator animator;

        private TouchingDirections touchingDirections;

        //데미지어블 가저오기
        private Damageable damageable;

        //플레이어 걷기 속도
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float runSpeed = 8f;
        [SerializeField] private float airSpeed = 2f;
        public float CurrentMoveSpeed
        {
            get
            {
                if (CanMove)
                {
                    if (IsMove && touchingDirections.IsWall == false)
                    {
                        if (touchingDirections.IsGround)
                        {
                            if (isRun)
                            {
                                return runSpeed;
                            }
                            else
                            {
                                return walkSpeed;
                            }
                        }
                        else
                        {
                            return airSpeed;
                        }
                    }
                    else
                    {
                        return 0f;  //idle state
                    }
                }
                else
                {
                    return 0f;  //움직이지 못할때
                }
            }
        }
        //이동여부
        public bool CanMove
        {
            get
            {
                return animator.GetBool(AnimationString.CanMove);
            }
        }

        //플레이어 이동과 관련된 입력값
        private Vector2 inputMove;

        //걷기
        [SerializeField] private bool isMove = false;
        public bool IsMove
        {
            get { return isMove; }
            set 
            {
                isMove = value;
                animator.SetBool(AnimationString.IsMove, value);
            }
        }
        //뛰기
        [SerializeField] private bool isRun = false;
        public bool IsRun
        {
            get { return isRun; }
            set
            {
                isRun = value;
                animator.SetBool(AnimationString.IsRun, value);
            }
         }
        //좌우 반전
        [SerializeField] private bool isFacingRight = true; 
        public bool IsFacingRight
        {
            get { return isFacingRight; }
            set
            {
                //반전
                if (isFacingRight != value)
                {
                    transform.localScale *= new Vector2(-1, 1);
                }
                isFacingRight = value;
            }
        }
        //점프
        [SerializeField] private float jumpForce = 5f;

        //죽음 체크
        public bool IsDeath
        {
            get { return animator.GetBool(AnimationString.IsDeath); }
        }
        #endregion


        private void Awake()
        {
            rb2D = this.GetComponent<Rigidbody2D>();
            animator = this.GetComponent<Animator>();
            //rb2D.velocity
            touchingDirections = GetComponent<TouchingDirections>();

            damageable = GetComponent<Damageable>();
            damageable.hitAction += OnHit; //데미지어블에서 가져와서 등록 (유니티액션 델리게이트 함수에 등록)
        }

        private void FixedUpdate()
        {
            if (!damageable.LockVelocity) //잠겨서 적용안됨
            {//플레이어 좌우 이동
                rb2D.velocity = new Vector2(inputMove.x * CurrentMoveSpeed, rb2D.velocity.y);
            }

            //플레이어 좌우 이동
            //rb2D.velocity = new Vector2(inputMove.x * walkSpeed, rb2D.velocity.y);
            //rb2D.velocity = new Vector2(inputMove.x * walkSpeed, inputMove.y * walkSpeed);
            //rb2D.velocity = new Vector2(inputMove.x * CurrentMoveSpeed, rb2D.velocity.y);
            //애니메이션 값
            animator.SetFloat(AnimationString.YVelocity, rb2D.velocity.y);
        }
        //방향 전환
        void SetFacingDirection(Vector2 moveInput)
        {
            if(moveInput.x > 0f && IsFacingRight == false)
            {
                //오른쪽을 바라본다
                IsFacingRight = true;   
            }
           else if(moveInput.x < 0f && IsFacingRight == true)
           {
                //왼쪽을 바라본다
                IsFacingRight=false;
           }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();

            if (IsDeath)
            {
                IsMove = false;
            }
            else  //살아있으면
            {
                IsMove = (inputMove != Vector2.zero);
                //방향전환
                SetFacingDirection(inputMove);
            }
            //Debug.Log("inputMove: " + inputMove);
        }
        public void OnRun(InputAction.CallbackContext context)
        {
            //누르기 시작하는 순간
            if(context.started)
            {
                IsRun = true;
            }
            else if (context.canceled)      //손가락 땐 순간. 릴리즈 하는순간
            {
                IsRun = false;  
            }
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            //누르기 시작하는 순간, 이중 점프 x
            if (context.started && touchingDirections.IsGround)
            {
                Debug.Log("Jump triggered");
                animator.SetTrigger(AnimationString.JumpTrigger);
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            }
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            //마우스 클릭순간 시작하는 순간
            if (context.started && touchingDirections.IsGround)
            {
                animator.SetTrigger(AnimationString.AttackTrigger);
            }
        }
        public void OnBowAttack(InputAction.CallbackContext context)
        {
            //F키 누르는 순간
            if (context.started && touchingDirections.IsGround)
            {
                animator.SetTrigger(AnimationString.BowTrigger);
            }
        }
        public void OnHit(float dmg, Vector2 knockback) 
        {
            rb2D.velocity = new Vector2(knockback.x, rb2D.velocity.y + knockback.y); //y는 기본축에 더하기
        }
    }
}