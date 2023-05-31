using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 50;
    public AudioClip projectileSFX;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Vector3 shotPosition = transform.position + Camera.main.transform.forward + new Vector3(0, 1.5f, 0);
            
            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
            {
                mouseWorldPosition = raycastHit.point;
            }
            else
            {
                mouseWorldPosition = Camera.main.transform.position + Camera.main.transform.forward * 20;
            }
            Vector3 aimDirection = (mouseWorldPosition - shotPosition).normalized;
            Quaternion aimRotation = Quaternion.LookRotation(aimDirection, Vector3.up);

            GameObject projectile = Instantiate(projectilePrefab,
                shotPosition, aimRotation) as GameObject;
            
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(aimDirection * projectileSpeed, ForceMode.VelocityChange);

            projectile.transform.SetParent(
                GameObject.FindGameObjectWithTag("ProjectileParent").transform);
            
            AudioSource.PlayClipAtPoint(projectileSFX, Camera.main.transform.position);
        }
    }
}
