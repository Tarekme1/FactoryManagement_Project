using System;
using System.Configuration;

namespace VP_Midterm_Project
{
    internal static class DatabaseSettings
    {
        internal static string ConnectionString
        {
            get
            {
                var environmentValue = Environment.GetEnvironmentVariable("FACTORY_MANAGEMENT_CONNECTION_STRING");
                if (!string.IsNullOrWhiteSpace(environmentValue))
                {
                    return environmentValue;
                }

                var configuredValue = ConfigurationManager.ConnectionStrings["FactoryManagement"];
                if (configuredValue == null || string.IsNullOrWhiteSpace(configuredValue.ConnectionString))
                {
                    throw new ConfigurationErrorsException(
                        "Configure FactoryManagement in App.config or set the FACTORY_MANAGEMENT_CONNECTION_STRING environment variable.");
                }

                return configuredValue.ConnectionString;
            }
        }
    }
}
