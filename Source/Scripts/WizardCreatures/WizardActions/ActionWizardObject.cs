using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionWizardObject : MonoBehaviour
{
    [SerializeField] private string objectType;
    [SerializeField] private Tilemap selfPlacedTilemap;
    [SerializeField] private bool isSlime;
    [SerializeField][Header("Width % 3 == 0")] private int width;
    [SerializeField][Header("Height % 3 == 0")] private int height;

    private List<Vector3Int> coordinates = new();

    public IReadOnlyCollection<Vector3Int> Coordinates => coordinates;
    public Vector3Int GetFirstCoordinate => coordinates[0];
    public bool IsSlime => isSlime;
    public string ObjectType => objectType;

    private void Start()
    {
        if (isSlime)
            selfPlacedTilemap = WizardActionsController.Instance.SlimesTilemap;

        AlignObjectToTilemap();
        CalculateCoordinates();

        WizardActionsController.Instance.RegisterActionObject(this);
    }

    private void AlignObjectToTilemap()
    {
        coordinates.Add(selfPlacedTilemap.WorldToCell(transform.position));

        if (isSlime)
            return;

        transform.position = selfPlacedTilemap.GetCellCenterWorld(coordinates[0]);
    }

    public void SetOnlyOneCoordinate(Vector3Int coordinate)
    {
        coordinates = new()
        {
            coordinate
        };
    }

    private void CalculateCoordinates()
    {
        if (width > 1)
        {
            for (int i = 0; i < (width - 1) / 2; i++)
            {
                coordinates.Add(coordinates[0] + new Vector3Int(1 * (i + 1), 0, 0));
            }
            for (int i = 0; i < (width - 1) / 2; i++)
            {
                coordinates.Add(coordinates[0] + new Vector3Int(1 * (i - 1), 0, 0));
            }

        }
        if (height > 1)
        {
            for (int i = 0; i < (height - 1) / 2; i++)
            {
                coordinates.Add(coordinates[0] + new Vector3Int(0, 1 * (i + 1), 0));
            }
            for (int i = 0; i < (height - 1) / 2; i++)
            {
                coordinates.Add(coordinates[0] + new Vector3Int(0, 1 * (i - 1), 0));
            }
        }
    }
}