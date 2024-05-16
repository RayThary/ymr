public abstract class Card
{
    public Card(Player player)
    {
        user = player;
    }
    public Player user;

    protected string exp = "";
    protected string _name = "Card";
    public string Explanation { get { return exp; } }
    public string Name { get { return _name; } }
    protected CardManager.CardSprite sprite;
    public CardManager.CardSprite Sprite { get { return sprite; } }

    public abstract void Activation();
    public abstract void Deactivation();
}
