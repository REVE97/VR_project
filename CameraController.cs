using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Tilemap2D tilemap2D; // 맵 크기 정보를 얻어오기 위한 Tilemap2D
    private Camera mainCamera; // 카메라 시야 설정을 위한 Camera

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
    }



    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
