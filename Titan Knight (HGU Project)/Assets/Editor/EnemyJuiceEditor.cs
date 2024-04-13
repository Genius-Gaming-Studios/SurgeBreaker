using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyJuice))]
public class EnemyJuiceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyJuice enemyJuice = (EnemyJuice)target;

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Chance of SFX Playing");
        enemyJuice.loudness = EditorGUILayout.IntSlider(enemyJuice.loudness,6, 0);
        EditorGUILayout.EndHorizontal();


        Rect controlRect = EditorGUILayout.GetControlRect();
        controlRect.x += EditorGUIUtility.labelWidth;
        controlRect.width -= EditorGUIUtility.labelWidth;

        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 40f;

        EditorGUI.LabelField(controlRect, "Low Chance", EditorStyles.miniLabel);

        controlRect.x += controlRect.width - 120f;
        EditorGUI.LabelField(controlRect, "High Chance", EditorStyles.miniLabel);

        EditorGUIUtility.labelWidth = labelWidth;

        GUILayout.Space(10);

        base.OnInspectorGUI();

        GUILayout.Space(10);

        if (GUILayout.Button("Restore Default SFX"))
        {
            Undo.RecordObject(enemyJuice, "Reset Enemy Juice Variables");

            // Set default audio clip paths here
            string defaultSpawnSoundPath = "Assets/Audio/FX/Enemy SFX/Sapper/Sapper Spawn.wav";
            string defaultAttackSoundPath = "Assets/Audio/FX/Enemy SFX/Warrior/Warrior Attack.wav";
            string defaultDeathSound1Path = "Assets/Audio/FX/Enemy SFX/Warrior/Warrior Death.wav";
            string defaultDeathSound2Path = "Assets/Audio/FX/Enemy SFX/Warrior/Warrior Death_2.wav";
            string defaultHitSound1Path = "Assets/Audio/FX/Enemy SFX/Warrior/Warrior Pain.wav";
            string defaultHitSound2Path = "Assets/Audio/FX/Enemy SFX/Warrior/Warrior Pain 2.wav";


            enemyJuice.loudness = 2;


            // Load default audio clips
            enemyJuice.spawnSound = AssetDatabase.LoadAssetAtPath<AudioClip>(defaultSpawnSoundPath);
            enemyJuice.attackSound = AssetDatabase.LoadAssetAtPath<AudioClip>(defaultAttackSoundPath);
            enemyJuice.deathVariation1 = AssetDatabase.LoadAssetAtPath<AudioClip>(defaultDeathSound1Path);
            enemyJuice.deathVariation2 = AssetDatabase.LoadAssetAtPath<AudioClip>(defaultDeathSound2Path);
            enemyJuice.hurtVariation1 = AssetDatabase.LoadAssetAtPath<AudioClip>(defaultHitSound1Path);
            enemyJuice.hurtVariation2 = AssetDatabase.LoadAssetAtPath<AudioClip>(defaultHitSound2Path);


            EditorUtility.SetDirty(enemyJuice);
        }
    }
}
