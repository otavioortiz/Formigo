using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacebookAvatarObject : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if(gameObject.transform.FindChild("tName"))
            gameObject.transform.FindChild("tName").gameObject.GetComponent<Text>().text = DataManager.getInstance().userData.userName;

        if (DataManager.getInstance().userData.facebookId != "" && DataManager.getInstance().userData.facebookId != null)
        {
            FB.API("https" + "://graph.facebook.com/" + DataManager.getInstance().userData.facebookId + "/picture?type=large", HttpMethod.GET, delegate (IGraphResult result)
            {
                if (result.Texture.width > 50)
                    gameObject.transform.FindChild("avatarMask").FindChild("avatarArea").gameObject.GetComponent<Image>().sprite = Sprite.Create(result.Texture, new Rect(0, 0, 200, 200), new Vector2(0f, 0f), 100);
            });
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
