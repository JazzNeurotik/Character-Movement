using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMechanics : MonoBehaviour
{
    public float speedMove;//скорость персонажа
    public float jumpPower;//сила прыжка

    //Параметры геймплея для персонажа
    private float gravityForce;
    private Vector3 moveVector;

    //Ссылки на компоненты
    private CharacterController ch_controller;
    private Animator ch_animator;

    private void Start()
    {
        ch_controller = GetComponent<CharacterController>();
        ch_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CharacterMove();
        GamingGravity();
    }

    //метод перемещения персонажа
    private void CharacterMove()
    {
        if(ch_controller.isGrounded)
        {
            ch_animator.ResetTrigger("Jump");
            ch_animator.SetBool("Falling", false);

            moveVector = Vector3.zero;
            moveVector.x -= Input.GetAxis("Vertical") * speedMove;
            moveVector.z = Input.GetAxis("Horizontal") * speedMove;

            //анимация персонажа
            if(moveVector.x!=0||moveVector.z!=0)ch_animator.SetBool("Move",true);
            else ch_animator.SetBool("Move",false);

            if(Vector3.Angle(Vector3.forward,moveVector)>1f|| Vector3.Angle(Vector3.forward, moveVector) == 0)
            {
                Vector3 direct = Vector3.RotateTowards(transform.forward,moveVector, speedMove, 0.0f);
                transform.rotation = Quaternion.LookRotation(direct);
            }
        }
        else
        {
            if(gravityForce<-3f) ch_animator.SetBool("Falling", true);
        }

        moveVector.y = gravityForce;
        ch_controller.Move(moveVector * Time.deltaTime);
    }

    //метод гравитации
    private void GamingGravity()
    {
        if(!ch_controller.isGrounded) gravityForce -= 20f* Time.deltaTime;
        else gravityForce = -1f;
        if (Input.GetKeyDown(KeyCode.Space) && ch_controller.isGrounded) 
        {
            gravityForce = jumpPower;
            ch_animator.SetTrigger("Jump");
        }
    }
}
