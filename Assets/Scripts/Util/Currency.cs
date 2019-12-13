using System;
[Serializable]
public class Currency
{
    public CURRENCY_TYPE Type;

    public double value;

    public Currency()
    {

    }

    public Currency(CURRENCY_TYPE Type, double value)
    {
        this.Type = Type;
        this.value = value;
    }

    public double ConvertCurrency(CURRENCY_TYPE toType)
    {
        return 0;
    }

}


public enum CURRENCY_TYPE
{
    Coin,
    Gem,
}