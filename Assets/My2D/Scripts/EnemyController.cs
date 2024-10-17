using UnityEngine;

namespace My2D
{
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        private Rigidbody2D rb2D;
        private Animator animator;
        private TouchingDirections touchingDirections;

        //데미지어블 가저오기
        private Damageable damageable;

        //플레이어 감지
        public DetectionZone detectionZone;

        //낭떨어지 감지
        public DetectionZone detectionCliff;

        //이동속도
        [SerializeField] private float runSpeed = 4f;
    
        //이동방향
        private Vector2 direction = Vector2.right;

        //이동 가능 방향
        public enum WalkableDirection { Left, Right } 
        //현재 이동 방향 
        private WalkableDirection walkDirection = WalkableDirection.Right;
        public WalkableDirection WalkDirection
        {
            get {return walkDirection; }
            set {
                //이미지 플립
                transform.localScale *= new Vector2(-1, 1);

                //이동하는 방향값
                if (value == WalkableDirection.Left)
                {
                    direction = Vector2.left;
                }
                else if (value == WalkableDirection.Right)
                {
                    direction = Vector2.right;
                }
                walkDirection = value;
            }
        }

        [SerializeField] private bool hasTarget = false;
        public bool HasTarget
        {
            get { return hasTarget; }
            set 
            { 
                hasTarget = value;
                animator.SetBool(AnimationString.HasTarget, value);
            } 
        }
        //이동 가능 상태/불가능 상태 - 이동제한
        public bool CanMove
        {
            get { return animator.GetBool(AnimationString.CanMove); }
        }
        //감속 개수
        [SerializeField] private float stopRate = 0.2f;
        #endregion

        void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            touchingDirections = GetComponent<TouchingDirections>();

            damageable = GetComponent<Damageable>();
            damageable.hitAction += OnHit;

            detectionCliff.noColliderRemain += OnCliff;
        }
        private void Update()
        {
            //적 감지 충돌체의 리스트 갯수 
            HasTarget = (detectionZone.detectedColliders.Count > 0);    
        }

        void FixedUpdate()
        {
            // 벽에 충돌 시 로그 출력
            if (touchingDirections.IsWall)
            {
                Debug.Log("벽에 충돌함");
            }
            //Debug.Log("IsWall: " + touchingDirections.IsWall + ", IsGround: " + touchingDirections.IsGround);
            // 땅에서 이동시 벽을 만나면 방향 전환
            if (touchingDirections.IsWall && touchingDirections.IsGround)
            {
                // 방향 전환
                Flip();
            }
            if(!damageable.LockVelocity)
            {
                // 이동 
                if (CanMove)
                {
                    rb2D.velocity = new Vector2(direction.x * runSpeed, rb2D.velocity.y);
                }
                else
                {
                    // rb2D.velocity.x -> 0 : Lerp
                    rb2D.velocity = new Vector2(Mathf.Lerp(rb2D.velocity.x, 0f, stopRate), rb2D.velocity.y);
                }
            }
        }
        void Flip()
        {
            // 현재 방향과 반대 방향으로만 반전
            if (WalkDirection == WalkableDirection.Left && direction == Vector2.left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else if (WalkDirection == WalkableDirection.Right && direction == Vector2.right)
            {
                WalkDirection = WalkableDirection.Left;
            }
        }
        public void OnHit(float dmg, Vector2 knockback)
        {
            rb2D.velocity = new Vector2(knockback.x, rb2D.velocity.y + knockback.y); //y는 기본축에 더하기
        }
        public void OnCliff()
        {
            if (touchingDirections.IsGround)
            {
                Flip();
            }
        }
    }
}
