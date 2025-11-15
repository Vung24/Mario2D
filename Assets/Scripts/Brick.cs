using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public enum BrickType
    {
        Normal_Brick,
        Secret_Brick1,
        Secret_Brick2
    }
    [SerializeField] private BrickType _myType;
    [SerializeField] private Sprite _sprNormal;
    [SerializeField] private Sprite _sprSecret1;
    [SerializeField] private Sprite _sprSecret2;

    [SerializeField] private SpriteRenderer _mySpriteRender;
    [SerializeField] private GameObject _objCoinEarn;
    private Animator _myAnim;
    void Start()
    {
        switch (_myType)
        {
            case BrickType.Normal_Brick:
                _mySpriteRender.sprite = _sprNormal;
                break;
            case BrickType.Secret_Brick1:
                _mySpriteRender.sprite = _sprSecret1;
                break;
            case BrickType.Secret_Brick2:
                _mySpriteRender.sprite = _sprSecret2;
                break;
        }
        _myAnim = this.GetComponent<Animator>();
    }
    public void OnHitBrick()
    {
        switch (_myType)
        {
            case BrickType.Normal_Brick:
                _myAnim.SetTrigger("Hit");
                break;
            case BrickType.Secret_Brick1:
                _objCoinEarn.SetActive(true);
                _mySpriteRender.sprite = _sprNormal;
                StartCoroutine(IDeativeCoin1());
                _myType = BrickType.Normal_Brick;
                IEnumerator IDeativeCoin1()
                {
                    yield return new WaitForSeconds(1.1f);
                    _objCoinEarn.SetActive(false);
                }
                break;
            case BrickType.Secret_Brick2:
                _objCoinEarn.SetActive(true);
                _mySpriteRender.sprite = _sprNormal;
                StartCoroutine(IDeativeCoin2());
                _myType = BrickType.Normal_Brick;
                IEnumerator IDeativeCoin2()
                {
                    yield return new WaitForSeconds(1.1f);
                    _objCoinEarn.SetActive(false);
                }
                break;
        }
    }
}