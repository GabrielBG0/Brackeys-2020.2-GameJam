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
       SplitToning st;
       if (postProcessing.profile.TryGet<SplitToning>(out st))
       {
           pPSplitToning = st;
       }
       pPSplitToning.active = false;
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

            pastTileSet.SetActive(true);
            presentTileSet.SetActive(false);
            pPSplitToning.active = true;

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

            yield return new WaitForSeconds(1.25f);
            player.GetComponent<PlayerControl>().Unfreeze();
        }
        

    }

}
