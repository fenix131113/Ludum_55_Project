using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionWizardObject : MonoBehaviour
{
    [SerializeField] private Tilemap selfPlacedTilemap;

    private Vector3Int coordinates;


    private void Start()
    {
        coordinates = selfPlacedTilemap.WorldToCell(transform.position);
        transform.position = selfPlacedTilemap.GetCellCenterWorld(coordinates);
    }
}