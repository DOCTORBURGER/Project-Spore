using Microsoft.Xna.Framework;

namespace Project_Spore.Camera
{
    public interface ICamera
    {
        Matrix View { get; }

        Matrix Projection { get; }
    }
}
