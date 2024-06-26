﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereToDataAccess.Interfaces
{
    public interface IRepository
    {
        public interface IRepository<T,K> where T : class
        {
            IQueryable<T> GetAll();
            T Get(K id);
            void Create(T item);
            void Update(T item);
            void Delete(K id);
        }
    }
}
