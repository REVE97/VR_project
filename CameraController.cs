using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Tilemap2D tilemap2D; // 맵 크기 정보를 얻어오기 위한 Tilemap2D
    private Camera mainCamera; // 카메라 시야 설정을 위한 Camera

    [SerializeField]
    private float moveSpeed;  // 카메라 이동 속도
    [SerializeField]
    private float zoomSpeed; // 카메라 줌 속도
    [SerializeField]
    private float minViewSize = 2; // 카메라 시야 최소 크기
    private float maxViewSize; // 카메라 시야 최대 크기

    private float wDelta = 0.4f; // width 시야 delta 값
    private float hDelta = 0.6f; //height 시야 delta 값

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    public void SetupCamera()
    {
        // 맵의 크기 정보
        int width = tilemap2D.Width;
        int height = tilemap2D.Height;

        // 카메라 시야 설정 ( 전체 맵이 화면 안에 들어오도록 설정)
        float size = (width>height) ? width*wDelta : height*hDelta;
        mainCamera.orthographicSize = size;

        // 카메라 위치 설정 (y축 좌표)
        if ( height > width )
        {
            // 맵의 높이 (height) 가 더 클 경우 카메라의 y축 위치를 일부 수정
            Vector3 position = new Vector3 (0, 0.05f, -10);
            position.y *= height;

            transform.position = position;
        }

        maxViewSize = mainCamera.orthographicSize;
    }

    public void SetPosition(float x,float y)
    {
        transform.position += new Vector3(x,y,0) * moveSpeed * Time.deltaTime;
    }

    public void SetOrthographicSize(float size)
    {
        if (size == 0) return;

        // 카메라 시야 설정
        mainCamera.orthographicSize += size * zoomSpeed * Time.deltaTime;
        // 카메라 시야의 범위는 (minViewSize <= orthographicSize <= maxViewSize)
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minViewSize, maxViewSize);
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
