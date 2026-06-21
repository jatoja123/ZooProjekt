namespace Zoo.Needs;

public abstract class Need
{
    private int _value;
    private int _maxValue;
    private int _threshold;
    
    public int Value => _value;

    public int MaxValue => _maxValue;
    public int Missing => _maxValue - _value;
    public int PassiveDecrease { get; private set; }
    public NeedType Type { get; private set; }

    public Need(NeedType type, int value, int maxValue = 100, int threshold = 20, int passiveDecrease = 0)
    {
        if(value > maxValue)
        {
            _maxValue = value;
        } 
        else
        {
            _maxValue = maxValue;
        }
        
        PassiveDecrease = passiveDecrease;
        
        _value = value;
        Type = type;
        if(threshold > _maxValue)
        {
            throw new ArgumentException("Próg krytyczny nie może być większy niż wartość maksymalna!");
        }
        else
        {
            _threshold = threshold;
        }
    }

    public void Decrease(int amount)
    {
        _value -= amount;
        if (_value < 0)
        {
            _value = 0;
        }
    }

    public void Increase(int amount)
    {
        _value += amount;
        if(_value > _maxValue)
        {
            _value = _maxValue;
        }
    }

    public bool IsCritical()
    {
        return _value <= _threshold;
    }
}