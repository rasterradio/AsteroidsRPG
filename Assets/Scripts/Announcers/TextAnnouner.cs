using TMPro;

public class TextComponentAnnouncer : Announcer
{
    public TextMeshProUGUI text;

    public static TextComponentAnnouncer New(TextMeshProUGUI text)
    {
        var instance = CreateInstance<TextComponentAnnouncer>();
        instance.text = text;
        return instance;
    }

    public override void Announce(string message)
    {
        if (text)
            text.text = message;
    }
}