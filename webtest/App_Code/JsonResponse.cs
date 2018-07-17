// Объектная модель JSON-ответа от биржи

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class JsonResponse
{
    public Currency btc_usd { get; set; }
    public Currency btc_eur { get; set; }
    public Currency btc_rur { get; set; }
}

public class Currency
{
    public double high { get; set; }
    public double low { get; set; }
    public double avg { get; set; }
    public double vol { get; set; }
    public double vol_cur { get; set; }
    public double last { get; set; }
    public double buy { get; set; }
    public double sell { get; set; }
    public double updated { get; set; }
}