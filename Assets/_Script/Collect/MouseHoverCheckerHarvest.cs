using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class MouseHoverCheckerHarvest : MonoBehaviour
{
    [SerializeField]PlayerController _PlayerController;
    public GameObject harvestIcon;
    void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPos);

        Seeds foundPlant = null;

        foreach (var hit in hits)
        {
            Seeds plant = hit.GetComponent<Seeds>();
            if (plant != null && plant.isReadyToHarvest)
            {
                foundPlant = plant;
                break;
            }
        }

        if (foundPlant != null)
        {
            harvestIcon.SetActive(true);
            harvestIcon.transform.position = foundPlant.transform.position + new Vector3(0, 1f, 0);
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(_PlayerController.MoveToClickPosition(foundPlant.transform.position, () =>
                {
                    foundPlant.Harvest(_PlayerController.transform.position);
                }));
            }
        }
        else
        {
            harvestIcon.SetActive(false);
        }
    }
}
