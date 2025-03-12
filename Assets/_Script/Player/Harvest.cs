using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{

    Collider2D _harvest;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Potato"))
        {
            _harvest = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Potato"))
        {
            _harvest = null;
        }
    }
    public void CanHarvest()
    {
        if (_harvest != null) {
            _harvest.GetComponent<CanGethit>().Gethit();
        }
    }
}
