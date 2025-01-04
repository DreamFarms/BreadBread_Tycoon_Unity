using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dough : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    // �ִϸ��̼� 2�� �� ���ߴ� �޼���
    public void PlayAnimation()
    {
        _animator.SetBool("isAnimation", true);
    }

    public void StopAnimation()
    {
        _animator.SetBool("isAnimation", false);
    }
}
