using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodScript : MonoBehaviour
{
    private float timer = 0.0f;
    
    public IEnumerator opened()
    {
        this.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        this.gameObject.SetActive(false);
    }
}
