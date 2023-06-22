using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

enum PlayerProjectile
{
    Fireball,
    Icespear,
    Venombomb
}

public class ShootProjectile : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject bluefireballPrefab;
    public GameObject icespearPrefab;
    public GameObject venomBombPrefab;
    public float projectileSpeed = 50;
    public AudioClip projectileSFX;
    public float fireballCooldown = 0.7f;
    public float iceSpearCooldown = 1f;
    public float venomBombCooldown = 5f;
    public GameObject spellUI;
    public Vector3 baseSpellIconSize = new Vector3(0.8f, 0.8f, 1);
    public GameObject fireballSlider;
    public GameObject iceSpearSlider;
    public GameObject venomBombSlider;
    
    Animator anim;
    PlayerProjectile[] projectiles;
    PlayerProjectile playerProjectile = PlayerProjectile.Fireball;
    float fireballTimer;
    float iceSpearTimer;
    float venomBombTimer;
    Button[] buttons;
    int UIIndex;

    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        projectiles = new [] {PlayerProjectile.Fireball, PlayerProjectile.Icespear, PlayerProjectile.Venombomb};

        buttons = spellUI.GetComponentsInChildren<Button>();
        UIIndex = PlayerPrefs.GetInt("spellIndex", 0);
        ChangeSpell();

        fireballSlider.SetActive(false);
        fireballSlider.GetComponentInChildren<Slider>().maxValue = fireballCooldown;
        fireballTimer = fireballCooldown;
        iceSpearSlider.SetActive(false);
        iceSpearSlider.GetComponentInChildren<Slider>().maxValue = iceSpearCooldown;
        iceSpearTimer = iceSpearCooldown;
        venomBombSlider.SetActive(false);
        venomBombSlider.GetComponentInChildren<Slider>().maxValue = venomBombCooldown;
        venomBombTimer = venomBombCooldown;

        if(FindObjectOfType<PlayerItems>().HasItem("Blueflame"))
        {
            fireballPrefab = bluefireballPrefab;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!PauseMenuBehavior.isGamePaused && !PlayerHealth.isPlayerDead)
        {
            PollWeaponSwap();
            HandleShoot();
            UpdateCooldowns();
        }
    }

    void UpdateCooldowns()
    {
        fireballTimer += Time.deltaTime;
        iceSpearTimer += Time.deltaTime;
        venomBombTimer += Time.deltaTime;
        
        UpdateSpellCooldown(fireballSlider, fireballTimer, fireballCooldown);
        UpdateSpellCooldown(iceSpearSlider, iceSpearTimer, iceSpearCooldown);
        UpdateSpellCooldown(venomBombSlider, venomBombTimer, venomBombCooldown);
    }

    void UpdateSpellCooldown(GameObject slider, float timer, float cooldown)
    {
        if(timer > cooldown)
            slider.SetActive(false);
        else
            slider.GetComponentInChildren<Slider>().value = timer;
    }

    private void PollWeaponSwap()
    {
        int currentUIIndex = UIIndex;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerProjectile = PlayerProjectile.Fireball;
            UIIndex = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerProjectile = PlayerProjectile.Icespear;
            UIIndex = 1;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerProjectile = PlayerProjectile.Venombomb;
            UIIndex = 2;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(UIIndex >= buttons.Length - 1)
            {
                UIIndex = 0;
            }
            else
            {
                UIIndex++;
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(UIIndex <= 0)
            {
                UIIndex = buttons.Length - 1;
            }
            else
            {
                UIIndex--;
            }
        }
        if(currentUIIndex != UIIndex)
        {
            ChangeSpell();
        }
    }

    void ChangeSpell()
    {
        int i = 0;

        foreach (Button spellIcon in buttons)
        {
            if(i == UIIndex)
            {
                spellIcon.transform.localScale *= 1.25f;
            }
            else
            {
                spellIcon.transform.localScale = baseSpellIconSize;
            }

            i++;
        }

        PlayerPrefs.SetInt("spellIndex", UIIndex);
        playerProjectile = projectiles[UIIndex];
    }

    private void HandleShoot()
    {
        // Do not allow firing when cooldown has not been met.

        if (Input.GetButtonDown("Fire1") && CooldownMet())
        {
            SetCooldownTimer();
            
            anim.SetInteger("Attack", UnityEngine.Random.Range(1, 3));
            Invoke("AttackDelay", 0.15f);
            Invoke("ResetAttackAnim", 0.5f);
        }
    }

    void AttackDelay()
    {
        var shotPosition = CalculateShotPosition(out var mouseWorldPosition);
        Vector3 aimDirection = (mouseWorldPosition - shotPosition).normalized;
        Quaternion aimRotation = Quaternion.LookRotation(aimDirection, Vector3.up);

        GameObject projectile = Instantiate(GetProjectilePrefab(),
                shotPosition, aimRotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.AddForce(aimDirection * projectileSpeed, ForceMode.VelocityChange);

        projectile.transform.SetParent(
                GameObject.FindGameObjectWithTag("ProjectileParent").transform);
    }

    void ResetAttackAnim()
    {
        anim.SetInteger("Attack", 0);
    }

    private bool CooldownMet()
    {
        switch (playerProjectile)
        {
            case PlayerProjectile.Fireball:
                return fireballTimer >= fireballCooldown;
            case PlayerProjectile.Icespear:
                return iceSpearTimer >= iceSpearCooldown;
            case PlayerProjectile.Venombomb:
                return venomBombTimer >= venomBombCooldown;
            default:
                throw new Exception("Unsupported weapon projectile used.");
        }
    }

    void SetCooldownTimer()
    {
        switch (playerProjectile)
        {
            case PlayerProjectile.Fireball:
                fireballSlider.SetActive(true);
                fireballTimer = 0;
                fireballSlider.GetComponentInChildren<Slider>().value = fireballTimer;
                break;
            case PlayerProjectile.Icespear:
                iceSpearSlider.SetActive(true);
                iceSpearTimer = 0;
                iceSpearSlider.GetComponentInChildren<Slider>().value = iceSpearTimer;
                break;
            case PlayerProjectile.Venombomb:
                venomBombSlider.SetActive(true);
                venomBombTimer = 0;
                venomBombSlider.GetComponentInChildren<Slider>().value = venomBombTimer;
                break;
            default:
                throw new Exception("Unsupported weapon projectile used.");
        }
    }

    private Vector3 CalculateShotPosition(out Vector3 mouseWorldPosition)
    {
        Vector3 shotPosition = transform.position + Camera.main.transform.forward + new Vector3(0, 1.5f, 0);

        mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
        {
            mouseWorldPosition = raycastHit.point;
        }
        else
        {
            mouseWorldPosition = Camera.main.transform.position + Camera.main.transform.forward * 100;
        }

        return shotPosition;
    }

    private GameObject GetProjectilePrefab()
    {
        switch (playerProjectile)
        {
            case PlayerProjectile.Fireball:
                return fireballPrefab;
            case PlayerProjectile.Icespear:
                return icespearPrefab;
            case PlayerProjectile.Venombomb:
                return venomBombPrefab;
            default:
                throw new Exception("Unsupported weapon projectile used.");
        }
    }
}
