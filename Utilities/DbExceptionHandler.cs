using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace DatabaseService.Utilities;

public static class DbExceptionHandler
{
    public static void Handle(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2601:
                    case 2627:
                        throw new InvalidOperationException(
                            "Username already exists.",
                            ex
                        );

                    case 547:
                        throw new InvalidOperationException(
                            "Update violates database constraints.",
                            ex
                        );
                }
            }

            throw ex;
        }    
}
