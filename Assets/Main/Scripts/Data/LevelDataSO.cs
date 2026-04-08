using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelDataSO : ScriptableObject
{
    public string levelName;
    public string sceneName;
    public int enemiesToKill;
}