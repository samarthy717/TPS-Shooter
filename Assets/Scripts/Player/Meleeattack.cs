using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Meleeattack : MonoBehaviour
{
    public ThirdPersonController player;
    Animator animator;
    RigBuilder rigBuilder;
    public GameObject katana;
    public float meleeDamage = 20f;
    public float meleeRange = 5f;
    public LayerMask Enemies;
    void Start()
    {
        animator = player.GetComponent<Animator>();
        rigBuilder = player.GetComponent<RigBuilder>();
        katana.SetActive(true);
        if (rigBuilder != null)
        {
            rigBuilder.enabled = false; // Disable the RigBuilder component initially
        }
    }

    void Update()
    {
        katana.SetActive(true);
        DisableRigBuilder();
        if (Input.GetMouseButtonDown(0)) // Assuming left mouse button for attacking
        {
            Debug.Log("Attack");
            animator.SetTrigger("IsAttack");

            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Collider[] colliders = Physics.OverlapSphere(hit.point,meleeRange, Enemies);
                foreach (Collider collider in colliders)
                {
                    Idamageable damagable = collider.GetComponent<Idamageable>();
                    if (damagable != null)
                    {
                        damagable.DamageAmount(meleeDamage);
                    }
                }
            }
        }
    }


    // Method to enable the RigBuilder (for example, when exiting attack mode)
    public void EnableRigBuilder()
    {
        if (rigBuilder != null)
        {
            rigBuilder.enabled = true;
        }
    }

    // Method to disable the RigBuilder (for example, when entering attack mode)
    public void DisableRigBuilder()
    {
        if (rigBuilder != null)
        {
            rigBuilder.enabled = false;
        }
    }
}
