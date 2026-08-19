using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Services
{
    /// <summary>
    /// XML serializer for Customer collections using System.Xml.Serialization.
    /// Returns ServiceResult and does not throw for common file/format errors.
    /// </summary>
    public class XmlCustomerSerializer : ICustomerSerializer
    {
        public ServiceResult Serialize(string path, IEnumerable<Customer> customers)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return ServiceResult.Failure("Invalid file path.");
            }
            if (customers == null)
            {
                return ServiceResult.Failure("Customers collection is null.");
            }

            try
            {
                var serializer = new XmlSerializer(typeof(List<Customer>));
                var dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var settings = new XmlWriterSettings { Indent = true };
                using (var stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, new List<Customer>(customers));
                }

                return ServiceResult.Success();
            }
            catch (UnauthorizedAccessException ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("Access denied writing file.");
            }
            catch (DirectoryNotFoundException ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("Directory not found.");
            }
            catch (IOException ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("I/O error writing file.");
            }
            catch (InvalidOperationException ex)
            {
                // XmlSerializer can throw InvalidOperationException wrapping inner exception for serialization errors
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("XML serialization error.");
            }
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("Unexpected error during XML serialization. See logs for details.");
            }
        }

        public ServiceResult Deserialize(string path, out List<Customer> customers)
        {
            customers = new List<Customer>();

            if (string.IsNullOrWhiteSpace(path))
            {
                return ServiceResult.Failure("Invalid file path.");
            }

            if (!File.Exists(path))
            {
                return ServiceResult.Failure($"File not found: {path}");
            }

            try
            {
                var serializer = new XmlSerializer(typeof(List<Customer>));
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var reader = XmlReader.Create(stream))
                {
                    var obj = serializer.Deserialize(reader) as List<Customer>;
                    if (obj == null)
                    {
                        return ServiceResult.Failure("XML structure is invalid or does not represent a list of customers.");
                    }

                    customers = obj;
                    return ServiceResult.Success();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("Access denied reading file.");
            }
            catch (IOException ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("I/O error reading file.");
            }
            catch (InvalidOperationException ex)
            {
                // Malformed XML or type mismatch
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("Invalid XML format.");
            }
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("Unexpected error during XML deserialization. See logs for details.");
            }
        }
    }
}
