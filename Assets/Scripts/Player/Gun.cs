using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] Transform aimPos;
    [SerializeField] float aimSpeed;
    [SerializeField] float zoomFOV = 30f; // Field of view when zoomed in
    [SerializeField] float normalFOV = 60f; // Default field of view
    [SerializeField] Transform gunpos;
    public float attackrange=2f;
    public LayerMask Enemies;

    Camera mainCamera;
    bool isAiming = false;
    [SerializeField] Image crosshair;
    public ParticleSystem particlesystem;
    public RigBuilder rigBuilder;
    public GameObject katana;
    void Start()
    {
        mainCamera = Camera.main;
        rigBuilder.enabled = true;
        katana.SetActive(false);
    }

    void Update()
    {
        katana.SetActive(false);
        rigBuilder.enabled = true;

        aimPos.position= transform.position;
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            isAiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        if (isAiming)
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, zoomFOV, aimSpeed * Time.deltaTime);
            crosshair.enabled = true;
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, normalFOV, aimSpeed * Time.deltaTime);
            crosshair.enabled = false;

        }

        if (Input.GetMouseButtonDown(0)) // Assuming left mouse button for shooting
        {
            particlesystem.Play();
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSpeed * Time.deltaTime);

            // Print the name of the object hit by the raycast
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
            Collider[] colliders = Physics.OverlapSphere(hit.point, attackrange, Enemies);
            foreach (Collider collider in colliders)
            {
                Idamageable damagable = collider.GetComponent<Idamageable>();
                if (damagable != null)
                {
                    damagable.DamageAmount(damage);
                }
            }
        }
        // Reset the gun's position to the stored initial position
    }

}
