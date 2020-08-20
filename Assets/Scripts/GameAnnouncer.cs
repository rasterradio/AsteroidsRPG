using TMPro;

public class GameAnnouncer : Announcer
{
    const string title = "Asteroids RPG";
    const string gameOver = "Game over!";

    public Announcer strategy;

    public static GameAnnouncer AnnounceTo(TextMeshProUGUI text)
    {
        return AnnounceTo(TextComponent(text));
    }

    public static GameAnnouncer AnnounceTo(params Announcer[] strategies)
    {
        return AnnounceTo(Many(strategies));
    }

    public static GameAnnouncer AnnounceTo(Announcer strategy)
    {
        var instance = CreateInstance<GameAnnouncer>();
        instance.strategy = strategy;
        return instance;
    }

    public virtual void Title()
    {
        Announce(title);
    }

    public virtual void GameOver()
    {
        Announce(gameOver);
    }

    public override void Announce(string message)
    {
        strategy.Announce(message);
    }
}
