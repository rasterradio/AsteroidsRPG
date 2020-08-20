using UnityEngine;

public static class ShipInput
{
    public static bool IsShooting()
    {
        return Input.GetButtonDown("Fire1");
    }

    public static bool IsBraking()
    {
        return Input.GetButton("Fire2");
    }

    public static float GetTurnAxis()
    {
        return Input.GetAxis("Horizontal");
    }

    public static float GetForwardThrust()
    {
        float axis = Input.GetAxis("Vertical");
        return Mathf.Clamp01(axis);
    }

    public static float GetMouseWheel()
    {
        return Input.GetAxis("MouseWheel");
    }
}
/*public void Fire(Vector3 position, Vector3 direction)
{
    if (timeSinceLastFire > 1 / fireRate)
    {
        //instantiate projectile
        projectile.transform.position = position;
        projectile.transform.up = direction;
        projectile.currVelocity = direction.normalized * projectileSpeed;
        projectile.duration = projectileDuration;
        projectile.isFromPlayer = isPlayerWeapon;
        timeSinceLastFire = 0;
    }*/