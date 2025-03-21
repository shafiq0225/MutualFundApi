using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nav.Application.Interfaces;
using Nav.Domain.Entities;
using Nav.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Nav.Application.Services
{
    public class NavService : INavService
    {
        private readonly IApplicationDbContext _context;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public NavService(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddNavAsync(IFormFile file)
        {
            decimal defaultDecimal = default;
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);

            var filePath = Path.Combine(_uploadPath, file.FileName);

            // Save the file to server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            string filePathFromAmfi = filePath;
            var amfiDatas = File.ReadAllLines(filePathFromAmfi).ToList();
            amfiDatas.RemoveAt(0);
            amfiDatas.RemoveAt(0);
            var mfList = new List<Fund>();
            foreach (var amfiData in amfiDatas)
            {
                var entries = amfiData.Split(';');
                Fund mf = new Fund();
                if (entries.Length == 1)
                {
                    mf.Name = entries[0];
                    mfList.Add(mf);
                    continue;
                }
                mf.SchemeId = entries[0];
                mf.SchemeName = entries[3];
                mf.Rate = entries[4] == "N.A." ? defaultDecimal : decimal.Parse(entries[4]);
                mf.Date = DateTime.Parse(entries[5]);
                mf.SchemeVisible = true;
                mf.FundVisible = true;
                mfList.Add(mf);
            }
            var mutualFundTitle = mfList.Where(x => x.SchemeId == null && x.Name.Contains("Mutual Fund")).ToList();
            var db = new string[] // get it from sqllite
            {
                "Aditya Birla Sun Life Mutual Fund",
                "Canara Robeco Mutual Fund",
                "Franklin Templeton Mutual Fund",
                "HDFC Mutual Fund"
            };
            var excludeDuplicateTitles = mutualFundTitle.Select(i => new { i.Name })
            .Distinct().Select(x => new Fund { Name = x.Name }).OrderBy(x => x.Name).Where(x => db.Contains(x.Name)).ToList();

            var mutualFunds = new List<Fund>();
            foreach (var mfTitle in excludeDuplicateTitles)
            {
                var i = 1;
                var tilteSplit = mfTitle.Name.Split(' ').FirstOrDefault();
                tilteSplit = tilteSplit + " ";
                var funds = mfList.Where(x => x.SchemeName != null && x.SchemeName.Contains(tilteSplit)).ToList();
                foreach (var fund in funds)
                {
                    fund.Name = mfTitle.Name;
                    fund.Id = Guid.NewGuid().ToString();
                    fund.FundId = tilteSplit.Split(' ').FirstOrDefault() + "_" + i;
                    mutualFunds.Add(fund);
                    _context.Funds.Add(fund);
                }
            }
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<IEnumerable<Fund>> GetAllNavByDate(DateTime startDate, DateTime endDate)
        {
            return await _context.Funds.Where(x => x.FundVisible && x.SchemeVisible && x.Date >= startDate && x.Date <= endDate).OrderBy(x => x.FundId).ToListAsync();
        }

        public async Task<IndividualNav> GetIndividualNav(string fundId, string schemeId, DateTime startDate, DateTime endDate)
        {
            var individalNav = new IndividualNav();
            var schemeRateByDate = new List<SchemeHistory>();

            var navs = await _context.Funds.Where(x => x.FundVisible && x.FundId == fundId && x.SchemeVisible && x.SchemeId == schemeId && x.Date >= startDate && x.Date <= endDate).GroupBy(x => x.SchemeId).ToListAsync();
            if (navs.Count == 0)
            {
                return individalNav;
            }
            foreach (var nav in navs)
            {
                var fundFiltered = nav.Where(x => x.SchemeId == schemeId);
                foreach (var fund in fundFiltered)
                {
                    individalNav.FundId = fund.FundId;
                    individalNav.SchemeId = fund.SchemeId;
                    individalNav.SchemeName = fund.SchemeName;
                    individalNav.FundName = fund.Name;
                    var scheme = new SchemeHistory()
                    {
                        Rate = Math.Round(fund.Rate, 2),
                        SchemeDate = DateOnly.Parse(fund.Date.ToShortDateString())
                    };
                    schemeRateByDate.Add(scheme);
                    individalNav.SchemeHistory = schemeRateByDate;
                }
                
            }
            return individalNav;
        }

        public async Task<IEnumerable<Funds>> GetVisibleFunds()
        {
            var funds = new List<Funds>();
            var navs = await _context.Funds.Where(x => x.FundVisible).OrderBy(x => x.FundId).GroupBy(x => x.FundId).ToListAsync();

            foreach (var nav in navs)
            {
                var fundFiltered = nav.FirstOrDefault(x => x.FundId == nav.Key);
                if (fundFiltered?.FundId == nav.Key)
                {
                    var fund = new Funds
                    {
                        FundName = fundFiltered.Name,
                        FundId = fundFiltered.FundId
                    };
                    funds.Add(fund);
                }
            }
            return funds;
        }

        public async Task<IEnumerable<Scheme>> GetVisibleSchemes(string fundId)
        {
            var schemes = new List<Scheme>();
            var navs = await _context.Funds.Where(x => x.FundVisible && x.FundId == fundId && x.SchemeVisible).GroupBy(x => x.SchemeId).ToListAsync();

            foreach (var nav in navs)
            {
                var schemeFiltered = nav.FirstOrDefault(x => x.SchemeId == nav.Key);
                if (schemeFiltered?.SchemeId == nav.Key)
                {
                    var scheme = new Scheme
                    {
                        SchemeId = schemeFiltered.SchemeId,
                        SchemeName = schemeFiltered.SchemeName,
                    };
                    schemes.Add(scheme);
                }
            }
            return schemes;
        }

        public async Task UpdateIsVisible(string fundId, bool isVisible)
        {
            var Funds = _context.Funds.Where(x => x.FundId == fundId).ToList();

            foreach (var scheme in Funds)
            {
                if (scheme != null)
                {
                    scheme.FundVisible = isVisible;
                    scheme.SchemeVisible = isVisible;

                    _context.Funds.Update(scheme);
                }
            }
            await _context.SaveChangesAsync(CancellationToken.None);

        }

        public async Task UpdateIsVisibleInScheme(string fundId, string schemeId, bool isVisible)
        {
            var Funds = _context.Funds.Where(x => x.FundId == fundId).ToList();

            foreach (var scheme in Funds)
            {
                if (scheme.SchemeId != schemeId)
                {
                    continue;
                }
                scheme.SchemeVisible = isVisible;
                _context.Funds.Update(scheme);
            }
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
