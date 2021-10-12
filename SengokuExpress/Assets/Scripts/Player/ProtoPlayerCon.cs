using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoPlayerCon : MonoBehaviour
{
    public CharacterController cc;
    public Animator anim;
    public Collider lightHB;
    public Collider heavyHB;
    public Collider barkHB;

    public float vert;
    public float hor;
    public float moveSpd;
    public float dashSpd;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    public void OnMoveInput(float horIn, float vertIn)
    {
        vert = vertIn;
        hor = horIn;
    }
    public void OnLightAttackInput(bool lAtkInp)
    {
        anim.SetTrigger("lightAttack");
    }
    public void OnHeavyAttackInput(bool hAtkInp)
    {
        anim.SetTrigger("heavyAttack");
    }
    public void OnBarkInput(bool barkInp)
    {
        anim.SetTrigger("bark");
    }
    public void OnDashInput(bool dashInp)
    {
        anim.SetTrigger("dash");
        Vector3 moveDir = Vector3.forward * vert * dashSpd + Vector3.right * hor * dashSpd;

        cc.Move(moveDir);

    }

    private void FixedUpdate()
    {
        Vector3 moveDir = Vector3.forward * vert * moveSpd + Vector3.right * hor * moveSpd;
        cc.Move(moveDir);
    }
}
