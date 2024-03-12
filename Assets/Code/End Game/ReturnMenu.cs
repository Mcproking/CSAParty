using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMenu : MonoBehaviour
{
    IEnumerator startAuto;
    private void Start()
    {
        startAuto = autoMenu();
        StartCoroutine(startAuto);
    }
    public void returnMenu()
    {
        StopAllCoroutines(); 
        SceneManager.LoadScene(0);
    }

    IEnumerator autoMenu()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);
    }
}
