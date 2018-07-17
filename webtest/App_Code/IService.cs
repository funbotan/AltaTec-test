// Интерфейс сервиса

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

[ServiceContract]
public interface IService
{
	[OperationContract]
	CompositeType GetDataUsingDataContract(string currency, DateTime start, DateTime end);
}

[DataContract]
public class CompositeType
{
    DateTime[] time;
    double[] course;

	[DataMember]
	public DateTime[] Time
	{
		get { return time; }
		set { time = value; }
	}

	[DataMember]
	public double[] Course
	{
		get { return course; }
		set { course = value; }
	}
}
