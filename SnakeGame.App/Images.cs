using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SnakeGame.App;

public static class Images
{
    public readonly static ImageSource Empty = LoadImage("Empty.png");
    public readonly static ImageSource Body = LoadImage("Body.png");
    public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");
    public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");
    public readonly static ImageSource Food = LoadImage("Food.png");
    public readonly static ImageSource Head = LoadImage("Head.png");

    private static ImageSource LoadImage(string filename) 
        => new BitmapImage(new Uri($"Assets/{filename}", UriKind.Relative));
}
