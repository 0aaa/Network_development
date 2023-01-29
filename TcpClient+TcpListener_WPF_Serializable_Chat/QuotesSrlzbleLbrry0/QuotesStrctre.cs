using System;

public struct QuotesStrctre
{
    public string RandomQuote { get => String.Format("quote" + new Random().Next(10)); }
}