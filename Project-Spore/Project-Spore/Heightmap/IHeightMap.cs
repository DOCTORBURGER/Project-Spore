using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Spore.Heightmap
{
    public interface IHeightMap
    {
        float GetHeightAt(float x, float z);
    }
}
