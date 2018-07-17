// Основной код сервиса

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Data.SQLite;
using System.Net;
using System.Web.Script.Serialization;
using System.Data.Linq;
using LinqToDB;
using System.Configuration;
using System.Web.Configuration;

public class Service : IService
{
    static Timer timer;

    static Service()
    {
        // Очищаем БД перед запуском
        using (var db = new TestDataDB())
        {
            db.Usd.Delete();
            db.Eur.Delete();
            db.Rub.Delete();
        }

        // Запуск периодического обновления котировок
        timer = new Timer(5*60*1000);
        timer.AutoReset = true;
        timer.Elapsed += Update;
        timer.Start();
    }

    // Метод обновления котировок
    static void Update(object sender, EventArgs e)
    {
        string response;
        using (WebClient wc = new WebClient())
        {
            // получение данных
            response = wc.DownloadString("https://wex.nz/api/3/ticker/btc_usd-btc_eur-btc_rur");
        }
        JsonResponse data = new JavaScriptSerializer().Deserialize<JsonResponse>(response);
        using (var db = new TestDataDB())
        {
            // сохранение в БД
            db.Insert<Usd>(new Usd() { course = data.btc_usd.last, moment = DateTime.Now.Ticks });
            db.Insert<Eur>(new Eur() { course = data.btc_eur.last, moment = DateTime.Now.Ticks });
            db.Insert<Rub>(new Rub() { course = data.btc_rur.last, moment = DateTime.Now.Ticks });
        }
    }

    // Метод для общения с клиентом
    // start, end - грантицы временного интервала, currency - очевидно
    public CompositeType GetDataUsingDataContract(string currency, DateTime start, DateTime end)
	{
        using (var db = new TestDataDB())
        {
            LinqToDB.ITable<Entry> table;
            switch (currency)
            {
                case "usd": table = db.GetTable<Usd>(); break;
                case "eur": table = db.GetTable<Eur>(); break;
                case "rub": table = db.GetTable<Rub>(); break;
                default: throw new Exception("Unsupported currency");
            }
            // Возвращаемые данные - два массива, для дат и для самих котировок
            CompositeType data = new CompositeType();
            var query = from e in table
                        where (start.Ticks < e.moment && e.moment < end.Ticks)
                        select e;
            data.Time = (from e in query select new DateTime(e.moment)).ToArray();
            data.Course = (from e in query select e.course).ToArray();
            return data;
        }
    }
}
