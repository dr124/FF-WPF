namespace FF.WPF.Filters.Implementations;

public class TestThresholdParams : FilterParams
{
    private float _ratio = 0.5f;

    public float Ratio
    {
        get => _ratio;
        set => SetPropertyAndNotify(ref _ratio, value);
    }
}