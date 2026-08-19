using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Services
{
    /// <summary>
    /// JSON serializer for Customer collections using System.Text.Json.
    /// Handles file and parsing errors gracefully and returns ServiceResult for callers to inspect.
    /// </summary>
    public class JsonCustomerSerializer : ICustomerSerializer
    {
        private readonly JsonSerializerOptions _options;

        public JsonCustomerSerializer()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

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
                var json = JsonSerializer.Serialize(customers, _options);
                File.WriteAllText(path, json);
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
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("Unexpected error during serialization. See logs for details.");
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
                var json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return ServiceResult.Failure("File is empty.");
                }

                var result = JsonSerializer.Deserialize<List<Customer>>(json, _options);
                if (result == null)
                {
                    return ServiceResult.Failure("JSON structure is invalid or does not represent a list of customers.");
                }

                customers = result;
                return ServiceResult.Success();
            }
            catch (JsonException ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("Invalid JSON format.");
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
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                return ServiceResult.Failure("Unexpected error during deserialization. See logs for details.");
            }
        }
    }
}
