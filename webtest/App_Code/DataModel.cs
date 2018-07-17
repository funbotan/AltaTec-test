using System;
using System.Linq;
using LinqToDB;
using LinqToDB.Mapping;

public partial class Entry
{
    [Column(IsPrimaryKey = true)]
    public long moment { get; set; }

    [Column]
    public double course { get; set; }
}

[Table("Usd")]
public partial class Usd : Entry { }

[Table("Eur")]
public partial class Eur : Entry { }

[Table("Rub")]
public partial class Rub : Entry { }

public partial class TestDataDB : LinqToDB.Data.DataConnection
{
    public ITable<Usd> Usd { get { return this.GetTable<Usd>(); } }
    public ITable<Eur> Eur { get { return this.GetTable<Eur>(); } }
    public ITable<Rub> Rub { get { return this.GetTable<Rub>(); } }

    public TestDataDB()
    {
    }

    public TestDataDB(string configuration) : base("DB")
    {
    }
}