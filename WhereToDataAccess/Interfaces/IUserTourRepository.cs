﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereToDataAccess.Entities;

namespace WhereToDataAccess.Interfaces
{
    public interface IUserTourRepository
    {
        IQueryable<UserTour> GetAll();
        UserTour Get(int id);
        void Create(UserTour item);
        void Update(UserTour item);
        void Delete(int id);
    }
}
