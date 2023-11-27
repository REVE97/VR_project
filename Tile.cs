using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    Empty = 0,Base, Broke, Boom, Jump, StraightLeft, StraightRight, Blink,   //타일
    ItemCoin = 10,                                                           //아이템
    Player = 100                                                             //플레이어
}
public class Tile : MonoBehaviour
{
    [SerializeField]
    private Sprite[] tileImages; //타일 이미지 배열
    [SerializeField]
    private Sprite[] itemImages; // 아이탬 이미지 배열
    [SerializeField]
    private Sprite playerImage; // 플레이어 이미지

    private TileType tileType;

    private SpriteRenderer spriteRenderer;

    public void Setup(TileType tileType)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        TileType = tileType;
    }    

    public TileType TileType
    {
        set
        {
            tileType = value;

            // 타일 ( Empty, Base, Broke, Boom, Jump, StraightLeft, StraightRight, Blink )
            if ((int)tileType < (int)TileType.ItemCoin)
            {
                spriteRenderer.sprite = tileImages[(int)tileType];
            }

            // 아이템 (Coin)
            else if ( (int)tileType < (int)TileType.Player)
            {
                spriteRenderer.sprite = itemImages[(int)tileType-(int)TileType.ItemCoin];
            }

            // 플레이어 캐릭터 (맵 에디터에 보여주기 위해 설정하였으며,
            // 저장할 땐 위치 정보를 저장하고 플레이어 위치의 타일은 Empty로 설정함
            else if ( (int)tileType == (int)TileType.Player )
            {
                spriteRenderer.sprite = playerImage;
            }   
        }
        get => tileType;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
