using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타일의 속성 ( 빈 공간, 기본, 부서질 수 있는, 폭탄, 점프, 왼쪽 직진, 오른쪽 직진, 순간이동, 개수 파악용)
public enum TileType { Empty = 0, Base, Broke, Boom, Jump, StraightLeft, StraightRight, Blink, LastIndex}

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Sprite[] images;                     // 타일에 적용될 수 있는 이미지 배열
    private SpriteRenderer spriteRenderer;       // 타일 이미지 변경을 위한 SpriteRenderer
    private TileType       tileType;             // 현재 타일의 속성

    public void Setup(TileType tileType)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        TileType       = tileType;
    }

    public TileType TileType
    {
        set
        {
            tileType              = value;
            spriteRenderer.sprite = images[(int)tileType-1];
        }
        get => tileType;
    }
    
    
    
    
}
