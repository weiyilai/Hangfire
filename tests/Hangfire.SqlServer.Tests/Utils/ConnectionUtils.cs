﻿using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Hangfire.SqlServer.Tests
{
    public static class ConnectionUtils
    {
        private const string DatabaseVariable = "Hangfire_SqlServer_DatabaseName";
        private const string ConnectionStringTemplateVariable 
            = "Hangfire_SqlServer_ConnectionStringTemplate";

        private const string MasterDatabaseName = "master";
        private const string DefaultDatabaseName =
#if NET452
                "Hangfire.SqlServer.Tests.net452"
#elif NET461
                "Hangfire.SqlServer.Tests.net461"
#elif NETCOREAPP3_1
                "Hangfire.SqlServer.Tests.netcoreapp3_1"
#elif NET6_0
                "Hangfire.SqlServer.Tests.net6_0"
#else
                "Hangfire.SqlServer.Tests"
#endif
            ;
        private const string DefaultConnectionStringTemplate
            = @"Server=.\;Database={0};Trusted_Connection=True;TrustServerCertificate=True;";

        public static string GetDatabaseName()
        {
            return Environment.GetEnvironmentVariable(DatabaseVariable) ?? DefaultDatabaseName;
        }

        public static string GetMasterConnectionString()
        {
            return String.Format(GetConnectionStringTemplate(), MasterDatabaseName);
        }

        public static string GetConnectionString()
        {
            return String.Format(GetConnectionStringTemplate(), GetDatabaseName());
        }

        private static string GetConnectionStringTemplate()
        {
            return Environment.GetEnvironmentVariable(ConnectionStringTemplateVariable)
                   ?? DefaultConnectionStringTemplate;
        }

        public static DbConnection CreateConnection(bool microsoftDataSqlClient)
        {
            var connection =
#if !NET452 && !NET461
                microsoftDataSqlClient ? (DbConnection)new Microsoft.Data.SqlClient.SqlConnection(GetConnectionString()) :
#endif
                new SqlConnection(GetConnectionString());

            connection.Open();

            return connection;
        }
    }
}
