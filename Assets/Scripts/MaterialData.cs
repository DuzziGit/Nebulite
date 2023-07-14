using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "ScriptableObjects/Material")]
public class MaterialData : ScriptableObject
{
    public Sprite materialSprite;
    public int materialProperty;
    public GameObject materialPrefab;
    public Color spriteTint = Color.white;
}
