using UnityEngine;

//TODO: Rewrite so each powerup can get their own sound
//TODO: Only play audio if != null
//TODO: Fix audio to play quicker

[System.Serializable]
public class Pickup : MonoBehaviour
{
    public SpeedPowerup speedPowerup;
    public FireRatePowerup fireRatePowerup;
    public HealPowerup healPowerup;
    public ShieldPowerup shieldPowerup;
    public AudioClip pickupSound;
    public PickupType pickupType;

    public void OnTriggerEnter(Collider other)
    {
        PowerupController powerupController = other.GetComponentInParent<PowerupController>();

        if (powerupController == null) return;

        Transform tf = other.GetComponent<Transform>();
        //Transform tf = other.gameObject.GetComponentInParent<Transform>() == null ? other.gameObject.GetComponent<Transform>() : other.gameObject.GetComponentInParent<Transform>();


        switch (pickupType)
        {
            case PickupType.Speed:
                if (speedPowerup.audio != null)
                    AudioSource.PlayClipAtPoint(speedPowerup.audio, tf.position, AudioManager.instance.SFXVolume.value);
                powerupController.Add(speedPowerup);
                break;

            case PickupType.FireRate:
                if (fireRatePowerup.audio != null)
                    AudioSource.PlayClipAtPoint(fireRatePowerup.audio, tf.position, AudioManager.instance.SFXVolume.value);
                powerupController.Add(fireRatePowerup);
                break;

            case PickupType.Health:
                if (healPowerup.audio != null)
                    AudioSource.PlayClipAtPoint(healPowerup.audio, tf.position, AudioManager.instance.SFXVolume.value);
                powerupController.Add(healPowerup);
                break;

            case PickupType.Shield:
                if (shieldPowerup.audio != null)
                    AudioSource.PlayClipAtPoint(shieldPowerup.audio, tf.position, AudioManager.instance.SFXVolume.value);
                powerupController.Add(shieldPowerup);
                break;
        }

        Destroy(gameObject);
    }

}

public enum PickupType { Speed, FireRate, Health, Shield }