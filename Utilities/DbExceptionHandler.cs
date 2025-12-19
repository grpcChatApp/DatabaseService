using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DatabaseService.Utilities;

public static class DbExceptionHandler
{
        public static void Handle(DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pgEx)
            {
                // 23505 = unique_violation, 23503 = foreign_key_violation
                switch (pgEx.SqlState)
                {
                    case "23505":
                        throw new InvalidOperationException(
                            "Username already exists.",
                            ex
                        );

                    case "23503":
                        throw new InvalidOperationException(
                            "Update violates database constraints.",
                            ex
                        );
                }
            }

            throw ex;
        }
}
