//using Autofac;
//using SpecFlow.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using UserService.Client;

using UserService.Utils;

namespace UserService.Di
{
   // internal class SetUpFixtureUserService
   // {
   //     [ScenarioDependencies]
   //     public static ContainerBuilder ScenarioDependecies() {
   //         var builder = new ContainerBuilder();
   //         builder
   //             .RegisterModule<TestDependecyModule>();
   //         return builder;
   //     }
   //     [BeforeTestRun]
   //     public static void BeforeTestRun() {
   //         var container = ScenarioDependecies().Build();
   //     }
   // }
   //
   // public class TestDependecyModule : Module {
   //
   //     protected override void Load(ContainerBuilder builder)
   //     {
   //         builder
   //             .RegisterType<UserServiceClient>()
   //             .AsSelf();
   //
   //         builder
   //             .RegisterType<UserGenerator>()
   //             .AsSelf();
   //
   //         builder
   //             .RegisterType<UserStepsAssertion>()
   //             .AsSelf(); 
   //
   //         builder
   //             .RegisterType<UserSteps>()
   //             .AsSelf();
   //
   //         builder
   //          .RegisterType<DataContext>()
   //          .AsSelf()
   //          .SingleInstance();
   //
   //     }
   // }
}
