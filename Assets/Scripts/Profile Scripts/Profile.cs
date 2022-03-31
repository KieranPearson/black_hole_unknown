
[System.Serializable]
public class Profile
{
    private string name;
    private int highscore;

    public Profile(string name)
    {
        this.name = name;
        highscore = 0;
    }

    public string GetName()
    {
        return name;
    }

    public int GetHighscore()
    {
        return highscore;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SetHighscore(int highscore)
    {
        this.highscore = highscore;
    }
}
