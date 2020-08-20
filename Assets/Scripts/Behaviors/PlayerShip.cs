using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public static PlayerShip Spawn(GameObject prefab)
    {
        GameObject clone = Instantiate(prefab);
        var existingShip = clone.GetComponent<PlayerShip>();
        return existingShip ? existingShip : clone.AddComponent<PlayerShip>();
    }
    public virtual bool IsAlive { get { return gameObject.activeSelf; } }

    public virtual void Recover()
    {
        if (!IsAlive)
        {
            ResetTransform();
            gameObject.SetActive(true);
        }
    }

    public void EnableControls()
    {
        GetComponent<PlayerShipMovement>().enabled = true;
        //GetComponent<ShipShooter>().enabled = true;
    }

    public void DisableControls()
    {
        GetComponent<PlayerShipMovement>().enabled = false;
        //GetComponent<ShipShooter>().enabled = false;
    }

    public void ResetTransform()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
