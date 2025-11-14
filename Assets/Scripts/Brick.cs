using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public enum BrickType
    {
        Normal_Brick,
        Secret_Brick
    }
    [SerializeField] private BrickType _myType;
    [SerializeField] private Sprite _sprNormal;
    [SerializeField] private Sprite _sprSecret;
    [SerializeField] private SpriteRenderer _mySpriteRender;
    void Start()
    {
        switch (_myType)
        {
            case BrickType.Normal_Brick:
                _mySpriteRender.sprite = _sprNormal;
                break;
            case BrickType.Secret_Brick:
                _mySpriteRender.sprite = _sprSecret;
                break;
        }
    }
}