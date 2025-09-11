using UnityEngine;

public struct BrakeResult
{
    public float breakPos;   // 开始减速的位置
    public int direction;    // 减速时的运动方向 (1 右, -1 左)

    public BrakeResult(float breakPos, int direction)
    {
        this.breakPos = breakPos;
        this.direction = direction;
    }
}

public static class BallBrakeSolver
{
    /// <summary>
    /// 计算小球的减速触发点
    /// </summary>
    /// <param name="Smax">边界，墙在 [-Smax, Smax]</param>
    /// <param name="pos">当前球位置</param>
    /// <param name="direction">当前方向 (1 右, -1 左)</param>
    /// <param name="v">当前速度大小 (>=0)</param>
    /// <param name="a">减速度大小 (正数)</param>
    /// <param name="target">希望停下来的位置</param>
    /// <returns>减速起点和方向，如果没解返回 breakPos = NaN</returns>
    public static BrakeResult GetBrakePosition(
        float Smax, float pos, int direction, float v, float a, float target)
    {
        if (a <= 0f || v <= 0f) return new BrakeResult(float.NaN, direction);

        float d = (v * v) / (2f * a); // 制动距离

        // 尝试几个展开像点
        for (int n = -5; n <= 5; n++)
        {
            float target_n = ((n % 2 == 0) ? 1f : -1f) * target + 2f * n * Smax;
            float brake_n = target_n - direction * d;

            // 必须在原区间内
            if (brake_n < -Smax || brake_n > Smax)
                continue;

            // 判断是否能到达 (pos 出发, 有反射)
            if (WillPassPosition(pos, direction, v, Smax, brake_n))
            {
                return new BrakeResult(brake_n, direction);
            }
        }

        return new BrakeResult(float.NaN, direction);
    }

    /// <summary>
    /// 判断从 pos,direction 出发的小球是否会经过目标点
    /// （简单模拟一段时间，考虑反射）
    /// </summary>
    private static bool WillPassPosition(float pos, int direction, float v, float Smax, float targetPos)
    {
        float currentPos = pos;
        float currentV = v * direction;

        for (int i = 0; i < 10; i++) // 最多 10 次反射
        {
            float wall = currentV > 0 ? Smax : -Smax;
            float distToWall = wall - currentPos;
            float timeToWall = distToWall / currentV;

            // 在到达墙之前，是否经过 targetPos?
            if ((targetPos - currentPos) / currentV >= 0 &&
                Mathf.Abs(targetPos - currentPos) <= Mathf.Abs(distToWall))
            {
                return true;
            }

            // 撞墙反射
            currentPos = wall;
            currentV = -currentV;
        }

        return false;
    }
}
