using System;
using System.IO;
using System.Xml.Serialization;

namespace RecordApp.Services
{
    /// <summary>
    /// Implements IDataPersistence<T> using XML serialization.
    /// This class is registered in DI so ViewModels can persist data
    /// without knowing the storage details.
    /// </summary>
    public class XmlDataPersistence<T> : IDataPersistence<T>
    {
        private readonly string _filePath;
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// Constructor takes a file path for XML storage.
        /// DI will provide this path when creating the service.
        /// </summary>
        public XmlDataPersistence(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            _filePath = filePath;
            _serializer = new XmlSerializer(typeof(T[])); // Serializes arrays of T
        }

        /// <summary>
        /// Reads all records from the XML file.
        /// Returns an empty array if the file does not exist.
        /// Includes error handling for malformed XML and I/O issues.
        /// </summary>
        public T[] ReadDataRecords()
        {
            if (!File.Exists(_filePath))
                return Array.Empty<T>();

            try
            {
                using var reader = new StreamReader(_filePath);
                return (T[])_serializer.Deserialize(reader);
            }
            catch (InvalidOperationException ex)
            {
                // XML is malformed or doesn't match expected type
                throw new ApplicationException("Failed to deserialize XML data. Check file format.", ex);
            }
            catch (IOException ex)
            {
                // File I/O issues (locked file, disk error)
                throw new ApplicationException("I/O error occurred while reading data.", ex);
            }
        }

        /// <summary>
        /// Writes the provided array of T objects to the XML file.
        /// Overwrites any existing content.
        /// Includes error handling for serialization and I/O issues.
        /// </summary>
        public void WriteDataRecords(T[] dataRecords)
        {
            try
            {
                using var writer = new StreamWriter(_filePath);
                _serializer.Serialize(writer, dataRecords);
            }
            catch (IOException ex)
            {
                throw new ApplicationException("I/O error occurred while writing data.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new ApplicationException("Failed to serialize data to XML.", ex);
            }
        }

        /// <summary>
        /// Clears all stored data by writing an empty string to the file.
        /// Used before saving fresh data.
        /// </summary>
        public void ClearAllStoredDataRecords()
        {
            try
            {
                File.WriteAllText(_filePath, string.Empty);
            }
            catch (IOException ex)
            {
                throw new ApplicationException("I/O error occurred while clearing data.", ex);
            }
        }
    }
}




/*

namespace RecordApp.Services
{
    public class XmlDataPersistence<T> : IDataPersistence<T>
    {
        private readonly string _filePath;
        private readonly XmlSerializer _serializer;

        public XmlDataPersistence(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            _filePath = filePath;
            _serializer = new XmlSerializer(typeof(T[]));
        }

        public T[] ReadDataRecords()
        {
            if (!File.Exists(_filePath))
                return Array.Empty<T>();

            using var reader = new StreamReader(_filePath);
            return (T[])_serializer.Deserialize(reader);
        }

        public void WriteDataRecords(T[] dataRecords)
        {
            using var writer = new StreamWriter(_filePath);
            _serializer.Serialize(writer, dataRecords);
        }

        public void ClearAllStoredDataRecords()
        {
            File.WriteAllText(_filePath, string.Empty);
        }
    }
}
*/