using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    public static GameManager gm;
    public GameObject player;

    private bool isPresent = true;


#pragma warning disable 649
    [SerializeField] private GameObject presentTileSet;
    [SerializeField] private GameObject pastTileSet;
    [SerializeField] private GameObject canvas;
    public Animator timeTransition;
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
       canvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveTime()
    {

        StartCoroutine(TimeTransition());

    }

    IEnumerator TimeTransition()
    {
        if (isPresent)
        {
            isPresent = false;
            player.GetComponent<PlayerControl>().Freeze();
            timeTransition.SetTrigger("Rewind");

            yield return new WaitForSeconds(1f);

            Debug.Log("faze 2");
            pastTileSet.SetActive(true);
            presentTileSet.SetActive(false);

            yield return new WaitForSeconds(0.25f);

            yield return new WaitForSeconds(1f);
            Debug.Log("faze 3");
            player.GetComponent<PlayerControl>().Unfreeze();
        }
        else
        {
            isPresent = true;
            player.GetComponent<PlayerControl>().Freeze();
            timeTransition.SetTrigger("Forward");

            yield return new WaitForSeconds(1f);

            Debug.Log("faze 2");
            pastTileSet.SetActive(false);
            presentTileSet.SetActive(true);

            yield return new WaitForSeconds(0.25f);

            yield return new WaitForSeconds(1f);
            Debug.Log("faze 3");
            player.GetComponent<PlayerControl>().Unfreeze();
        }
        

    }

}
