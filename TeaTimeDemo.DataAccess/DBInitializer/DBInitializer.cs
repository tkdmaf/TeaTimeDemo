using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaTimeDemo.DataAccess.Data;

namespace TeaTimeDemo.DataAccess.DBInitializer
{
    public class DBInitializer: IDBInitializer
    {
        private readonly ApplicationDbContext _db;

        public DBInitializer(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            return;
        }
    }
}
