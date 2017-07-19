using System.Collections.Generic;

public class DataManager{

    private int currentShowedAntId = 1;
    private static DataManager instance;

    private List<AntData> antList;
    public UserData userData;

    public DataManager(){
        //userData = new UserData();
        createAntList();
    }

    public void setUserData(UserData userData)
    {
        this.userData = userData;
    }

    public static DataManager getInstance(){
        if (instance == null)
            instance = new DataManager();

        return instance;
    }

    private void createAntList(){
        antList = new List<AntData>();
        /*
        antList.Add(new AntData(1, "fomiga 1", 1));
        antList.Add(new AntData(2, "fomiga 2", 1));
        antList.Add(new AntData(3, "fomiga 3", 1));
        antList.Add(new AntData(4, "fomiga 4", 1));
        antList.Add(new AntData(5, "fomiga 5", 2));
        antList.Add(new AntData(6, "fomiga 6", 2));
        antList.Add(new AntData(7, "fomiga 7", 2));
        antList.Add(new AntData(8, "fomiga 8", 3));
        antList.Add(new AntData(9, "fomiga 9", 3));
        */
    }

    public List<AntData> getAntList(){
        return antList;
    }

    public void cleanAntData()
    {
        antList = new List<AntData>();
    }

    public AntData getAntData(int id){
        foreach (AntData currentAntData in antList)
            if (currentAntData.id == id)
                return currentAntData;

        return null;
    }

    public AntData getAntDataByGraphicId(int graphicId)
    {
        foreach (AntData currentAntData in antList)
            if (currentAntData.arGraphicId == graphicId)
                return currentAntData;

        return null;
    }

    public void setCurrentShowedAntId(int antId)
    {
        currentShowedAntId = antId;
    }

    public int getCurrentShowedAntId()
    {
        return currentShowedAntId;
    }
}
