using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class backgroundmusicinfo : MonoBehaviour
{
    public backgroundmusicmanager.levtype lt;
    private void Awake()
    {
        backgroundmusicmanager.instance.changeBackgroundMusic(lt);
    }
}
