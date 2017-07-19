
using System;

public class AntDataComparator{

	public static JsonAnt getUpdatedAnt(JsonAnt localAntData, JsonAnt otherAntData)
    {
        localAntData.updated = false;
        otherAntData.updated = false;

        DateTime localAntDate = Convert.ToDateTime(localAntData.updated_at);
        DateTime otherAntDate = Convert.ToDateTime(otherAntData.updated_at);

        if (otherAntDate.CompareTo(localAntDate) == 1)
        {
            otherAntData.updated = true;
            return otherAntData;
        }

        return localAntData;
    }
}
