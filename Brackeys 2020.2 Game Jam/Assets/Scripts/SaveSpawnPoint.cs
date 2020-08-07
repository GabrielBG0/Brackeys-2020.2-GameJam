using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSpawnPoint : MonoBehaviour
{

    private GameManager gm;
    public int id;

    void Start()
    {
        gm = GameManager.gm;
    }
   public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            gm.UpdateSpawnPoint(id);

            this.gameObject.SetActive(false);
        }
    }
}
