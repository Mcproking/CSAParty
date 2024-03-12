using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tempscrip : MonoBehaviour
{
    public void jumpscene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
