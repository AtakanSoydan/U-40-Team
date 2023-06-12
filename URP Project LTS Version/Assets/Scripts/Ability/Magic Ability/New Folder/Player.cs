using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static event Action<Vector2> Move;

    public static event Action Jump;

    public static event Action Dash;
    public static event Action Attack;
    public static event Action Block;
    public static event Action BloodMagic;
    //public static event Action<TransformEnum> Transform;
    //public static Func<TransformEnum,bool> Stamina;

    private void OnMove(InputValue input)
    {
        Move?.Invoke(input.Get<Vector2>());
    }

    private void OnJump()
    {
        Jump?.Invoke();
    }

    private void OnDash()
    {
        Dash?.Invoke();
    }

    private void OnBlock()
    {
        Block?.Invoke();
    }

    private void OnBloodMagic()
    {
        BloodMagic?.Invoke();
    }

    private void OnAttack()
    {   
        Attack?.Invoke();
    }
    /*
    private void OnBatTransform()
    {
        Transform?.Invoke(TransformEnum.Bat);
    }
    */
}
