﻿using System;
using Realms;

namespace Flip2Learn.Shared.Database
{
    public class CountrySnapshot : RealmObject
    {
        [PrimaryKey, Indexed]
        public string Id { get; set; }

        [Indexed]
        public bool IsMarkedAsKnown { get; set; }
    }
}
