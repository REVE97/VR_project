using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [Header("Raycast Collision")]
    [SerializeField]
    private LayerMask   collisionLayer;             // 광선과 부딪히는 충돌 레이어

    [Header("Raycast")]
    [SerializeField]
    private int         horizontalRayCount = 4;     // x축 방향으로 발사되는 광선의 개수
    [SerializeField]
    private int         verticalRayCount = 4;       // y축 방향으로 발사되는 광선의 개수

    private float       horizontalRaySpacing;       // x축 방향의 광선 사이 간격
    private float       verticalRaySpacing;         // y축 방향의 광선 사이 간격

    [Header("Movement")]
    [SerializeField]
    private float       moveSpeed;                  // 이동 속도
    [SerializeField]
    private float       jumpForce = 10;             // 점프 힘
    private float       gravity = -20.0f;           // 중력

    private Vector3     velocity;                   // 오브젝트 속력
    private readonly float skinWidth = 0.015f;      // 오브젝트 안쪽으로 파고드는 소량의 범위

    private Collider2D collider2D;                  // 광선 발사 위치 설정을 위한 충돌 범위
    private ColliderCorner colliderCorner;          // 광선 발사를 위한 모서리 점
    private CollisionChecker collisionChecker;      // 4면의 충돌 여부 체크

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        CalculateRaySpacing();        // 광선 사이의 거리 갱신
        UpdateColliderCorner();       // 충돌 범위의 외곽 위치 갱신
        collisionChecker.Reset();     // 충돌 여부 초기화 (위/아래/좌/우)

        // 이동 업데이트
        UpdateMovement();

        // 천장이나 바닥에 닿으면 velocity.y 값을 0으로 설정
        if ( collisionChecker.up || collisionChecker.down )
        {
            velocity.y = 0;
        }

        // 점프 업데이트 (바닥을 밟으면 바로 점프가 되도록 JumpTo()를 계속 호출)
        // 추후 타일에서 호출하게 됨
        JumpTo();
    }

    private void UpdateMovement()
    {
        // 중력 적용
        velocity.y += gravity * Time.deltaTime;

        // 현재 프레임에 적용될 실제 속력
        Vector3 currentVelocity = velocity * Time.deltaTime;

        // 속력이 0이 아닐 때 광선을 발사해 이동 가능 여부 조사
        if ( currentVelocity.x != 0 )
        {
            RaycastsHorizontal(ref currentVelocity);
        }
        if ( currentVelocity.y != 0 )
        {
            RaycastsVertical(ref currentVelocity);
        }

        // 오브젝트 이동
        transform.position += currentVelocity;
    }


    /// <summary>
    /// 타 클래스에서 호출 ( x축 이동 행동 )
    /// </summary>
    
    public void MoveTo(float x)
    {
        // x축 방향 속력을 x * moveSpeed 로 설정
        velocity.x = x*moveSpeed;
    }

    /// <summary>
    ///  타 클래스에서 호출 ( 점프 행동 )
    ///  </summary>

    public void JumpTo()
    {
        // 바닥에 닿아있으면
        if ( collisionChecker.down )
        {
            // y축 속력을 jumpForce 로 설정해 점프
            velocity.y = jumpForce;
        }
    }


    /// <summary>
    /// x축 방향으로 광선 발사 ( 좌/우 이동 )
    /// </summary>
    
    private void RaycastsHorizontal(ref Vector3 velocity)
    {
        float direction = Mathf.Sign(velocity.x);               // 이동 방향 (오:1 왼: -1 )
        float distance = Mathf.Abs(velocity.x) + skinWidth;     // 광선 길이
        Vector2 rayPosition = Vector2.zero;                     // 광선이 발사되는 위치
        RaycastHit2D hit;

        for (int i = 0; i < horizontalRayCount; ++i)
        {
            rayPosition = (direction==1) ? colliderCorner.bottomRight:colliderCorner.bottomLeft;
            rayPosition += Vector2.up * (horizontalRaySpacing * i);

            hit = Physics2D.Raycast(rayPosition, Vector2.right * direction, distance, collisionLayer);

            if(hit)
            {
                // x축 속력을 광선과 오브젝트 사이의 거리로 설정 ( 거리가 0 이면 속력 0 )
                velocity.x = (hit.distance - skinWidth) * direction;
                // 다음에 발사되는 광선의 거리 설정
                distance = hit.distance;

                // 현재 진행방향, 부딪힌 방향의 정보가 true로 변경
                collisionChecker.left = direction==-1;
                collisionChecker.right = direction==1;
            }

            // Debug : 발사되는 광선을 Scene View 에서 확인
            Debug.DrawLine(rayPosition, rayPosition+Vector2.right*direction*distance, Color.yellow);
        }
    }





    
}
