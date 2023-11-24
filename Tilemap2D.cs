using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap2D : MonoBehaviour
{
     [SerializeField]
     private GameObject tilePRefab;  // 맵에 배치되는 타일 프리팹

     // 맵 x,y 크기 프로퍼티
     public int Width {private set; get; } = 10;
     public int Height {private set; get;} = 10;

    private void Awake()
    {
        GenerateTilemap();
    }

    public void GenerateTilemap()
    {
        for(int y = 0; y < Height; ++y)
        {
            for(int x = 0; x < Width; ++x)
            {
                //생성하는 타일맵의 중앙이 (0,0,0)인 위치
                Vector3 position = new Vector3((-Width*0.5f + 0.5f) + x, (Height*0.5f - 0.5f) - y, 0);

                SpawnTile(TileType.Empty, position);
            }
        }
    }

    private void SpawnTile(TileType tileType, Vector3 position)
    {
        GameObject clone = Instantiate(tilePRefab, position, Quaternion.identity);

        clone.name = "Tile"; // Tile 오브젝트의 이름을 "Tile"로 설정
        clone.transform.SetParent(transform); // Tilemap2D 오브젝트를 Tile 오브젝트의 부모로 설정
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
