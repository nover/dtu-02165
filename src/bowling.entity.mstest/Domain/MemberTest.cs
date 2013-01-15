﻿using bowling.entity.mstest;
using FluentNHibernate;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateSrc.Tests.Domain
{
    [TestClass]
    public class MemberTest : EntityTestBase
    {
        [TestMethod]
        public void TestCRUD()
        {
            new PersistenceSpecification<Bowling.Entity.Domain.Member>(this._session)
                .CheckProperty(c => c.Id, 1)
                .CheckProperty(c => c.Name, "John Doe")
                .CheckProperty(c => c.Email, "john@doe.dk")
                .VerifyTheMappings();
        }
    }
}
