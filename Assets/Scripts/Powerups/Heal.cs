[System.Serializable]
public class HealPowerup : Powerup
{
    public int amount;

    public HealPowerup()
    {
        type = PickupType.Health;
        //amount = GameManager.instance.powerupSettings.healPowerup.amount;
    }

    public override void Activate(TankData tankData)
    {
        if (tankData.currentHealth < tankData.maxHealth)
        {
            if (tankData.currentHealth + amount > tankData.maxHealth)
                tankData.currentHealth = tankData.maxHealth;
            else
                tankData.currentHealth += amount;
        }
    }

    public override void Deactivate(TankData tankData)
    {
        return;
    }

    public override void Update(TankData tankData)
    {
    }
}
