using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordApp.Services
{

    public interface IDataPersistence<T>
    {
        T[] ReadDataRecords();
        void WriteDataRecords(T[] dataRecords);
        void ClearAllStoredDataRecords();
    }

}
