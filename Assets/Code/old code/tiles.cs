using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class tiles : MonoBehaviour
{
    public master master;

    public string p1Spawn;
    public string p2Spawn;
    public string p3Spawn;
    public string p4Spawn;

    bool buttonPressed;
    bool skipTile;

    Button convert;
    Button cancel;

    //public master Master;

    public Canvas canvas;

    public string spawnTile(Player player, int playerNumber) // assign the varible for which player s thier own spawn tile. also use for tile point coverter
    {
        string spawnTileName;
        if (Physics.Raycast(new Ray(player.transform.position, -transform.up), out RaycastHit hit, 2))
        {
            spawnTileName = hit.collider.name;
            switch (playerNumber)
            {
                case 1:
                    return spawnTileName;

                case 2:
                    return spawnTileName;
                case 3:
                    return spawnTileName;
                case 4:
                    return spawnTileName;
                default:
                    return null;
            }
        }
        else
        {
            return null;
        }
    }

    public IEnumerator checkTile(RaycastHit hit, Player player)
    {
        string playerSpawn = null;

        switch (player.name)
        {
            case "Player 1":
                playerSpawn = p1Spawn;
                break;

            case "Player 2":
                playerSpawn = p2Spawn;
                break;

            case "Player 3":
                playerSpawn = p3Spawn;
                break;

            case "Player 4":
                playerSpawn = p4Spawn;
                break;
        }



        //check for player standing on thier own spawn poinnt
        if (playerSpawn == null)
        {
            Debug.LogWarning("Cannot save Player Spawn inside tiles.cs");
        }

        if (hit.collider.name == playerSpawn)
        {
            // when the player stand on his own tile, show the button for the player to see it want to convert or not

            yield return StartCoroutine(spawnTile(player));

            //Debug.Log("Player at own spawn tile");
        }
        //Debug.Log("random tile");
        yield return null;
    }

    IEnumerator spawnTile(Player player) // when player stand on his own tile
    {
        Debug.Log("waiting for button");

        if (player.smallPoint < 5)
        {
            Debug.Log("Not enough point to convert");
            yield break;
        }

        convert.gameObject.SetActive(true);
        cancel.gameObject.SetActive(true);

        // show ui for point change
        // check if user want to change its small point to big point
        while (!buttonPressed)
        {
            if (skipTile)
            {
                Debug.Log("not to convert");
                skipTile = false;
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }

        //Debug.Log("button press to convert");

        if (player.smallPoint % 5 == 0)
        {
            Debug.Log("point with no remainder");
            player.bigPoint = player.smallPoint / 5;
            player.smallPoint = 0;
        }

        if (player.smallPoint % 5 != 0)
        {
            Debug.Log("Point with remainder");
            int remainder = player.smallPoint % 5;
            player.smallPoint -= remainder;
            player.bigPoint = player.smallPoint / 5;
            player.smallPoint = remainder;
        }

        //Debug.Log(player.name +":" + player.smallPoint % 5);
        master.updatePoint(player);
        yield return new WaitForSeconds(2f);

        buttonPressed = false;
        convert.gameObject.SetActive(false);
        cancel.gameObject.SetActive(false);



        yield return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        buttonPressed = false;
        skipTile = false;

        Button[] comps = canvas.GetComponentsInChildren<Button>(true);


        foreach (Button comp in comps)
        {
            if (comp.name == "convert") { convert = comp; }
            if (comp.name == "cancel") { cancel = comp; }
        }


        //Component[] temp = GetComponentsInChildren<Component>();

        //GameObject[] comps = GameObject.FindGameObjectsWithTag("Tile");

        //foreach (GameObject go in comps)
        //{
        //    if (go != null)
        //    {
        //        Debug.Log(go.name + " - " + go.GetType());
        //    }
        //}

    }

    public void buttonConvert()
    {
        //Debug.Log("Convert Pressed");
        buttonPressed = true;
    }

    public void buttonSkip()
    {
        //Debug.Log("Skip pressed");
        skipTile = true;
    }

}
