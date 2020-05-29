public enum RPSType
{
    Rock,
    Paper,
    Scissors
}
public enum RPSWinner
{
    Left,
    Right,
    Draw
}

public class RPS
{
    public static RPSWinner Play(RPSType left, RPSType right)
    {
        if (left == right) {
            return RPSWinner.Draw;
        }

        if ((( ((int) left) | 1 << (2)) - ( ((int) right) | 0 << (2))) % 3 != 0)
        {
            return RPSWinner.Left;
        }

        return RPSWinner.Right;
    }
}