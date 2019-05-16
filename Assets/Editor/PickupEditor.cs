using UnityEditor;

//TODO: Optimize this code and remove unused variables

//Make a custom editor for the Pickup script 
[CustomEditor(typeof(Pickup))]

//This is the custom editor 
public class PickupEditor : Editor
{
    //The variables we want to edit from the Pickup script

    //SpeedPowerup
    SerializedObject m_object;
    SerializedProperty speedAudio;
    SerializedProperty speedPowerup;
    SerializedProperty moveSpeedModifier;
    SerializedProperty turnSpeedModifier;
    SerializedProperty speedDuration;
    SerializedProperty speedIsPermanent;
    SerializedProperty speedVisualModifier;
    SerializedProperty speedPowerupTexture;
    SerializedProperty speedColor;

    //FireRatePowerup
    SerializedProperty fireRateAudio;
    SerializedProperty fireRateModifier;
    SerializedProperty fireRateDuration;
    SerializedProperty fireRateIsPermanent;
    SerializedProperty fireRateVisualModifier;
    SerializedProperty fireRatePowerupTexture;
    SerializedProperty fireRateColor;

    //HealPowerup
    SerializedProperty healAudio;
    SerializedProperty healAmount;
    SerializedProperty healDuration;
    SerializedProperty healIsPermanent;
    SerializedProperty healVisualModifier;
    SerializedProperty healPowerupTexture;
    SerializedProperty healColor;

    //ShieldPowerup
    SerializedProperty shieldAudio;
    SerializedProperty shieldPrefab;
    SerializedProperty shieldMaxHealth;
    //SerializedProperty shieldCurrentHealth;
    //SerializedProperty shieldHealthBarBackground;
    //SerializedProperty shieldHealthBar;
    SerializedProperty shieldDuration;
    SerializedProperty shieldIsPermanent;
    SerializedProperty shieldVisualModifier;
    SerializedProperty shieldPowerupTexture;
    SerializedProperty shieldColor;

    //HelperPowerup
    SerializedProperty helperDuration;
    SerializedProperty helperIsPermanent;
    SerializedProperty helperVisualModifier;
    SerializedProperty helperPowerupTexture;
    SerializedProperty helperColor;

    //Template
    //SerializedProperty Duration;
    //SerializedProperty VisualModifier;
    //SerializedProperty isPermanent;
    //SerializedProperty PowerupTexture;
    //SerializedProperty Color;
    //SerializedProperty overShieldObject;

    void OnEnable()
    {
        m_object = new SerializedObject(target);

        //Speed
        speedAudio = serializedObject.FindProperty("speedPowerup.audio");
        moveSpeedModifier = serializedObject.FindProperty("speedPowerup.moveSpeedModifier");
        turnSpeedModifier = serializedObject.FindProperty("speedPowerup.turnSpeedModifier");
        speedDuration = serializedObject.FindProperty("speedPowerup.duration");
        speedIsPermanent = serializedObject.FindProperty("speedPowerup.isPermanent");
        speedVisualModifier = serializedObject.FindProperty("speedPowerup.visualModifier");
        speedPowerupTexture = serializedObject.FindProperty("speedPowerup.mainTexture");
        speedColor = serializedObject.FindProperty("speedPowerup.color");

        //FireRate
        fireRateAudio = serializedObject.FindProperty("fireRatePowerup.audio");
        fireRateModifier = serializedObject.FindProperty("fireRatePowerup.fireRateModifier");
        fireRateDuration = serializedObject.FindProperty("fireRatePowerup.duration");
        fireRateIsPermanent = serializedObject.FindProperty("fireRatePowerup.isPermanent");
        fireRateVisualModifier = serializedObject.FindProperty("fireRatePowerup.visualModifier");
        fireRatePowerupTexture = serializedObject.FindProperty("fireRatePowerup.mainTexture");
        fireRateColor = serializedObject.FindProperty("fireRatePowerup.color");

        //Heal
        healAudio = serializedObject.FindProperty("healPowerup.audio");
        healAmount = serializedObject.FindProperty("healPowerup.amount");
        //Duration = serializedObject.FindProperty("Powerup.duration");
        //IsPermanent = serializedObject.FindProperty("Powerup.isPermanent");
        //VisualModifier = serializedObject.FindProperty("Powerup.visualModifier");
        //PowerupTexture = serializedObject.FindProperty("Powerup.mainTexture");
        //Color = serializedObject.FindProperty("Powerup.color");

        //Shield
        shieldAudio = serializedObject.FindProperty("shieldPowerup.audio");
        shieldPrefab = serializedObject.FindProperty("shieldPowerup.shieldPrefab");
        shieldDuration = serializedObject.FindProperty("shieldPowerup.duration");
        shieldMaxHealth = serializedObject.FindProperty("shieldPowerup.maxHealth");
        //shieldCurrentHealth = serializedObject.FindProperty("shieldPowerup.curentHealth");
        //shieldHealthBarBackground = serializedObject.FindProperty("shieldPowerup.healthBarBackground");
        //shieldHealthBar = serializedObject.FindProperty("shieldPowerup.healthBar");
        shieldIsPermanent = serializedObject.FindProperty("shieldPowerup.isPermanent");
        shieldVisualModifier = serializedObject.FindProperty("shieldPowerup.visualModifier");
        shieldPowerupTexture = serializedObject.FindProperty("shieldPowerup.mainTexture");
        shieldColor = serializedObject.FindProperty("shieldPowerup.color");

        speedPowerup = serializedObject.FindProperty("speedPowerup");
        //Duration = serializedObject.FindProperty("Powerup.duration");
        //IsPermanent = serializedObject.FindProperty("Powerup.isPermanent");
        //VisualModifier = serializedObject.FindProperty("Powerup.visualModifier");
        //PowerupTexture = serializedObject.FindProperty("Powerup.mainTexture");
        //Color = serializedObject.FindProperty("Powerup.color");


        //helperDuration = serializedObject.FindProperty("shieldPowerup.duration");  //Change to the right class

    }


    public override void OnInspectorGUI()
    {


        //All the options to show in the custom inspector
        serializedObject.Update();

        Pickup pickup = (Pickup)target;
        pickup.pickupType = (PickupType)EditorGUILayout.EnumPopup("Pickup Type", pickup.pickupType);
        //pickup.speedPowerup = EditorGUILayout.PropertyField("Visual Modifier", pickup.speedPowerup, true, );
        //EditorGUILayout.PropertyField(pickup.speedPowerup);
        //EditorGUILayout.PropertyField(duration);

        switch (pickup.pickupType)
        {
            case PickupType.Speed:
                EditorGUILayout.PropertyField(speedIsPermanent);
                EditorGUILayout.PropertyField(speedVisualModifier);
                EditorGUILayout.PropertyField(speedAudio);
                EditorGUILayout.PropertyField(speedDuration);
                EditorGUILayout.PropertyField(moveSpeedModifier);
                EditorGUILayout.PropertyField(turnSpeedModifier);
                if (pickup.speedPowerup.visualModifier)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Vehicle Visuals", EditorStyles.boldLabel);
                    pickup.speedPowerup.vehiclePart = (VehiclePart)EditorGUILayout.EnumPopup("Vehicle Part", pickup.speedPowerup.vehiclePart);
                    EditorGUILayout.PropertyField(speedPowerupTexture);
                    EditorGUILayout.PropertyField(speedColor);
                }

                break;
            case PickupType.FireRate:
                EditorGUILayout.PropertyField(fireRateIsPermanent);
                EditorGUILayout.PropertyField(fireRateVisualModifier);
                EditorGUILayout.PropertyField(fireRateAudio);
                EditorGUILayout.PropertyField(fireRateDuration);
                EditorGUILayout.PropertyField(fireRateModifier);
                if (pickup.fireRatePowerup.visualModifier)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Vehicle Visuals", EditorStyles.boldLabel);
                    pickup.fireRatePowerup.vehiclePart = (VehiclePart)EditorGUILayout.EnumPopup("Vehicle Part", pickup.fireRatePowerup.vehiclePart);
                    EditorGUILayout.PropertyField(fireRatePowerupTexture);
                    EditorGUILayout.PropertyField(fireRateColor);
                }
                break;
            case PickupType.Health:
                //EditorGUILayout.PropertyField(healVisualModifier);
                EditorGUILayout.PropertyField(healAudio);
                EditorGUILayout.PropertyField(healAmount);
                //if (pickup.healPowerup.visualModifier)
                //{
                //    EditorGUILayout.Space();
                //    EditorGUILayout.LabelField("Vehicle Visuals", EditorStyles.boldLabel);
                //    pickup.healPowerup.vehiclePart = (VehiclePart)EditorGUILayout.EnumPopup("Vehicle Part", pickup.healPowerup.vehiclePart);
                //    EditorGUILayout.PropertyField(healPowerupTexture);
                //    EditorGUILayout.PropertyField(healColor);
                //}
                break;
            case PickupType.Shield:
                EditorGUILayout.PropertyField(shieldIsPermanent);
                EditorGUILayout.PropertyField(shieldVisualModifier);
                EditorGUILayout.PropertyField(shieldAudio);
                EditorGUILayout.PropertyField(shieldDuration);
                EditorGUILayout.PropertyField(shieldMaxHealth);
                //EditorGUILayout.PropertyField(shieldHealthBarBackground);
                //EditorGUILayout.PropertyField(shieldHealthBar);
                //EditorGUILayout.PropertyField(shieldCurrentHealth);
                EditorGUILayout.PropertyField(shieldPrefab);
                if (pickup.shieldPowerup.visualModifier)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Vehicle Visuals", EditorStyles.boldLabel);
                    pickup.shieldPowerup.vehiclePart = (VehiclePart)EditorGUILayout.EnumPopup("Vehicle Part", pickup.shieldPowerup.vehiclePart);
                    EditorGUILayout.PropertyField(shieldPowerupTexture);
                    EditorGUILayout.PropertyField(shieldColor);
                }
                break;

            default:
                break;
        }

        //If the visual modifier is true show the options for editing the Tank
        //if (pickup.powerup.visualModifier)
        //    {
        //        EditorGUILayout.Space();
        //        EditorGUILayout.LabelField("Vehicle Visuals", EditorStyles.boldLabel);
        //        pickup.powerup.vehiclePart = (VehiclePart)EditorGUILayout.EnumPopup("Vehicle Part", pickup.powerup.vehiclePart);
        //        EditorGUILayout.PropertyField(tankPowerupTexture);
        //        EditorGUILayout.PropertyField(color);
        //    }
        serializedObject.ApplyModifiedProperties();
    }
}