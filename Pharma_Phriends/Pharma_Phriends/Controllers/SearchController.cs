﻿using Microsoft.AspNetCore.Mvc;
using Pharma_Phriends.Data;
using Pharma_Phriends.Models;
using Pharma_Phriends.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Pharma_Phriends.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext context;
        public SearchController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

 

        [HttpGet]
        public IActionResult Index()
        {
            SearchViewModel searchView = new SearchViewModel(context.RxDrugs.ToList());
            return View(searchView);
        }

        [HttpPost]
        public IActionResult Index(SearchViewModel searchViewModel)
        {
            SearchViewModel searchView = new SearchViewModel(context.RxDrugs.ToList());
           
            if (ModelState.IsValid)
            {
                List<PharmaPrice> pharmaPrices = new List<PharmaPrice>();
                RxDrug theRxDrug = context.RxDrugs.Find(searchViewModel.RxDrugsId);
                List<Pharmacy> pharmacies = context.Pharmacies
                    .Where(p => p.ZipCode == searchViewModel.ZipCode)
                    .ToList();
                List<Price> prices = new List<Price>();
                foreach (Pharmacy phar in pharmacies)
                {
                    Price price = context.Prices
                        .Where(price => price.PharmacyId == phar.Id)
                        .Where(price => price.RxDrugsId == theRxDrug.Id)
                        .Single();
                    pharmaPrices.Add(new PharmaPrice(phar.PharmacyName, price.DrugPrice));
                }
                searchView.SearchResult = new SearchResult(theRxDrug.DrugName, pharmaPrices, theRxDrug.Id);
                return View(searchView);
            }
            return View(searchView);
        }
     
    }
}
