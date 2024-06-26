﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereToDataAccess.Entities;
using WhereToDataAccess.Interfaces;
using WhereToServices.Interfaces;

namespace WhereToServices
{
    public class TourService : ITourService
    {
        private readonly IUnitOfWork uow;

        public TourService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public void CreateTour(Tour tour)
        {
            uow.Tours.Create(tour);
            uow.Save();
        }

        public void Delete(int id)
        {
            uow.Tours.Delete(id);
            uow.Save();
        }

        public Tour GetTourById(int id)
        {
            var tour = uow.Tours.Get(id);

            return tour ?? throw new KeyNotFoundException();
        }

        public IEnumerable<Tour> GetTours()
        {
            var tours = uow.Tours.GetAll();

            return tours ?? throw new KeyNotFoundException();
        }

        public IEnumerable<Tour> GetToursByCity(int cityId)
        {
            var tours = uow.Tours.GetToursByCity(cityId);

            return tours ?? throw new KeyNotFoundException();
        }

        public IEnumerable<Tour> GetToursByDateRange(DateTime startDate, DateTime endDate)
        {
            var tours = uow.Tours.GetToursByDateRange(startDate.Date, endDate.Date);

            return tours ?? throw new KeyNotFoundException();
        }

        public IEnumerable<Tour> GetUpcomingTours()
        {
            var tours = uow.Tours.GetToursByDateRange(DateTime.Now.Date, null);
            return tours ?? throw new KeyNotFoundException();
        }

        public void Update(Tour tour)
        {
            uow.Tours.Update(tour);
            uow.Save();
        }
    }
}
