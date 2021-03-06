using DblDip.Core.Models;
using DblDip.Core.ValueObjects;
using System;
using static DblDip.Core.Constants.Rates;

namespace DblDip.Testing.Builders
{
    public class PhotographyRateBuilder
    {
        private Rate _photographyRate;

        public static Rate WithDefaults()
        {
            return new Rate(nameof(PhotographyRate), (Price)100, PhotographyRate);
        }

        public PhotographyRateBuilder()
        {
            throw new NotImplementedException();
        }

        public PhotographyRateBuilder(Rate rate)
        {
            _photographyRate = rate;
        }

        public Rate Build()
        {
            return _photographyRate;
        }
    }
}
