﻿using System;
using System.Data.Common;
using System.Threading.Tasks;
using Abp.Collections;
using Abp.Modules;
using Abp.TestBase.SampleApplication.EntityFramework;
using Castle.MicroKernel.Registration;

namespace Abp.TestBase.SampleApplication.Tests
{
    public abstract class SampleApplicationTestBase : AbpIntegratedTestBase
    {
        protected SampleApplicationTestBase()
        {
            //Fake DbConnection using Effort!
            LocalIocManager.IocContainer.Register(
                Component.For<DbConnection>()
                    .UsingFactoryMethod(Effort.DbConnectionFactory.CreateTransient)
                    .LifestyleSingleton()
                );
        }

        protected override void AddModules(ITypeList<AbpModule> modules)
        {
            base.AddModules(modules);
            modules.Add<SampleApplicationModule>();
        }

        public void UsingDbContext(Action<SampleApplicationDbContext> action)
        {
            using (var context = LocalIocManager.Resolve<SampleApplicationDbContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        public T UsingDbContext<T>(Func<SampleApplicationDbContext, T> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<SampleApplicationDbContext>())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        public async Task<T> UsingDbContextAsync<T>(Func<SampleApplicationDbContext, Task<T>> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<SampleApplicationDbContext>())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}