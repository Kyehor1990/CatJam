using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveReal : MonoBehaviour
{
   [SerializeField] private Rigidbody2D myRb;
   [SerializeField] private float horizontal;
   [SerializeField] private float speed;  
   [SerializeField] private int jumpPower;
   [SerializeField] private Transform groundCheck;
   [SerializeField] private LayerMask groundLayer;

   private bool isDead = false;
   private bool isFacingRight = true;
   private bool isGroundedNow;
   private bool wasGroundedLastFrame = true;
   private bool isWalking = false;

   private float walkSoundDelay = 0.3f; // Yürüyüş sesi çalma gecikmesi
   private float nextWalkSoundTime = 0f; // Bir sonraki yürüyüş sesi çalma zamanı
  // [SerializeField] private InputActionReference movement, attack, pointer;
   void Update()
   {
       if (isDead) return;
       
       horizontal = Input.GetAxisRaw("Horizontal");

       // Hareket
       myRb.velocity = new Vector2(horizontal * speed, myRb.velocity.y);

       // Karakterin yönünü ayarla
       if (horizontal > 0 && !isFacingRight)
       {
           Flip();
       }
       else if (horizontal < 0 && isFacingRight)
       {
           Flip();
       }

       // Yere çarpma kontrolü
       isGroundedNow = isGrounded();
       

       if (isGroundedNow)
       {
           
           if (Input.GetButtonDown("Jump"))
           {
               myRb.velocity = new Vector2(myRb.velocity.x, jumpPower);
           }
       }

       // Zıplama esnasında buton bırakılırsa hızını azalt
       if (Input.GetButtonUp("Jump") && myRb.velocity.y > 0f)
       {
           myRb.velocity = new Vector2(myRb.velocity.x, myRb.velocity.y * 0.5f);
       }

       // Yürüme sesini kontrol et
       if (horizontal != 0 && isGroundedNow && Time.time >= nextWalkSoundTime)
       {
           isWalking = true;
           nextWalkSoundTime = Time.time + walkSoundDelay; // Bir sonraki ses çalma zamanını ayarla
       }
       else if (horizontal == 0 || !isGroundedNow)
       {
           isWalking = false;
       }

       wasGroundedLastFrame = isGroundedNow;
   }
   
   private bool isGrounded()
   {
       return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
   }

   private void Flip()
   {
       isFacingRight = !isFacingRight;
       Vector3 localScale = transform.localScale;
       localScale.x *= -1;
       transform.localScale = localScale;
   }
   
}
