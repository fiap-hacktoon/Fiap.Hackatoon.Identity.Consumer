﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.TechChallenge.UserHub.Domain.Interfaces.Elastic
{
    public interface IElasticSettings
    {
        string ApiKey { get; set; }

        string CloudId { get; set; }
    }
}
