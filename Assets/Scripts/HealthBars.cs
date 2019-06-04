using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public Image healthBar;
    public GameObject shieldHealthBarBackground;
    public Image shieldHealthBar;
    public PowerupController powerupController;
    public Player player;



    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        powerupController = player.pawn.powerupController;
        currentHealth = player.pawn.currentHealth;
        maxHealth = player.pawn.maxHealth;

        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;

        //Prevent errors if there is no powerup controller
        if (powerupController == null)
        {
            Debug.LogWarning("Missing PowerupController!");
            return;
        }


        if (powerupController.activePowerups[PickupType.Shield] != null)
        {

            if (powerupController.shieldPowerup.currentHealth <= 0 || powerupController.shieldPowerup.duration <= 0)
                shieldHealthBarBackground.gameObject.SetActive(false);
            else
            {
                shieldHealthBar.fillAmount = (float)powerupController.shieldPowerup.currentHealth / (float)powerupController.shieldPowerup.maxHealth;
                shieldHealthBarBackground.gameObject.SetActive(true);
            }
        }
        else
            shieldHealthBarBackground.gameObject.SetActive(false);
    }
}
