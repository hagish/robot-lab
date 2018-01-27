using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandIndicator : MonoBehaviour {

    public List<GameObject> indicators;
    public AvatarController player;
	// Use this for initialization
	void Start ()
    {
        


    }
	
	// Update is called once per frame
	void Update ()
    {
        if(indicators != null)
        {
            foreach( GameObject i in indicators)
            {
                if(i.name == player.SelectedCommand)
                {
                    i.SetActive(true);
                }
                else
                {
                    i.SetActive(false);
                }
                
            }
        }
		
	}
}
