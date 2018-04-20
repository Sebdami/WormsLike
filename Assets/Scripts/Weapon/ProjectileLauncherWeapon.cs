using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncherWeapon : Weapon {
    UIPowerBar uiPowerBar;
    [SerializeField]
    GameObject ProjectilePrefab;
    [SerializeField]
    float minLaunchPower = 5;
    [SerializeField]
    float maxLaunchPower = 30;
    [SerializeField]
    float rotationSpeed = 40.0f;
    [SerializeField]
    float maxAngleUp = 85f;
    [SerializeField]
    float maxAngleDown = 75f;
    float currentAngle;
    float currentLaunchPower;
    float currentLaunchTimer;
    float launchMaxTime = 1.0f;

    bool isShooting = false;

    new void Start()
    {
        base.Start();
        uiPowerBar = GameManager.instance.LevelCanvas.GetComponentInChildren<UIPowerBar>(true);
    }
    bool forceShoot = false;
    protected new void Update()
    {
        base.Update();
        if(isShooting)
        {

            currentLaunchTimer += Time.deltaTime;
            if(currentLaunchTimer >= launchMaxTime + .25f)
            {
                forceShoot = true;
            }

            uiPowerBar.UpdateFillValue(currentLaunchTimer/launchMaxTime);

            if (forceShoot)
            {
                Shoot();
            }
        }
        else
        {
            if(ownerController.CurrentState == WormState.WeaponHandled)
            {
                ownerController.CurrentState = WormState.Movement;
            }
        }

        transform.Rotate(-Vector3.right * Input.GetAxisRaw("Vertical") * rotationSpeed * Time.deltaTime);
        currentAngle = transform.localEulerAngles.x;
        currentAngle = (currentAngle > 180) ? currentAngle - 360 : currentAngle;
        currentAngle = Mathf.Clamp(currentAngle, -maxAngleUp, maxAngleDown);
        transform.localEulerAngles = new Vector3(currentAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    protected override void OnUseKeyPressed()
    {
        base.OnUseKeyPressed();
        StartShooting();
    }

    protected override void OnUseKeyReleased()
    {
        base.OnUseKeyReleased();
        Shoot();
    }

    void StartShooting()
    {
        uiPowerBar.UpdateFillValue(0.0f);
        uiPowerBar.gameObject.SetActive(true);
        isShooting = true;
        currentLaunchPower = minLaunchPower;
        currentLaunchTimer = 0.0f;
        ownerController.CurrentState = WormState.WeaponHandled;
        DecreaseAmmo();
    }

    void Shoot()
    {
        if (isShooting)
        {
            forceShoot = false;

            GameObject projectile = Instantiate(ProjectilePrefab, transform.GetChild(1).position, transform.GetChild(1).rotation);
            StartCoroutine(DisableCollisionsForSeconds(GetComponentInParent<WormController>().GetComponentsInChildren<Collider>(), projectile.GetComponent<Collider>(), 0.5f));
            projectile.GetComponent<Projectile>().Launch(projectile.transform.forward, Mathf.Lerp(minLaunchPower, maxLaunchPower, currentLaunchTimer / launchMaxTime));
            isShooting = false;
            uiPowerBar.UpdateFillValue(0.0f);
            uiPowerBar.gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        currentLaunchPower = minLaunchPower;
        currentLaunchTimer = 0.0f;

        isShooting = false;
    }

    IEnumerator DisableCollisionsForSeconds(Collider[] cols, Collider col2, float seconds)
    {
        for(int i = 0; i < cols.Length; i++)
        {
            Physics.IgnoreCollision(cols[i], col2, true);
        }
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < cols.Length; i++)
        {
            if(cols[i] && col2)
                Physics.IgnoreCollision(cols[i], col2, false);
        }
    }
}
