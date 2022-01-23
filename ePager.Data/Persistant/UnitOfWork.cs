﻿namespace ePager.Data.Persistant
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EPDbContext _context;

        public UnitOfWork(EPDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
