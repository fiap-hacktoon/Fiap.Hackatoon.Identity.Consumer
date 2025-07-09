using FIAP.TechChallenge.UserHub.Domain.Interfaces.Elastic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.TechChallenge.UserHub.Infrastructure.ElasticSearch
{
    public class ElasticSettings : IElasticSettings
    {
        public string ApiKey { get; set; }

        public string CloudId { get; set; }
    }
}
