public class Range
{
    public float min;
    public float max;
    
    public bool isInRange(float value)
    {
        if (value >= min && value <= max)
        {
            return true;
        }
        return false;
    }
}
