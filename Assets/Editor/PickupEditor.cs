using UnityEditor;
using UnityEngine;

//Make a custom editor for the Pickup script 
[CustomEditor(typeof(Pickup))]

//This is the custom editor 
public class PickupEditor : Editor
{
    //The variables we want to edit from the Pickup script
    SerializedProperty moveSpeedModifier;
    SerializedProperty turnSpeedModifier;
    SerializedProperty shieldModifier;
    SerializedProperty healthModifier;
    SerializedProperty fireRateModifier;
    SerializedProperty duration;
    SerializedProperty visualModifier;
    SerializedProperty isPermanent;
    SerializedProperty tankPowerupTexture;
    SerializedProperty color;
    SerializedProperty outerShell;
    SerializedProperty overShieldObject;

    void OnEnable()
    {
        moveSpeedModifier = serializedObject.FindProperty("SpeedPowerup.moveSpeedModifier");
        turnSpeedModifier = serializedObject.FindProperty("powerup.turnSpeedModifier");
        shieldModifier = serializedObject.FindProperty("powerup.shieldModifier");
        healthModifier = serializedObject.FindProperty("powerup.healthModifier");
        fireRateModifier = serializedObject.FindProperty("powerup.fireRateModifier");
        duration = serializedObject.FindProperty("powerup.duration");
        visualModifier = serializedObject.FindProperty("powerup.visualModifier");
        isPermanent = serializedObject.FindProperty("powerup.isPermanent");
        tankPowerupTexture = serializedObject.FindProperty("powerup.mainTexture");
        color = serializedObject.FindProperty("powerup.color");
        outerShell = serializedObject.FindProperty("powerup.outerShell");
        overShieldObject = serializedObject.FindProperty("powerup.overShieldObject");
    }

    
    public override void OnInspectorGUI()
    {
        //All the options to show in the custom inspector
        serializedObject.Update();

        Pickup pickup = (Pickup)target;
        pickup.pickupType = (PickupType)EditorGUILayout.EnumPopup("Pickup Type", pickup.pickupType);
        pickup.powerup.visualModifier = EditorGUILayout.Toggle("Visual Modifier", pickup.powerup.visualModifier);
        EditorGUILayout.PropertyField(isPermanent);
        EditorGUILayout.PropertyField(duration);

        switch (pickup.pickupType)
        {
            case PickupType.Speed:
                EditorGUILayout.PropertyField(moveSpeedModifier);
                EditorGUILayout.PropertyField(turnSpeedModifier);

                break;
            case PickupType.FireRate:
                EditorGUILayout.PropertyField(fireRateModifier);
                break;
            case PickupType.Health:
                EditorGUILayout.PropertyField(healthModifier);
                break;
            case PickupType.Shield:
                EditorGUILayout.PropertyField(shieldModifier);
                EditorGUILayout.PropertyField(overShieldObject);
                break;

            default:
                break;
        }

        //If the visual modifier is true show the options for editing the Tank
        if (pickup.powerup.visualModifier)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Vehicle Visuals", EditorStyles.boldLabel);
            pickup.powerup.vehiclePart = (VehiclePart)EditorGUILayout.EnumPopup("Vehicle Part", pickup.powerup.vehiclePart);
            EditorGUILayout.PropertyField(tankPowerupTexture);
            EditorGUILayout.PropertyField(color);
        }
        serializedObject.ApplyModifiedProperties();
    }
}