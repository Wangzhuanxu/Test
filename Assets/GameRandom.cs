using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 游戏中的随机类
/// </summary>
public class GameRandom
{
    private Random _rd;
    public GameRandom()
    {
        _rd = new Random();
    }

    public GameRandom(int seed)
    {
        _rd = new Random(seed);
    }

    public int Next(int min,int max)
    {
        return _rd.Next(min, max);
    }

    public float Next(float min,float max)
    {
        float rd = (float)_rd.NextDouble();
        return min +  rd * (max - min);
    }

}
