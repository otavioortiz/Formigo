using System.Collections.Generic;

public class UserData{

    public int id;
    public int score = 999;
    public string userName = "testUser";
    public string userEmail = "user@test.com";
    public string facebookId;

    private List<int> antIdList;

    public UserData(JsonPlayer jsonPlayer){
        antIdList = new List<int>();

        this.id = jsonPlayer.id;
        this.score = jsonPlayer.score;
        this.userName = jsonPlayer.name;
        this.userEmail = jsonPlayer.email;
        this.facebookId = jsonPlayer.facebookId;

        for(int i=0;i<jsonPlayer.antList.Length;i++)
            addAnt(jsonPlayer.antList[i]);
    }

    public bool verifyAntCaptured(int antId){
        foreach (int currentAntId in antIdList)
            if (currentAntId == antId)
                return true;

        return false;
    }

    public void addAnt(int antId){
        antIdList.Add(antId);
    }

    public List<int> getAntIdList()
    {
        return antIdList;
    }
}
