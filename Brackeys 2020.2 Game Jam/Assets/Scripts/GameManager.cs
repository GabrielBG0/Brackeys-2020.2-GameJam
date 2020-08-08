using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{

    public static GameManager gm;
    public GameObject player;

    private bool isPresent = true;
    public Transform[] spawnPoints;
    private int presentSpawnPoint = 0;

    public GameObject[] win;

#pragma warning disable 649
    [SerializeField] private GameObject presentTileSet;
    [SerializeField] private GameObject pastTileSet;
    [SerializeField] private GameObject canvas;
    public Volume postProcessing;
    private SplitToning pPSplitToning;
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
       presentTileSet.SetActive(true);
       pastTileSet.SetActive(false);
       postProcessing.enabled = true;
       win[0].SetActive(true);
       win[1].SetActive(false);
       SplitToning st;
       if (postProcessing.profile.TryGet<SplitToning>(out st))
       {
           pPSplitToning = st;
       }
       pPSplitToning.active = false;

       Respawn();
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

            pastTileSet.SetActive(true);
            presentTileSet.SetActive(false);
            pPSplitToning.active = true;
            win[0].SetActive(false);
            win[1].SetActive(true);

            yield return new WaitForSeconds(1.25f);
            player.GetComponent<PlayerControl>().Unfreeze();
        }
        else
        {
            isPresent = true;
            player.GetComponent<PlayerControl>().Freeze();
            timeTransition.SetTrigger("Forward");

            yield return new WaitForSeconds(1f);

            pastTileSet.SetActive(false);
            presentTileSet.SetActive(true);
            pPSplitToning.active = false;
            win[0].SetActive(true);
            win[1].SetActive(false);

            yield return new WaitForSeconds(1.25f);
            player.GetComponent<PlayerControl>().Unfreeze();
        }
        

    }

    public void UpdateSpawnPoint(int id)
    {
        presentSpawnPoint = id;
    }

    public void Respawn()
    {
        player.GetComponent<Transform>().position = spawnPoints[presentSpawnPoint].position;
    }


    public bool GetIsPresent()
    {
        return isPresent;
    }
}
