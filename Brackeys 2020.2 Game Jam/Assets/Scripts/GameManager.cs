using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    public static GameManager gm;

    private bool isPresent = true;

#pragma warning disable 649
    [SerializeField] private GameObject presentTileSet;
    [SerializeField] private GameObject pastTileSet;
#pragma warning restore 649

    void Awake()
    {
        if (gm != null && gm != this)
        {
            Destroy(gameObject);
            return;
        }
        gm = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isPresent)
        {
            pastTileSet.SetActive(false);
            presentTileSet.SetActive(true);
        }
        else
        {
            pastTileSet.SetActive(true);
            presentTileSet.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPresent)
        {
            pastTileSet.SetActive(false);
            presentTileSet.SetActive(true);
        }
        else
        {
            pastTileSet.SetActive(true);
            presentTileSet.SetActive(false);
        }
    }

    public void MoveTime()
    {

        Debug.Log("change triggered");
        if (isPresent)
        {
            isPresent = false;
        }
        else
        {

            isPresent = true;
        }
    }

}
