using UnityEngine;

public struct BrakeResult
{
    public float breakPos;   // ��ʼ���ٵ�λ��
    public int direction;    // ����ʱ���˶����� (1 ��, -1 ��)

    public BrakeResult(float breakPos, int direction)
    {
        this.breakPos = breakPos;
        this.direction = direction;
    }
}

public static class BallBrakeSolver
{
    /// <summary>
    /// ����С��ļ��ٴ�����
    /// </summary>
    /// <param name="Smax">�߽磬ǽ�� [-Smax, Smax]</param>
    /// <param name="pos">��ǰ��λ��</param>
    /// <param name="direction">��ǰ���� (1 ��, -1 ��)</param>
    /// <param name="v">��ǰ�ٶȴ�С (>=0)</param>
    /// <param name="a">���ٶȴ�С (����)</param>
    /// <param name="target">ϣ��ͣ������λ��</param>
    /// <returns>�������ͷ������û�ⷵ�� breakPos = NaN</returns>
    public static BrakeResult GetBrakePosition(
        float Smax, float pos, int direction, float v, float a, float target)
    {
        if (a <= 0f || v <= 0f) return new BrakeResult(float.NaN, direction);

        float d = (v * v) / (2f * a); // �ƶ�����

        // ���Լ���չ�����
        for (int n = -5; n <= 5; n++)
        {
            float target_n = ((n % 2 == 0) ? 1f : -1f) * target + 2f * n * Smax;
            float brake_n = target_n - direction * d;

            // ������ԭ������
            if (brake_n < -Smax || brake_n > Smax)
                continue;

            // �ж��Ƿ��ܵ��� (pos ����, �з���)
            if (WillPassPosition(pos, direction, v, Smax, brake_n))
            {
                return new BrakeResult(brake_n, direction);
            }
        }

        return new BrakeResult(float.NaN, direction);
    }

    /// <summary>
    /// �жϴ� pos,direction ������С���Ƿ�ᾭ��Ŀ���
    /// ����ģ��һ��ʱ�䣬���Ƿ��䣩
    /// </summary>
    private static bool WillPassPosition(float pos, int direction, float v, float Smax, float targetPos)
    {
        float currentPos = pos;
        float currentV = v * direction;

        for (int i = 0; i < 10; i++) // ��� 10 �η���
        {
            float wall = currentV > 0 ? Smax : -Smax;
            float distToWall = wall - currentPos;
            float timeToWall = distToWall / currentV;

            // �ڵ���ǽ֮ǰ���Ƿ񾭹� targetPos?
            if ((targetPos - currentPos) / currentV >= 0 &&
                Mathf.Abs(targetPos - currentPos) <= Mathf.Abs(distToWall))
            {
                return true;
            }

            // ײǽ����
            currentPos = wall;
            currentV = -currentV;
        }

        return false;
    }
}
