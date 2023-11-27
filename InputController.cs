using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private Camera     mainCamera;
    private TileType   currentType = TileType.Empty;   // 마우스 클릭된 위치의 타일을 currentType 속성으로 변경

    private void Update()
    {
        RaycastHit hit;
        
        // 마우스 왼쪽 버튼을 누르고 있을 때
        if ( Input.GetMouseButton(0) )
        {
            // 카메라로부터 현재 마우스 위치로 뻗어나가는 광선 생성
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            // 광선이 부딪힌 오브젝트가 존재하면 hit에 저장
            if( Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // hit 오브젝트의 Tile 컴포넌트 정보를 불러와 tile 변수에 저장
                // 이 때 hit 오브젝트에 Tile 컴포넌트가 없으면 null 반환
                Tile tile = hit.transform.GetComponent<Tile>();
                if (tile != null)
                {
                    // 부딪힌 오브젝트를 tileType 속성으로 변경 (타일, 아이템, 플레이어 캐릭터)
                    tile.TileType = currentType;
                }
            }
        }
        
    }

    /// <summary>
    /// 타일, 아이템, 플레이어 캐릭터 버튼을 눌러 tileType을 변경
    /// </summary>
    
    public void SetTileType(int tileType)
    {
        currentType = (TileType)tileType;
    }

    void Start()
    {
        
    }

    
    
}
