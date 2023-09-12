public class Bullet 
{
    public string name { get; set; } //Name
    public PlayerBullets.BULLET_TYPE type { get; set; }

    public PlayerBullets.BULLET_SUBTYPE subType { get; set; }

    public int count { get; set; }

    public Bullet(PlayerBullets.BULLET_TYPE type, PlayerBullets.BULLET_SUBTYPE subType, int count)
    {
        this.type = type;
        this.subType = subType;
        this.count = count;

        name = type.ToString() + "_" + subType.ToString();
    }

    //EMPTY
    public Bullet()
    {
        name = "EMPTY";
    }
}
