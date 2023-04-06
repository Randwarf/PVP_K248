namespace Benchmarker.MVVM.Model
{
    interface IGraphable
    {
        string GetGraphString(int height, int width);
        double GetCurrentValue();
        double GetMaxValue();
    }
}
