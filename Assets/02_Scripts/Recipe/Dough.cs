using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dough : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    // 애니메이션 2초 후 멈추는 메서드
    public void PlayAnimation()
    {
        _animator.SetBool("isAnimation", true);
    }

    public void StopAnimation()
    {
        _animator.SetBool("isAnimation", false);
    }
}
