public abstract class Card
{
    public Card(Player player)
    {
        user = player;
    }
    public Player user;

    protected string exp = "";
    public string Explanation { get { return exp; } }

    public abstract void Activation();
    public abstract void Deactivation();
}
