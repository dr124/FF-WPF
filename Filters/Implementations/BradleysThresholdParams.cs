namespace FF.WPF.Filters.Implementations;

public class BradleysThresholdParams : FilterParams
{
    private int _s = 2;
    public int T
    {
        get => _t;
        set => SetPropertyAndNotify(ref _t, value);
    }

    private int _t = 50;
    public int S
    {
        get => _s;
        set => SetPropertyAndNotify(ref _s, value);
    }
}