using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pickup))]
public class PickupEditor : Editor
{
    public override void OnInspectorGUI()
    {

        //base.OnInspectorGUI();
        Pickup pickup = (Pickup)target;
        pickup.pickupType = (PickupType)EditorGUILayout.EnumPopup("Pickup Type", pickup.pickupType);
        SerializedProperty serializedProperty = serializedObject.FindProperty("m_Power"); 
       //pickup.powerup = (Powerup)EditorGUILayout.DropdownButton("Powerup", pickup.powerup);

        switch (pickup.pickupType)
        {
            case PickupType.Speed:
                pickup.powerup.isPermanent = EditorGUILayout.Toggle("Permanent", pickup.powerup.isPermanent);
                pickup.powerup.duration = EditorGUILayout.FloatField("Duration", pickup.powerup.duration);
                pickup.powerup.moveSpeedModifier = EditorGUILayout.FloatField("Move Speed", pickup.powerup.moveSpeedModifier);
                pickup.powerup.turnSpeedModifier = EditorGUILayout.FloatField("Turn Speed", pickup.powerup.turnSpeedModifier);
                Debug.Log(pickup.powerup);
                break;
            case PickupType.FireRate:
                pickup.powerup.isPermanent = EditorGUILayout.Toggle("Permanent", pickup.powerup.isPermanent);
                pickup.powerup.duration = EditorGUILayout.FloatField("Duration", pickup.powerup.duration);
                pickup.powerup.fireRateModifier = EditorGUILayout.FloatField("Fire Rate", pickup.powerup.fireRateModifier);
                break;
            case PickupType.Health:
                pickup.powerup.isPermanent = EditorGUILayout.Toggle("Permanent", pickup.powerup.isPermanent);
                pickup.powerup.duration = EditorGUILayout.FloatField("Duration", pickup.powerup.duration);
                pickup.powerup.healthModifier = EditorGUILayout.FloatField("Health", pickup.powerup.healthModifier);
                break;
            case PickupType.Shield:
                pickup.powerup.isPermanent = EditorGUILayout.Toggle("Permanent", pickup.powerup.isPermanent);
                pickup.powerup.duration = EditorGUILayout.FloatField("Duration", pickup.powerup.duration);
                pickup.powerup.shieldModifier = EditorGUILayout.FloatField("Shield Modifier", pickup.powerup.shieldModifier);
                break;
            default:
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
