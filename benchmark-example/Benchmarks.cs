using AutoMapper;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace benchmark_example
{
    public class Benchmarks
    {
        private Product[] products;
        private IMapper _mapper;

        [Params(10, 100, 1000)]
        public int NumberOfElements { get; set; }

        [GlobalSetup]
        public void Init()
        {

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProductDescription))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price + src.Price * 100 / src.VatPercentage)));


            _mapper = config.CreateMapper();

            products = Enumerable.Range(1, NumberOfElements)
                .Select(n => new Product
                {
                    ProductName = $"Product name {n}",
                    ProductDescription = $"Product edscription {n}",
                    Price = 100,
                    VatPercentage = 100
                })
                .ToArray();
        }

        [Benchmark]
        public void WithAutoMapper()
        {
            foreach (var product in products)
            {
                var productDto = _mapper.Map<ProductDto>(product);
            }
        }

        [Benchmark]
        public void WithDirectAssignment()
        {
            foreach (var product in products)
            {
                var productDto = ProductDto.FromProduct(product);
            }
        }
    }
}
