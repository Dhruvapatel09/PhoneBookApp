﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookTests.Utilities
{
    public class TempDataDictionaryFactory : ITempDataDictionaryFactory
    {
        private readonly ITempDataProvider _tempDataProvider;
        public TempDataDictionaryFactory(ITempDataProvider tempDataProvider)
        {
            _tempDataProvider = tempDataProvider;
        }
        public ITempDataDictionary CreateTempData(HttpContext context)
        {
            if (_tempDataProvider == null)
            {
                throw new InvalidOperationException($"No {nameof(ITempDataDictionary)} was set.");
            }
            return new TempDataDictionary(context, _tempDataProvider);
        }
        public ITempDataDictionary GetTempData(HttpContext context)
        {
            return CreateTempData(context);
        }
    }
}